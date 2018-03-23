//-- Data: 01/07/2015 18:05
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class PlanoAdquiridoContratoUsuario // Tabela: BNE_Plano_Adquirido_Contrato_Usuario
    {

        #region Consultas

        #region Sppossuecontratoaceite
        private const string Sppossuecontratoaceite = @"
        SELECT  PACU.*
        FROM    bne.BNE_Plano_Adquirido PA
		        INNER JOIN BNE.BNE_Plano_Adquirido_Contrato PAC ON PAC.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
		        INNER JOIN BNE.BNE_Plano_Adquirido_Contrato_Usuario PACU ON PACU.Idf_Plano_Adquirido_Contrato = PAC.Idf_Plano_Adquirido_Contrato
        WHERE   PA.Dta_Inicio_Plano > @DataImplantacaoAceiteContrato
                AND PA.Idf_Plano_Situacao = 1
		        AND PA.Idf_Plano_Adquirido = @Idf_Plano_Adquirido";
        #endregion

        #endregion

        #region Métodos

        #region RecuperarAceites
        public static List<PlanoAdquiridoContratoUsuario> RecuperarAceites(PlanoAdquirido objPlanoAdquirido)
        {
            var parametro = Convert.ToDateTime(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DataImplantacaoAceiteContrato));
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlanoAdquirido.IdPlanoAdquirido },
                    new SqlParameter { ParameterName = "@DataImplantacaoAceiteContrato", SqlDbType = SqlDbType.DateTime, Size = 4, Value = parametro }
                };

            var lista = new List<PlanoAdquiridoContratoUsuario>();
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sppossuecontratoaceite, parms))
            {
                var objPlanoAdquiridoContratoUsuario = new PlanoAdquiridoContratoUsuario();
                while (SetInstanceNotDipose(dr, objPlanoAdquiridoContratoUsuario))
                {
                    lista.Add(objPlanoAdquiridoContratoUsuario);
                    objPlanoAdquiridoContratoUsuario = new PlanoAdquiridoContratoUsuario();
                }
            }

            return lista;
        }
        #endregion

        #region SetInstanceNotDipose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objPlanoAdquiridoContratoUsuario">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstanceNotDipose(IDataReader dr, PlanoAdquiridoContratoUsuario objPlanoAdquiridoContratoUsuario)
        {
            if (dr.Read())
            {
                objPlanoAdquiridoContratoUsuario._idPlanoAdquiridoContratoUsuario = Convert.ToInt32(dr["Idf_Plano_Adquirido_Contrato_Usuario"]);
                objPlanoAdquiridoContratoUsuario._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                objPlanoAdquiridoContratoUsuario._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objPlanoAdquiridoContratoUsuario._planoAdquiridoContrato = new PlanoAdquiridoContrato(Convert.ToInt32(dr["Idf_Plano_Adquirido_Contrato"]));

                objPlanoAdquiridoContratoUsuario._persisted = true;
                objPlanoAdquiridoContratoUsuario._modified = false;

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #endregion

    }
}