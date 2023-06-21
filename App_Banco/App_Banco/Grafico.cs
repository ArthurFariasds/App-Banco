using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace App_Banco
{
    public partial class Grafico : Form
    {
        ConexaoBanco conexao = new ConexaoBanco();
        SqlCommand cmd;
        SqlDataReader dr;
        public Grafico()
        {
            InitializeComponent();
            Graphic();
        }

        ArrayList Producto = new ArrayList();
        ArrayList Cant = new ArrayList();
        private void Graphic()
        {
            try
            {
                cmd = new SqlCommand("sp_Grafico", conexao.conectar());
                cmd.CommandType = CommandType.StoredProcedure;
                conexao.conectar();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    SqlMoney valorSqlMoney = dr.GetSqlMoney(1);
                    double valorDouble = Convert.ToDouble(valorSqlMoney.Value);
                    Producto.Add(dr.GetString(0));
                    Cant.Add(valorDouble);
                }
                chart.Series[0].Points.DataBindXY(Producto, Cant);
                chart.Series[0].LabelFormat = "C2";
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha ao buscar dados! {ex.Message}");
            }
            finally
            {
                conexao.desconectar();
            }
        }
    }
}
