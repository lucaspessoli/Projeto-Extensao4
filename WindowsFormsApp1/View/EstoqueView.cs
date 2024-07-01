using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WindowsFormsApp1.DAO;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1.View
{
    public partial class EstoqueView : Form
    {
        ConexaoDAO con = new ConexaoDAO();

        public EstoqueView()
        {
            InitializeComponent();
        }

        private void EstoqueView_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Produto produto = new Produto();
            produto.nomeProduto = textBox1.Text;
            produto.quantidadeProduto = int.Parse(textBox2.Text);
            produto.valorProduto = Double.Parse(textBox3.Text);
            if(con.InserirProduto(produto) == true)
            {
                MessageBox.Show("produto inserido!");
            }
            else
            {
                MessageBox.Show("Erro ao inserir produto");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Produto produto = new Produto();
            produto.id = int.Parse(textBox4.Text);
            produto.quantidadeProduto = int.Parse(textBox5.Text);
            if(con.RemoverQuantidadeProduto(produto.id, produto.quantidadeProduto) == true)
            {
                MessageBox.Show("Quantidade removida do produto!");
            }
            else
            {
                MessageBox.Show("Erro ao remover quantidade");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int idProduto = int.Parse(textBox6.Text);
            if(con.DeletarProdutoPorId(idProduto) == true)
            {
                MessageBox.Show("Produto deletado!");
            }
            else
            {
                MessageBox.Show("erro ao deletar produto!");
            }
        }
    }
}
