using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CapaDatos
{
    public class Conexion
    {
        public static string cadena = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\App_Data\GestionUsuarios.accdb";
    }
}
