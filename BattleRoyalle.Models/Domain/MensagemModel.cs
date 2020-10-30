using BattleRoyalle.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleRoyalle.Models.Domain
{
    public class MensagemModel : BaseModel
    {
        public MensagemModel() { }

        public MensagemModel(TipoMensagemEnum _tipoMensagem, string _data)
        {
            this.TipoMensagem = _tipoMensagem;
            this.data = _data;
        }

        public MensagemModel(RegistroModel _registro)
        {
            this.registro = _registro;
            this.TipoMensagem = TipoMensagemEnum.MensagemRegistro;
        }

        public TipoMensagemEnum TipoMensagem { get; set; }
        public string data { get { return _data; } set { _data = value != null ? (value.Length > 3500 ? value.Substring(0, 3500) : value) : ""; } }
        private string _data { get; set; }
        public RegistroModel registro { get; set; }

        public string RetornaMensagemParaMostrarNaTela(IRetornoMensagem _retornoMensagem)
        {
            return _retornoMensagem?.RetornaMensagemParaMostrarNaTela();
        }

    }
}
