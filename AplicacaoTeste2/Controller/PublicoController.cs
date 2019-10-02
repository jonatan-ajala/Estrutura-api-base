using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAgendaDocumentos.Context;
using ApiAgendaDocumentos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoTeste2.Controller
{
    /// <summary>
    /// Criação de rotas públicas para serem consumidas no Site
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class PublicoController : ControllerBase
    {
        private readonly ConexaoContext _context;

        public PublicoController(ConexaoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            return await _context.usuarios.ToListAsync();
        }
    }
}