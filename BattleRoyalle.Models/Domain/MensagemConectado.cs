using System;
using System.Collections.Generic;
using System.Text;

namespace BattleRoyalle.Models.Domain
{
    public class MensagemConectado : IRetornoMensagem
    {
        private string mensagemConectado { get; }

        public MensagemConectado(string _conectado)
        {
            this.mensagemConectado= _conectado;
        }

        public string RetornaMensagemParaMostrarNaTela()
        {
            return $">> {mensagemConectado}";
        }
    }
}
