using BattleRoyalle.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalle.WebApp.WebSocketManager.Handlres
{
    public class MessageHandler : WebSocketHandler
    {
        public MessageHandler(ConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = WebSocketConnectionManager.GetId(socket);

            MensagemModel mensagem = new MensagemModel();
            mensagem.TipoMensagem = Models.Enum.TipoMensagemEnum.MensagemConectado;
            mensagem.data = $"{socketId} is now connected";
            await SendMessageToAllAsync(mensagem.ToString());
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            var input = Encoding.UTF8.GetString(buffer, 0, result.Count) ;
            MensagemModel mensagem = Newtonsoft.Json.JsonConvert.DeserializeObject<MensagemModel>(input); 

            await SendMessageToAllAsync(mensagem.ToString());
        }

    }
}
