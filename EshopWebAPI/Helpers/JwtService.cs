using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EshopWebAPI.Helpers
{
    public class JwtService
    {
        private readonly string _key = "super_secret_jwt_key_for_Eshop_Console_and_Web_App";

        public string GenerateToken(int userId, string userRole)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, userRole)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

//using BCrypt.Net;

//// 1. REJESTRACJA: Haszowanie hasła przed zapisem do bazy
//string password = "MojeTajneHaslo123";
//string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

//// To zapisujesz w bazie danych (np. kolumna PasswordHash)
//Console.WriteLine($"Hasz do bazy: {passwordHash}");

//// ---------------------------------------------------------

//// 2. LOGOWANIE: Sprawdzanie czy podane hasło pasuje do hasza z bazy
//string inputPassword = "MojeTajneHaslo123"; // To wpisał użytkownik
//bool isCorrect = BCrypt.Net.BCrypt.Verify(inputPassword, passwordHash);

//if (isCorrect)
//{
//    Console.WriteLine("Zalogowano pomyślnie!");
//}
//else
//{
//    Console.WriteLine("Błędne hasło.");
//}
////Liczba 12 to "cost factor" - im wyższa, tym wolniej generuje się hasz (bezpieczniej)
//string strongerHash = BCrypt.Net.BCrypt.HashPassword(password, 12);