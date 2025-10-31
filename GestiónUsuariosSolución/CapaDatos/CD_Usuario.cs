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

                    string query = "SELECT IdUsuario, Nombres, Apellidos, Correo, Clave, Restablecer, Activo FROM USUARIO";


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
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Restablecer = Convert.ToBoolean(dr["Restablecer"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Usuario>();
                    Console.WriteLine("Error al listar usuarios desde Access: " + ex.Message);
                }
            }
            return lista;
        }
    }
}
