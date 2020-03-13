﻿using System.Collections.Generic;

namespace EstacionamentoEAI.Definition
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
