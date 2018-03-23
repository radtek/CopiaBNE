using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class PessoaFisica
    {
        #region Mapeamento Novo -> Velho
        public void SalvarMigracao(SqlTransaction trans)
        {
            if (!this._persisted)
                this.InsertMigracao(trans);
            else
                this.UpdateMigracao(trans);
        }

        #region InsertMigracao
        /// <summary>
        /// Método utilizado para inserir uma instância de Pessoa Fisica no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void InsertMigracao(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParametersMigracao(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPessoaFisica = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Filial no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void UpdateMigracao(SqlTransaction trans)
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                SetParametersMigracao(parms);
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                this._modified = false;
            }
        }
        #endregion

        #region SetParametersMigracao
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParametersMigracao(List<SqlParameter> parms)
        {
            parms[0].Value = this._idPessoaFisica;
            parms[1].Value = this._numeroCPF;
            parms[2].Value = this._nomePessoa;

            if (!String.IsNullOrEmpty(this._apelidoPessoa))
                parms[3].Value = this._apelidoPessoa;
            else
                parms[3].Value = DBNull.Value;


            if (this._sexo != null)
                parms[4].Value = this._sexo.IdSexo;
            else
                parms[4].Value = DBNull.Value;


            if (this._nacionalidade != null)
                parms[5].Value = this._nacionalidade.IdNacionalidade;
            else
                parms[5].Value = DBNull.Value;


            if (this._cidade != null)
                parms[6].Value = this._cidade.IdCidade;
            else
                parms[6].Value = DBNull.Value;


            if (this._dataChegadaBrasil.HasValue)
                parms[7].Value = this._dataChegadaBrasil;
            else
                parms[7].Value = DBNull.Value;

            parms[8].Value = this._dataNascimento;

            if (!String.IsNullOrEmpty(this._nomeMae))
                parms[9].Value = this._nomeMae;
            else
                parms[9].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._nomePai))
                parms[10].Value = this._nomePai;
            else
                parms[10].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroRG))
                parms[11].Value = this._numeroRG;
            else
                parms[11].Value = DBNull.Value;


            if (this._dataExpedicaoRG.HasValue)
                parms[12].Value = this._dataExpedicaoRG;
            else
                parms[12].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._nomeOrgaoEmissor))
                parms[13].Value = this._nomeOrgaoEmissor;
            else
                parms[13].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._siglaUFEmissaoRG))
                parms[14].Value = this._siglaUFEmissaoRG;
            else
                parms[14].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroPIS))
                parms[15].Value = this._numeroPIS;
            else
                parms[15].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroCTPS))
                parms[16].Value = this._numeroCTPS;
            else
                parms[16].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoSerieCTPS))
                parms[17].Value = this._descricaoSerieCTPS;
            else
                parms[17].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._siglaUFCTPS))
                parms[18].Value = this._siglaUFCTPS;
            else
                parms[18].Value = DBNull.Value;


            if (this._raca != null)
                parms[19].Value = this._raca.IdRaca;
            else
                parms[19].Value = DBNull.Value;


            if (this._deficiencia != null)
                parms[20].Value = this._deficiencia.IdDeficiencia;
            else
                parms[20].Value = DBNull.Value;


            if (this._endereco != null)
                parms[21].Value = this._endereco.IdEndereco;
            else
                parms[21].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroDDDTelefone))
                parms[22].Value = this._numeroDDDTelefone;
            else
                parms[22].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroTelefone))
                parms[23].Value = this._numeroTelefone;
            else
                parms[23].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroDDDCelular))
                parms[24].Value = this._numeroDDDCelular;
            else
                parms[24].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroCelular))
                parms[25].Value = this._numeroCelular;
            else
                parms[25].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._emailPessoa))
                parms[26].Value = this._emailPessoa;
            else
                parms[26].Value = DBNull.Value;


            if (this._flagPossuiDependentes.HasValue)
                parms[27].Value = this._flagPossuiDependentes;
            else
                parms[27].Value = DBNull.Value;


            if (this._flagImportado.HasValue)
                parms[30].Value = this._flagImportado;
            else
                parms[30].Value = DBNull.Value;


            if (this._escolaridade != null)
                parms[31].Value = this._escolaridade.IdEscolaridade;
            else
                parms[31].Value = DBNull.Value;

            parms[32].Value = this._nomePessoaPesquisa;

            if (this._estadoCivil != null)
                parms[33].Value = this._estadoCivil.IdEstadoCivil;
            else
                parms[33].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._siglaEstado))
                parms[34].Value = this._siglaEstado;
            else
                parms[34].Value = DBNull.Value;


            if (this._flagInativo.HasValue)
                parms[35].Value = this._flagInativo;
            else
                parms[35].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoIP))
                parms[36].Value = this._descricaoIP;
            else
                parms[36].Value = DBNull.Value;


            if (this._operadoraCelular != null)
                parms[37].Value = this._operadoraCelular.IdOperadoraCelular;
            else
                parms[37].Value = DBNull.Value;


            if (this._emailSituacaoConfirmacao != null)
                parms[38].Value = this._emailSituacaoConfirmacao.IdEmailSituacao;
            else
                parms[38].Value = DBNull.Value;


            if (this._emailSituacaoValidacao != null)
                parms[39].Value = this._emailSituacaoValidacao.IdEmailSituacao;
            else
                parms[39].Value = DBNull.Value;


            if (this._emailSituacaoBloqueio != null)
                parms[40].Value = this._emailSituacaoBloqueio.IdEmailSituacao;
            else
                parms[40].Value = DBNull.Value;

            parms[41].Value = this._flagCelularConfirmado;
            parms[42].Value = this._flagEmailConfirmado;


            if (this._flagWhatsApp.HasValue)
                parms[43].Value = this._flagWhatsApp;
            else
                parms[43].Value = DBNull.Value;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[28].Value = this._dataCadastro;
            parms[29].Value = this._dataAlteracao;
        }
        #endregion

        #endregion
    }
}