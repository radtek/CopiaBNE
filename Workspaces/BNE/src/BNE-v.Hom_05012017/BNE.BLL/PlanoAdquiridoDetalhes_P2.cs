//-- Data: 11/03/2011 09:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PlanoAdquiridoDetalhes: ICloneable // Tabela: BNE_Plano_Adquirido_Detalhes
    {
        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consultas

        #region SPSELECTPORPLANOADQUIRIDO
        private const string SPSELECTPORPLANOADQUIRIDO = @"SELECT * FROM BNE_Plano_Adquirido_Detalhes WITH(NOLOCK) WHERE Idf_Plano_Adquirido = @Idf_Plano_Adquirido";
        #endregion

        #region SpExistePlanoAdquiridoDetalhes
        private const string SpExistePlanoAdquiridoDetalhes = @"
        select count(pad.Idf_Plano_Adquirido_Detalhes)
        from bne.bne_plano_adquirido_detalhes pad with(nolock)
        where pad.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
	        and pad.Idf_Vaga = @Idf_Vaga  
        ";
        #endregion

        #region SpRecuperarIdVagaPorPlanoAdquirido
        private const string SpRecuperarIdVagaPorPlanoAdquirido = @"
        select TOP 1 pad.Idf_Vaga 
                , vg.Cod_Vaga
        from bne.bne_plano_adquirido_detalhes pad with(nolock)
            join bne.bne_vaga vg with(nolock) on pad.Idf_Vaga = vg.Idf_Vaga
        where pad.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
        ";
        #endregion

        #region SpVerificarSituacaoPlanoEnvioSMSEmailVaga
        private const string SpVerificarSituacaoPlanoEnvioSMSEmailVaga = @"
        select PAD.*
        from bne.BNE_Plano_Adquirido_Detalhes pad with(nolock)
	        join bne.BNE_Plano_Adquirido pa with(nolock) on pa.Idf_Plano_Adquirido = pad.Idf_Plano_Adquirido
        where pad.Idf_Vaga = @Idf_Vaga
            and pa.Idf_Plano_Situacao = 1
            and pa.Idf_Filial = @Idf_Filial
        ";
        #endregion

        #region spCarregarPlanoAntesDoVencimento
        private const string spCarregarPlanoAntesDoVencimento = @"
               select pa.idf_filial, pa.Idf_Usuario_Filial_Perfil, pa.Idf_Usuario_Filial_Perfil,pa.idf_Plano,
                uf.Idf_Usuario_Filial, pa.Dta_Fim_Plano, pad.Eml_Envio_Boleto, pad.Nme_Res_Plano_Adquirido, pa.idf_plano_adquirido 
                from BNE_Plano_Adquirido pa with(nolock)
                join BNE_Plano_Adquirido_Detalhes pad with(nolock) on pad.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
                join bne_usuario_filial uf with(nolock) on uf.Idf_Usuario_Filial_Perfil = pa.idf_Usuario_Filial_perfil
                where pa.Idf_Filial is not null and pa.idf_plano = @Idf_Plano and pa.idf_Plano_Situacao = 1 --Liberado
                and datediff(day,GETDATE(), pa.Dta_Fim_Plano ) = @Dias";
        #endregion

        #region sp_CarregarPlanoRecorrenteBoleto

        private const string sp_CarregarPlanoRecorrenteBoleto = @"
            SELECT  DISTINCT pa.idf_plano_adquirido,  pa.idf_filial ,
                    pa.Idf_Usuario_Filial_Perfil ,
                    pa.Idf_Usuario_Filial_Perfil ,
                    pa.idf_Plano ,
                    uf.Idf_Usuario_Filial ,
                    pa.Dta_Fim_Plano ,
                    pad.Eml_Envio_Boleto ,
                    pad.Nme_Res_Plano_Adquirido
                   
            FROM    bne.BNE_Plano_Adquirido pa WITH ( NOLOCK )
		            INNER JOIN BNE.BNE_Plano p ON p.Idf_Plano = pa.Idf_Plano
                    INNER JOIN bne.BNE_Plano_Adquirido_Detalhes pad WITH ( NOLOCK ) ON pad.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
                    INNER JOIN bne.bne_usuario_filial uf WITH ( NOLOCK ) ON uf.Idf_Usuario_Filial_Perfil = pa.idf_Usuario_Filial_perfil
		            INNER JOIN BNE.BNE_Plano_Parcela pp ON pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
					
            WHERE   pa.Idf_Filial IS NOT NULL
                    AND pa.idf_Plano_Situacao = 1
		            AND p.flg_recorrente = 1
		            AND pp.Idf_Plano_Parcela_Situacao = 1					
		            AND DATEDIFF(DAY, CONVERT(DATE, GETDATE()), CAST(pa.Dta_Fim_Plano AS DATE)) = @diasAntesVencimento
            ";
        #endregion 
        #endregion

        #region Metodos

        #region CarregarPorPlanoAdquirido
        /// <summary>
        /// Método responsável por carregar uma instancia de PlanoAdquiridoDetalhes através de um plano adquirido
        /// </summary>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPlanoAdquirido(PlanoAdquirido objPlanoAdquirido, out PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes)
        {
            return CarregarPorPlanoAdquirido(null, objPlanoAdquirido, out objPlanoAdquiridoDetalhes);
        }

        /// <summary>
        /// Método responsável por carregar uma instancia de currículo através do
        /// identificar de uma pessoa física.
        /// </summary>
        /// <param name="idPessoaFisica">Identificador da Pessoa Física</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPlanoAdquirido(SqlTransaction trans, PlanoAdquirido objPlanoAdquirido, out PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
            parms[0].Value = objPlanoAdquirido.IdPlanoAdquirido;

            IDataReader dr;
            if (trans == null)
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPLANOADQUIRIDO, parms);
            else
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTPORPLANOADQUIRIDO, parms);

            objPlanoAdquiridoDetalhes = new PlanoAdquiridoDetalhes();
            if (SetInstance(dr, objPlanoAdquiridoDetalhes))
            {
                if (!dr.IsClosed)
                    dr.Close();

                return true;
            }

            if (!dr.IsClosed)
                dr.Close();

            objPlanoAdquiridoDetalhes = null;
            return false;
        }
        #endregion

        #region ExistePladnoAdquiridoDetalhes
        private static bool ExistePladnoAdquiridoDetalhes(int idfPlanoAdquirido, int idfVaga)
        {
            bool retorno = false;

            var parms = new List<SqlParameter> { 
                  new SqlParameter{ ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Value = idfPlanoAdquirido},
                  new SqlParameter{ ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Value = idfVaga}
            };

            if (Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpExistePlanoAdquiridoDetalhes, parms)) > 0)
                retorno = true;


            return retorno;

        }
        #endregion

        #region CriarPladoAdDetalhes
        public static void CriarPladoAdDetalhesPlanoSmsVaga(PlanoAdquirido objPlanoAdquirido, int idVaga)
        {
            if (!ExistePladnoAdquiridoDetalhes(objPlanoAdquirido.IdPlanoAdquirido, idVaga))
            {
                var objVaga = Vaga.LoadObject(idVaga);

                objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CompleteObject();

                var objPlanoAdquiridoDetalhes = new PlanoAdquiridoDetalhes
                {
                    Vaga = objVaga,
                    PlanoAdquirido = objPlanoAdquirido,
                    FlagNotaFiscal = true,
                    NomeResPlanoAdquirido = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NomePessoa,
                    NumeroResDDDTelefone = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular,
                    NumeroResTelefone = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroCelular,
                    EmailEnvioBoleto = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.EmailPessoa,
                    FilialGestora = new Filial(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.FilialGestoraPadraoDoPlano)))
                };

                objPlanoAdquiridoDetalhes.Save();
            }
        }
        #endregion

        #region RecuperarIdVagaPorPlanoAdquirido
        public static int RecuperarIdVagaPorPlanoAdquirido(int idPlanoAdquirido)
        {
            string codVaga;
            int idVaga;
            RecuperarIdVagaPorPlanoAdquirido(idPlanoAdquirido, out idVaga, out codVaga);
            return idVaga;
        }

        public static bool RecuperarIdVagaPorPlanoAdquirido(int idPlanoAdquirido, out int idVaga, out string codVaga)
        {
            idVaga = 0;
            codVaga = string.Empty;
            bool retorno = false;

            var parms = new List<SqlParameter> { 
                new SqlParameter{ ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Value = idPlanoAdquirido}
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarIdVagaPorPlanoAdquirido, parms))
            {
                if (dr.Read())
                {
                    var idfVaga = dr["Idf_Vaga"];
                    var codeVaga = dr["Cod_Vaga"];

                    idVaga = Convert.IsDBNull(idfVaga) ? 0 : Convert.ToInt32(dr["Idf_Vaga"]);
                    codVaga = Convert.IsDBNull(codeVaga) ? string.Empty : codeVaga.ToString();

                    retorno = true;
                }

            };


            return retorno;
        }
        #endregion

        #region PlanoEnvioSmsEmailVagaLiberado
        public static PlanoAdquiridoDetalhes PlanoEnvioSmsEmailVagaLiberado(Vaga objVaga, Filial objFilial)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Value = objVaga.IdVaga },
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Value = objFilial.IdFilial }
                };

            var objPlanoAdquiridoDetalhes = new PlanoAdquiridoDetalhes();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpVerificarSituacaoPlanoEnvioSMSEmailVaga, parms))
            {
                if (!SetInstance(dr, objPlanoAdquiridoDetalhes))
                    objPlanoAdquiridoDetalhes = null;
            }

            return objPlanoAdquiridoDetalhes;
        }
        #endregion

        #region CriarPlanoAdquiridoDetalheVagaPremium
        public static void CriarPlanoAdquiridoDetalheVagaPremium(PlanoAdquirido objPlanoAdquirido, int idVaga)
        {
            try
            {
                var objVaga = Vaga.LoadObject(idVaga);

                objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CompleteObject();

                var objPlanoAdquiridoDetalhes = new PlanoAdquiridoDetalhes
                {
                    Vaga = objVaga,
                    PlanoAdquirido = objPlanoAdquirido,
                    FlagNotaFiscal = true,
                    NomeResPlanoAdquirido = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NomePessoa,
                    NumeroResDDDTelefone = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular,
                    NumeroResTelefone = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroCelular,
                    EmailEnvioBoleto = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.EmailPessoa,
                };

                objPlanoAdquiridoDetalhes.Save();
            }
            catch (Exception ex)
            {
                var MensagemErro = string.Empty;
                EL.GerenciadorException.GravarExcecao(ex,out MensagemErro, "Criar PlanoAdquiridoDetalhe VagaPremium");
            }
            
        }
        #endregion


        #region CarregarPlanoAntesDoVencimento
        /// <summary>
        /// Retorna a lista dos plano adquiridos que estão para vencer, a dias(parametro) antes de vencer
        /// </summary>
        /// <param name="idPlano"></param>
        /// <returns></returns>
        public static DataTable CarregarPlanoAntesDoVencimento(int idPlano)
        {
            var parms = new List<SqlParameter>(){
                new SqlParameter { ParameterName = "@Idf_Plano", SqlDbType = SqlDbType.Int, Size = 4, Value = idPlano},
                new SqlParameter { ParameterName = "@Dias", SqlDbType = SqlDbType.Int, Size = 4, Value = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DiasEnvioBoletoAntesDeVencerPlano))}
            };
            using (DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.Text, spCarregarPlanoAntesDoVencimento, parms).Tables[0])
            {
                return dt;
            }
          
        }
        #endregion

        #region CarregaPlanosBoletoRecorrentes

        public static DataTable CarregaPlanosBoletoRecorrentes(int quantidadeDiasVenciamento)
        {
            var param = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    ParameterName = "@diasAntesVencimento",
                    SqlDbType = SqlDbType.Int,
                    Size = 4,
                    Value = quantidadeDiasVenciamento

                }
            };
            using (DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.Text, sp_CarregarPlanoRecorrenteBoleto, param).Tables[0])
            {
                return dt;
            }
            
        }

        #endregion


        #endregion
    }
}