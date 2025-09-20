using System.Data;
using microCatalagoProductos.API.Model;
using microCatalagoProductos.API.Model.Request;
using microCatalagoProductos.API.Model.Response;
using microCatalagoProductos.API.Utils;
using MySql.Data.MySqlClient;

namespace microCatalagoProductos.API.Dao
{
    public static class DACatalogoProductos
    {
        public static GeneralResponse ObtenerProductos(ObtenerProductosRequest request)
        {
            var response = new GeneralResponse();
            var productosLista = new List<ObtenerProductosResponse>();

            using (MySqlConnection conn = new MySqlConnection(Variables.Conexion.cnx))
            {
                try
                {
                    conn.Open();
                    // Base SQL
                    string sql = @"
                        SELECT 
                            producto.cer_vch_nombre AS NombreProducto,
                            producto.cer_dec_precio AS Precio,
                            categoria.cer_vch_categoria AS Categoria,
                            marca.cer_vch_nombre AS Marca
                        FROM 
                            tbl_cer_productos AS producto
                        INNER JOIN 
                            tbl_cer_categoria AS categoria 
                            ON categoria.cer_int_idcategoria = producto.cer_int_idcategoria
                        INNER JOIN 
                            tbl_cer_marca AS marca 
                            ON marca.cer_int_idmarca = producto.cer_int_idmarca
                        WHERE 1 = 1";

                    var cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    // Agrega filtros dinámicamente y con parámetros seguros
                    if (request.IdMarca != 0)
                    {
                        sql += " AND marca.cer_int_idmarca = @IdMarca";
                        cmd.Parameters.AddWithValue("@IdMarca", request.IdMarca);
                    }

                    if (request.IdCategoria != 0)
                    {
                        sql += " AND categoria.cer_int_idcategoria = @IdCategoria";
                        cmd.Parameters.AddWithValue("@IdCategoria", request.IdCategoria);
                    }

                    if (request.Precio >= 500000)
                    {
                        sql += " AND producto.cer_dec_precio >= @Precio";
                        cmd.Parameters.AddWithValue("@Precio", request.Precio);
                    }

                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var producto = new ObtenerProductosResponse
                        {
                            Categoria = reader["Categoria"].ToString(),
                            Marca = reader["Marca"].ToString(),
                            Descripcion = reader["NombreProducto"].ToString(),
                            Precio = reader["Precio"] != DBNull.Value ? Convert.ToDecimal(reader["Precio"]) : 0
                        };
                        productosLista.Add(producto);
                    }
                    

                    response.data = productosLista;
                    response.status = 200;
                    response.message = "Productos obtenidos correctamente.";
                    conn.Close();
                }
                catch (Exception ex)
                {
                    response.data = null;
                    response.message = "Ocurrió un error al traer los productos del catálogo.";
                    response.status = Variables.Resultado.ERROR;
                    conn.Close();
                    // Opcional: puedes loguear el error con algún logger
                }
            }
            return response;
            
        }

    }
}