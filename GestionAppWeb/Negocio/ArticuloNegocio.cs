using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Windows.Forms;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo> ();
            AccesoDatos datos = new AccesoDatos ();

            try
            {
                datos.setearConsulta("select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, IdMarca, IdCategoria, M.Descripcion as Marca, C.Descripcion as Categoria from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca and C.Id = A.IdCategoria");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo ();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodigoArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if(!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    }
                    aux.Precio = Math.Round((decimal)datos.Lector["Precio"], 2);
                    aux.Marca= new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    lista.Add(aux);

                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
            
            
            }

            public void agregar(Articulo nuevoArt)
            {
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    datos.setearConsulta("insert into ARTICULOS(Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values(@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @UrlImagen, @Precio)");
                    datos.setearParametro("@Codigo", nuevoArt.CodigoArticulo);
                    datos.setearParametro("@Nombre", nuevoArt.Nombre);
                    datos.setearParametro("@Descripcion", nuevoArt.Descripcion);
                    datos.setearParametro("@IdMarca", nuevoArt.Marca.Id);
                    datos.setearParametro("@IdCategoria", nuevoArt.Categoria.Id);
                    datos.setearParametro("@UrlImagen", nuevoArt.UrlImagen);
                    datos.setearParametro("@Precio", nuevoArt.Precio);
                    datos.ejecutarAccion();

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    datos.cerrarConexion();
                }
            }
        public void modificar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @ImagenUrl, Precio = @Precio where Id= @Id");
                datos.setearParametro("@Id", articulo.Id);
                datos.setearParametro("@Codigo", articulo.CodigoArticulo);
                datos.setearParametro("@Nombre", articulo.Nombre);
                datos.setearParametro("@Descripcion", articulo.Descripcion);
                datos.setearParametro("@IdMarca", articulo.Marca.Id);
                datos.setearParametro("@IdCategoria", articulo.Categoria.Id);
                datos.setearParametro("@ImagenUrl", articulo.UrlImagen);
                datos.setearParametro("@Precio", articulo.Precio);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void eliminar(Articulo articulo) 
        { 
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("delete from ARTICULOS where Id = @Id ");
                datos.setearParametro("@Id", articulo.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Articulo> listaArticulosFiltrada = new List<Articulo>();
            try
            {
                string consulta = "select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, IdMarca, IdCategoria, M.Descripcion as Marca, C.Descripcion as Categoria from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca and C.Id = A.IdCategoria and ";
                if (campo == "Precio") 
                { 
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Precio < " + filtro;
                            break;
                        case "Igual a":
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }else
                {
                    if (campo == "Marca")
                    {
                        campo = null;
                        campo = "M. Descripcion";
                    }else if (campo == "Categoria")
                    {
                        campo = null;
                        campo = "C. Descripcion";
                    }else 
                    {
                        campo = null;
                        campo = "Nombre";
                    }
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += campo + " like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += campo + " like " + "'%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += campo + " like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodigoArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    }
                    aux.Precio = Math.Round((decimal)datos.Lector["Precio"],2);
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    listaArticulosFiltrada.Add(aux);

                }

                return listaArticulosFiltrada;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
