using System;
using System.Collections.Generic;

namespace BattleRoyalle.Models.Domain
{
    public class RegistroModel : BaseModel
    {
        public string Nome_Maquina { get; set; }
        public string IP { get; set; }
        public string AntiVirus { get; set; }
        public List<string> Firewall { get; set; }
        public string Versao_Windows { get; set; }
        public string Versao_DotNet { get; set; }
        public List<Disco_RigidoModel> Discos { get; set; }

    }
}
