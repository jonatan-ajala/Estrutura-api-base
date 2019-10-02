using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ApiAgendaDocumentos.Context;
using ApiAgendaDocumentos.Util;

namespace ApiAgendaDocumentos.Controllers
{
    public partial class ModelAutenticar
    {
        public string login { get; set; }
        public string senha { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class LoginAcessoController : ControllerBase
    {
        private readonly ConexaoContext _context;
        private readonly IConfiguration _configuration;

        public LoginAcessoController(ConexaoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Autenticar")]
        public ActionResult Autenticar(ModelAutenticar ma)
        {
            ma.senha = Utilitarios.Sha512(ma.senha);

            var aux = _configuration.GetSection("Token").Get<List<string>>();

            string token = Utilitarios.CreateToken(ma.login, aux[3].ToString(), aux[2].ToString(), aux[0].ToString(), Convert.ToInt32(aux[4]));

            var dados = (from u in _context.usuarios
                         where u.login == ma.login
                         && u.senha == ma.senha
                         select new
                         {
                             u.usuario_id,
                             u.nome,
                             u.login,
                             u.email,
                             token,
                             u.avatar
                         }).FirstOrDefault();

            if (dados == null)
            {
                return NotFound();
            }

            return Ok(dados);
        }
    }
}