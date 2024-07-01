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
using WindowsFormsApp1.View;

namespace WindowsFormsApp1
{
    public partial class LoginView : Form
    {
        Form formCadastro = new CadastroView();
        Form formEstoque = new EstoqueView();
        ConexaoDAO con = new ConexaoDAO();
        public LoginView()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string senha = textBox2.Text;
            if (con.LoginUsuario(login, senha) == true)
            {
                formEstoque.Show();
            }
            else
            {
                MessageBox.Show("Erro ao logar!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            formCadastro.Show();
        }
    }
}
