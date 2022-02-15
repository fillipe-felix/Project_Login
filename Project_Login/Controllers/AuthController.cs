using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Project_Login.Interfaces;
using Project_Login.ViewModels;

namespace Project_Login.Controllers;

[Route("api/")]
public class AuthController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAuthService _authService;

    public AuthController(SignInManager<IdentityUser> signInManager, 
                          UserManager<IdentityUser> userManager, 
                          IAuthService authService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Registrar([FromBody] RegisterUserViewModel registerUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new IdentityUser
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true // ja garante a confirmação de email
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            var token = await _authService.GerarJwt(user.Email);
            return Ok(token);
        }
        
        
        return BadRequest(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserViewModel loginUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

        if (result.Succeeded)
        {
            var token = await _authService.GerarJwt(loginUser.Email);
            return Ok(token);
        }
        
        if (result.IsLockedOut)
        {
            return BadRequest("Usuário temporiariamente bloqueado por tentativas inválidas");
        }
        
        return BadRequest("Usuário ou senha incorretos");
    }
}
