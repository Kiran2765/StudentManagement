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
        private readonly IPrincipalRepository _principalRepository;
        private readonly IConfiguration _configuration;

        public LoginService(
            IStudentRepository studentRepository,
            IPrincipalRepository principalRepository,
            IConfiguration configuration)
        {
            _studentRepository = studentRepository;
            _principalRepository = principalRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            if (string.IsNullOrEmpty(dto.Role))
                return null;

            string role = dto.Role.Trim().ToLower();
            string jwtKey = _configuration["JwtSettings:Key"];
            string jwtIssuer = _configuration["JwtSettings:Issuer"];
            string jwtAudience = _configuration["JwtSettings:Audience"];
            double jwtDuration = Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenHandler = new JwtSecurityTokenHandler();

            if (role == "student")
            {
                var student = await _studentRepository.GetByEmailAsync(dto.Email);
                if (student == null || !BCrypt.Net.BCrypt.Verify(dto.Password, student.Password))
                    return null;

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
                    new Claim(ClaimTypes.Email, student.Email),
                    new Claim(ClaimTypes.Name, student.Name),
                    new Claim(ClaimTypes.Role, "Student")
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(jwtDuration),
                    Issuer = jwtIssuer,
                    Audience = jwtAudience,
                    SigningCredentials = creds
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                string tokenString = tokenHandler.WriteToken(token);

                return new LoginResponseDto
                {
                    Token = tokenString,
                    Email = student.Email,
                    Name = student.Name,
                    Role = "Student"
                };
            }
            else if (role == "principal")
            {
                var principal = await _principalRepository.GetByEmailAsync(dto.Email);
                if (principal == null || !BCrypt.Net.BCrypt.Verify(dto.Password, principal.PasswordHash))
                    return null;

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, principal.Id.ToString()),
                    new Claim(ClaimTypes.Email, principal.Email),
                    new Claim(ClaimTypes.Name, principal.Name),
                    new Claim(ClaimTypes.Role, "Principal")
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(jwtDuration),
                    Issuer = jwtIssuer,
                    Audience = jwtAudience,
                    SigningCredentials = creds
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                string tokenString = tokenHandler.WriteToken(token);

                return new LoginResponseDto
                {
                    Token = tokenString,
                    Email = principal.Email,
                    Name = principal.Name,
                    Role = "Principal"
                };
            }

            return null;
        }
    }
}
