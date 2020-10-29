using BattleRoyalle.Models;
using BattleRoyalle.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleRoyalle.Models.Services
{
    public interface IInfoMaquinaService
    {
        string retornarNomeMaquina();
        string retornarVersaoSO();
        string retornarVersaoDotNet();
        List<Disco_RigidoModel> retornarInformacoesDisco();
        string retornarIPMaquinaLocal();
    }
}
