using microCatalagoProductos.API.Logic;
using microCatalagoProductos.API.Model;
using microCatalagoProductos.API.Model.Request;
using microCatalagoProductos.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace microCatalagoProductos.API.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogoController : ControllerBase
    {
        [HttpPost]
        [Route("[action]")]
        public ActionResult productos(ProductosRequest filtros)
        {
            GeneralResponse res = BLCatalogoProductos.ObtenerProductos(filtros);
            if (res.status == Variables.Response.OK)
            {
                return Ok(res);
            }
            else
            {
                return StatusCode(res.status, res);
            }
        }
    }
}
