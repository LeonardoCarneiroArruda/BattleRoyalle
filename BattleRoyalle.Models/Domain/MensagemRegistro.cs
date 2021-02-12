using System;
using System.Collections.Generic;
using System.Text;

namespace BattleRoyalle.Models.Domain
{
    public class MensagemRegistro : IRetornoMensagem
    {
        private string mensagemRegistro { get; }

        public MensagemRegistro(string _mensagem)
        {
            this.mensagemRegistro = _mensagem;
        }

        public string RetornaMensagemParaMostrarNaTela()
        {
            return $">> Registrado: {mensagemRegistro}";
        }
    }
}
