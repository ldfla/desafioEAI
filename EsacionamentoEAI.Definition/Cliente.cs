using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsacionamentoEAI.Definition
{
    class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Documento { get; set; }
        public string Telefone { get; set; }
        public List<Veiculo> Veiculos { get; set; }
    }
}
