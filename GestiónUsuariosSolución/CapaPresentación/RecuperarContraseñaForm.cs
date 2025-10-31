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
    public partial class RecuperarContraseñaForm : Form
    {
        private List<Usuario> usuarios;
        private Usuario usuarioActual;
        private string codigoGenerado;

        public RecuperarContraseñaForm(List<Usuario> listaUsuarios)
        {
            InitializeComponent();
            usuarios = listaUsuarios;
            txtCodigo.Enabled = false;
            txtNuevaPass.Enabled = false;
            txtConfirmar.Enabled = false;
            btnGuardar.Enabled = false;
        }

        private void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            usuarioActual = usuarios.FirstOrDefault(u => u.Email == email);

            if (usuarioActual == null)
            {
                MessageBox.Show("El correo no está registrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Random rand = new Random();
            codigoGenerado = rand.Next(100000, 999999).ToString();
            MessageBox.Show($"Código de verificación: {codigoGenerado}", "Simulación de envío", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtCodigo.Enabled = true;
            txtNuevaPass.Enabled = true;
            txtConfirmar.Enabled = true;
            btnGuardar.Enabled = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text.Trim() != codigoGenerado)
            {
                MessageBox.Show("Código incorrecto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNuevaPass.Text) || string.IsNullOrWhiteSpace(txtConfirmar.Text))
            {
                MessageBox.Show("Completá ambos campos de contraseña.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtNuevaPass.Text != txtConfirmar.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            usuarioActual.Contraseña = txtNuevaPass.Text;
            MessageBox.Show("Contraseña actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }

}
