using System;
using System.Collections.Generic;
using System.Text;

namespace BattleRoyalle.Models.Domain
{
    public class Disco_RigidoModel
    {
        public string Nome_Disco { get; set; }
        public long Memoria_Disponivel { get; set; }
        public long Memoria_Total { get; set; }
        public long Memoria_Utilizada { get { return Memoria_Total - Memoria_Disponivel; } }
    }
}
