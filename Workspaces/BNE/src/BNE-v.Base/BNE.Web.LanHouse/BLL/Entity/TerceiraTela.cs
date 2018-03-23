using System;
using System.Globalization;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL.Entity
{
    public sealed class TerceiraTela
    {

        #region Propriedades
        public decimal Cpf { get; private set; }
        public string Email { get; private set; }
        public int? Cargo { get; private set; }
        public string DescricaoCargo { get; private set; }
        public decimal Salario { get; private set; }
        #endregion

        public TerceiraTela(Models.ModelAjaxTerceiraTela model)
        {
            Cpf = Code.Helper.ConverterCpfParaDecimal(model.Cpf);
            Email = model.Email;

            TAB_Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(model.Cargo, out objFuncao))
            {
                DescricaoCargo = String.Empty;
                Cargo = objFuncao.Idf_Funcao;
            }
            else
            {
                Cargo = null;
                DescricaoCargo = model.Cargo;
            }

            Salario = Convert.ToDecimal(model.Salario, CultureInfo.GetCultureInfo("pt-br"));
        }
    }
}