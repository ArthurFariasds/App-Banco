using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static App_Banco.BuscaBanco;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace App_Banco
{
    public partial class Pesquisa : Form
    {
        private int paginaAtual;
        int totalPaginas;
        private int tamanhoPagina = 100;
        private int totalRegistros = 0;
        private DataTable dataTable = new DataTable();

        public Pesquisa()
        {
            InitializeComponent();
            btnLimpar.Enabled = false;
            btnPaginaAnterior.Enabled = false;
            btnProximaPagina.Enabled = false;
            pbSearchNotFound.Visible = false;

            BuscaBanco busca = new BuscaBanco();
            cbStatus.Items.Clear();
            DataTable dt = busca.PreencherComboBox();
            foreach (DataRow dr in dt.Rows)
            {
                cbStatus.Items.Add(dr[0].ToString());
            }
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            paginaAtual = 1;
            BuscaBanco busca = new BuscaBanco();

            if (cbNotPriced.Checked == true)
            {
                txtPrecoMin.Text = "";
                txtPrecoMax.Text = "";
                txtPrecoMin.Enabled = false;
                txtPrecoMax.Enabled = false;

                dataGridView.DataSource = busca.Buscar(txtMarca.Text, cbStatus.Text, "0", "0", paginaAtual, tamanhoPagina);
                totalRegistros = busca.ReturnRegistrosTotal(txtMarca.Text, cbStatus.Text, "0", "0", paginaAtual, tamanhoPagina);
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanhoPagina);

            }
            else
            {
                txtPrecoMin.Enabled = true;
                txtPrecoMax.Enabled = true;
                dataGridView.DataSource = busca.Buscar(txtMarca.Text, cbStatus.Text, txtPrecoMin.Text, txtPrecoMax.Text, paginaAtual, tamanhoPagina);
                totalRegistros = busca.ReturnRegistrosTotal(txtMarca.Text, cbStatus.Text, txtPrecoMin.Text, txtPrecoMax.Text, paginaAtual, tamanhoPagina);
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanhoPagina);

            }


            if (paginaAtual < totalPaginas)
            {
                btnProximaPagina.Enabled = true;
            }

            if (paginaAtual == totalPaginas)
            {
                btnPaginaAnterior.Enabled = false;
                btnProximaPagina.Enabled = false;
            }



            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                string valueString = row.Cells[2].Value.ToString();

                if (decimal.TryParse(valueString, out decimal value))
                {
                    row.Cells[2].Value = value.ToString("C");
                }

                if (valueString == "0")
                {
                    row.Cells[2].Value = "Not Priced";
                }
            }
            if (dataGridView.RowCount == 0)
            {
                dataGridView.Columns.Clear();
                pbSearchNotFound.Visible = true;
                lblPags.Text = "- / -";
            }
            else
            {
                pbSearchNotFound.Visible = false;
                lblPags.Text = $"{paginaAtual} / {totalPaginas}";
            }

            if (txtMarca.Text != "" || cbStatus.Text != "" || txtPrecoMin.Text != "" || txtPrecoMax.Text != "" || cbNotPriced.Checked == true)
            {
                btnLimpar.Enabled = true;
            }

        }


        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtMarca.Text = "";
            txtPrecoMin.Text = "";
            txtPrecoMax.Text = "";
            cbStatus.SelectedIndex = -1;
            btnLimpar.Enabled = false;
            cbNotPriced.Checked = false;

            txtPrecoMin.Enabled = true;
            txtPrecoMax.Enabled = true;
        }

        private void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            if (paginaAtual > 1)
            {
                paginaAtual--;
                CarregarDadosDoBanco();
                btnPaginaAnterior.Enabled = true;
            }

            if (paginaAtual == 1)
            {
                btnPaginaAnterior.Enabled = false;
            }

            if (paginaAtual < totalPaginas)
            {
                btnProximaPagina.Enabled = true;
            }

            lblPags.Text = $"{paginaAtual} / {totalPaginas}";
        }

        private void btnProximaPagina_Click(object sender, EventArgs e)
        {
            BuscaBanco busca = new BuscaBanco();

            if (cbNotPriced.Checked == true)
            {
                totalRegistros = busca.ReturnRegistrosTotal(txtMarca.Text, cbStatus.Text, "0", "0", paginaAtual, tamanhoPagina);
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanhoPagina);
            }
            else
            {
                totalRegistros = busca.ReturnRegistrosTotal(txtMarca.Text, cbStatus.Text, txtPrecoMin.Text, txtPrecoMax.Text, paginaAtual, tamanhoPagina);
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanhoPagina);
            }


            if (paginaAtual < totalPaginas)
            {
                paginaAtual++;
                CarregarDadosDoBanco();
            }
            if (paginaAtual > 1)
            {
                btnPaginaAnterior.Enabled = true;
            }

            if (paginaAtual < totalPaginas)
            {
                btnProximaPagina.Enabled = true;
            }
            else
            {
                btnProximaPagina.Enabled = false;
            }

            lblPags.Text = $"{paginaAtual} / {totalPaginas}";
        }

        private void CarregarDadosDoBanco()
        {
            BuscaBanco busca = new BuscaBanco();
            dataTable = busca.Buscar(txtMarca.Text, cbStatus.Text, txtPrecoMin.Text, txtPrecoMax.Text, paginaAtual, tamanhoPagina);
            dataGridView.DataSource = dataTable;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                string valueString = row.Cells[2].Value.ToString();

                if (decimal.TryParse(valueString, out decimal value))
                {
                    row.Cells[2].Value = value.ToString("C");
                }

                if (valueString == "0")
                {
                    row.Cells[2].Value = "Not Priced";
                }
            }
        }
    }
}
