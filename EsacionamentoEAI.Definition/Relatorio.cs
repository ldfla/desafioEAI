using System;
using System.Collections.Generic;

namespace EstacionamentoEAI.Definition
{
    public class Relatorio
    {        
        public Estacionamento Estacionamento { get; }
        public Semana Semana { get; }

        public int SemanasAnteriores { get; set; }
        public int SemanasPosteriores { get; set; }
        public bool IncluirCarrosEstacionados { get; set; }
        public List<Registro> Registros { get; set; }
        public List<Veiculo> VeiculosFrequentes { get; set; }
        public List<RelatorioItem> View { get; set; }

        /// <summary>
        /// Construtor para os relatorios.
        /// </summary>
        /// <param name="semana">Semana que será extraido o Relatorio</param>
        /// <param name="estacionamento">Estacionamento que será consultado</param>
        /// <param name="carrosEstacionados">True para incluir no relatório os veiculos que ainda estão estacionados</param>
        public Relatorio(Semana semana, Estacionamento estacionamento)
        {
            Semana = semana;
            Estacionamento = estacionamento;
            IncluirCarrosEstacionados = true;
            SemanasAnteriores = 0;
            SemanasPosteriores = 0;
        }

    }
}
