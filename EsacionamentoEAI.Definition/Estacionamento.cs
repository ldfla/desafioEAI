using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstacionamentoEAI.Definition
{
    public class Estacionamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Responsavel { get; set; }
        public string Telefone { get; set; }
        public int NumeroDeVagas { get; set; }
        public Decimal CustoHora { get; set; }
        public Usuario Manager { get; set; }
    }
}
