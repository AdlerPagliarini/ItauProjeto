using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using ItauProjeto.Models;
using ItauProjeto.Funcoes;

namespace ItauProjeto.CRUD
{
    public class CRUDCliente
    {
        private ConexaoDAO dao;
        private List<SqlParameter> sp;
        private string table = "tCliente";
        private string ID = "idCliente";
        private string OrderBy = " ORDER BY Login";

        private int Inserir(ModelCliente model)
        {
            var strQuery = "";
            int idInserted = 0;
            strQuery += " INSERT INTO " + table + " (Email, Senha) ";
            strQuery += " OUTPUT Inserted.idCliente VALUES (@Email, @Senha) ";

            sp = gerarParametros(model);

            using (dao = new ConexaoDAO())
            {
                var returnDataReader = dao.ExecutarComandoRetornandoID(strQuery, sp);
                idInserted = readerToListObjectIDInserted(returnDataReader);
            }
            return idInserted;
        }
        private void Alterar(ModelCliente model)
        {
            var strQuery = "";
            strQuery += " UPDATE " + table + " SET ";
            strQuery += " Email = @Email ";
            strQuery += " WHERE " + ID + " = @id ";

            sp = gerarParametros(model);

            using (dao = new ConexaoDAO())
            {
                dao.ExecutarComando(strQuery, sp);
            }
        }
        public void AlterarSenha(ModelCliente model)
        {
            var strQuery = "";
            strQuery += " UPDATE " + table + " SET ";
            strQuery += " Email = @Email, ";
            strQuery += " Senha = @Senha ";
            strQuery += " WHERE " + ID + " = @id ";

            List<SqlParameter> sp = new List<SqlParameter>()
            {
                    new SqlParameter() {ParameterName = "@Email", SqlDbType = SqlDbType.NVarChar, Value= model.Email },
                    new SqlParameter() {ParameterName = "@Senha", SqlDbType = SqlDbType.NVarChar, Value= HashMD5.gerarHashMD5(model.Senha) },
                    new SqlParameter() {ParameterName = "@id", SqlDbType = SqlDbType.Int, Value= model.id}
            };

            using (dao = new ConexaoDAO())
            {
                dao.ExecutarComando(strQuery, sp);
            }
        }

