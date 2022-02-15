using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Project_Login.Extensions;
using Project_Login.Interfaces;
using Project_Login.ViewModels;

namespace Project_Login.Services;

public class AuthService : IAuthService
{
    private readonly AppSettings _appSettings;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    

    public AuthService(IOptions<AppSettings> appSettings, 
                       UserManager<IdentityUser> userManager, 
                       SignInManager<IdentityUser> signInManager)
    {
        _appSettings = appSettings.Value;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<LoginResponseViewModel> GerarJwt(string loginUserEmail)
    {
        var user = await _userManager.FindByEmailAsync(loginUserEmail);
        
        var claims = await CreateClaims(user);

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var encodedToken = EncodedToken(identityClaims);

        var response = new LoginResponseViewModel
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
            UserToken = new UserTokenViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
            }
        };

        return response;
    }

    private string EncodedToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
        var token = tokenHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
        {
            Issuer = _appSettings.Emissor,
            Audience = _appSettings.ValidoEm,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        });

        var encodedToken = tokenHandler.WriteToken(token);
        
        return encodedToken;
    }

    private async Task<IList<Claim>> CreateClaims(IdentityUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        foreach (var role in userRoles)
        {
            claims.Add(new Claim("role", role));
        }
        
        return claims;
    }

    private static long ToUnixEpochDate(DateTime date)
    {
        return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
