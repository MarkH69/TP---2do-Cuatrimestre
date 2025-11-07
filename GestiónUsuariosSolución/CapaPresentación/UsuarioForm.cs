using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using CapaNegocio;
using CapaEntidad;

namespace AppUsuarios
{
    public partial class UsuarioForm : Form
    {
        private Usuario usuario;

        public UsuarioForm(Usuario usuarioLogueado)
        {
            InitializeComponent();
            usuario = usuarioLogueado;
            CargarDatos();
        }

        private void CargarDatos()
        {
            txtNombre.Text = usuario.NombreUsuario;
            txtEmail.Text = usuario.Email;
            txtPassword.Text = usuario.Contraseña;
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

            string contraseñaIngresada = Interaction.InputBox("Para guardar los cambios, por favor, introduce tu contraseña actual:", "Confirmar Identidad", "");

            if (string.IsNullOrEmpty(contraseñaIngresada))
            {
                return;
            }

            if (contraseñaIngresada == usuario.Contraseña)
            {
                usuario.NombreUsuario = txtNombre.Text.Trim();
                usuario.Email = txtEmail.Text.Trim();
                usuario.Contraseña = txtPassword.Text;

                CN_Usuario cn_usuario = new CN_Usuario();
                string resultado = cn_usuario.Editar(usuario);

                if (string.IsNullOrEmpty(resultado))
                {
                    MessageBox.Show("Datos actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(resultado, "Error al guardar en la BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("La contraseña es incorrecta. Los cambios no se han guardado.", "Verificación Fallida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }

}
