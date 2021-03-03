using AutoMapper;
using CRUD.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CRUD_NwDb.Areas.Identity
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(IFormCollection userModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            UserRegistrationModel userR = new UserRegistrationModel();
            userR.FirstName = userModel["FirstName"];
            userR.LastName = userModel["LastName"];
            userR.Email = userModel["Email"];
            userR.Password = userModel["Password"];
            userR.ConfirmPassword = userModel["ConfirmPassword"];

            var user = _mapper.Map<User>(userR);
            var result = await _userManager.CreateAsync(user, userR.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View(userR);
            }
            await _userManager.AddToRoleAsync(user, "User");
            return Ok(userR);
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(Index), "Home");

        }

        [HttpPost("Login")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection userModel, string returnUrl = null)
        {
            

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                UserLoginModel userL = new UserLoginModel();
                userL.Email = userModel["Email"];
                userL.Password = userModel["Password"];
                var rememberme = userModel["RememberMe"];
                userL.RememberMe = false; //(bool)Convert.ChangeType(userModel["RememberMe"], typeof(bool));

                var result = await _signInManager.PasswordSignInAsync(userL.Email, userL.Password, userL.RememberMe, false);

                if (result.Succeeded)
                {
                return Ok(result);//RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName or Password");
                    return Ok(result);
                }
            
            
        }
    }
}
