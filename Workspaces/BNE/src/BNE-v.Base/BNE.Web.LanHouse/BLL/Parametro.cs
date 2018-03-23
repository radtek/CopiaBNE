using System;
using System.Linq;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class Parametro
    {
        public static decimal SalarioMinimo()
        {
            return RetornarDecimal(Enumeradores.Parametro.SalarioMinimoNacional);
        }

        public static int IdentificadorPlanoVIP()
        {
            return RetornarInt32(Enumeradores.Parametro.VendaPlanoLAN_PlanoBasico);
        }

        public static int NumeroResultadosAutoCompleteFuncao()
        {
            return RetornarInt32(Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao);
        }

        public static int NumeroResultadosAutoCompleteCidade()
        {
            return RetornarInt32(Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade);
        }

        public static int NumeroLetrasInicioAutoCompleteCidade()
        {
            return RetornarInt32(Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteCidade);
        }

        public static int NumeroLetrasInicioAutoCompleteFuncao()
        {
            return RetornarInt32(Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao);
        }

        public static string CartaApresentacao(TAB_Filial objFilial)
        {
            var entity = new LanEntities();

            var objParametroFilial = entity.TAB_Parametro_Filial.FirstOrDefault(e => e.Idf_Filial == objFilial.Idf_Filial && e.Idf_Parametro == (int)Enumeradores.Parametro.CartaApresentacao);
            if (objParametroFilial != null && !string.IsNullOrEmpty(objParametroFilial.Vlr_Parametro))
                return objParametroFilial.Vlr_Parametro;

            return RetornarString(Enumeradores.Parametro.CartaApresentacao);
        }

        public static string CartaAgradecimentoCandidatura(TAB_Filial objFilial)
        {
            var entity = new LanEntities();
            
            var objParametroFilial = entity.TAB_Parametro_Filial.FirstOrDefault(e => e.Idf_Filial == objFilial.Idf_Filial && e.Idf_Parametro == (int)Enumeradores.Parametro.CartaAgradecimentoCandidatura);
            if (objParametroFilial != null && !string.IsNullOrEmpty(objParametroFilial.Vlr_Parametro))
                return objParametroFilial.Vlr_Parametro;

            return RetornarString(Enumeradores.Parametro.CartaAgradecimentoCandidatura);
        }

        #region Métodos privados

        private static int RetornarInt32(Enumeradores.Parametro parametro)
        {
            return Int32.Parse(RetornarParametro(parametro));
        }

        private static string RetornarString(Enumeradores.Parametro parametro)
        {
            return RetornarParametro(parametro);
        }

        private static decimal RetornarDecimal(Enumeradores.Parametro parametro)
        {
            return Decimal.Parse(RetornarParametro(parametro));
        }

        private static string RetornarParametro(Enumeradores.Parametro parametro)
        {
            var entity = new LanEntities();

            var objParametro =
               entity
                   .TAB_Parametro
                   .FirstOrDefault(p => p.Idf_Parametro == (int)parametro);

            if (objParametro == null)
                throw new InvalidOperationException(String.Format("Não encontrado {0}", parametro.ToString()));

            return objParametro.Vlr_Parametro;
        }


        #endregion Métodos privados

    }
}