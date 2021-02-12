using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleRoyalle.Models.Interfaces;
using BattleRoyalle.WinService.Services;
using BattleRoyalle.WinService.Services.Handlres;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BattleRoyalle.WinService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IInfoMaquina, RegistroService>();
                    services.AddTransient<IClientWebSocketSend, ClientWebSocketSend>();
                    services.AddTransient<IControladorDeMensagemNoConsole, ControladorDeMensagemNoConsole>();
                    
                    services.AddHostedService<Worker>();
                });
    }
}
