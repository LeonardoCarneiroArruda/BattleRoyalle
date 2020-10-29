import { Component } from '@angular/core';
import { WebsocketService } from './services/websocket.service';
import { MensagemModel } from './models/mensagem.model';
import { TipoMensagemEnum } from './enums/tipo-mensagem.enum';
import { RegistroModel } from './models/registro.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  retornoTexto: String[] = [];
  retornoRegistros: RegistroModel[] = [];

  constructor(private socketService: WebsocketService) {

    socketService.returnRegistros$.subscribe(reg => {
      if (reg != null)
        this.retornoRegistros.push(reg);
    });

    socketService.returnText$.subscribe(rt => {
      this.retornoTexto.push(rt);
    });
  }

  ngOnInit() {
    this.socketService.connet();
  }

  enviarComando(comando: string) {
    if (comando == 'clear') {
      //this.retornoTexto. = '';
    }
    else {
      var mensagem = new MensagemModel();
      mensagem.TipoMensagem = TipoMensagemEnum.MensagemComando;
      mensagem.data = comando;
      this.retornoTexto.push('>> ' + comando + '\n');
    }

    this.socketService.envia(mensagem);
  }


}
