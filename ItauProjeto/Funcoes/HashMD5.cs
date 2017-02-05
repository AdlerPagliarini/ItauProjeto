using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ItauProjeto.Funcoes
{
    public class HashMD5
    {
        public static string gerarHashMD5(string valor)
        {
            return string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(valor)).Select(s => s.ToString("x2")));
        }
        public static string CriarSenha(int tamanhoDaSenha)
        {
            const string valida = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < tamanhoDaSenha--)
            {
                res.Append(valida[rnd.Next(valida.Length)]);
            }
            return res.ToString();
        }
    }
}