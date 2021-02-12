using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalle.WinService.Services
{
    public interface IClientWebSocketSend
    {
        Task Send(ClientWebSocket socket, string data);

    }
}
