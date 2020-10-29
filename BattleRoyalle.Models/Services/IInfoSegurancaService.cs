using System;
using System.Collections.Generic;
using System.Text;

namespace BattleRoyalle.Models.Services
{
    public interface IInfoSegurancaService
    {
        List<string> retornarFirewall();
        string retornarAntiVirus();
    }
}
