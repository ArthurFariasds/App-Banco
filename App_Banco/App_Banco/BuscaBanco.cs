using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace App_Banco
{
    public class BuscaBanco
    {
        ConexaoBanco conexao = new ConexaoBanco();

        public DataTable Buscar(string modelo, string status, string precoMin, string precoMax, int numPaginas, int itensQtd)
        {
            DataTable dataTable = new DataTable();

            try
            {
                conexao.conectar();
                SqlCommand command = new SqlCommand("pesquisaTeste", conexao.conectar());
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@modelo", modelo);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@preco1", precoMin);
                command.Parameters.AddWithValue("@preco2", precoMax);
                command.Parameters.AddWithValue("@numPagina", numPaginas);
                command.Parameters.AddWithValue("@itensQtd", itensQtd);

                SqlParameter outputParameter = new SqlParameter("@totalRegistros", SqlDbType.Int);
                outputParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputParameter);

                SqlDataAdapter da = new SqlDataAdapter(command);

                da.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha ao buscar dados! {ex.Message}");
            }
            finally
            {
                conexao.desconectar();
            }

            return dataTable;
        }
        int totalRegistro;
        public int ReturnRegistrosTotal(string modelo, string status, string precoMin, string precoMax, int numPaginas, int itensQtd)
        {
            DataTable dataTable = new DataTable();

            try
            {
                conexao.conectar();
                SqlCommand command = new SqlCommand("pesquisaTeste", conexao.conectar());
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@modelo", modelo);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@preco1", precoMin);
                command.Parameters.AddWithValue("@preco2", precoMax);
                command.Parameters.AddWithValue("@numPagina", numPaginas);
                command.Parameters.AddWithValue("@itensQtd", itensQtd);

                SqlParameter outputParameter = new SqlParameter("@totalRegistros", SqlDbType.Int);
                outputParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputParameter);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dataTable); // Preencher o DataTable

                totalRegistro = Convert.ToInt32(outputParameter.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha ao buscar dados! {ex.Message}");
            }
            finally
            {
                conexao.desconectar();
            }

            return totalRegistro;
        }
        public DataTable PreencherComboBox()
        {
            DataTable dataTable = new DataTable();

            try
            {
                conexao.conectar();
                SqlDataAdapter da = new SqlDataAdapter(null, conexao.conectar());
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_status";
                da.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha ao buscar dados! {ex.Message}");
            }
            finally
            {
                conexao.desconectar();
            }

            return dataTable;
        }
    }
}
