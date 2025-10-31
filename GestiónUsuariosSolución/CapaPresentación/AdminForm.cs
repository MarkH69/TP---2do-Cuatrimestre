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
using CapaDatos;

namespace AppUsuarios
{
    public partial class AdminForm : Form
    {
        private List<Usuario> usuarios;
        private Usuario _adminLogueado;

        public AdminForm(List<Usuario> listaUsuarios, Usuario adminLogueado)
        {
            InitializeComponent();
            this.usuarios = listaUsuarios;
            this._adminLogueado = adminLogueado;
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = usuarios.Select(u => new
            {
                u.NombreUsuario,
                u.Email,
                Contraseña = "••••••",
                Nivel = u.EsAdmin ? "Administrador" : "Usuario"
            }).ToList();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = Microsoft.VisualBasic.Interaction.InputBox("Nombre de usuario:", "Nuevo Usuario");
            string email = Microsoft.VisualBasic.Interaction.InputBox("Email:", "Nuevo Usuario");
            string pass = Microsoft.VisualBasic.Interaction.InputBox("Contraseña:", "Nuevo Usuario");
            DialogResult esAdmin = MessageBox.Show("¿Es Administrador?", "Nivel", MessageBoxButtons.YesNo);

            if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(pass))
            {
                usuarios.Add(new Usuario
                {
                    NombreUsuario = nombre,
                    Email = email,
                    Contraseña = pass,
                    EsAdmin = (esAdmin == DialogResult.Yes)
                });

                CargarUsuarios();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccioná un usuario para editar.", "Aviso");
                return;
            }

            string nombreSeleccionado = dgvUsuarios.CurrentRow.Cells[0].Value.ToString();
            Usuario usuarioSeleccionado = usuarios.FirstOrDefault(u => u.NombreUsuario == nombreSeleccionado);

            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("No se pudo encontrar el usuario.", "Error");
                return;
            }

            EditarUsuarioForm formEditar = new EditarUsuarioForm(usuarioSeleccionado);
            if (formEditar.ShowDialog() == DialogResult.OK)
            {
                CargarUsuarios(); 
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione un usuario para eliminar.",
                                "Selección requerida",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string nombreUsuarioSeleccionado = dgvUsuarios.CurrentRow.Cells["NombreUsuario"].Value.ToString();

            if (nombreUsuarioSeleccionado == _adminLogueado.NombreUsuario)
            {
                MessageBox.Show("No puede eliminar su propia cuenta de administrador.",
                                "Acción no permitida",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            DialogResult confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar al usuario '{nombreUsuarioSeleccionado}'? Esta acción no se puede deshacer.", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.No)
            {
                return;
            }

            Usuario usuarioAEliminar = usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuarioSeleccionado);

            if (usuarioAEliminar != null)
            {
                usuarios.Remove(usuarioAEliminar);
                CargarUsuarios();

                MessageBox.Show("Usuario eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
