using microCatalagoProductos.API.Dao;
using microCatalagoProductos.API.Model;
using microCatalagoProductos.API.Model.Request;
using microCatalagoProductos.API.Model.Response;

namespace microCatalagoProductos.API.Logic
{
    public static class BLCatalogoProductos
    {
        public static GeneralResponse ObtenerProductos(ObtenerProductosRequest request)
        {
            GeneralResponse productos = DACatalogoProductos.ObtenerProductos(request);
            return productos;
        }
    }
}