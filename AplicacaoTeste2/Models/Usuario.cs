using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAgendaDocumentos.Models
{
    public class Usuario
    {
        [Key]
        public int usuario_id { get; set; }
        public string nome { get; set; }
        public string login { get; set; }
        public string senha { get; set; }   
        public string email { get; set; }
        public byte[] avatar { get; set; }
    }
}
