//-- Data: 02/08/2016 17:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
	public partial class ProcessamentoJornalVagasSteps // Tabela: alerta.LOG_Processamento_Jornal_Vagas_Steps
	{
		#region Atributos
		private ProcessamentoJornalVagas _processamentoJornalVagas;
		private string _descricaoStep;
		private DateTime _dataCadastro;
        private TimeSpan _elapsedTime;

        private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdProcessamentoJornalVagas
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public ProcessamentoJornalVagas ProcessamentoJornalVagas
		{
			get
			{
				return this._processamentoJornalVagas;
			}
            set
            {
                this._processamentoJornalVagas = value;
                this._modified = true;
            }
        }
		#endregion 

		#region DescricaoStep
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string DescricaoStep
		{
			get
			{
				return this._descricaoStep;
			}
			set
			{
				this._descricaoStep = value;
				this._modified = true;
			}
		}
        #endregion

        #region ElapsedTime
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get
            {
                return this._elapsedTime;
            }
            set
            {
                this._elapsedTime = value;
                this._modified = true;
            }
        }
        #endregion

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
		}
		#endregion 

		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO alerta.LOG_Processamento_Jornal_Vagas_Steps (Des_Step, Dta_Cadastro, ElapsedTime, Idf_Processamento_Jornal_Vagas) VALUES (@Des_Step, @Dta_Cadastro, @ElapsedTime, @Idf_Processamento_Jornal_Vagas);";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Processamento_Jornal_Vagas", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Step", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@ElapsedTime", SqlDbType.Time, 5));
			return(parms);
        }
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._processamentoJornalVagas.IdProcessamentoJornalVagas;
            parms[1].Value = this._descricaoStep;
            parms[3].Value = this._elapsedTime;

            if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de ProcessamentoJornalVagasSteps no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert()
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);

			using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					try
					{
						SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
						cmd.Parameters.Clear();
						this._persisted = true;
						this._modified = false;
						trans.Commit();
					}
					catch
					{
						trans.Rollback();
						throw;
					}
				}
			}
		}
		/// <summary>
		/// Método utilizado para inserir uma instância de ProcessamentoJornalVagasSteps no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Save
		/// <summary>
		/// Método utilizado para salvar uma instância de ProcessamentoJornalVagasSteps no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de ProcessamentoJornalVagasSteps no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public void Save(SqlTransaction trans)
		{
			if (!this._persisted)
				this.Insert(trans);
		}
		#endregion

		#endregion

	}
}