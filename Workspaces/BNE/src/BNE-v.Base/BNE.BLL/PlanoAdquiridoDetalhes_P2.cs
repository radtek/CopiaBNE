//-- Data: 11/03/2011 09:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PlanoAdquiridoDetalhes // Tabela: BNE_Plano_Adquirido_Detalhes
    {

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

        #endregion
    }
}