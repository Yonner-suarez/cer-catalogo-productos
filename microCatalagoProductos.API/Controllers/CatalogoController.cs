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
        public ActionResult producto(int idEmpleado)
        {
            //GeneralResponse res = BLActivacionContrato.ListarEstados(idEmpleado);
            return Ok();
        }
    }
}
