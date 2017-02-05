using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ItauProjeto.CRUD
{
    public class ConexaoDAO : IDisposable
    {
        private readonly SqlConnection minhaConexao;

        public ConexaoDAO()
        {
            minhaConexao = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
            minhaConexao.Open();
        }

        public void ExeutarComando(string query)//insert,update,delete
        {
            var cmdCommand = new SqlCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = minhaConexao
            };
            cmdCommand.ExecuteNonQuery();
        }

        public void ExecutarComando(string query, List<SqlParameter> SQLparam)//insert,update,delete, with param
        {
            var cmdCommand = new SqlCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = minhaConexao
            };
            cmdCommand.Parameters.AddRange(SQLparam.ToArray());
            try
            {
                cmdCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader ExecutarComandoRetornandoID(string query, List<SqlParameter> SQLparam)//insert,update,delete, with param
        {
            var cmdCommand = new SqlCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = minhaConexao
            };
            cmdCommand.Parameters.AddRange(SQLparam.ToArray());
            return cmdCommand.ExecuteReader();
        }

        public SqlDataReader ExecutarComandoDeLeitura(string query)//select
        {
            var cmdCommand = new SqlCommand(query, minhaConexao);
            return cmdCommand.ExecuteReader();
        }
        public SqlDataReader ExecutarComandoDeLeitura(string query, List<SqlParameter> SQLparam)//select, with param
        {
            var cmdCommand = new SqlCommand(query, minhaConexao);
            cmdCommand.Parameters.AddRange(SQLparam.ToArray());
            return cmdCommand.ExecuteReader();
        }

        // para sempre fechar a conexão
        public void Dispose()
        {
            if (minhaConexao.State == ConnectionState.Open)
            {
                minhaConexao.Close();
            }
        }
    }
}