using AutoMapper;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Services.Interfaces;
using DiagramGenerator.Web.ViewModels;
using DiagramGenerator.Web.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DiagramGenerator.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<DiagramController> logger;
        private readonly SignInManager<User> signInManager;
        private readonly IUserSeedManager userSeedManager;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public AccountController(
            ILogger<DiagramController> logger,
            SignInManager<User> signInManager,
            IUserSeedManager userSeedManager,
            IMapper mapper,
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.userSeedManager = userSeedManager;
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }

            return View();
        }

        [HttpGet()]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        Redirect(Request.Query["ReturnUrl"].First());
                    }
                    else
                    {
                        return RedirectToAction("Index", "App");
                        //ViewBag.UserMessage = "Your credatials are incorrect!";
                        //return View();
                    }
                }
            }

            ModelState.AddModelError("", "Failed to login");

            return View();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(LoginViewModel model)
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }

        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            userSeedManager.AddUser(mapper.Map<SignUpViewModel, User>(model), model.Password).Wait();
            return RedirectToAction("Index", "App");
        }

        [HttpPost("createToken")]
        public async Task<IActionResult> CreateToken(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Username);

                if(user != null)
                {
                    var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (result.Succeeded)
                    {
                        // Create token
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Token:Key")));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            issuer: configuration.GetValue<string>("Token:Issuer"), 
                            audience: configuration.GetValue<string>("Token:Audience"), 
                            claims: claims,
                            signingCredentials: creds,
                            expires: DateTime.UtcNow.AddMinutes(20));

                        return Created("", new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }

                
            }

            return BadRequest();
        }
    } 
}
