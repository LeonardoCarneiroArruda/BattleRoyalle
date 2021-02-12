using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleRoyalle.WinService.Services.Handlres
{
    public class ClientWebSocketSend : IClientWebSocketSend
    {

        public async Task Send(ClientWebSocket socket, string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

    }
}