        public int Salvar(ModelCliente model)
        {
            int rID = 0;
            if (model.id > 0)
                Alterar(model);
            else
                rID = Inserir(model);

            CRUDClienteDados tempCrud = new CRUDClienteDados();
            tempCrud.Salvar(model, rID);

            return rID;

        }
        public bool Excluir(ModelCliente model)
        {
            var strQuery = string.Format(" DELETE FROM " + table + " WHERE " + ID + " = @id");
            sp = gerarParametroID(model);

            using (dao = new ConexaoDAO())
            {
                try
                {
                    dao.ExecutarComando(strQuery, sp);
                    Diretorio.excluirPasta("upload\\", model.id.ToString());
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public IEnumerable<ModelCliente> ListarTodos()
        {
            var strQuery = " SELECT * FROM " + table + OrderBy;

            using (dao = new ConexaoDAO())
            {
                var returnDataReader = dao.ExecutarComandoDeLeitura(strQuery);
                return readerToListObject(returnDataReader);
            }
        }
        public ModelCliente ListarPorID(string id)
        {
            var strQuery = string.Format(" SELECT * FROM " + table + " WHERE " + ID + " = @id ");
            ModelCliente model = new ModelCliente();
            model.id = Convert.ToInt32(id);
            sp = gerarParametroID(model);

            using (dao = new ConexaoDAO())
            {
                var returnDataReader = dao.ExecutarComandoDeLeitura(strQuery, sp);
                return readerToListObject(returnDataReader).FirstOrDefault();
            }
        }
        public IEnumerable<ModelCliente> ListarBusca(string email)
        {
            var strQuery = string.Format(" SELECT * FROM " + table + " WHERE Email LIKE @Email");

            List<SqlParameter> sp = new List<SqlParameter>()
            {
                    new SqlParameter() {ParameterName = "@Email", SqlDbType = SqlDbType.NVarChar, Value= email + "%"},
            };

            using (dao = new ConexaoDAO())
            {
                var returnDataReader = dao.ExecutarComandoDeLeitura(strQuery, sp);
                return readerToListObject(returnDataReader);
            }
        }
        private List<ModelCliente> readerToListObject(SqlDataReader reader)
        {
            var modelList = new List<ModelCliente>();
            while (reader.Read())
            {
                CRUDClienteDados tempCrud = new CRUDClienteDados();
                ModelClienteDados modelClienteDados = tempCrud.ListarPorID(reader[ID].ToString());
                var tempObject = new ModelCliente()
                {
                    id = int.Parse(reader[ID].ToString()),
                    Email = reader["Email"].ToString(),
                    Senha = reader["Senha"].ToString(),
                    Bairro = modelClienteDados.Bairro,
                    CEP = modelClienteDados.CEP,
                    Cidade = modelClienteDados.Cidade,
                    DataNascimento = modelClienteDados.DataNascimento,
                    Endereco = modelClienteDados.Endereco,
                    Estado = modelClienteDados.Estado,
                    Nome = modelClienteDados.Nome,
                    NomeDoArquivo = modelClienteDados.NomeDoArquivo,
                    Numero = modelClienteDados.Numero
                };
                modelList.Add(tempObject);
            }
            reader.Close();
            return modelList;
        }
        private int readerToListObjectIDInserted(SqlDataReader reader)
        {
            int r = 0;
            while (reader.Read())
            {
                r = int.Parse(reader[ID].ToString());
            }
            reader.Close();
            return r;
        }
        private List<SqlParameter> gerarParametros(ModelCliente model)
        {

            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@Email", SqlDbType = SqlDbType.NVarChar, Value = model.Email}
            };

            if (!string.IsNullOrEmpty(model.Senha))
            {
                sp.Add(new SqlParameter() { ParameterName = "@Senha", SqlDbType = SqlDbType.NVarChar, Value = HashMD5.gerarHashMD5(model.Senha) });
            }

            if (model.id > 0)
            {
                sp.Add(new SqlParameter() { ParameterName = "@id", SqlDbType = SqlDbType.Int, Value = model.id });
            }

            return sp;
        }
        private List<SqlParameter> gerarParametroID(ModelCliente model)
        {
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", SqlDbType = SqlDbType.Int, Value = model.id }
            };

            return sp;
        }
        public ModelCliente validarLoginSenha(string email, string senha)
        {
            var strQuery = string.Format(" SELECT * FROM " + table + " WHERE email = @Email and senha = @Senha");

            string hash = HashMD5.gerarHashMD5(senha);
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                    new SqlParameter() {ParameterName = "@Email", SqlDbType = SqlDbType.NVarChar, Value= email},
                    new SqlParameter() {ParameterName = "@Senha", SqlDbType = SqlDbType.NVarChar, Value = hash},
            };

            using (dao = new ConexaoDAO())
            {
                var returnDataReader = dao.ExecutarComandoDeLeitura(strQuery, sp);
                return readerToListObject(returnDataReader).FirstOrDefault();
            }
        }
        public bool validarDisponibilidadeEmail(string email)
        {
            var strQuery = string.Format(" SELECT * FROM " + table + " WHERE email = @Email");

            List<SqlParameter> sp = new List<SqlParameter>()
            {
                    new SqlParameter() {ParameterName = "@Email", SqlDbType = SqlDbType.NVarChar, Value= email}
            };

            using (dao = new ConexaoDAO())
            {
                var returnDataReader = dao.ExecutarComandoDeLeitura(strQuery, sp);
                if (!(readerToListObject(returnDataReader).ToArray().Length > 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool validarDisponibilidadeEmail(string email, string id)
        {
            var strQuery = string.Format(" SELECT * FROM " + table + " WHERE email = @Email and " + ID + " = @id ");

            List<SqlParameter> sp = new List<SqlParameter>()
            {
                    new SqlParameter() {ParameterName = "@Email", SqlDbType = SqlDbType.NVarChar, Value= email},
                    new SqlParameter() {ParameterName = "@id", SqlDbType = SqlDbType.Int, Value= Convert.ToInt32(id)}
            };

            using (dao = new ConexaoDAO())
            {
                var returnDataReader = dao.ExecutarComandoDeLeitura(strQuery, sp);
                if ((readerToListObject(returnDataReader).ToArray().Length > 0))
                {
                    return true;
                }
                else
                {
                    return validarDisponibilidadeEmail(email);
                    //return false;
                }
            }
        }
        public bool validarSenhaAtual(string senha, string id)
        {
            var strQuery = string.Format(" SELECT * FROM " + table + " WHERE senha = @Senha and " + ID + " = @id ");

            List<SqlParameter> sp = new List<SqlParameter>()
            {
                    new SqlParameter() {ParameterName = "@Senha", SqlDbType = SqlDbType.NVarChar, Value= HashMD5.gerarHashMD5(senha)},
                    new SqlParameter() {ParameterName = "@id", SqlDbType = SqlDbType.Int, Value= Convert.ToInt32(id)}
            };

            using (dao = new ConexaoDAO())
            {
                var returnDataReader = dao.ExecutarComandoDeLeitura(strQuery, sp);
                if ((readerToListObject(returnDataReader).ToArray().Length > 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}