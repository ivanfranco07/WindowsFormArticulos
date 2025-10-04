using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listarArticulos()
        {
            List<Articulo> listaArticulos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            string consulta = "select a.Id, a.Codigo, a.Nombre, a.Descripcion, m.Descripcion Marca, c.Descripcion Categoria, a.ImagenUrl, a.Precio, a.IdMarca, a.IdCategoria from ARTICULOS a\r\ninner join Categorias c\r\non a.IdCategoria=c.Id\r\ninner join Marcas m\r\non a.IdMarca= m.Id;";
            datos.SetearConsulta(consulta);

            try
            {
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo auxiliar = new Articulo();
                    auxiliar.Id = (int)datos.Lector["Id"];
                    if(!(datos.Lector["Codigo"] is DBNull))
                        auxiliar.Codigo = (string)datos.Lector["Codigo"];
                    if(!(datos.Lector["Nombre"] is DBNull))
                        auxiliar.Nombre = (string)datos.Lector["Nombre"];
                    if(!(datos.Lector["Descripcion"] is DBNull))
                        auxiliar.Descripcion = (string)datos.Lector["Descripcion"];
                    if(!(datos.Lector["Marca"] is DBNull))
                    {
                        auxiliar.IdMarca = new Marca();
                        auxiliar.IdMarca.Id = (int)datos.Lector["IdMarca"];
                        auxiliar.IdMarca.Descripcion = (string)datos.Lector["Marca"];
                    }
                    if(!(datos.Lector["Categoria"] is DBNull))
                    {
                        auxiliar.IdCategoria = new Categoria();
                        auxiliar.IdCategoria.Id = (int)datos.Lector["IdCategoria"];
                        auxiliar.IdCategoria.Descripcion = (string)datos.Lector["Categoria"];
                    }
                    if(!(datos.Lector["ImagenUrl"] is DBNull))
                        auxiliar.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    if(!(datos.Lector["Precio"] is DBNull))
                        auxiliar.Precio = (decimal)datos.Lector["Precio"];

                    listaArticulos.Add(auxiliar);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            datos.CerrarConexion();
            return listaArticulos;
        }
        public void modificarArticulo(Articulo articulo)
        { 
            AccesoDatos datos= new AccesoDatos();
            try
            {
                datos.SetearConsulta("Update articulos Set Codigo= @codigo, Nombre=@nombre, Descripcion=@descripcion, IdMarca=@idMarca, IdCategoria= @idCategoria, ImagenUrl= @imagenUrl, Precio=@precio where Id= @id");
                datos.SetearParametro("@codigo", articulo.Codigo);
                datos.SetearParametro("@nombre", articulo.Nombre);
                datos.SetearParametro("@descripcion", articulo.Descripcion);
                datos.SetearParametro("@idMarca", articulo.IdMarca.Id);
                datos.SetearParametro("@idCategoria", articulo.IdCategoria.Id);
                datos.SetearParametro("@imagenUrl", articulo.ImagenUrl);
                datos.SetearParametro("@precio", articulo.Precio);
                datos.SetearParametro("@id", articulo.Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            datos.CerrarConexion();
        }
        public void agregarArticulo(Articulo articulo) 
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)\r\nValues (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @imagenUrl, @precio)");
                datos.SetearParametro("@codigo", articulo.Codigo);
                datos.SetearParametro("@nombre", articulo.Nombre);
                datos.SetearParametro("@descripcion", articulo.Descripcion);
                datos.SetearParametro("@idMarca", articulo.IdMarca.Id);
                datos.SetearParametro("@idCategoria", articulo.IdCategoria.Id);
                datos.SetearParametro("@imagenUrl", articulo.ImagenUrl);
                datos.SetearParametro("@precio", articulo.Precio);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            datos.CerrarConexion();
        }
        public void eliminarArticulo(Articulo articulo) 
        {
            AccesoDatos datos = new AccesoDatos();
            string consulta = "delete from articulos where id= @idArticulo";
            datos.SetearParametro("@idArticulo", articulo.Id);
            datos.SetearConsulta(consulta);
            try
            {
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            datos.CerrarConexion();
        }
        public List<Articulo> filtrarArticulos(string campo, string criterio, string parametro)
        {
            List<Articulo> listaArticulos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            string consulta = "select a.Id, a.Codigo, a.Nombre, a.Descripcion, m.Descripcion Marca, c.Descripcion Categoria, a.ImagenUrl, a.Precio, a.IdMarca, a.IdCategoria from ARTICULOS a inner join Categorias c on a.IdCategoria=c.Id inner join Marcas m on a.IdMarca= m.Id where ";
            switch (campo)
            {
                case "Marca":
                    if (criterio == "Empieza con")
                    {
                        consulta += "m.Descripcion like '" + parametro + "%'";
                    }
                    else if (criterio == "Termina con")
                    {
                        consulta += "m.Descripcion like '%" + parametro + "'";
                    }
                    else
                    {
                        consulta += "m.Descripcion like '%"+ parametro+ "%'";
                    }
                    break;
                
                case "Categoría":
                    if (criterio == "Empieza con")
                    {
                        consulta += "c.Descripcion like '" + parametro + "%'";
                    }
                    else if (criterio == "Termina con")
                    {
                        consulta += "c.Descripcion like '%" + parametro + "'";
                    }
                    else
                    {
                        consulta += "c.Descripcion like '%" + parametro + "%'";
                    }
                    break;

                default:
                    if (criterio == "Mayor a")
                    {
                        consulta += "a.Precio > " + parametro;
                    }
                    else if (criterio == "Menor a")
                    {
                        consulta += "a.Precio <" + parametro;
                    }
                    else
                    {
                        consulta += "a.Precio = " + parametro;
                    }
                    break;

            }
            datos.SetearConsulta(consulta);

            try
            {
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo auxiliar = new Articulo();
                    auxiliar.Id = (int)datos.Lector["Id"];
                    if (!(datos.Lector["Codigo"] is DBNull))
                        auxiliar.Codigo = (string)datos.Lector["Codigo"];
                    if (!(datos.Lector["Nombre"] is DBNull))
                        auxiliar.Nombre = (string)datos.Lector["Nombre"];
                    if (!(datos.Lector["Descripcion"] is DBNull))
                        auxiliar.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["Marca"] is DBNull))
                    {
                        auxiliar.IdMarca = new Marca();
                        auxiliar.IdMarca.Id = (int)datos.Lector["IdMarca"];
                        auxiliar.IdMarca.Descripcion = (string)datos.Lector["Marca"];
                    }
                    if (!(datos.Lector["Categoria"] is DBNull))
                    {
                        auxiliar.IdCategoria = new Categoria();
                        auxiliar.IdCategoria.Id = (int)datos.Lector["IdCategoria"];
                        auxiliar.IdCategoria.Descripcion = (string)datos.Lector["Categoria"];
                    }
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        auxiliar.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    if (!(datos.Lector["Precio"] is DBNull))
                        auxiliar.Precio = (decimal)datos.Lector["Precio"];

                    listaArticulos.Add(auxiliar);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            datos.CerrarConexion();
            return listaArticulos;
        }

    }
}

