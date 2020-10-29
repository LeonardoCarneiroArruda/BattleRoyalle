import { Injectable } from '@angular/core';
import { MensagemModel } from '../models/mensagem.model';
import { TipoMensagemEnum } from '../enums/tipo-mensagem.enum';
import { BehaviorSubject } from 'rxjs';
import { RegistroModel } from '../models/registro.model';

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {
  private socket: WebSocket;
  private endereco: string = 'localhost';
  private porta: string = '5000';
  private uri: string = 'ws://' + this.endereco + ':' + this.porta + '/ws';
  returnText$: BehaviorSubject<string> = new BehaviorSubject<string>('');
  returnRegistros$: BehaviorSubject<RegistroModel> = new BehaviorSubject<RegistroModel>(null);

  constructor() { }

  connet() {

    this.socket = new WebSocket(this.uri);

    this.socket.addEventListener("open", (event => {
      console.log("opened connection to " + this.uri);
    }));

    this.socket.addEventListener("message", (event => {
      console.log("retorno: " + event.data);
      var message: MensagemModel = JSON.parse(event.data);
      if (message.TipoMensagem == TipoMensagemEnum.MensagemRegistro) {
        this.returnRegistros$.next(message.registro);
      }
      else if (message.TipoMensagem != TipoMensagemEnum.MensagemComando) {
          this.returnText$.next(message.data);
      }
    }));

    this.socket.addEventListener("close", (event => {
      console.log("closed connection from " + this.uri);
    }));

    this.socket.addEventListener("error", (event => {
      console.log("error: " + this.uri);
    }));
  }

  envia(mensagem: MensagemModel) {
    var json = JSON.stringify(mensagem);
    console.log(json);
    this.socket.send(json);
  }


}
