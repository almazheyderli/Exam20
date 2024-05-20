using Exam.Core.DTOs.AccountDto;
using Exam.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam20.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _role;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> role)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _role = role;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        { 

            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                User user = new User()
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    Surname = registerDto.Surname,
                    UserName = registerDto.UserName,
                };
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded) {

                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View();

                }
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");


            }


        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.FindByNameAsync(loginDto.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginDto.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username or email is wrong");
                    return View();
                }
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Birazdan yeniden cehd edin");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or email is wrong");
                return View();
            }
            await _signInManager.SignInAsync(user, loginDto.IsRemember);
        
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        { 
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        }
    }


