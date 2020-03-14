using EstacionamentoEAI.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EstacionamentoEAI.Models
{
    public class Estacionamento
    {
        public Usuario Usuario { get; set; }        
        public Veiculo Veiculo { get; set; }
    }
}