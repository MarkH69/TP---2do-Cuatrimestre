using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
using CapaEntidad;

namespace AppUsuarios
{
    public partial class LoginForm : Form
    {
        private Usuario usuarioEncontrado;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usuarioIngresado = txtUsuario.Text.Trim();
            string contraseñaIngresada = txtPassword.Text;

            
            if (string.IsNullOrWhiteSpace(usuarioIngresado) || string.IsNullOrWhiteSpace(contraseñaIngresada))
            {
                MessageBox.Show("Por favor, completá todos los campos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            CN_Usuario cn_usuario = new CN_Usuario();
            usuarioEncontrado = cn_usuario.Loguear(usuarioIngresado, contraseñaIngresada);

            if (usuarioEncontrado == null)
            {
                
                MessageBox.Show("Usuario, email o contraseña incorrectos.", "Error de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                return;
            }


            if (usuarioEncontrado.EsAdmin)
            {
                AdminForm adminForm = new AdminForm(usuarioEncontrado);

                this.Hide();
                adminForm.ShowDialog();
                txtUsuario.Clear();
                txtPassword.Clear();
                chkMostrarContraseña.Checked = false;
                this.Show();
            }
            else 
            {
                Random random = new Random();
                string codigoVerificacion = random.Next(100000, 999999).ToString();

                MessageBox.Show($"Se ha enviado un código de verificación.\n\nTu código es: {codigoVerificacion}",
                                "Verificación de Dos Pasos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                VerificacionForm formVerificacion = new VerificacionForm(codigoVerificacion, usuarioEncontrado);

                DialogResult resultado = formVerificacion.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    UsuarioForm userForm = new UsuarioForm(usuarioEncontrado);
                    this.Hide();
                    userForm.ShowDialog();

                    txtUsuario.Clear();
                    txtPassword.Clear();
                    chkMostrarContraseña.Checked = false;

                    this.Show();
                }
                else
                {
                    MessageBox.Show("La verificación de dos pasos fue cancelada o falló.",
                                    "Verificación Incompleta",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    txtPassword.Clear();
                }
            }
        }


        private void LoginForm_Load(object sender, EventArgs e)
        {
            groupBox1.Left = (this.ClientSize.Width - groupBox1.Width) / 2;
            groupBox1.Top = (this.ClientSize.Height - groupBox1.Height) / 2;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RecuperarContraseñaForm recuperarForm = new RecuperarContraseñaForm();
            recuperarForm.ShowDialog();
        }

        private void chkMostrarContraseña_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMostrarContraseña.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void LoginForm_Resize(object sender, EventArgs e)
        {
            groupBox1.Left = (this.ClientSize.Width - groupBox1.Width) / 2;
            groupBox1.Top = (this.ClientSize.Height - groupBox1.Height) / 2;
        }
    }
}
