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
        public ActionResult productos(ObtenerProductosRequest request)
        {
            GeneralResponse res = BLCatalogoProductos.ObtenerProductos(request);
            if (res.status != Variables.Resultado.OK)
            {
                return StatusCode(res.status, res);
            }
            return Ok(res);
        }
    }
}
