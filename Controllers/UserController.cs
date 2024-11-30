using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Teether.OperationalObjects;

namespace Teether.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController(UserManager<User> userManager) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<StatusCodeResult> Register([FromBody] UserForRegistration userForRegistration)
        {
            var t = await userManager.CreateAsync(new()
            {
                UserName = userForRegistration.UserName,
                Email = userForRegistration.Email,
                FirstName = userForRegistration.FirstName,
                LastName = userForRegistration.LastName,
                Patronymic = userForRegistration.Patronymic
            }, userForRegistration.Password);

            await userManager.AddToRoleAsync(userManager.FindByNameAsync(userForRegistration.UserName).Result!, userForRegistration.Role);

            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost("login")]
        public async Task<ActionResult<bool>> Login([FromBody] UserForLogin userForLogin)
        {
            User? u;

            if ((u = await userManager.FindByNameAsync(userForLogin.UserName)) is null)
                return false;

            return await userManager.CheckPasswordAsync(u, userForLogin.Password);
        }
    }
}
