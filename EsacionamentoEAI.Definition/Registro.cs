using System;

namespace EstacionamentoEAI.Definition
{
    public class Registro
    {
        public int Id { get; set; }
        public Estacionamento Estacionamento { get; set; }
        public Veiculo Veiculo { get; set; }
        public DateTime DataDeEntrada { get; set; }
        public Usuario UsuarioEntrada { get; set; }
        public DateTime DataDeSaida { get; set; }
        public Usuario UsuarioSaida { get; set; }
        public Decimal Valor { get; set; }
        public Decimal CustoHora { get; set; }
    }
}
