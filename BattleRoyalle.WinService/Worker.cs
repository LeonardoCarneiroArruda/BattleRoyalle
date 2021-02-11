using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Net.WebSockets;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BattleRoyalle.Models;
using BattleRoyalle.Models.Domain;
using BattleRoyalle.Models.Services;
using BattleRoyalle.WinService.Services;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BattleRoyalle.WinService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static IInfoMaquina services_registro;
        private static readonly string endereco = "localhost";
        private static readonly string porta = "5000";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            services_registro = new RegistroService();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            RunWebSockets().GetAwaiter().GetResult();
        }

        private static async Task RunWebSockets()
        {
            RegistroModel registro = new RegistroModel();
            registro = services_registro.RegistrarInformacoesMaquina();

            var client = new ClientWebSocket();
            await client.ConnectAsync(new Uri($"ws://{endereco}:{porta}/ws"), CancellationToken.None);

            MensagemModel mensagem = new MensagemModel(registro);
            await Send(client, mensagem.ToString());

            Console.WriteLine("Connected!");

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

        private static async Task Receiving(ClientWebSocket client)
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
                        escreverNaTelaSeNaoForComando(mensagem);
                    }
                    else if (mensagem.TipoMensagem == Models.Enum.TipoMensagemEnum.MensagemComando)
                    {

                        List<string> retorno = new List<string>();

                        try { retorno = new ComandoServices().Execute(mensagem.data); } catch (Exception ex) { };
                        Console.WriteLine(input);
                    
                        string data = "";
                        foreach (string item in retorno)
                        {
                            Console.WriteLine(item);
                            data += item + "\n";
                        }

                        mensagem.data = data;
                        mensagem.TipoMensagem = Models.Enum.TipoMensagemEnum.MensagemResposta;

                        await Send(client, mensagem.ToString());                            
                    }
                    
                   
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }
            }
        }

        private static async Task Send(ClientWebSocket socket, string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private static void escreverNaTelaSeNaoForComando(MensagemModel mensagem)
        {
            IRetornoMensagem retornoMensagem = null;
            switch (mensagem.TipoMensagem)
            {
                case Models.Enum.TipoMensagemEnum.MensagemConectado:
                    retornoMensagem = new MensagemConectado(mensagem.data);
                    break;
                case Models.Enum.TipoMensagemEnum.MensagemRegistro:
                    retornoMensagem = new MensagemRegistro(mensagem.registro.ToString());
                    break;
            }

            Console.Write(mensagem.RetornaMensagemParaMostrarNaTela(retornoMensagem));
        }

    }
}
