using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Services.Test
{
    public class CustomAllinEmailQuemMeViu : AllinEmailQuemMeViu
    {
        protected override IEnumerable<BLL.CurriculoQuemMeViu.RelatorioQuemMeViuModel> GetRelatorioQuantidadePorCurriculo(int limit)
        {
            yield return new BLL.CurriculoQuemMeViu.RelatorioQuemMeViuModel { IdCurriculo = 3353507, Total = 11 };
        }
    }
}
