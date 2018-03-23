//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RegiaoMetropolitanaCidade // Tabela: TAB_Regiao_Metropolitana_Cidade
	{
		#region Atributos
		private RegiaoMetropolitana _regiaoMetropolitana;
		private Cidade _cidade;
		private DateTime _datacadastro;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region RegiaoMetropolitana
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public RegiaoMetropolitana RegiaoMetropolitana
		{
			get
			{
				return this._regiaoMetropolitana;
			}
			set
			{
				this._regiaoMetropolitana = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Cidade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Cidade Cidade
		{
			get
			{
				return this._cidade;
			}
			set
			{
				this._cidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Datacadastro
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime Datacadastro
		{
			get
			{
				return this._datacadastro;
			}
			set
			{
				this._datacadastro = value;
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
		public RegiaoMetropolitanaCidade()
		{
		}
		public RegiaoMetropolitanaCidade(RegiaoMetropolitana regiaoMetropolitana, Cidade cidade)
		{
			this._regiaoMetropolitana = regiaoMetropolitana;
			this._cidade = cidade;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Regiao_Metropolitana_Cidade (Idf_Regiao_Metropolitana, Idf_Cidade, Dta_cadastro, Flg_Inativo) VALUES (@Idf_Regiao_Metropolitana, @Idf_Cidade, @Dta_cadastro, @Flg_Inativo);";
		private const string SPUPDATE = "UPDATE TAB_Regiao_Metropolitana_Cidade SET Dta_cadastro = @Dta_cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Regiao_Metropolitana = @Idf_Regiao_Metropolitana AND Idf_Cidade = @Idf_Cidade";
		private const string SPDELETE = "DELETE FROM TAB_Regiao_Metropolitana_Cidade WHERE Idf_Regiao_Metropolitana = @Idf_Regiao_Metropolitana AND Idf_Cidade = @Idf_Cidade";
		private const string SPSELECTID = "SELECT * FROM TAB_Regiao_Metropolitana_Cidade WHERE Idf_Regiao_Metropolitana = @Idf_Regiao_Metropolitana AND Idf_Cidade = @Idf_Cidade";
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
			parms.Add(new SqlParameter("@Idf_Regiao_Metropolitana", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._regiaoMetropolitana.IdRegiaoMetropolitana;
			parms[1].Value = this._cidade.IdCidade;
			parms[2].Value = this._datacadastro;
			parms[3].Value = this._flagInativo;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de RegiaoMetropolitanaCidade no banco de dados.
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
		/// Método utilizado para inserir uma instância de RegiaoMetropolitanaCidade no banco de dados, dentro de uma transação.
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

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RegiaoMetropolitanaCidade no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RegiaoMetropolitanaCidade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de RegiaoMetropolitanaCidade no banco de dados.
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
		/// Método utilizado para salvar uma instância de RegiaoMetropolitanaCidade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RegiaoMetropolitanaCidade no banco de dados.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRegiaoMetropolitana, int idCidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regiao_Metropolitana", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idRegiaoMetropolitana;
			parms[1].Value = idCidade;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RegiaoMetropolitanaCidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRegiaoMetropolitana, int idCidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regiao_Metropolitana", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idRegiaoMetropolitana;
			parms[1].Value = idCidade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRegiaoMetropolitana, int idCidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regiao_Metropolitana", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idRegiaoMetropolitana;
			parms[1].Value = idCidade;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRegiaoMetropolitana, int idCidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regiao_Metropolitana", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idRegiaoMetropolitana;
			parms[1].Value = idCidade;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Reg.Idf_Regiao_Metropolitana, Reg.Idf_Cidade, Reg.Dta_cadastro, Reg.Flg_Inativo FROM TAB_Regiao_Metropolitana_Cidade Reg";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RegiaoMetropolitanaCidade a partir do banco de dados.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <returns>Instância de RegiaoMetropolitanaCidade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RegiaoMetropolitanaCidade LoadObject(int idRegiaoMetropolitana, int idCidade)
		{
			using (IDataReader dr = LoadDataReader(idRegiaoMetropolitana, idCidade))
			{
				RegiaoMetropolitanaCidade objRegiaoMetropolitanaCidade = new RegiaoMetropolitanaCidade();
				if (SetInstance(dr, objRegiaoMetropolitanaCidade))
					return objRegiaoMetropolitanaCidade;
			}
			throw (new RecordNotFoundException(typeof(RegiaoMetropolitanaCidade)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RegiaoMetropolitanaCidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RegiaoMetropolitanaCidade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RegiaoMetropolitanaCidade LoadObject(int idRegiaoMetropolitana, int idCidade, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRegiaoMetropolitana, idCidade, trans))
			{
				RegiaoMetropolitanaCidade objRegiaoMetropolitanaCidade = new RegiaoMetropolitanaCidade();
				if (SetInstance(dr, objRegiaoMetropolitanaCidade))
					return objRegiaoMetropolitanaCidade;
			}
			throw (new RecordNotFoundException(typeof(RegiaoMetropolitanaCidade)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RegiaoMetropolitanaCidade a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._regiaoMetropolitana.IdRegiaoMetropolitana, this._cidade.IdCidade))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RegiaoMetropolitanaCidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._regiaoMetropolitana.IdRegiaoMetropolitana, this._cidade.IdCidade, trans))
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
		/// <param name="objRegiaoMetropolitanaCidade">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RegiaoMetropolitanaCidade objRegiaoMetropolitanaCidade)
		{
			try
			{
				if (dr.Read())
				{
					objRegiaoMetropolitanaCidade._regiaoMetropolitana = new RegiaoMetropolitana(Convert.ToInt32(dr["Idf_Regiao_Metropolitana"]));
					objRegiaoMetropolitanaCidade._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					objRegiaoMetropolitanaCidade._datacadastro = Convert.ToDateTime(dr["Dta_cadastro"]);
					objRegiaoMetropolitanaCidade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objRegiaoMetropolitanaCidade._persisted = true;
					objRegiaoMetropolitanaCidade._modified = false;

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