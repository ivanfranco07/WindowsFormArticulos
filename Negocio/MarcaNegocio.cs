using Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MarcaNegocio
    {
        List<Marca> lista = new List<Marca>();
        public List<Marca> listarMarcas()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Id, Descripcion from Marca;";
                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Marca m = new Marca();
                    m.Id = (int)datos.Lector["Id"];
                    m.Descripcion = (string)datos.Lector["Descripcion"];
                    lista.Add(m);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            datos.CerrarConexion();
            return lista;
        }
    }
}
