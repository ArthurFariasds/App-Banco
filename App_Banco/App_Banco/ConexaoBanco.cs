using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace App_Banco
{
    public class ConexaoBanco
    {

        SqlConnection connection = new SqlConnection();

        public ConexaoBanco()
        {
            connection.ConnectionString = @"Data Source=LAPTOP-NPH5PN9H\SQLEXPRESS;Initial Catalog=atividade_n2;Integrated Security=True";
            //connection.ConnectionString = @"Data Source=CE-LAB6338\;Initial Catalog=atividade_n2;User ID=sa;Password=123456";
        }

        public SqlConnection conectar()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            return connection;
        }
        public void desconectar()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
