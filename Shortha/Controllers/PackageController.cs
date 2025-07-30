using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Services;

namespace Shortha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController(IPackagesService service) : Base
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllPackages()
        {
            var packages = await service.GetActivePackages();
            return Success(packages);
        }
    }
}