
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using DevHouse1.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Security;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;


namespace DevHouse1.Controllers

{

    /// <summary>
    /// Controller for handling authentication and generating JWT tokens.
    /// </summary>


    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        //create local variable for JWT setting
        private readonly JwtSettings _jwtSettings;

        public AuthController(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        // <summary>
        /// Generates a JWT token for authentication.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     POST /api/Auth/token
        /// 
        /// **Example Response:**
        /// 
        /// ```json
        /// {
        ///   "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">JWT token generated successfully</response>

        [HttpPost("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GenerateToken()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,"testuser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes ?? 60),
                signingCredentials: creds
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));

        }
    }



}



