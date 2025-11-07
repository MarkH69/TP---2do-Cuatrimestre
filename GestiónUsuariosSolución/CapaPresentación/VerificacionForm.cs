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
    public partial class VerificacionForm : Form
    {
        private string _codigoCorrecto;
        private Usuario _usuarioVerificado;

        public VerificacionForm(string codigo, Usuario usuario)
        {
            InitializeComponent();
            _codigoCorrecto = codigo;
            _usuarioVerificado = usuario;
        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text == _codigoCorrecto)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("El código de verificación es incorrecto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCodigo.Clear();
            }
        }
    }
}
