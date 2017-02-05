using System.IO;
using System.Web;

namespace ItauProjeto.Funcoes
{
    public class Diretorio
    {
        public static bool criarPasta(string caminho, string pasta)
        {
            try
            {
                //verifica se a pasta ainda não existe
                if (!Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + caminho + pasta))
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + caminho + pasta);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool excluirPasta(string caminho, string pasta)
        {
            try
            {
                //verifica se a pasta existe antes de excluir. 
                if (Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + caminho + pasta))
                {
                    //excluir a pasta e o conteudo interno
                    Directory.Delete(HttpContext.Current.Request.PhysicalApplicationPath + caminho + pasta, true);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}