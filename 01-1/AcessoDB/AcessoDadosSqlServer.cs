using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Classes que manipulam dados
using System.Data; //Organiza classes que trabalham com dados
using System.Data.SqlClient; //Classes específicas para trabalhar com SQL Server
using AcessoDB.Properties; //Para utilizar settings criadas para conexão

namespace AcessoDB

{
    public class AcessoDadosSqlServer
    {
        //Cria conexão
        private SqlConnection CriarConexão()
        {
            return new SqlConnection(Settings.Default.stringConexão);
        }

        //Parâmetros que vão para banco
        private SqlParameterCollection sqlParameterCollection = new SqlCommand().Parameters;
            
        public void LimparParametros()
        {
            sqlParameterCollection.Clear();
        }

        public void AdicionarParametros(string nomeParametro, object valorParametro)
        {
            sqlParameterCollection.Add(new SqlParameter(nomeParametro, valorParametro));
        }

        //Persistência - Inserer, Alterar, Excluir
        public object ExecutarManipulação(CommandType commandType, string nomeStoredProcedureOuTextoSql)
        {
            try
            {
                //Cria Conexão
                SqlConnection sqlConnection = CriarConexão();

                //Abrir Conexão
                sqlConnection.Open();

                //Cria Comando que vai levar a info para o banco
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                //Colocando as coisas dentro do comando (dentro da caixa que vai trafegar na conexão)
                sqlCommand.CommandType = commandType;
                sqlCommand.CommandText = nomeStoredProcedureOuTextoSql;
                sqlCommand.CommandTimeout = 7200; // Em segundos / padrão = 30

                //Adicionar os parâmetros no comando
                foreach (SqlParameter sqlParameter in sqlParameterCollection)
                    sqlCommand.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));

                //Executar o comando, ou seja, mandar o comando ir até o banco de dados
                return sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Consultar registros do banco de dados
        public DataTable ExecutarConsulta(CommandType commandType, string nomeStoredProcedureOuTextoSql)
        {
            try
            {
                //Cria Conexão
                SqlConnection sqlConnection = CriarConexão();

                //Abrir Conexão
                sqlConnection.Open();

                //Cria Comando que vai levar a info para o banco
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                //Colocando as coisas dentro do comando (dentro da caixa que vai trafegar na conexão)
                sqlCommand.CommandType = commandType;
                sqlCommand.CommandText = nomeStoredProcedureOuTextoSql;
                sqlCommand.CommandTimeout = 7200; // Em segundos / padrão = 30

                //Adicionar os parâmetros no comando
                foreach (SqlParameter sqlParameter in sqlParameterCollection)
                    sqlCommand.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));

                //Criar um adaptador
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                //DataTable vazia onde serão colocadas as infos
                DataTable dataTable = new DataTable();

                //Mandar o comando ir até o banco buscar os dados e o adaptador preencher o datatable
                sqlDataAdapter.Fill(dataTable);

                return dataTable;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
