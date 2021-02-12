using BattleRoyalle.Models.Domain;
using BattleRoyalle.Models.Interfaces;
using BattleRoyalle.WinService.Services;
using BattleRoyalle.WinService.Services.Handlres;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleRoyalle.WinService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IInfoMaquina _services_registro;
        private IClientWebSocketSend _clientWebSocketSendMessage;
        private IControladorDeMensagemNoConsole _controladorDeMensagemNoConsole;

        private readonly string endereco = "localhost";
        private readonly string porta = "5000";

        public Worker(ILogger<Worker> logger, IInfoMaquina registroMaquina, 
            IClientWebSocketSend clientWebSocketSendMessage, IControladorDeMensagemNoConsole controladorDeMensagemNoConsole)
        {
            _logger = logger;
            _services_registro = registroMaquina;
            _clientWebSocketSendMessage = clientWebSocketSendMessage;
            _controladorDeMensagemNoConsole = controladorDeMensagemNoConsole;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RunWebSockets().GetAwaiter().GetResult();
        }

        private async Task RunWebSockets()
        {
            RegistroModel registro = new RegistroModel();
            registro = _services_registro.RegistrarInformacoesMaquina();

            var client = new ClientWebSocket();
            await client.ConnectAsync(new Uri($"ws://{endereco}:{porta}/ws"), CancellationToken.None);

            MensagemModel mensagem = new MensagemModel(registro);
            await _clientWebSocketSendMessage.Send(client, mensagem.ToString());

            Console.WriteLine(">> Connected!");
                
            var sending = Task.Run(async () =>
            {
                string line;
                while ((line = Console.ReadLine()) != null && line != String.Empty)
                {
                    var bytes = Encoding.UTF8.GetBytes(line);
                    await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }

                await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            });

            var receiving = Receiving(client);

            await Task.WhenAll(sending, receiving);
        }

        private async Task Receiving(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];

            while (true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string input = (Encoding.UTF8.GetString(buffer, 0, result.Count));
                    MensagemModel mensagem = Newtonsoft.Json.JsonConvert.DeserializeObject<MensagemModel>(input);

                    if (mensagem.TipoMensagem == Models.Enum.TipoMensagemEnum.MensagemConectado
                        || mensagem.TipoMensagem == Models.Enum.TipoMensagemEnum.MensagemRegistro)
                    {
                        _controladorDeMensagemNoConsole.EscreverMensagemNoConsole(new MensagemNoConsoleConectadoOuRegistro(mensagem));
                    }
                    else if (mensagem.TipoMensagem == Models.Enum.TipoMensagemEnum.MensagemComando)
                    {
                        _controladorDeMensagemNoConsole.EscreverMensagemNoConsole(new MensagemNoConsoleComandoEEnvia(mensagem, input, this._clientWebSocketSendMessage, client));
                    }
                    
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }
            }
        }

    }
}
