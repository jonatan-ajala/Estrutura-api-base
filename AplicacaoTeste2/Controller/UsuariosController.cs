using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiAgendaDocumentos.Context;
using ApiAgendaDocumentos.Models;
using ApiAgendaDocumentos.Util;
using Microsoft.AspNetCore.Authorization;

namespace ApiAgendaDocumentos.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ConexaoContext _context;

        public UsuariosController(ConexaoContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            return await _context.usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
           
            if (id != usuario.usuario_id)
            {
                return BadRequest();
            }

            if (usuario.senha != string.Empty)
                usuario.senha = Utilitarios.Sha512(usuario.senha);
            else
                usuario.senha = (from u in _context.usuarios where u.usuario_id == id select u.senha).FirstOrDefault();

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            usuario.senha = Utilitarios.Sha512(usuario.senha);

            _context.usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.usuario_id }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpPost("DeleteUsuario")]
        public async Task<ActionResult<Usuario>> DeleteUsuario(int[] id)
        {
            foreach (int aux in id)
            {
                Usuario usuario = _context.usuarios.Find(aux);
                if (usuario == null)
                {
                    return NotFound();
                }

                _context.usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return Ok(id);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> DeleteUsuario(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        [HttpPost("PostPerfil")]
        public async Task<ActionResult> PostPerfil(ModelPerfil mp)
        {
            var dados = await (from u in _context.usuarios where u.usuario_id == mp.usuario_id select u).FirstOrDefaultAsync();

            if (mp.senha != null)
                dados.senha = Utilitarios.Sha512(mp.senha);
            if (mp.avatar != null)
                dados.avatar = Convert.FromBase64String(mp.avatar.Replace("data:image/jpeg;base64,", String.Empty));
            
            await _context.SaveChangesAsync();

            return Ok(mp);
        }

        private bool UsuarioExists(int id)
        {
            return _context.usuarios.Any(e => e.usuario_id == id);
        }
    }

    public class ModelPerfil
    {
        public int usuario_id { get; set; }
        public string senha { get; set; }
        public string avatar { get; set; }
    }
}
