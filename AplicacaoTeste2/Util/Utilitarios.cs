using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ApiAgendaDocumentos.Util
{
    public class Utilitarios
    {
        public static string CreateToken(string userId, string TokenIssuer, string TokenAudience, string SigningKey, int Expires)
        {
            var handler = new JwtSecurityTokenHandler();

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
            });

            var now = DateTime.UtcNow;

            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("B053079-077080-076073-053085-085077-085085-081084-07B073-055077-080085-081053-07607F"));

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = TokenIssuer,
                Audience = TokenAudience,
                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256),
                IssuedAt = now,
                Expires = now.AddMinutes(Expires)
            });

            return handler.WriteToken(securityToken);
        }

        public static string Sha512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        public static string RemoveCaracteresEpeciais(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }

        public static string SomenteNumeros(string str)
        {
            return Regex.Replace(str, "[^0-9]+", "", RegexOptions.Compiled);
        }

        public static string RemoveAcentos(string palavacomacentos)
        {
            return Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(palavacomacentos)
            );
        }

        public static bool ValidarCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        public static bool ValidarNome(string nome)
        {
            nome = RemoveAcentos(nome);

            if (nome.Split(' ').Length < 2)
                return false;

            if (ValidarRepeticoesSequenciais(nome, 3))
                return false;

            return true;
        }

        public static bool ValidarRepeticoesSequenciais(string texto, int numeroRepeticoes)
        {
            return Regex.IsMatch(texto, "([a-zA-Z])\\1{" + (numeroRepeticoes - 1) + "}");
        }
    }

    public static class StringExtensions
    {
        public static string LimitarLength(this string texto, int maxLength)
        {
            if (texto.Length <= maxLength)
            {
                return texto;
            }

            return texto.Substring(0, maxLength);
        }

    }
}