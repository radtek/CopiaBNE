//-- Data: 22/09/2017 09:29
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class PlanoDesconto // Tabela: BNE_Plano_Desconto
    {

        #region [Consultas]

        #region [spRecuperaDesconto]
        private const string spRecuperaDesconto = @"declare @dtaUtilizado datetime = (select top 1 dta_utilizacao from bne.bne_plano_desconto with(nolock)
				                                                    where idf_plano_adquirido = @Idf_Plano_Adquirido
					                                                    and dta_utilizacao is not null and flg_inativo = 1
					                                                    order by dta_utilizacao desc)
                                                    if(@dtaUtilizado !=null)
                                                    begin
                                                    select top 1 pd.*, pf.nme_pessoa from BNE.BNE_Plano_Desconto pd with(nolock)
                                                    join bne.tab_usuario_filial_perfil ufp with(nolock) on ufp.Idf_Usuario_Filial_Perfil = pd.Idf_Usuario_Filial_Perfil
                                                    join bne.tab_pessoa_Fisica pf with(nolock) on pf.idf_pessoa_Fisica = ufp.idf_pessoa_Fisica
                                                            where pd.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
                                                            and pd.Flg_Inativo = 0 and pd.Dta_Utilizacao is null
		                                                    and @dtaUtilizado < pd.dta_cadastro
		                                                    order by pd.dta_cadastro desc
                                                    end
                                                    else
                                                    begin
                                                    select top 1 pd.*, pf.nme_pessoa from BNE.BNE_Plano_Desconto pd with(nolock)
                                                    join bne.tab_usuario_filial_perfil ufp with(nolock) on ufp.Idf_Usuario_Filial_Perfil = pd.Idf_Usuario_Filial_Perfil
                                                    join bne.tab_pessoa_Fisica pf with(nolock) on pf.idf_pessoa_Fisica = ufp.idf_pessoa_Fisica
                                                            where pd.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
                                                            and pd.Flg_Inativo = 0 and pd.Dta_Utilizacao is null
		                                                    order by pd.dta_cadastro desc
                                                    end";
        #endregion


        #endregion
        #region [RecuperaDesconto]
        /// <summary>
        /// Recupera o desconto Não utilizado para a próxima parcela do plano adquirido
        /// </summary>
        /// <param name="idPlanoAdquirido"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool RecuperaDesconto(int idPlanoAdquirido, out PlanoDesconto objPlanoDesconto, out string UsuarioInterno)
        {
            objPlanoDesconto = new PlanoDesconto();
            UsuarioInterno = string.Empty;
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter{ParameterName = "@Idf_Plano_Adquirido", SqlDbType= SqlDbType.Int,Value=idPlanoAdquirido}
            };
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spRecuperaDesconto, parametros))
            {
                if (dr.Read())
                {
                    objPlanoDesconto._idPlanoDesconto = Convert.ToInt32(dr["Idf_Plano_Desconto"]);
                    objPlanoDesconto._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
                    objPlanoDesconto._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    objPlanoDesconto._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objPlanoDesconto._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Vlr_Desconto"] != DBNull.Value)
                        objPlanoDesconto._valorDesconto = Convert.ToDecimal(dr["Vlr_Desconto"]);
                    if (dr["Dta_Utilizacao"] != DBNull.Value)
                        objPlanoDesconto._dataUtilizacao = Convert.ToDateTime(dr["Dta_Utilizacao"]);
                    if (dr["Idf_Plano_Parcela"] != DBNull.Value)
                        objPlanoDesconto._planoParcela = new PlanoParcela(Convert.ToInt32(dr["Idf_Plano_Parcela"]));
                    if (dr["Dta_Atualizacao"] != DBNull.Value)
                        objPlanoDesconto._dataAtualizacao = Convert.ToDateTime(dr["Dta_Atualizacao"]);

                    objPlanoDesconto._persisted = true;
                    objPlanoDesconto._modified = false;


                    UsuarioInterno = dr["nme_pessoa"].ToString();

                    return true;

                }
            }
            return false;
        }
        #endregion

    }
}