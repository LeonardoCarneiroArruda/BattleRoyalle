using BattleRoyalle.Models.Domain;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace BattleRoyalle.WinService.Services.Handlres
{
    public class MensagemNoConsoleComandoEEnvia : IMensagemNoConsole
    {
        private MensagemModel _mensagem;
        private string _input;
        private IClientWebSocketSend _clientWebSocketSendMessage;
        private ClientWebSocket _client;

        public MensagemNoConsoleComandoEEnvia(MensagemModel mensagem, string input, IClientWebSocketSend clientWebSocketSendMessage, ClientWebSocket client)
        {
            this._mensagem = mensagem;
            this._input = input;
            this._clientWebSocketSendMessage = clientWebSocketSendMessage;
            this._client = client;
        }

        public async Task Escrever()
        {
            List<string> retorno = new List<string>();

            try { retorno = new ComandoServices().Execute(_mensagem.data); } catch (Exception ex) { };
            Console.WriteLine(_input);

            string data = "";
            foreach (string item in retorno)
            {
                Console.WriteLine(item);
                data += item + "\n";
            }

            _mensagem.data = data;
            _mensagem.TipoMensagem = Models.Enum.TipoMensagemEnum.MensagemResposta;

            await _clientWebSocketSendMessage.Send(_client, _mensagem.ToString());
        }
    }
}
