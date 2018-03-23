//-- Data: 16/07/2013 16:45
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class MobileToken // Tabela: BNE_Mobile_Token
	{
		#region Atributos
		private int _idMobileToken;
		private Curriculo _curriculo;
		private string _codigoToken;
        private string _codigoDispositivo;
		private DateTime _dataCadastro;
		private DateTime _dataAlteracao;
		private TipoSistemaMobile _tipoSistemaMobile;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdMobileToken
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdMobileToken
		{
			get
			{
				return this._idMobileToken;
			}
		}
		#endregion 

		#region Curriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Curriculo Curriculo
		{
			get
			{
				return this._curriculo;
			}
			set
			{
				this._curriculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoToken
		/// <summary>
		/// Tamanho do campo: 4096.
		/// Campo obrigatório.
		/// </summary>
		public string CodigoToken
		{
			get
			{
				return this._codigoToken;
			}
			set
			{
				this._codigoToken = value;
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

		#region DataAlteracao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataAlteracao
		{
			get
			{
				return this._dataAlteracao;
			}
		}
		#endregion 

		#region TipoSistemaMobile
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoSistemaMobile TipoSistemaMobile
		{
			get
			{
				return this._tipoSistemaMobile;
			}
			set
			{
				this._tipoSistemaMobile = value;
				this._modified = true;
			}
		}
		#endregion 

        #region CodigoDispositivo
        /// <summary>
        /// Tamanho do campo: 16.
        /// Campo obrigatório.
        /// </summary>
        public string CodigoDispositivo
        {
            get
            {
                return this._codigoDispositivo;
            }
            set
            {
                this._codigoDispositivo = value;
                this._modified = true;
            }
        }
        #endregion 

		#endregion

		#region Construtores
		public MobileToken()
		{
		}
		public MobileToken(int idMobileToken)
		{
			this._idMobileToken = idMobileToken;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Mobile_Token (Idf_Curriculo, Cod_Token, Dta_Cadastro, Dta_Alteracao, Idf_Tipo_Sistema_Mobile, Cod_Dispositivo) VALUES (@Idf_Curriculo, @Cod_Token, @Dta_Cadastro, @Dta_Alteracao, @Idf_Tipo_Sistema_Mobile, @Cod_Dispositivo);SET @Idf_Mobile_Token = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Mobile_Token SET Idf_Curriculo = @Idf_Curriculo, Cod_Token = @Cod_Token, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Idf_Tipo_Sistema_Mobile = @Idf_Tipo_Sistema_Mobile, Cod_Dispositivo = @Cod_Dispositivo WHERE Idf_Mobile_Token = @Idf_Mobile_Token";
		private const string SPDELETE = "DELETE FROM BNE_Mobile_Token WHERE Idf_Mobile_Token = @Idf_Mobile_Token";
		private const string SPSELECTID = "SELECT * FROM BNE_Mobile_Token WITH(NOLOCK) WHERE Idf_Mobile_Token = @Idf_Mobile_Token";
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
			parms.Add(new SqlParameter("@Idf_Mobile_Token", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Cod_Token", SqlDbType.VarChar, 4096));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Tipo_Sistema_Mobile", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Cod_Dispositivo", SqlDbType.VarChar, 16));
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
			parms[0].Value = this._idMobileToken;
			parms[1].Value = this._curriculo.IdCurriculo;
			parms[2].Value = this._codigoToken;
			parms[5].Value = this._tipoSistemaMobile.IdTipoSistemaMobile;
            parms[6].Value = this._codigoDispositivo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[3].Value = this._dataCadastro;
			this._dataAlteracao = DateTime.Now;
			parms[4].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de MobileToken no banco de dados.
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
						this._idMobileToken = Convert.ToInt32(cmd.Parameters["@Idf_Mobile_Token"].Value);
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
		/// Método utilizado para inserir uma instância de MobileToken no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idMobileToken = Convert.ToInt32(cmd.Parameters["@Idf_Mobile_Token"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de MobileToken no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
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
		/// Método utilizado para atualizar uma instância de MobileToken no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
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
		/// Método utilizado para salvar uma instância de MobileToken no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de MobileToken no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
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
		/// Método utilizado para excluir uma instância de MobileToken no banco de dados.
		/// </summary>
		/// <param name="idMobileToken">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMobileToken)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mobile_Token", SqlDbType.Int, 4));

			parms[0].Value = idMobileToken;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de MobileToken no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMobileToken">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMobileToken, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mobile_Token", SqlDbType.Int, 4));

			parms[0].Value = idMobileToken;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de MobileToken no banco de dados.
		/// </summary>
		/// <param name="idMobileToken">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idMobileToken)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Mobile_Token where Idf_Mobile_Token in (";

			for (int i = 0; i < idMobileToken.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idMobileToken[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idMobileToken">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMobileToken)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mobile_Token", SqlDbType.Int, 4));

			parms[0].Value = idMobileToken;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMobileToken">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMobileToken, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mobile_Token", SqlDbType.Int, 4));

			parms[0].Value = idMobileToken;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Mob.Idf_Mobile_Token, Mob.Idf_Curriculo, Mob.Cod_Token, Mob.Dta_Cadastro, Mob.Dta_Alteracao, Mob.Idf_Tipo_Sistema_Mobile, Mob.Cod_Dispositivo FROM BNE_Mobile_Token Mob";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de MobileToken a partir do banco de dados.
		/// </summary>
		/// <param name="idMobileToken">Chave do registro.</param>
		/// <returns>Instância de MobileToken.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static MobileToken LoadObject(int idMobileToken)
		{
			using (IDataReader dr = LoadDataReader(idMobileToken))
			{
				MobileToken objMobileToken = new MobileToken();
				if (SetInstance(dr, objMobileToken))
					return objMobileToken;
			}
			throw (new RecordNotFoundException(typeof(MobileToken)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de MobileToken a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMobileToken">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de MobileToken.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static MobileToken LoadObject(int idMobileToken, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idMobileToken, trans))
			{
				MobileToken objMobileToken = new MobileToken();
				if (SetInstance(dr, objMobileToken))
					return objMobileToken;
			}
			throw (new RecordNotFoundException(typeof(MobileToken)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de MobileToken a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idMobileToken))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de MobileToken a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idMobileToken, trans))
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
		/// <param name="objMobileToken">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, MobileToken objMobileToken)
		{
			try
			{
				if (dr.Read())
				{
					objMobileToken._idMobileToken = Convert.ToInt32(dr["Idf_Mobile_Token"]);
					objMobileToken._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objMobileToken._codigoToken = Convert.ToString(dr["Cod_Token"]);
					objMobileToken._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objMobileToken._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objMobileToken._tipoSistemaMobile = new TipoSistemaMobile(Convert.ToInt32(dr["Idf_Tipo_Sistema_Mobile"]));
                    objMobileToken._codigoDispositivo = Convert.ToString(dr["Cod_Dispositivo"]);

					objMobileToken._persisted = true;
					objMobileToken._modified = false;

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
				dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}