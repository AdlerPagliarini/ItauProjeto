namespace ItauProjeto.Models
{
    public class ViewModelHome
    {
        public ModelCliente modelCliente { get; set; }
        public bool permanecerLogado { get; set; }
        public string respostaLogin { get; set; }

        public ViewModelHome()
        {
            respostaLogin = "";
            permanecerLogado = false;
        }
    }
}
