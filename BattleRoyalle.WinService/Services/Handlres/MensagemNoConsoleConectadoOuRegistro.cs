using BattleRoyalle.Models.Domain;
using System;
using System.Threading.Tasks;

namespace BattleRoyalle.WinService.Services.Handlres
{
    public class MensagemNoConsoleConectadoOuRegistro : IMensagemNoConsole
    {
        private MensagemModel _mensagem { get; set; }

        public MensagemNoConsoleConectadoOuRegistro(MensagemModel mensagem)
        {
            this._mensagem = mensagem;
        }

        public async Task Escrever()
        {
            escreverNaTelaSeNaoForComando();
        }

        private void escreverNaTelaSeNaoForComando()
        {
            IRetornoMensagem retornoMensagem = null;
            switch (_mensagem.TipoMensagem)
            {
                case Models.Enum.TipoMensagemEnum.MensagemConectado:
                    retornoMensagem = new MensagemConectado(_mensagem.data);
                    break;
                case Models.Enum.TipoMensagemEnum.MensagemRegistro:
                    retornoMensagem = new MensagemRegistro(_mensagem.registro.ToString());
                    break;
            }

            Console.WriteLine(_mensagem.RetornaMensagemParaMostrarNaTela(retornoMensagem));
        }
    }
}
