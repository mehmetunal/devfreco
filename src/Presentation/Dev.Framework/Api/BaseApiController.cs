using Microsoft.AspNetCore.Mvc;

namespace Dev.Framework.Api
{
    [Produces("application/json")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        //[ApiExplorerSettings(IgnoreApi = true)]
    }
}
