using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos;

namespace AppUsuarios
{
    public partial class EditarUsuarioForm : Form
    {
        private Usuario usuario;

        public EditarUsuarioForm(Usuario usuarioAEditar)
        {
            InitializeComponent();
            usuario = usuarioAEditar;
            CargarDatos();
        }

        private void CargarDatos()
        {
            txtNombre.Text = usuario.NombreUsuario;
            txtEmail.Text = usuario.Email;
            txtPassword.Text = usuario.Contraseña;
            chkEsAdmin.Checked = usuario.EsAdmin;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            usuario.NombreUsuario = txtNombre.Text.Trim();
            usuario.Email = txtEmail.Text.Trim();
            usuario.Contraseña = txtPassword.Text;
            usuario.EsAdmin = chkEsAdmin.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void groupBox1_Resize(object sender, EventArgs e)
        {
            groupBox1.Left = (this.ClientSize.Width - groupBox1.Width) / 2;
            groupBox1.Top = (this.ClientSize.Height - groupBox1.Height) / 2;
        }

        private void EditarUsuarioForm_Load(object sender, EventArgs e)
        {
            groupBox1.Left = (this.ClientSize.Width - groupBox1.Width) / 2;
            groupBox1.Top = (this.ClientSize.Height - groupBox1.Height) / 2;
        }
    }

}
