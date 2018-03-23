using System;
using System.Collections.Generic;
using System.Linq;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class Companhia
    {
        private static LanEntities entity = new LanEntities();

        #region SelectByID
        public static LAN_Companhia SelectByID(int id)
        {
            return entity.LAN_Companhia.FirstOrDefault(l => l.Idf_Companhia == id);
        }
        #endregion

        public static List<LAN_Companhia> SelectAll()
        {
            return entity.LAN_Companhia.ToList();
        }

        #region RecuperarCartaApresentacao
        public static bool RecuperarCartaApresentacao(decimal numeroCNPJ, out string carta)
        {
            carta = string.Empty;

            LAN_Companhia objCompanhia =
                entity
                    .LAN_Companhia
                    .FirstOrDefault(c => c.Num_CNPJ == numeroCNPJ);

            if (objCompanhia != null && !string.IsNullOrEmpty(objCompanhia.Des_Carta_Apresentacao))
            {
                carta = objCompanhia.Des_Carta_Apresentacao;
            }
            else
            {
                var bne = entity.LAN_Companhia.FirstOrDefault(f => f.TAB_Filial.Num_CNPJ == numeroCNPJ);

                if (bne != null)
                    carta = Parametro.CartaApresentacao(bne.TAB_Filial);
            }

            return !String.IsNullOrEmpty(carta);
        }
        #endregion

        #region RecuperarCartaBoasVindas
        public static bool RecuperarCartaBoasVindas(decimal numeroCNPJ, out string carta)
        {
            carta = string.Empty;

            LAN_Companhia objCompanhia =
                entity
                    .LAN_Companhia
                    .FirstOrDefault(c => c.Num_CNPJ == numeroCNPJ);

            if (objCompanhia != null && !string.IsNullOrEmpty(objCompanhia.Des_Carta_Agradecimento))
            {
                carta = objCompanhia.Des_Carta_Agradecimento;
            }
            else
            {
                var bne = entity.LAN_Companhia.FirstOrDefault(f => f.TAB_Filial.Num_CNPJ == numeroCNPJ);

                if (bne != null)
                {
                    carta = Parametro.CartaAgradecimentoCandidatura(bne.TAB_Filial);
                    carta = carta.Replace("{Empresa}", bne.TAB_Filial.Nme_Fantasia);
                }
            }

            return !String.IsNullOrEmpty(carta);
        }
        #endregion

        #region CarregarPorCnpj
        public static bool CarregarPorCnpj(decimal numeroCNPJ, out LAN_Companhia objCompanhia)
        {
            objCompanhia = entity.LAN_Companhia.FirstOrDefault(c => c.Num_CNPJ == numeroCNPJ) ?? entity.LAN_Companhia.FirstOrDefault(f => f.TAB_Filial.Num_CNPJ == numeroCNPJ);

            return objCompanhia != null;
        }
        #endregion

        #region RecuperarFoto
        public static byte[] RecuperarFoto(decimal numeroCNPJ)
        {
            var lan = entity.LAN_Companhia.FirstOrDefault(c => c.Num_CNPJ == numeroCNPJ);

            if (lan != null && lan.Img_Logo != null)
                return lan.Img_Logo;

            var bne = entity.LAN_Companhia.FirstOrDefault(f => f.TAB_Filial.Num_CNPJ == numeroCNPJ);

            if (bne != null && bne.TAB_Filial.TAB_Filial_Logo.Img_Logo != null && bne.TAB_Filial.TAB_Filial_Logo.Flg_Inativo.Equals(false))
                return bne.TAB_Filial.TAB_Filial_Logo.Img_Logo;

            return null;
        }
        #endregion

        #region Salvar
        public static bool Salvar(int idCompanhia, decimal cnpj, string nome, byte[] logo)
        {
            bool retorno = false;

            LAN_Companhia objCompanhia = entity
                   .LAN_Companhia
                   .First(lc =>
                       lc.Idf_Companhia == idCompanhia);

            objCompanhia.Num_CNPJ = cnpj;
            objCompanhia.Nme_Companhia = nome;

            entity.Entry(objCompanhia).Property(c => c.Num_CNPJ).IsModified = true;
            entity.Entry(objCompanhia).Property(c => c.Nme_Companhia).IsModified = true;

            if (logo != null)
            {
                objCompanhia.Img_Logo = logo;
                entity.Entry(objCompanhia).Property(c => c.Img_Logo).IsModified = true;
            }

            if (entity.SaveChanges() == 1)
                retorno = true;

            return retorno;
        }
        #endregion
        
    }
}