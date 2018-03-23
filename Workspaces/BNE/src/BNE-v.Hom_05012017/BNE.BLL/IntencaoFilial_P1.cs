//-- Data: 28/04/2010 08:41
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class IntencaoFilial // Tabela: BNE_Intencao_Filial
	{
		#region Atributos
		private int _idIntencaoFilial;
		private Curriculo _curriculo;
		private Filial _filial;
		private DateTime _dataCadastro;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdIntencaoFilial
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdIntencaoFilial
		{
			get
			{
				return this._idIntencaoFilial;
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

		#region Filial
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Filial Filial
		{
			get
			{
				return this._filial;
			}
			set
			{
				this._filial = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagInativo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagInativo
		{
			get
			{
				return this._flagInativo;
			}
			set
			{
				this._flagInativo = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public IntencaoFilial()
		{
		}
		public IntencaoFilial(int idIntencaoFilial)
		{
			this._idIntencaoFilial = idIntencaoFilial;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Intencao_Filial (Idf_Curriculo, Idf_Filial, Dta_Cadastro, Flg_Inativo) VALUES (@Idf_Curriculo, @Idf_Filial, @Dta_Cadastro, @Flg_Inativo);SET @Idf_Intencao_Filial = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Intencao_Filial SET Idf_Curriculo = @Idf_Curriculo, Idf_Filial = @Idf_Filial, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Intencao_Filial = @Idf_Intencao_Filial";
		private const string SPDELETE = "DELETE FROM BNE_Intencao_Filial WHERE Idf_Intencao_Filial = @Idf_Intencao_Filial";
		private const string SPSELECTID = "SELECT * FROM BNE_Intencao_Filial WHERE Idf_Intencao_Filial = @Idf_Intencao_Filial";
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
			parms.Add(new SqlParameter("@Idf_Intencao_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idIntencaoFilial;
			parms[1].Value = this._curriculo.IdCurriculo;
			parms[2].Value = this._filial.IdFilial;
			parms[4].Value = this._flagInativo;

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
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de IntencaoFilial no banco de dados.
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
						this._idIntencaoFilial = Convert.ToInt32(cmd.Parameters["@Idf_Intencao_Filial"].Value);
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
		/// Método utilizado para inserir uma instância de IntencaoFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idIntencaoFilial = Convert.ToInt32(cmd.Parameters["@Idf_Intencao_Filial"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}				
		#endregion

		#region BulkInsert
		/// <summary>
		/// Método utilizado para inserir em massa todas as IntencaoFilial na CurriculoOrigem.
		/// </summary>
		/// <remarks>Jhonatan Taborda</remarks>
		private static void BulkInsert(DataTable dt)
		{
			DataAccessLayer.SaveBulkTable(dt, "BNE.BNE_Curriculo_Origem");		
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de IntencaoFilial no banco de dados.
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
		/// Método utilizado para atualizar uma instância de IntencaoFilial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de IntencaoFilial no banco de dados.
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
		/// Método utilizado para salvar uma instância de IntencaoFilial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de IntencaoFilial no banco de dados.
		/// </summary>
		/// <param name="idIntencaoFilial">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idIntencaoFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Intencao_Filial", SqlDbType.Int, 4));

			parms[0].Value = idIntencaoFilial;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de IntencaoFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idIntencaoFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idIntencaoFilial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Intencao_Filial", SqlDbType.Int, 4));

			parms[0].Value = idIntencaoFilial;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de IntencaoFilial no banco de dados.
		/// </summary>
		/// <param name="idIntencaoFilial">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idIntencaoFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Intencao_Filial where Idf_Intencao_Filial in (";

			for (int i = 0; i < idIntencaoFilial.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idIntencaoFilial[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idIntencaoFilial">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idIntencaoFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Intencao_Filial", SqlDbType.Int, 4));

			parms[0].Value = idIntencaoFilial;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idIntencaoFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idIntencaoFilial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Intencao_Filial", SqlDbType.Int, 4));

			parms[0].Value = idIntencaoFilial;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Int.Idf_Intencao_Filial, Int.Idf_Curriculo, Int.Idf_Filial, Int.Dta_Cadastro, Int.Flg_Inativo FROM BNE_Intencao_Filial Int";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de IntencaoFilial a partir do banco de dados.
		/// </summary>
		/// <param name="idIntencaoFilial">Chave do registro.</param>
		/// <returns>Instância de IntencaoFilial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static IntencaoFilial LoadObject(int idIntencaoFilial)
		{
			using (IDataReader dr = LoadDataReader(idIntencaoFilial))
			{
				IntencaoFilial objIntencaoFilial = new IntencaoFilial();
				if (SetInstance(dr, objIntencaoFilial))
					return objIntencaoFilial;
			}
			throw (new RecordNotFoundException(typeof(IntencaoFilial)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de IntencaoFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idIntencaoFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de IntencaoFilial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static IntencaoFilial LoadObject(int idIntencaoFilial, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idIntencaoFilial, trans))
			{
				IntencaoFilial objIntencaoFilial = new IntencaoFilial();
				if (SetInstance(dr, objIntencaoFilial))
					return objIntencaoFilial;
			}
			throw (new RecordNotFoundException(typeof(IntencaoFilial)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de IntencaoFilial a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idIntencaoFilial))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de IntencaoFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idIntencaoFilial, trans))
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
		/// <param name="objIntencaoFilial">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, IntencaoFilial objIntencaoFilial)
		{
			try
			{
				if (dr.Read())
				{
					objIntencaoFilial._idIntencaoFilial = Convert.ToInt32(dr["Idf_Intencao_Filial"]);
					objIntencaoFilial._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objIntencaoFilial._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
					objIntencaoFilial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objIntencaoFilial._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objIntencaoFilial._persisted = true;
					objIntencaoFilial._modified = false;

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