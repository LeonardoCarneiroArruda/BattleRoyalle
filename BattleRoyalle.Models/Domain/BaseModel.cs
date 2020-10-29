using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BattleRoyalle.Models.Domain
{
    public abstract class BaseModel
    {
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
