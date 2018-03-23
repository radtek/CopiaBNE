//-- Data: 18/07/2016 15:08
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PlanoMotivoCancelamento // Tabela: BNE_Plano_Motivo_Cancelamento
	{
		#region Atributos
		private int _idPlanoMotivoCancelamento;
		private int _idPlanoAdquirido;
		private int _idMotivoCancelamento;
		private DateTime? _dataCancelamento;
		private string _descricaoDetalheMotivoCancelamento;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private bool? _FlgInativo;

        private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPlanoMotivoCancelamento
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPlanoMotivoCancelamento
		{
			get
			{
				return this._idPlanoMotivoCancelamento;
			}
		}
		#endregion 

		#region IdPlanoAdquirido
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdPlanoAdquirido
		{
			get
			{
				return this._idPlanoAdquirido;
			}
			set
			{
				this._idPlanoAdquirido = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdMotivoCancelamento
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdMotivoCancelamento
		{
			get
			{
				return this._idMotivoCancelamento;
			}
			set
			{
				this._idMotivoCancelamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCancelamento
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataCancelamento
		{
			get
			{
				return this._dataCancelamento;
			}
			set
			{
				this._dataCancelamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoDetalheMotivoCancelamento
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoDetalheMotivoCancelamento
		{
			get
			{
				return this._descricaoDetalheMotivoCancelamento;
			}
			set
			{
				this._descricaoDetalheMotivoCancelamento = value;
				this._modified = true;
			}
		}
        #endregion

        #region UsuarioFilialPerfil
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public UsuarioFilialPerfil UsuarioFilialPerfil
        {
            get
            {
                return this._usuarioFilialPerfil;
            }
            set
            {
                this._usuarioFilialPerfil = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlgInativo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlgInativo
        {
            get
            {
                return this._FlgInativo;
            }
            set
            {
                this._FlgInativo = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public PlanoMotivoCancelamento()
		{
		}
		public PlanoMotivoCancelamento(int idfMotivoCancelamento)
		{
            this._idMotivoCancelamento = idfMotivoCancelamento;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Plano_Motivo_Cancelamento (Idf_Plano_Adquirido, Idf_Motivo_Cancelamento, Dta_Cancelamento, Des_Detalhe_Motivo_Cancelamento,Idf_Usuario_Filial_Perfil, Flg_Inativo) VALUES (@Idf_Plano_Adquirido, @Idf_Motivo_Cancelamento, @Dta_Cancelamento, @Des_Detalhe_Motivo_Cancelamento, @Idf_Usuario_Filial_Perfil, @Flg_Inativo);SET @Idf_Plano_Motivo_Cancelamento = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Plano_Motivo_Cancelamento SET Idf_Plano_Motivo_Cancelamento = @Idf_Plano_Motivo_Cancelamento, Idf_Plano_Adquirido = @Idf_Plano_Adquirido, Idf_Motivo_Cancelamento = @Idf_Motivo_Cancelamento, Dta_Cancelamento = @Dta_Cancelamento, Des_Detalhe_Motivo_Cancelamento = @Des_Detalhe_Motivo_Cancelamento, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Flg_Inativo = @Flg_Inativo WHERE Idf_Plano_Motivo_Cancelamento = @Idf_Plano_Motivo_Cancelamento";
        private const string SPDELETE = "DELETE FROM BNE_Plano_Motivo_Cancelamento Where idf_plano_motivo_cancelamento  = @idf_plano_motivo_cancelamento ";
        private const string SPSELECTID = "SELECT * FROM BNE_Plano_Motivo_Cancelamento WITH(NOLOCK) where idf_plano_motivo_cancelamento = @idf_plano_motivo_cancelamento ";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Mailson</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Motivo_Cancelamento", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Motivo_Cancelamento", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cancelamento", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Detalhe_Motivo_Cancelamento", SqlDbType.VarChar, 250));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            return (parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Mailson</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idPlanoMotivoCancelamento;
			parms[1].Value = this._idPlanoAdquirido;
			parms[2].Value = this._idMotivoCancelamento;

			if (this._dataCancelamento.HasValue)
				parms[3].Value = this._dataCancelamento;
			else
				parms[3].Value = DateTime.Now;


			if (!String.IsNullOrEmpty(this._descricaoDetalheMotivoCancelamento))
				parms[4].Value = this._descricaoDetalheMotivoCancelamento;
			else
				parms[4].Value = DBNull.Value;

            if (this._usuarioFilialPerfil != null)
                parms[5].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            else
                parms[5].Value = DBNull.Value;

            if (this._FlgInativo.HasValue)
                parms[6].Value = this._FlgInativo.Value;
            else
                parms[6].Value = DBNull.Value;


            if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PlanoMotivoCancelamento no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
						this._idPlanoMotivoCancelamento = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Motivo_Cancelamento"].Value);
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
		/// Método utilizado para inserir uma instância de PlanoMotivoCancelamento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPlanoMotivoCancelamento = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Motivo_Cancelamento"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PlanoMotivoCancelamento no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
		private void Update()
		{
			if (this._modified)
			{
				List<SqlParameter> parms = GetParameters();
				SetParameters(parms);
				DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPUPDATE, parms);
				this._modified = false;
			}
		}
		/// <summary>
		/// Método utilizado para atualizar uma instância de PlanoMotivoCancelamento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		private void Update(SqlTransaction trans)
		{
			if (this._modified)
			{
				List<SqlParameter> parms = GetParameters();
				SetParameters(parms);
				DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
				this._modified = false;
			}
		}
		#endregion

		#region Save
		/// <summary>
		/// Método utilizado para salvar uma instância de PlanoMotivoCancelamento no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de PlanoMotivoCancelamento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public void Save(SqlTransaction trans)
		{
			if (!this._persisted)
				this.Insert(trans);
			else
				this.Update(trans);
		}
		#endregion

		#region Delete
		/// <summary>
		/// Método utilizado para excluir uma instância de PlanoMotivoCancelamento no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
        public static void Delete(int idPlanoMotivoCancelamento)
		{
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idf_plano_motivo_cancelamento ", SqlDbType.Int, 4));

            parms[0].Value = idPlanoMotivoCancelamento;


			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PlanoMotivoCancelamento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
        public static void Delete(int idPlanoMotivoCancelamento, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idf_plano_motivo_cancelamento ", SqlDbType.Int, 4));

            parms[0].Value = idPlanoMotivoCancelamento;


			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
        private static IDataReader LoadDataReader(int idPlanoMotivoCancelamento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idf_plano_motivo_cancelamento", SqlDbType.Int, 4));

			parms[0].Value = idPlanoMotivoCancelamento;


			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idPlanoMotivoCancelamento, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idf_plano_motivo_cancelamento", SqlDbType.Int, 4));

			parms[0].Value = idPlanoMotivoCancelamento;


			return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar uma consulta paginada do banco de dados.
		/// </summary>
		/// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
		/// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
		/// <param name="paginaCorrente">Número da página que será exibida.</param>
		/// <param name="tamanhoPagina">Quantidade de itens em cada página.</param>
		/// <param name="totalRegistros">Quantidade total de registros na tabela.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
		{
			int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
			int fim = paginaCorrente * tamanhoPagina;

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Motivo_Cancelamento, Pla.Idf_Plano_Adquirido, Pla.Idf_Motivo_Cancelamento, Pla.Dta_Cancelamento, Pla.Des_Detalhe_Motivo_Cancelamento, Pla.Idf_Usuario_Filial_Perfil, pla.Flg_Inativo FROM BNE_Plano_Motivo_Cancelamento Pla";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PlanoMotivoCancelamento a partir do banco de dados.
		/// </summary>
		/// <returns>Instância de PlanoMotivoCancelamento.</returns>
		/// <remarks>Mailson</remarks>
        public static PlanoMotivoCancelamento LoadObject(int idfPlanoMotivoCancelamento)
		{
            using (IDataReader dr = LoadDataReader(idfPlanoMotivoCancelamento))
			{
				PlanoMotivoCancelamento objPlanoMotivoCancelamento = new PlanoMotivoCancelamento();
				if (SetInstance(dr, objPlanoMotivoCancelamento))
					return objPlanoMotivoCancelamento;
			}
			throw (new RecordNotFoundException(typeof(PlanoMotivoCancelamento)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PlanoMotivoCancelamento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PlanoMotivoCancelamento.</returns>
		/// <remarks>Mailson</remarks>
		public static PlanoMotivoCancelamento LoadObject(int idMotivoCancelamento, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idMotivoCancelamento, trans))
			{
				PlanoMotivoCancelamento objPlanoMotivoCancelamento = new PlanoMotivoCancelamento();
				if (SetInstance(dr, objPlanoMotivoCancelamento))
					return objPlanoMotivoCancelamento;
			}
			throw (new RecordNotFoundException(typeof(PlanoMotivoCancelamento)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PlanoMotivoCancelamento a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this.IdPlanoMotivoCancelamento))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PlanoMotivoCancelamento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this.IdMotivoCancelamento, trans))
			{
				return SetInstance(dr, this);
			}
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objPlanoMotivoCancelamento">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, PlanoMotivoCancelamento objPlanoMotivoCancelamento, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objPlanoMotivoCancelamento._idPlanoMotivoCancelamento = Convert.ToInt32(dr["Idf_Plano_Motivo_Cancelamento"]);
					objPlanoMotivoCancelamento._idPlanoAdquirido = Convert.ToInt32(dr["Idf_Plano_Adquirido"]);
					objPlanoMotivoCancelamento._idMotivoCancelamento = Convert.ToInt32(dr["Idf_Motivo_Cancelamento"]);
					if (dr["Dta_Cancelamento"] != DBNull.Value)
						objPlanoMotivoCancelamento._dataCancelamento = Convert.ToDateTime(dr["Dta_Cancelamento"]);
					if (dr["Des_Detalhe_Motivo_Cancelamento"] != DBNull.Value)
						objPlanoMotivoCancelamento._descricaoDetalheMotivoCancelamento = Convert.ToString(dr["Des_Detalhe_Motivo_Cancelamento"]);
                    if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                        objPlanoMotivoCancelamento._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));

                    if (dr["Flg_Inativo"] != DBNull.Value)
                        objPlanoMotivoCancelamento._FlgInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

                    objPlanoMotivoCancelamento._persisted = true;
					objPlanoMotivoCancelamento._modified = false;

					return true;
				}
				else
				{
					return false;
				}
			}
			catch 
			{
				throw;
			}
			finally
			{
				if (dispose)
					dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}