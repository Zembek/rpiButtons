using Microsoft.AspNetCore.Mvc;

namespace RPiButtons.Interface.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarcoPoloController : Controller
    {
        [HttpGet(Name = "Marco")]
        public JsonResult Marco()
        {
            return Json("Polo");
        }
    }
}
