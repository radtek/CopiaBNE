using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Services.Mobile
{
    public static class BoundaryHelper
    {
        public static string RemoverAcentos(string texto)
        {
            return BLL.Custom.Helper.RemoverAcentos(texto);
        }

        public static string EliminarCaracteres(string entrada)
        {
            String acentos = "áàäâãéèêëíìïîóòöõôùúüûçÁÀÄÂÃÉÈÊËÍÌÏÎÓÒÖÕÔÙÚÜÛÇ";
            String semAcentos = "aaaaaeeeeiiiiooooouuuucAAAAAEEEEIIIIOOOOOUUUUC";
            String invalidos = "ºª~^|§";

            // Remove acentos inválidos
            for (int i = 0; i < acentos.Length; i++)
            {
                entrada = entrada.Replace(acentos[i], semAcentos[i]);
            }
            // Remove caracteres inválidos
            for (int i = 0; i < invalidos.Length; i++)
            {
                entrada = entrada.Replace(Convert.ToString(invalidos[i]), String.Empty);
            }

            return entrada;
        }
    }
}