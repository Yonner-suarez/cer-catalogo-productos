namespace microCatalagoProductos.API.Model.Response
{
    public class ProductoResponse
    {
        public Marca Marca { get; set; }
        public Categoria Categoria { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public byte[] Image { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
