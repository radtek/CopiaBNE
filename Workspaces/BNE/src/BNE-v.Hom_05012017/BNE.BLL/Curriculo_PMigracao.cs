using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class Curriculo
    {
        #region Propriedades
        
        #region DataCadastro
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataCadastro
        {
            get
            {
                return this._dataCadastro;
            }
            set
            {
                this._dataCadastro = value;
                this._modified = true;
            }
        }
        #endregion
        
        #endregion

        #region Mapeamento Novo -> Velho
        public void SalvarMigracao(SqlTransaction trans)
        {
            if (!this._persisted)
                this.InsertMigracao(trans);
            else
            {
                if (this.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.ExclusaoLogica))
                {
                    PessoaFisicaComplemento objPFComplemento = PessoaFisicaComplemento.LoadObject(this.PessoaFisica.IdPessoaFisica);
                    PessoaFisica.AtualizandoCadastroExlusaoLogicaParaAtivo(this.PessoaFisica, this, objPFComplemento,trans);
                    objPFComplemento.Save(trans);

                }
                this.UpdateMigracao(trans);
               
            }
              
        }

        #region InsertMigracao
        /// <summary>
        /// Método utilizado para inserir uma instância de Filial no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Fabiano Charan</remarks>
        private void InsertMigracao(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParametersMigracao(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo"].Value);
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
        /// <remarks>Fabiano Charan</remarks>
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
        /// <remarks>Fabiano Charan</remarks>
        private void SetParametersMigracao(List<SqlParameter> parms)
        {
            parms[0].Value = this._idCurriculo;
            parms[1].Value = this._pessoaFisica.IdPessoaFisica;

            if (this._valorPretensaoSalarial.HasValue)
                parms[2].Value = this._valorPretensaoSalarial;
            else
                parms[2].Value = DBNull.Value;

            parms[3].Value = this._tipoCurriculo.IdTipoCurriculo;
            parms[4].Value = this._situacaoCurriculo.IdSituacaoCurriculo;

            if (!String.IsNullOrEmpty(this._descricaoMiniCurriculo))
                parms[5].Value = this._descricaoMiniCurriculo;
            else
                parms[5].Value = DBNull.Value;

            parms[7].Value = this._dataAtualizacao;

            if (this._flagManha.HasValue)
                parms[8].Value = this._flagManha;
            else
                parms[8].Value = DBNull.Value;


            if (this._flagTarde.HasValue)
                parms[9].Value = this._flagTarde;
            else
                parms[9].Value = DBNull.Value;


            if (this._flagNoite.HasValue)
                parms[10].Value = this._flagNoite;
            else
                parms[10].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._observacaoCurriculo))
                parms[11].Value = this._observacaoCurriculo;
            else
                parms[11].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoAnalise))
                parms[12].Value = this._descricaoAnalise;
            else
                parms[12].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoSugestaoCarreira))
                parms[13].Value = this._descricaoSugestaoCarreira;
            else
                parms[13].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoCursosOferecidos))
                parms[14].Value = this._descricaoCursosOferecidos;
            else
                parms[14].Value = DBNull.Value;

            parms[15].Value = this._flagInativo;

            if (this._valorUltimoSalario.HasValue)
                parms[16].Value = this._valorUltimoSalario;
            else
                parms[16].Value = DBNull.Value;

            parms[17].Value = this._descricaoIP;

            if (this._cidadePretendida != null)
                parms[18].Value = this._cidadePretendida.IdCidade;
            else
                parms[18].Value = DBNull.Value;


            if (this._flagFinalSemana.HasValue)
                parms[19].Value = this._flagFinalSemana;
            else
                parms[19].Value = DBNull.Value;

            parms[20].Value = this._flagVIP;

            if (this._flagBoasVindas.HasValue)
                parms[21].Value = this._flagBoasVindas;
            else
                parms[21].Value = DBNull.Value;

            parms[22].Value = this._flagMSN;

            if (this._cidadeEndereco != null)
                parms[23].Value = this._cidadeEndereco.IdCidade;
            else
                parms[23].Value = DBNull.Value;

            if (this._descricaoLocalizacao != null)
                parms[24].Value = this._descricaoLocalizacao;
            else
                parms[24].Value = SqlGeography.Null;

            if (this._dataModificacaoCV.HasValue)
                parms[25].Value = this._dataModificacaoCV;
            else
                parms[25].Value = DBNull.Value;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }

            parms[6].Value = this._dataCadastro;            
        }
        #endregion

        #endregion

    }
}