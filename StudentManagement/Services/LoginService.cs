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
            var student = await _studentRepository.GetByEmailAsync(dto.Email);

            if (student != null && student.Password == dto.Password) // plain text check
            {
                // Generate JWT token
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
                        new Claim(ClaimTypes.Email, student.Email),
                        new Claim(ClaimTypes.Name, student.Name)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"])),
                    Issuer = _configuration["JwtSettings:Issuer"],
                    Audience = _configuration["JwtSettings:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new LoginResponseDto
                {
                    Token = tokenHandler.WriteToken(token),
                    Email = student.Email,
                    Name = student.Name
                };
            }

            return null;
        }
    }
}
