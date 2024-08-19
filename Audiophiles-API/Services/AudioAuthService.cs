using Audiophiles_API.Constants;
using Audiophiles_API.DTOs.Auth;
using Audiophiles_API.Helpers;
using Audiophiles_API.IServices;
using Audiophiles_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Audiophiles_API.Services
{
    public class AudioAuthService: IAudioAuthService
    {
        private readonly UserManager<AudioUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        public AudioAuthService(RoleManager<IdentityRole> roleManager, UserManager<AudioUser> userManager, IOptions<JWT> jwt)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            var authResult = new AuthModel();

            // Check if the email has been already existed
            if(await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                authResult.Message = "This Email already exist, Try another one!";
                return authResult;
            }

            var newUser = new AudioUser()
            {
                FullName = model.FullName,
                UserName = Guid.NewGuid().ToString().Substring(0, 6),
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTime.Now,
            };

            // Now, We can Create new user
            var result = await _userManager.CreateAsync(newUser, model.Password);
            // If there are some errors
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description;
                }
                authResult.Message = errors;
                return authResult;
            }

            // By Default every User in the system is a Usual user
            result = await _userManager.AddToRoleAsync(newUser, RolesConstants.User);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description;
                }
                authResult.Message = errors;
                return authResult;
            }

            // Generate new Token and log the user in
            var jwtToken = await GenerateJwtToken(newUser);
            authResult.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authResult.FullName = newUser.FullName;
            authResult.UserName = newUser.UserName;
            authResult.Email = newUser.Email;
            authResult.Roles = new List<string>() { RolesConstants.User };
            authResult.IsAuthenticated = true;
            authResult.Expires_at = jwtToken.ValidTo;

            return authResult;
        }

        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            var result = new AuthModel();
            // Check if the email is valid 
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                result.Message = "Invalid Email or Password!";
                return result;
            }

            // Get Current User Roles
            var roles = await _userManager.GetRolesAsync(user);
            // Generate New Token
            var jwtToken = await GenerateJwtToken(user);
            result.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            result.FullName = user.FullName;
            result.UserName = user.UserName!;
            result.Email = user.Email!;
            result.Roles = roles.ToList();
            result.IsAuthenticated = true;
            result.Expires_at = jwtToken.ValidTo;

            return result;
        }












        // Token Generation Factory
        private async Task<JwtSecurityToken> GenerateJwtToken(AudioUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesClaims = new List<Claim>();

            foreach (var role in userRoles)
            {
                rolesClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            }
            .Union(userClaims)
            .Union(rolesClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.TokenDurationInDays),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }
    }
}
