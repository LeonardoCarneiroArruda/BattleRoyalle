﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BattleRoyalle.WinService.Services
{
    public interface IComando
    {
        List<string> Execute(string commandString);
    }
}
