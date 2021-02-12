namespace BattleRoyalle.WinService.Services.Handlres
{
    public class ControladorDeMensagemNoConsole : IControladorDeMensagemNoConsole
    {

        public void EscreverMensagemNoConsole(IMensagemNoConsole mensagemNoConsole)
        {
            mensagemNoConsole.Escrever();
        }

    }
}
