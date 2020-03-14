﻿namespace EstacionamentoEAI.Definition
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public Modelo Modelo { get; set; }
        public string Observacao { get; set; }
        public Cliente Cliente { get; set; }
    }
}