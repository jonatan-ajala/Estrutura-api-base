using Microsoft.EntityFrameworkCore;

namespace ApiAgendaDocumentos.Context
{
    public partial class ConexaoContext : DbContext
    {
        public ConexaoContext(DbContextOptions<ConexaoContext> options) : base(options) { }
        public DbSet<ApiAgendaDocumentos.Models.Usuario> usuarios { get; set; }

    }
}
