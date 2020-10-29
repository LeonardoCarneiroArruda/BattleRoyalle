using BattleRoyalle.Models.Services;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace BattleRoyalle.WinService.Services
{
    public class ComandoServices : IComando
    {
        public List<string> Execute(string commandString)
        {
            using (var ps = PowerShell.Create())
            {
                List<string> resultList = new List<string>();

                var results = ps.AddScript(commandString).Invoke();

                foreach (var result in results)
                {
                    resultList.Add(result?.ToString());
                }

                return resultList;
            }
        }
    }
}
