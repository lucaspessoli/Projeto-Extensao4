using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1.View
{
    public partial class CadastroView : Form
    {
        ConexaoDAO con = new ConexaoDAO();
        Form formEstoque = new EstoqueView();
        public CadastroView()
        {
            InitializeComponent();
            Form formEstoque  = new EstoqueView();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Usuario usuario = new Usuario();
            usuario.nomeUsuario = textBox1.Text;
            usuario.senhaUsuario = textBox2.Text;
            if(con.CadastrarUsuario(usuario) == true)
            {
                MessageBox.Show("Usuario cadastrado!");
                formEstoque.Show();
            }
            else
            {
                MessageBox.Show("Erro ao se cadastrar!");
            }
        }
    }
}
