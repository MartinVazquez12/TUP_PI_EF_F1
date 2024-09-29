using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace TUP_PI_EF_F1.Datos
{
    public class AccesoDatos
    {
        private string cadenaConexion = @"Data Source=172.16.10.196;Initial Catalog=Formula1;User ID=alumno1w1;Password=alumno1w1";
        private SqlCommand comando;
        private SqlConnection conexion;

        public AccesoDatos()
        {
            conexion = new SqlConnection(cadenaConexion);
        }

        public void Conectar()
        {
            conexion.Open();
            comando = new SqlCommand();
            comando.Connection = conexion;
            comando.CommandType= CommandType.Text;
        }

        public void Desconectar()
        {
            conexion.Close();
        }

        public DataTable ConsultarBD(string nombreTabla)
        {
            DataTable tabla = new DataTable();
            Conectar();
            comando.CommandText = "SELECT * FROM " + nombreTabla;
            tabla.Load(comando.ExecuteReader());
            Desconectar();
            return tabla;
        }

        public bool InsertarBD(string nombreTabla, List<Parametro> parame) 
        {
            int filasAfectadas = 0;
            Conectar();
            comando.CommandText = "INSERT INTO " + nombreTabla + " VALUES(@numero, @nombre, @pais, @fechanac, @escuderia)";
            
            foreach (Parametro param in parame)
            {
                comando.Parameters.AddWithValue(param.Nombre, param.Valor);
            }

            filasAfectadas = comando.ExecuteNonQuery();
            Desconectar();

            return filasAfectadas == 1;
        }


    }
}
