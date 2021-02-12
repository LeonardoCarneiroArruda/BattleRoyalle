using BattleRoyalle.Models.Domain;
using BattleRoyalle.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Net;
using System.Net.Sockets;

namespace BattleRoyalle.WinService.Services.Handlres
{
    public class RegistroService : IInfoMaquina
    {

        public RegistroModel RegistrarInformacoesMaquina()
        {
            RegistroModel registro = new RegistroModel();
            registro.Nome_Maquina = RetornarNomeMaquina();
            registro.Versao_Windows = RetornarVersaoSO();
            registro.Versao_DotNet = RetornarVersaoDotNet();
            registro.IP = RetornarIPMaquinaLocal();
            registro.Discos = RetornarInformacoesDisco().ToList();
            registro.AntiVirus = RetornarAntiVirus();
            registro.Firewall = RetornarFirewall().ToList();

            return registro;
        }

        public string RetornarAntiVirus()
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

        public ICollection<string> RetornarFirewall()
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

        public string RetornarNomeMaquina() => Environment.MachineName;

        public string RetornarVersaoDotNet() => Environment.Version.ToString();

        public string RetornarVersaoSO() => Environment.OSVersion.VersionString;

        public ICollection<Disco_RigidoModel> RetornarInformacoesDisco()
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

        public string RetornarIPMaquinaLocal()
            => Dns.GetHostEntry(Dns.GetHostName()).AddressList
               .Where(address => address.AddressFamily == AddressFamily.InterNetwork)
               .FirstOrDefault().ToString();
    }
}
