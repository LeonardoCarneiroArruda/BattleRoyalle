using BattleRoyalle.Models;
using BattleRoyalle.Models.Domain;
using BattleRoyalle.WinService.Services;
using System;
using Xunit;

namespace BattleRoyalle.WinService.Test
{
    public class RegistroServiceTest
    {
        [Fact]
        public void deve_MontarObjetoDeRegistroDaMaquina_QuandoNecessario()
        {
            //arrange
            RegistroService service = new RegistroService();
            
            //act
            RegistroModel registro = service.RegistrarInformacoesMaquina();

            //asserts
            Assert.Equal("LEONARDO", registro.Nome_Maquina);
            Assert.Equal("192.168.0.4", registro.IP);
            Assert.Equal("Windows Defender", registro.AntiVirus);
            Assert.Equal(2, registro.Discos.Count);
            Assert.Equal("Microsoft Windows NT 10.0.19041.0", registro.Versao_Windows);
            Assert.Equal("3.1.8", registro.Versao_DotNet);
            Assert.Equal(3, registro.Firewall.Count);
           
        }
    }
}
