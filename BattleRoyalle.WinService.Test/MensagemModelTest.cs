using BattleRoyalle.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BattleRoyalle.WinService.Test
{
    public class MensagemModelTest
    {

        [Fact]
        public void deve_RetornarMensagemParaMostrarNaTela_QuandoChamadoPelaMensagemModel()
        {
            //arranje
            MensagemConectado conectado = new MensagemConectado("CONECTADO");
            MensagemModel mensagem1 = new MensagemModel();

            MensagemRegistro registro = new MensagemRegistro("REGISTRADO");
            MensagemModel mensagem2 = new MensagemModel();

            //act
            string retorno1 = mensagem1.RetornaMensagemParaMostrarNaTela(conectado);
            string retorno2 = mensagem2.RetornaMensagemParaMostrarNaTela(registro);

            //assert
            Assert.Equal("CONECTADO", retorno1);
            Assert.Equal("REGISTRADO", retorno2);
        }

    }
}
