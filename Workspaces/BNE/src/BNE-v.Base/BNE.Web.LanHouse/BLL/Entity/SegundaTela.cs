using System;
using System.Globalization;

namespace BNE.Web.LanHouse.BLL.Entity
{
    public sealed class SegundaTela
    {

        #region Propriedades
        public string NomeCompleto { get; private set; }
        public DateTime DataNasc { get; private set; }
        public int Sexo { get; private set; }
        public string DDD { get; private set; }
        public string NumCelular { get; private set; }
        public string CodigoValidacaoCelular { get; private set; }
        #endregion

        public SegundaTela(Models.ModelAjaxSegundaTela model)
        {
            NomeCompleto = Code.Helper.AjustarString(model.NomeCompleto);
            DataNasc = Convert.ToDateTime(model.DataNasc, CultureInfo.GetCultureInfo("pt-br"));
            Sexo = model.Sexo;
            DDD = model.DDD;
            NumCelular = Code.Helper.RetirarFormatacaoTelefone(model.NumCelular);
            CodigoValidacaoCelular = model.CodigoValidacaoCelular;
        }
    }
}