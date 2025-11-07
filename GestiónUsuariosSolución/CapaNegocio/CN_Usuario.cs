using System;
using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objetoCD = new CD_Usuario();
        public Usuario Loguear(string usuarioOEmail, string contraseña)
        {
            if (string.IsNullOrEmpty(usuarioOEmail))
            {
                return null;
            }
            return objetoCD.Loguear(usuarioOEmail, contraseña);
        }
        public List<Usuario> Listar()
        {
            return objetoCD.Listar();
        }
        public string Registrar(Usuario obj)
        {
            if (string.IsNullOrEmpty(obj.NombreUsuario))
            {
                return "Error: El nombre de usuario no puede ser vacío.";
            }
            if (string.IsNullOrEmpty(obj.Contraseña))
            {
                return "Error: La contraseña no puede ser vacía.";
            }
            if (objetoCD.ExisteUsuario(obj.NombreUsuario))
            {
                return "Error: El nombre de usuario ya está en uso. Por favor, elija otro.";
            }
            return objetoCD.Registrar(obj);
        }
        public string Editar(Usuario obj)
        {
            if (string.IsNullOrEmpty(obj.NombreUsuario))
            {
                return "Error: El nombre de usuario no puede ser vacío.";
            }

            return objetoCD.Editar(obj);
        }
        public string Eliminar(int idUsuario)
        {
            return objetoCD.Eliminar(idUsuario);
        }
        public Usuario ObtenerPorEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            return objetoCD.ObtenerPorEmail(email);
        }

    }
}