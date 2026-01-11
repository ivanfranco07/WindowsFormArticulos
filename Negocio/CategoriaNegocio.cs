using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class CategoriaNegocio
    {
        List<Categoria> lista= new List<Categoria>();

        public List<Categoria> listarCategorias()
        {
            AccesoDatos datos= new AccesoDatos();
            try
            {
                string consulta = "Select Id, Descripcion from Categoria;";
                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();
                while (datos.Lector.Read()) 
                {
                    Categoria c= new Categoria();
                    c.Id = (int)datos.Lector["Id"];
                    c.Descripcion= (string)datos.Lector ["Descripcion"];
                    lista.Add(c);
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
