using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class AccesoDatos
    {
        SqlConnection conexion;
        SqlCommand comando;
        SqlDataReader lector;

        public AccesoDatos()
        {
            conexion = new SqlConnection("server= COMPUTADORAHP\\SQLEXPRESS; database= CATALOGO_DB; integrated security= true");
            comando = new SqlCommand();
        }
        public SqlDataReader Lector
        {
            get { return lector; }
        }
        public void SetearConsulta(string consulta) 
        {
            comando.CommandType= System.Data.CommandType.Text;
            comando.CommandText= consulta;
        }
        public void CerrarConexion()
        {
            conexion.Close();
        }
        public void SetearParametro(string parametro, object valor)
        {
            comando.Parameters.AddWithValue(parametro, valor);
        }
        public void EjecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void EjecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
