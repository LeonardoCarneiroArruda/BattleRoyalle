using BattleRoyalle.Models;
using BattleRoyalle.Models.Domain;
using BattleRoyalle.Models.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BattleRoyalle.WinService.Services
{
    public class RegistroService : IInfoSegurancaService, IInfoMaquinaService
    {

        public RegistroModel RegistrarInformacoesMaquina()
        {
            RegistroModel registro = new RegistroModel();
            registro.Nome_Maquina = retornarNomeMaquina();
            registro.Versao_Windows = retornarVersaoSO();
            registro.Versao_DotNet = retornarVersaoDotNet();
            registro.IP = retornarIPMaquinaLocal();
            registro.Discos = retornarInformacoesDisco();
            registro.AntiVirus = retornarAntiVirus();
            registro.Firewall = retornarFirewall();

            return registro;
        }

        public string retornarAntiVirus()
        {
            int majorCodeWindowsXP = 5;
            string wmiNamespace = Environment.OSVersion.Version.Major > majorCodeWindowsXP
                ? "SecurityCenter2" : "SecurityCenter";
            string wmiPath = @"\\" + Environment.MachineName + @"\root\" + wmiNamespace;

            string antivirus = string.Empty;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiPath,
                    "SELECT * FROM AntivirusProduct");
                ManagementObjectCollection antivirusProduct = searcher.Get();
                foreach (ManagementObject product in antivirusProduct)
                    antivirus = product["displayName"].ToString();
            }
            catch
            {
                antivirus = "Não detectado.";
            }
            return antivirus;
        }

        public List<string> retornarFirewall()
        {
            List<string> firewallStatusList = new List<string>();

            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript("powershell { Get-NetFirewallProfile -Profile Domain, Public, Private | Select-Object Name, Enabled }");
                var psObjectList = powerShell.Invoke().ToList();
                foreach (var psObject in psObjectList)
                {
                    string firewallStatus = string.Empty;
                    var firewallArray = psObject.ToString().Split(";");
                    firewallStatus += $"{firewallArray[0].Substring(7)}: ";
                    firewallStatus += (firewallArray[1].Substring(9) == "True}" ? "Habilitado" : "Desabilitado");

                    firewallStatusList.Add(firewallStatus);
                }
            }
            return firewallStatusList;

        }

        public string retornarNomeMaquina() => Environment.MachineName;

        public string retornarVersaoDotNet() => Environment.Version.ToString();

        public string retornarVersaoSO() => Environment.OSVersion.VersionString;

        public List<Disco_RigidoModel> retornarInformacoesDisco()
        {
            List<Disco_RigidoModel> lista = new List<Disco_RigidoModel>();
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Fixed)
                {
                    Disco_RigidoModel disk = new Disco_RigidoModel();
                    disk.Nome_Disco = drive.Name;
                    disk.Memoria_Disponivel = drive.AvailableFreeSpace;
                    disk.Memoria_Total = drive.TotalSize;
                    lista.Add(disk);
                }
            }
            return lista;
        }

        public string retornarIPMaquinaLocal()
            => Dns.GetHostEntry(Dns.GetHostName()).AddressList
               .Where(address => address.AddressFamily == AddressFamily.InterNetwork)
               .FirstOrDefault().ToString();
    }
}
