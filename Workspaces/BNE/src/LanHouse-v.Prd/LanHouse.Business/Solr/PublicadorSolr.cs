using BNE.BLL.AsyncServices;
using AsyncEnums = BNE.BLL.AsyncServices.Enumeradores;
using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace LanHouse.Business.Solr
{
    public class PublicadorSolr
    {
        public static void EnviarParaFila(int IdCurriculo)
        {
            var parametros = new ParametroExecucaoCollection
            {
                {"idCurriculo", "Curriculo", IdCurriculo.ToString(CultureInfo.InvariantCulture), IdCurriculo.ToString(CultureInfo.InvariantCulture)}
            };

            ProcessoAssincrono.IniciarAtividade(AsyncEnums.TipoAtividade.PublicacaoCurriculo, parametros);
        }
    }
}
