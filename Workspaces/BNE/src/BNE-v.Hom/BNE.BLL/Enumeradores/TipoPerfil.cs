using System;
using System.Reflection;
namespace BNE.BLL.Enumeradores
{

    #region TipoPerfil
    public enum TipoPerfil
    {
        Candidato = 1,
        Empresa = 2,
        Interno = 3
    }
    #endregion

    #region TipoPerfilAttribute
    public class TipoPerfilAttribute : Attribute
    {
        public TipoPerfil TipoPerfil { get; private set; }

        public TipoPerfilAttribute(TipoPerfil tipoPerfil)
        {
            this.TipoPerfil = tipoPerfil;
        }

        #region ExtractAttribute
        /// <summary>This method will peer into an enum and extract this class if it exists.</summary>
        /// <param name="enumItem">The enumeration which contains the DatabaseConnectionAttribute.</param>
        /// <returns>The current object or null</returns>
        public static TipoPerfilAttribute ExtractAttribute(object enumItem)
        {
            TipoPerfilAttribute retVal = null;
            try
            {
                FieldInfo fieldInfo = enumItem.GetType().GetField(enumItem.ToString());
                TipoPerfilAttribute[] attributes = (TipoPerfilAttribute[])fieldInfo.GetCustomAttributes(typeof(TipoPerfilAttribute), false);

                if (attributes != null)
                    if (attributes.Length > 0)
                        retVal = attributes[0];
            }
            catch (NullReferenceException)
            {
                //Occurs when we attempt to get description of an enum value that does not exist
            }
            return retVal;
        }
        #endregion

    }
    #endregion

}