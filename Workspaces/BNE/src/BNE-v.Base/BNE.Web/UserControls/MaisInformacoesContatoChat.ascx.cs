using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Chat.Helper;

namespace BNE.Web.UserControls
{
    public partial class MaisInformacoesContatoChat : System.Web.UI.UserControl
    {
        private static readonly SetValueOrDefaultFact<HardConfig<int>, int> CacheInformacoesExtrasCurriculoEmMinutos =
       new HardConfig<int>("chat_cache_mais_informacoes_do_curriculo_em_minutos", 10).Wrap(a => a.Value);

        private static readonly CooperativeTimeCache<int, BLL.DTO.Curriculo> CacheInfo =
            new CooperativeTimeCache<int, BLL.DTO.Curriculo>(() =>
                TimeSpan.FromMinutes(CacheInformacoesExtrasCurriculoEmMinutos.Value));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void PreencherCampos(int curriculoId)
        {
            if (curriculoId <= 0)
                return;

            var curDto = CacheInfo.GetOrAdd(curriculoId, keyFactory =>
            {
                var res = Curriculo.CarregarCurriculoDTO(keyFactory, true, false, false, false, false, false, false, false);
                return res;
            });

            if (curDto == null)
                return;

            this.lblIdCurriculo.Text = curriculoId.ToString(CultureInfo.InvariantCulture);
            this.lblFuncaoCurriculo.Text = PegarFuncao(curDto);
            this.lblTelefoneCurriculo.Text = PegarTelefone(curDto);
        }

        private string PegarTelefone(BLL.DTO.Curriculo curDto)
        {
            var celularTexto = (curDto.NumeroCelular ?? "").Trim();

            if (celularTexto.Length > 4)
            {
                if (celularTexto.Length > 8)
                {
                    celularTexto = new string(celularTexto.Take(5).ToArray()) + "-" +
                                   new string(celularTexto.Skip(5).ToArray());
                }
                else
                {
                    celularTexto = new string(celularTexto.Take(4).ToArray()) + "-" +
                                  new string(celularTexto.Skip(4).ToArray());
                }
            }

            return string.Format("({0}) {1}", curDto.NumeroDDDCelular ?? "", celularTexto);
        }

        private string PegarFuncao(BLL.DTO.Curriculo curDto)
        {
            if (curDto.FuncoesPretendidas == null)
                return "";

            var funcao = curDto.FuncoesPretendidas.FirstOrDefault();

            if (funcao == null)
                return "";

            return funcao.NomeFuncaoPretendida ?? "";
        }
    }
}