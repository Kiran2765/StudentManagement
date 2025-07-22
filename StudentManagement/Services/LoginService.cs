using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.DTO;
using StudentManagement.Model;
using StudentManagement.Respository.IRepository;
using StudentManagement.Services.IServices;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class LoginService : ILoginService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IConfiguration _configuration;

        public LoginService(IStudentRepository studentRepository, IConfiguration configuration)
        {
            _studentRepository = studentRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            // 1. Retrieve student by email
            var student = await _studentRepository.GetByEmailAsync(dto.Email);
            if (student == null)
                return null;

            // 2. Verify hashed password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, student.Password);
            if (!isPasswordValid)
                return null;

            // 3. Load JWT settings
            string jwtKey = _configuration["JwtSettings:Key"];
            string jwtIssuer = _configuration["JwtSettings:Issuer"];
            string jwtAudience = _configuration["JwtSettings:Audience"];
            double jwtDuration = Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"]);

            // 4. Create security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 5. Prepare claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
                new Claim(ClaimTypes.Email, student.Email),
                new Claim(ClaimTypes.Name, student.Name)
            };

            // 6. Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtDuration),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            // 7. Return token and info
            return new LoginResponseDto
            {
                Token = tokenString,
                Email = student.Email,
                Name = student.Name
            };
        }
    }
}
