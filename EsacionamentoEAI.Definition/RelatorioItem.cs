using System;
using System.Collections.Generic;

namespace EstacionamentoEAI.Definition
{
    public class RelatorioItem
    {
        public DateTime Data { get; set; }

        public int TotalVeiculos { get; set; }

        public int ClientesUnicos { get; set; }

        public Decimal Receita { get; set; }
    }
}