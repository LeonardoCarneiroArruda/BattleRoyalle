import { TipoMensagemEnum } from "../enums/tipo-mensagem.enum";
import { RegistroModel } from "./registro.model";

export class MensagemModel {

  TipoMensagem: TipoMensagemEnum;
  data: string;
  registro: RegistroModel;
}
