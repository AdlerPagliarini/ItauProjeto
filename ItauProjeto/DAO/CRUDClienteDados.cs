using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using ItauProjeto.Models;
using ItauProjeto.Funcoes;

namespace ItauProjeto.CRUD
{
    public class CRUDClienteDados
    {
        private ConexaoDAO dao;
        private List<SqlParameter> sp;
        private string table = "tClienteDados";
        private string ID = "idClienteDados";
        private string OrderBy = " ORDER BY Nome";

        private void Inserir(ModelClienteDados model)
        {
            var strQuery = "";
            strQuery += " INSERT INTO " + table + " (idClienteDados, Nome, DataNascimento, CEP, Estado, Cidade, Bairro, Endereco, Numero, ComprovanteEndereco) ";
            strQuery += " VALUES (@idClienteDados, @Nome, @DataNascimento, @CEP, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @ComprovanteEndereco) ";

            sp = gerarParametros(model);

            using (dao = new ConexaoDAO())
            {
                dao.ExecutarComando(strQuery, sp);
            }
        }

        private void Alterar(ModelClienteDados model)
        {
            var strQuery = "";
            strQuery += " UPDATE " + table + " SET ";
            strQuery += " Nome = @Nome, ";
            strQuery += " DataNascimento = @DataNascimento, ";
            strQuery += " CEP = @CEP, ";
            strQuery += " Estado = @Estado, ";
            strQuery += " Cidade = @Cidade, ";
            strQuery += " Bairro = @Bairro, ";
            strQuery += " Endereco = @Endereco, ";
            strQuery += " Numero = @Numero, ";
            strQuery += " ComprovanteEndereco = @ComprovanteEndereco, ";
            strQuery = strQuery.Substring(0, strQuery.Length - 2);
            strQuery += " WHERE " + ID + " = @idClienteDados ";

            sp = gerarParametros(model);

            using (dao = new ConexaoDAO())
            {
                dao.ExecutarComando(strQuery, sp);
            }
        }

        public void Salvar(ModelClienteDados model, int id)
        {
            if (model.id > 0)
                Alterar(model);
            else {
                model.id = id;
                Inserir(model);
            }
        }

        public bool Excluir(ModelClienteDados model)
        {
            var strQuery = string.Format(" DELETE FROM " + table + " WHERE " + ID + " = @id");
            sp = gerarParametroID(model);

            using (dao = new ConexaoDAO())
            {
                try
                {
                    dao.ExecutarComando(strQuery, sp);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public IEnumerable<ModelClienteDados> ListarTodos()
        {
            var strQuery = " SELECT * FROM " + table + OrderBy;

            using (dao = new ConexaoDAO())
            {
                var returnDataReader = dao.ExecutarComandoDeLeitura(strQuery);
                return readerToListObject(returnDataReader);
            }
        }

        public ModelClienteDados ListarPorID(string id)
        {
            var strQuery = string.Format(" SELECT * FROM " + table + " WHERE " + ID + " = @idClienteDados ");
            ModelClienteDados model = new ModelClienteDados();
            model.id = Convert.ToInt32(id);
            sp = gerarParametroID(model);

            using (dao = new ConexaoDAO())
            {
                var returnDataReader = dao.ExecutarComandoDeLeitura(strQuery, sp);
                return readerToListObject(returnDataReader).FirstOrDefault();
            }
        }

        public IEnumerable<ModelClienteDados> ListarBusca(string nome)
        {
            var strQuery = string.Format(" SELECT * FROM " + table + " WHERE Nome LIKE @Nome ");

            List<SqlParameter> sp = new List<SqlParameter>()
            {
                    new SqlParameter() {ParameterName = "@Nome", SqlDbType = SqlDbType.NVarChar, Value= nome + "%"}
            };

            using (dao = new ConexaoDAO())
            {
                var returnDataReader = dao.ExecutarComandoDeLeitura(strQuery, sp);
                return readerToListObject(returnDataReader);
            }
        }

        private List<ModelClienteDados> readerToListObject(SqlDataReader reader)
        {
            var modelList = new List<ModelClienteDados>();
            while (reader.Read())
            {
                var tempObject = new ModelClienteDados()
                {
                    id = int.Parse(reader[ID].ToString()),
                    Nome = reader["Nome"].ToString(),
                    DataNascimento = DateTime.Parse(reader["DataNascimento"].ToString()),
                    CEP = reader["CEP"].ToString(),
                    Estado = reader["Estado"].ToString(),
                    Cidade = reader["Cidade"].ToString(),
                    Bairro = reader["Bairro"].ToString(),
                    Endereco = reader["Endereco"].ToString(),
                    Numero = reader["Numero"].ToString(),
                    NomeDoArquivo = reader["ComprovanteEndereco"].ToString()
                };
                modelList.Add(tempObject);
            }
            reader.Close();
            return modelList;
        }

        private List<SqlParameter> gerarParametros(ModelClienteDados model)
        {

            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@Nome", SqlDbType = SqlDbType.NVarChar, Value= model.Nome},
                new SqlParameter() {ParameterName = "@DataNascimento", SqlDbType = SqlDbType.SmallDateTime, Value= model.DataNascimento},
                new SqlParameter() {ParameterName = "@CEP", SqlDbType = SqlDbType.NVarChar, Value= model.CEP},
                new SqlParameter() {ParameterName = "@Estado", SqlDbType = SqlDbType.NVarChar, Value= model.Estado},
                new SqlParameter() {ParameterName = "@Cidade", SqlDbType = SqlDbType.NVarChar, Value= model.Cidade},
                new SqlParameter() {ParameterName = "@Bairro", SqlDbType = SqlDbType.NVarChar, Value= model.Bairro},
                new SqlParameter() {ParameterName = "@Endereco", SqlDbType = SqlDbType.NVarChar, Value= model.Endereco},
                new SqlParameter() {ParameterName = "@Numero", SqlDbType = SqlDbType.NVarChar, Value= model.Numero},
                new SqlParameter() {ParameterName = "@ComprovanteEndereco", SqlDbType = SqlDbType.NVarChar, Value= model.NomeDoArquivo},
                new SqlParameter() { ParameterName = "@idClienteDados", SqlDbType = SqlDbType.Int, Value = model.id }
            };

            return sp;
        }

        private List<SqlParameter> gerarParametroID(ModelClienteDados model)
        {
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@idClienteDados", SqlDbType = SqlDbType.Int, Value = model.id }
            };

            return sp;
        }

    }
}