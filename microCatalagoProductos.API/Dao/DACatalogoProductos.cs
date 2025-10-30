using microCatalagoProductos.API.Model;
using microCatalagoProductos.API.Model.Request;
using microCatalagoProductos.API.Model.Response;
using microCatalagoProductos.API.Utils;
using MySql.Data.MySqlClient;

namespace microCatalagoProductos.API.Dao
{
    public class DACatalogoProductos
    {
        public static GeneralResponse ObtenerProductos(ProductosRequest req)
        {
            var response = new GeneralResponse();
            var listaProductos = new List<ProductoResponse>();

            using (MySqlConnection conn = new MySqlConnection(Variables.Conexion.cnx))
            {
                try
                {
                    conn.Open();

                    // Base SQL
                    string sqlSelect = @"
                                        SELECT 
                                            p.cer_int_id_producto,
                                            p.cer_int_id_marca,
                                            m.cer_enum_nombre AS MarcaNombre,
                                            p.cer_int_id_categoria,
                                            c.cer_enum_nombre AS CategoriaNombre,
                                            p.cer_varchar_nombre,
                                            p.cer_text_descripcion,
                                            p.cer_decimal_precio,
                                            p.cer_int_stock,
                                            p.cer_blob_imagen,
                                            p.cer_decimal_peso
                                        FROM tbl_cer_producto p
                                        INNER JOIN tbl_cer_marca m ON p.cer_int_id_marca = m.cer_int_id_marca
                                        INNER JOIN tbl_cer_categoria c ON p.cer_int_id_categoria = c.cer_int_id_categoria
                                        WHERE p.cer_tinyint_estado = 1
                                          ";

                    // Construimos los filtros dinámicamente
                    var filtros = new List<string>();
                    var cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (req.IdMarca != 0)
                    {
                        filtros.Add("p.cer_int_id_marca = @IdMarca");
                        cmd.Parameters.AddWithValue("@IdMarca", req.IdMarca);
                    }

                    if (req.IdCategoria != 0)
                    {
                        filtros.Add("p.cer_int_id_categoria = @IdCategoria");
                        cmd.Parameters.AddWithValue("@IdCategoria", req.IdCategoria);
                    }

                    if (!string.IsNullOrEmpty(req.Busqueda))
                    {
                        filtros.Add("p.cer_text_descripcion LIKE @Busqueda");
                        cmd.Parameters.AddWithValue("@Busqueda", "%" + req.Busqueda + "%");
                    }

                    if (!string.IsNullOrEmpty(req.RangoPrecio))
                    {
                        switch (req.RangoPrecio)
                        {
                            case "0-50000":
                                filtros.Add("p.cer_decimal_precio BETWEEN 0 AND 50000");
                                break;
                            case "50001-1000000":
                                filtros.Add("p.cer_decimal_precio BETWEEN 50001 AND 1000000");
                                break;
                            case "1000000+":
                                filtros.Add("p.cer_decimal_precio > 1000000");
                                break;
                        }
                    }

                    // Si hay filtros adicionales, los agregamos a la consulta
                    if (filtros.Count > 0)
                    {
                        sqlSelect += " AND " + string.Join(" AND ", filtros);
                    }

                    cmd.CommandText = sqlSelect;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var producto = new ProductoResponse
                            {
                                Marca = new Marca
                                {
                                    IdMarca = Convert.ToInt32(reader["cer_int_id_marca"]),
                                    Nombre = reader["MarcaNombre"]?.ToString() ?? string.Empty,
                                },
                                Categoria = new Categoria
                                {
                                    IdCategoria = reader["cer_int_id_categoria"] != DBNull.Value ? Convert.ToInt32(reader["cer_int_id_categoria"]) : 0,
                                    Nombre = reader["CategoriaNombre"]?.ToString() ?? string.Empty,
                                },
                                Nombre = reader["cer_varchar_nombre"]?.ToString() ?? string.Empty,
                                Descripcion = reader["cer_text_descripcion"]?.ToString() ?? string.Empty,
                                Precio = Convert.ToDecimal(reader["cer_decimal_precio"]),
                                CantidadReal = Convert.ToInt32(reader["cer_int_stock"]),
                                Image = reader["cer_blob_imagen"] != DBNull.Value ? (byte[])reader["cer_blob_imagen"] : null,
                                IdProducto = Convert.ToInt32(reader["cer_int_id_producto"]),
                                Peso = Convert.ToInt32(reader["cer_decimal_peso"])

                            };

                            listaProductos.Add(producto);
                        }
                    }

                    response.status = Variables.Response.OK;
                    response.message = listaProductos.Count > 0 ? "Productos obtenidos correctamente." : "No se encontraron productos activos.";
                    response.data = listaProductos;
                }
                catch (Exception ex)
                {
                    response.status = Variables.Response.ERROR;
                    response.message = "Error al obtener productos: " + ex.Message;
                    response.data = null;
                }
                finally
                {
                    conn.Close();
                }
            }

            return response;
        }
        public static GeneralResponse ObtenerMarcas()
        {
            var response = new GeneralResponse();
            var listaMarcas = new List<Marca>();

            using (MySqlConnection conn = new MySqlConnection(Variables.Conexion.cnx))
            {
                try
                {
                    conn.Open();

                    string sqlSelect = @"
                                     SELECT 
                                        cer_int_id_marca AS IdMarca,
                                        cer_enum_nombre AS Nombre
                                    FROM tbl_cer_marca
                                    WHERE cer_tinyint_estado = 1;
                                                ";

                    var cmd = new MySqlCommand(sqlSelect, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var marca = new Marca
                            {
                                IdMarca = Convert.ToInt32(reader["IdMarca"]),
                                Nombre = reader["Nombre"]?.ToString() ?? string.Empty
                            };

                            listaMarcas.Add(marca);
                        }
                    }

                    response.status = Variables.Response.OK;
                    response.message = listaMarcas.Count > 0 ? "Marcas obtenidas correctamente." : "No se encontraron marcas.";
                    response.data = listaMarcas;
                }
                catch (Exception ex)
                {
                    response.status = Variables.Response.ERROR;
                    response.message = "Error al obtener marcas: " + ex.Message;
                    response.data = null;
                }
                finally
                {
                    conn.Close();
                }
            }

            return response;
        }
        public static GeneralResponse ObtenerCategorias()
        {
            var response = new GeneralResponse();
            var listaCategorias = new List<Categoria>();

            using (MySqlConnection conn = new MySqlConnection(Variables.Conexion.cnx))
            {
                try
                {
                    conn.Open();

                    string sqlSelect = @"
                                       SELECT 
                                            cer_int_id_categoria AS IdCategoria,
                                            cer_enum_nombre AS Nombre
                                        FROM tbl_cer_categoria
                                        WHERE cer_tinyint_estado = 1;
                                    ";

                    var cmd = new MySqlCommand(sqlSelect, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var categoria = new Categoria
                            {
                                IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                                Nombre = reader["Nombre"]?.ToString() ?? string.Empty
                            };

                            listaCategorias.Add(categoria);
                        }
                    }

                    response.status = Variables.Response.OK;
                    response.message = listaCategorias.Count > 0 ? "Categorías obtenidas correctamente." : "No se encontraron categorías.";
                    response.data = listaCategorias;
                }
                catch (Exception ex)
                {
                    response.status = Variables.Response.ERROR;
                    response.message = "Error al obtener categorías: " + ex.Message;
                    response.data = null;
                }
                finally
                {
                    conn.Close();
                }
            }

            return response;
        }


    }
}
