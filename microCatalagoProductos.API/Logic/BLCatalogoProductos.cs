using microCatalagoProductos.API.Dao;
using microCatalagoProductos.API.Model;
using microCatalagoProductos.API.Model.Request;

namespace microCatalagoProductos.API.Logic
{
    public class BLCatalogoProductos
    {
        public static GeneralResponse ObtenerProductos(ProductosRequest request)
        {
            var res = DACatalogoProductos.ObtenerProductos(request);
            return res;
        }
    }
}
