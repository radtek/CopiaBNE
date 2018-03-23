
namespace BNE.Web.LanHouse.BLL.Entity
{
    public class SextaTela
    {

        #region Propriedades
        public int IdEstadoCivil { get; private set; }
        public string Cep { get; private set; }
        public string TelefoneRecadoFone { get; private set; }
        public string TelefoneRecadoDDD { get; private set; }
        public string FalarCom { get; private set; }
        #endregion

        #region Construtor
        public SextaTela(Models.ModelAjaxSextaTela model)
        {
            IdEstadoCivil = model.IdEstadoCivil;
            Cep = model.Cep.Replace("-", string.Empty).Replace(".", string.Empty);
            TelefoneRecadoDDD = model.TelefoneRecadoDDD;
            TelefoneRecadoFone = model.TelefoneRecadoFone;
            FalarCom = model.FalarCom;
        }
        #endregion

    }
}