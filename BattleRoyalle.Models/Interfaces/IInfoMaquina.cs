using BattleRoyalle.Models;
using BattleRoyalle.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleRoyalle.Models.Services
{
    public interface IInfoMaquina
    {
        RegistroModel RegistrarInformacoesMaquina();
        string RetornarNomeMaquina();
        string RetornarVersaoSO();
        string RetornarVersaoDotNet();
        ICollection<Disco_RigidoModel> RetornarInformacoesDisco();
        string RetornarIPMaquinaLocal();
        ICollection<string> RetornarFirewall();
        string RetornarAntiVirus();
    }
}
