namespace microCatalagoProductos.API.Model.Request
{
    public class ObtenerProductosRequest
    {
        public int IdMarca { get; set; } = 0;
        public int IdCategoria { get; set; } = 0;
        public decimal Precio { get; set; } = 0;
    }
}
