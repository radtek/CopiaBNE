//-- Data: 01/07/2015 15:40
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.EL;

namespace BNE.BLL
{
    public partial class PlanoAdquiridoContrato // Tabela: BNE_Plano_Adquirido_Contrato
    {

        #region Consultas

        #region Sprecuperarcontrato
        private const string Sprecuperarcontrato = @"
        SELECT  ISNULL(PAC.Des_Contrato, TC.Des_Template_Contrato) as Contrato
        FROM    bne.BNE_Plano_Adquirido PA
		        LEFT JOIN BNE.BNE_Plano_Adquirido_Contrato PAC ON PAC.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
                LEFT JOIN plataforma.TAB_Template_Contrato TC ON PAC.Idf_Template_Contrato = TC.Idf_Template_Contrato
        WHERE   1 = 1 /* Dta_Inicio_Plano > @DataImplantacaoAceiteContrato */
		        AND PA.Idf_Plano_Adquirido = @Idf_Plano_Adquirido";
        #endregion

        #region Spexsitecontrato
        private const string Spexsitecontrato = @"
        SELECT  PAC.*
        FROM    bne.BNE_Plano_Adquirido PA
		        INNER JOIN BNE.BNE_Plano_Adquirido_Contrato PAC ON PAC.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
                INNER JOIN plataforma.TAB_Template_Contrato TC ON PAC.Idf_Template_Contrato = TC.Idf_Template_Contrato
        WHERE   1 = 1 /* Dta_Inicio_Plano > @DataImplantacaoAceiteContrato */
		        AND PA.Idf_Plano_Adquirido = @Idf_Plano_Adquirido";
        #endregion

        #endregion

        #region Métodos

        #region ExisteContrato
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objPlanoAdquirido"></param>
        /// <returns></returns>
        public static bool ExisteContrato(PlanoAdquirido objPlanoAdquirido, out PlanoAdquiridoContrato objPlanoAdquiridoContrato, SqlTransaction trans)
        {
            //var parametro = Convert.ToDateTime(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DataImplantacaoAceiteContrato));
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlanoAdquirido.IdPlanoAdquirido },
                    //new SqlParameter { ParameterName = "@DataImplantacaoAceiteContrato", SqlDbType = SqlDbType.DateTime, Size = 4, Value = parametro }
                };

            objPlanoAdquiridoContrato = new PlanoAdquiridoContrato();
            using (var dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spexsitecontrato, parms))
            {
                if (!SetInstance(dr, objPlanoAdquiridoContrato))
                    objPlanoAdquiridoContrato = null;
            }

            return objPlanoAdquiridoContrato != null;
        }
        #endregion

        #region RecuperarContrato
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objPlanoAdquirido"></param>
        /// <returns></returns>
        public static string RecuperarContrato(PlanoAdquirido objPlanoAdquirido)
        {
            //var parms = new List<SqlParameter>
            //    {
            //        new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlanoAdquirido.IdPlanoAdquirido },
            //    };

            //var retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperarcontrato, parms).ToString();


            //if (string.IsNullOrWhiteSpace(retorno))
            //{
                objPlanoAdquirido.Plano.CompleteObject();

                var templateContrato = RetornaTemplateContrato(objPlanoAdquirido.Plano);
                return BLL.TemplateContrato.RecuperarContratoVigente(templateContrato).DescricaoTemplateContrato;
            //}
                

            //return retorno;
        }
        #endregion

        /// <summary>
        /// Retorna o Template do Contrato de acordo com o Plano
        /// </summary>
        /// <param name="plano"></param>
        /// <returns></returns>
        public static int RetornaTemplateContrato(Plano plano)
        {
            var templateContrato = BLL.TemplateContrato.LoadObject(plano.TemplateContrato.IdTemplateContrato);
            return templateContrato != null ? templateContrato.IdTemplateContrato : 1;
        }

        #region SalvarAceite
        public static bool SalvarAceite(PlanoAdquirido objPlanoAdquirido, UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        PlanoAdquiridoContrato objPlanoAdquiridoContrato;
                        if (!ExisteContrato(objPlanoAdquirido, out objPlanoAdquiridoContrato, trans))
                        {
                            TemplateContrato template;
                            objPlanoAdquirido.Plano.CompleteObject();

                            var templateContrato = PlanoAdquiridoContrato.RetornaTemplateContrato(objPlanoAdquirido.Plano);
                            template = BLL.TemplateContrato.RecuperarContratoVigente(templateContrato, trans);
                           
                            objPlanoAdquiridoContrato = new PlanoAdquiridoContrato
                            {                                
                                TemplateContrato = template,
                                PlanoAdquirido = objPlanoAdquirido
                            };
                            objPlanoAdquiridoContrato.Save(trans);
                        }

                        var objPlanoAdquiridoContratoUsuario = new PlanoAdquiridoContratoUsuario
                        {
                            PlanoAdquiridoContrato = objPlanoAdquiridoContrato,
                            UsuarioFilialPerfil = objUsuarioFilialPerfil
                        };
                        objPlanoAdquiridoContratoUsuario.Save(trans);

                        trans.Commit();

                        try
                        {
                            UsuarioFilial objUsuarioFilial;
                            if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                            {
                                var contrato = GerarContratoPlano.ContratoPadraoPdf(objPlanoAdquirido.FlagRecorrente ? objPlanoAdquirido.ContratoPlanoRecorrenteCia(objPlanoAdquirido) :  objPlanoAdquirido.Contrato());
                                EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar("Contrato Plano BNE", "Segue contrato em anexo",null, "financeiro@bne.com.br", objUsuarioFilial.EmailComercial, "Contrato_BNE.pdf", contrato);
                            }
                        }
                        catch (Exception ex)
                        {
                            GerenciadorException.GravarExcecao(ex, "Falha ao enviar contrato");
                        }
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #endregion

    }
}