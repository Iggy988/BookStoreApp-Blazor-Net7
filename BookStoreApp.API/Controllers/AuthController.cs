using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        _logger.LogInformation($"Registration Attempt for {userDto.Email}");
        var user = _mapper.Map<ApiUser>(userDto);
        user.UserName = userDto.Email;
        var result = await _userManager.CreateAsync(user, userDto.Password);

        try
        {
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            await _userManager.AddToRoleAsync(user, "User");
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"Something went wrong in the {nameof(Register)}");
            return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
        }

    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto)
    {
        _logger.LogInformation($"Login Attempt for {userDto.Email}");

        try
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            var passwordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);

            if (user is null || passwordValid == false)
            {
                return Unauthorized(userDto); //401 response
            }

            string tokenString = await GenerateToken(user);

            var response = new AuthResponse
            {
                Email = userDto.Email,
                Token = tokenString,
                UserId = user.Id
            };

            //return Accepted(response);
            return response;
  
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"Something went wrong in the {nameof(Login)}");
            return Problem($"Something went wrong in the {nameof(Login)}", statusCode: 500);
        }
    }

    private async Task<string> GenerateToken(ApiUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var  roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

        var userClaims = await _userManager.GetClaimsAsync(user);   

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(CustomClaimTypes.Uid, user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var token = new JwtSecurityToken(
            issuer:_configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:Duration"])),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
