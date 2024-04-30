using Microsoft.AspNetCore.Mvc;

namespace RPiButtons.Interface.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarcoPoloController : Controller
    {
        [HttpGet]
        public JsonResult Marco()
        {
            return new JsonResult("Polo");
        }
    }
}
