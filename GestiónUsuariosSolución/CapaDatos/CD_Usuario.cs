using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Usuario
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            using (OleDbConnection oconexion = new OleDbConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT [IdUsuario], [NombreUsuario], [Email], [Contraseña], [EsAdmin], [Activo] FROM [Usuarios]";

                    OleDbCommand cmd = new OleDbCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (OleDbDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                NombreUsuario = dr["NombreUsuario"].ToString(),
                                Email = dr["Email"].ToString(),
                                Contraseña = dr["Contraseña"].ToString(),
                                EsAdmin = Convert.ToBoolean(dr["EsAdmin"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Usuario>();
                    Console.WriteLine("Error al listar usuarios: " + ex.Message);
                }
            }
            return lista;
        }

        public Usuario Loguear(string usuarioOEmail, string contraseña)
        {
            Usuario usuarioEncontrado = null;
            using (OleDbConnection oconexion = new OleDbConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT [IdUsuario], [NombreUsuario], [Email], [EsAdmin], [Activo], [Contraseña] FROM [Usuarios] " +
                                   "WHERE ([NombreUsuario] = ? OR [Email] = ?) AND [Contraseña] = ?";

                    OleDbCommand cmd = new OleDbCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("?", usuarioOEmail);
                    cmd.Parameters.AddWithValue("?", usuarioOEmail);
                    cmd.Parameters.AddWithValue("?", contraseña);

                    oconexion.Open();

                    using (OleDbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuarioEncontrado = new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                NombreUsuario = dr["NombreUsuario"].ToString(),
                                Email = dr["Email"].ToString(),
                                EsAdmin = Convert.ToBoolean(dr["EsAdmin"]),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                Contraseña = dr["Contraseña"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    usuarioEncontrado = null;
                    Console.WriteLine("Error al loguear: " + ex.Message);
                }
            }
            return usuarioEncontrado;
        }

        public string Registrar(Usuario obj)
        {
            string mensajeError = string.Empty;
            using (OleDbConnection oconexion = new OleDbConnection(Conexion.cadena))
            {
                try
                {
                    string query = "INSERT INTO [Usuarios] ([NombreUsuario], [Email], [Contraseña], [EsAdmin], [Activo]) VALUES (?, ?, ?, ?, ?)";

                    OleDbCommand cmd = new OleDbCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("?", obj.NombreUsuario);
                    cmd.Parameters.AddWithValue("?", obj.Email);
                    cmd.Parameters.AddWithValue("?", obj.Contraseña);
                    cmd.Parameters.AddWithValue("?", obj.EsAdmin);
                    cmd.Parameters.AddWithValue("?", true);

                    oconexion.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        mensajeError = "No se pudo insertar el registro. Causa desconocida.";
                    }
                }
                catch (Exception ex)
                {
                    mensajeError = ex.Message;
                }
            }
            return mensajeError;
        }

        public string Editar(Usuario obj)
        {
            string mensajeError = string.Empty;
            using (OleDbConnection oconexion = new OleDbConnection(Conexion.cadena))
            {
                try
                {
                    string query = "UPDATE [Usuarios] SET " +
                                   "[NombreUsuario] = ?, " +
                                   "[Email] = ?, " +
                                   "[Contraseña] = ?, " +
                                   "[EsAdmin] = ?, " +
                                   "[Activo] = ? " +
                                   "WHERE [IdUsuario] = ?";

                    OleDbCommand cmd = new OleDbCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("?", obj.NombreUsuario);
                    cmd.Parameters.AddWithValue("?", obj.Email);
                    cmd.Parameters.AddWithValue("?", obj.Contraseña);
                    cmd.Parameters.AddWithValue("?", obj.EsAdmin);
                    cmd.Parameters.AddWithValue("?", obj.Activo);
                    cmd.Parameters.AddWithValue("?", obj.IdUsuario);

                    oconexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        mensajeError = "No se pudo actualizar el registro. No se encontró el Id.";
                    }
                }
                catch (Exception ex)
                {
                    mensajeError = ex.Message;
                }
            }
            return mensajeError;
        }

        public string Eliminar(int idUsuario)
        {
            string mensajeError = string.Empty;
            using (OleDbConnection oconexion = new OleDbConnection(Conexion.cadena))
            {
                try
                {
                    string query = "DELETE FROM [Usuarios] WHERE [IdUsuario] = ?";

                    OleDbCommand cmd = new OleDbCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("?", idUsuario);

                    oconexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        mensajeError = "No se pudo eliminar el registro. No se encontró el Id.";
                    }
                }
                catch (Exception ex)
                {
                    mensajeError = ex.Message;
                }
            }
            return mensajeError;
        }
        public bool ExisteUsuario(string nombreUsuario)
        {
            bool existe = false;
            using (OleDbConnection oconexion = new OleDbConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT COUNT(1) FROM [Usuarios] WHERE [NombreUsuario] = ?";

                    OleDbCommand cmd = new OleDbCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("?", nombreUsuario);

                    oconexion.Open();

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        existe = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al verificar usuario: " + ex.Message);
                    existe = false;
                }
            }
            return existe;
        }

        public Usuario ObtenerPorEmail(string email)
        {
            Usuario usuarioEncontrado = null;
            using (OleDbConnection oconexion = new OleDbConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT * FROM [Usuarios] WHERE [Email] = ?";

                    OleDbCommand cmd = new OleDbCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("?", email);

                    oconexion.Open();

                    using (OleDbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuarioEncontrado = new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                NombreUsuario = dr["NombreUsuario"].ToString(),
                                Email = dr["Email"].ToString(),
                                EsAdmin = Convert.ToBoolean(dr["EsAdmin"]),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                Contraseña = dr["Contraseña"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    usuarioEncontrado = null;
                    Console.WriteLine("Error al obtener por email: " + ex.Message);
                }
            }
            return usuarioEncontrado;
        }

    }
}