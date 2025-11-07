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
    public partial class AdminForm : Form
    {
        private List<Usuario> usuarios;
        private Usuario _adminLogueado;

        public AdminForm(Usuario adminLogueado)
        {
            InitializeComponent();
            this._adminLogueado = adminLogueado;
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            CN_Usuario cn_usuario = new CN_Usuario();
            this.usuarios = cn_usuario.Listar();

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
            DialogResult esAdminResult = MessageBox.Show("¿Es Administrador?", "Nivel", MessageBoxButtons.YesNo);

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Debe completar todos los campos.");
                return;
            }

            Usuario nuevoUsuario = new Usuario()
            {
                NombreUsuario = nombre,
                Email = email,
                Contraseña = pass,
                EsAdmin = (esAdminResult == DialogResult.Yes)
            };

            CN_Usuario cn_usuario = new CN_Usuario();

            string resultado = cn_usuario.Registrar(nuevoUsuario);

            if (string.IsNullOrEmpty(resultado))
            {
                MessageBox.Show("¡Usuario creado exitosamente!");
                CargarUsuarios();
            }
            else
            {
                MessageBox.Show(resultado, "Error al crear el usuario");
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
                MessageBox.Show("Por favor, seleccione un usuario para eliminar.");
                return;
            }

            string nombreUsuarioSeleccionado = dgvUsuarios.CurrentRow.Cells["NombreUsuario"].Value.ToString();
            Usuario usuarioAEliminar = usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuarioSeleccionado);

            if (usuarioAEliminar == null)
            {
                MessageBox.Show("Error: No se pudo encontrar el usuario en la lista.");
                return;
            }

            if (usuarioAEliminar.IdUsuario == _adminLogueado.IdUsuario)
            {
                MessageBox.Show("No puede eliminar su propia cuenta de administrador.");
                return;
            }

            DialogResult confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar al usuario '{nombreUsuarioSeleccionado}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.Yes)
            {
                CN_Usuario cn_usuario = new CN_Usuario();
                string resultado = cn_usuario.Eliminar(usuarioAEliminar.IdUsuario);

                if (string.IsNullOrEmpty(resultado))
                {
                    MessageBox.Show("Usuario eliminado exitosamente.");
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show(resultado, "Error al eliminar");
                }
            }
        }
        private void AdminForm_Load(object sender, EventArgs e)
        {

        }
    }
}

