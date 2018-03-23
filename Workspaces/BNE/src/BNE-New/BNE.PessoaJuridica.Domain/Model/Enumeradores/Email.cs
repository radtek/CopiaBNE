using System.ComponentModel;

namespace BNE.PessoaJuridica.Domain.Model.Enumeradores
{
    public enum Email
    {
        [Description("f4203125-a12c-426e-9c3d-0851bfe2ba42")]
        InclusaoUsuarioAdicionalEmpresa
    }

    public static class EmailExtensions
    {
        public static string GetDescription(this Email val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
