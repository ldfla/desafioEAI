using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstacionamentoEAI.Definition
{
    public class Semana
    {
        private DayOfWeek _inicioDaSemana;

        public DateTime DataInicial { get; private set; }
        public DateTime DataFinal { get; private set; }

        public Semana(DateTime data, DayOfWeek inicioDaSemana)
        {
            if (data > DateTime.Now)
                data = DateTime.Now;

            _inicioDaSemana = inicioDaSemana;

            int offset = _inicioDaSemana - data.DayOfWeek;
            data = data.AddDays(offset);
            DataInicial = data;
            DataFinal = DataInicial.AddDays(7);

        }
    }
}
