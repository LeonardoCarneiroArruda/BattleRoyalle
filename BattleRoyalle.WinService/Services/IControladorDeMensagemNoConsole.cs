using System;
using System.Collections.Generic;
using System.Text;

namespace BattleRoyalle.WinService.Services
{
    public interface IControladorDeMensagemNoConsole
    {
        void EscreverMensagemNoConsole(IMensagemNoConsole mensagemNoConsole);
    }
}
