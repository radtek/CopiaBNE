//-- Data: 31/05/2010 14:46
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RegiaoMetropolitana // Tabela: TAB_Regiao_Metropolitana
	{
		#region Atributos
		private int _idRegiaoMetropolitana;
		private string _nomeRegiaoMetropolitana;
		private string _nomeRegiaoMetropolitanaPesquisa;
		private Cidade _cidade;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private string _siglaUF;
		private string _CIDRegiaoMetropolitana;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRegiaoMetropolitana
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRegiaoMetropolitana
		{
			get
			{
				return this._idRegiaoMetropolitana;
			}
		}
		#endregion 

		#region NomeRegiaoMetropolitana
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string NomeRegiaoMetropolitana
		{
			get
			{
				return this._nomeRegiaoMetropolitana;
			}
			set
			{
				this._nomeRegiaoMetropolitana = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeRegiaoMetropolitanaPesquisa
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string NomeRegiaoMetropolitanaPesquisa
		{
			get
			{
				return this._nomeRegiaoMetropolitanaPesquisa;
			}
			set
			{
				this._nomeRegiaoMetropolitanaPesquisa = value;
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

		#region SiglaUF
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo opcional.
		/// </summary>
		public string SiglaUF
		{
			get
			{
				return this._siglaUF;
			}
			set
			{
				this._siglaUF = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CIDRegiaoMetropolitana
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string CIDRegiaoMetropolitana
		{
			get
			{
				return this._CIDRegiaoMetropolitana;
			}
			set
			{
				this._CIDRegiaoMetropolitana = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public RegiaoMetropolitana()
		{
		}
		public RegiaoMetropolitana(int idRegiaoMetropolitana)
		{
			this._idRegiaoMetropolitana = idRegiaoMetropolitana;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Regiao_Metropolitana (Nme_Regiao_Metropolitana, Nme_Regiao_Metropolitana_Pesquisa, Idf_Cidade, Flg_Inativo, Dta_Cadastro, Sig_UF, CID_Regiao_Metropolitana) VALUES (@Nme_Regiao_Metropolitana, @Nme_Regiao_Metropolitana_Pesquisa, @Idf_Cidade, @Flg_Inativo, @Dta_Cadastro, @Sig_UF, @CID_Regiao_Metropolitana);SET @Idf_Regiao_Metropolitana = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Regiao_Metropolitana SET Nme_Regiao_Metropolitana = @Nme_Regiao_Metropolitana, Nme_Regiao_Metropolitana_Pesquisa = @Nme_Regiao_Metropolitana_Pesquisa, Idf_Cidade = @Idf_Cidade, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Sig_UF = @Sig_UF, CID_Regiao_Metropolitana = @CID_Regiao_Metropolitana WHERE Idf_Regiao_Metropolitana = @Idf_Regiao_Metropolitana";
		private const string SPDELETE = "DELETE FROM TAB_Regiao_Metropolitana WHERE Idf_Regiao_Metropolitana = @Idf_Regiao_Metropolitana";
		private const string SPSELECTID = "SELECT * FROM TAB_Regiao_Metropolitana WHERE Idf_Regiao_Metropolitana = @Idf_Regiao_Metropolitana";
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
			parms.Add(new SqlParameter("@Nme_Regiao_Metropolitana", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Nme_Regiao_Metropolitana_Pesquisa", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Sig_UF", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@CID_Regiao_Metropolitana", SqlDbType.VarChar, 200));
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
			parms[0].Value = this._idRegiaoMetropolitana;
			parms[1].Value = this._nomeRegiaoMetropolitana;
			parms[2].Value = this._nomeRegiaoMetropolitanaPesquisa;
			parms[3].Value = this._cidade.IdCidade;
			parms[4].Value = this._flagInativo;

			if (!String.IsNullOrEmpty(this._siglaUF))
				parms[6].Value = this._siglaUF;
			else
				parms[6].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._CIDRegiaoMetropolitana))
				parms[7].Value = this._CIDRegiaoMetropolitana;
			else
				parms[7].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[5].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de RegiaoMetropolitana no banco de dados.
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
						this._idRegiaoMetropolitana = Convert.ToInt32(cmd.Parameters["@Idf_Regiao_Metropolitana"].Value);
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
		/// Método utilizado para inserir uma instância de RegiaoMetropolitana no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRegiaoMetropolitana = Convert.ToInt32(cmd.Parameters["@Idf_Regiao_Metropolitana"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RegiaoMetropolitana no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RegiaoMetropolitana no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de RegiaoMetropolitana no banco de dados.
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
		/// Método utilizado para salvar uma instância de RegiaoMetropolitana no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RegiaoMetropolitana no banco de dados.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRegiaoMetropolitana)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regiao_Metropolitana", SqlDbType.Int, 4));

			parms[0].Value = idRegiaoMetropolitana;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RegiaoMetropolitana no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRegiaoMetropolitana, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regiao_Metropolitana", SqlDbType.Int, 4));

			parms[0].Value = idRegiaoMetropolitana;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RegiaoMetropolitana no banco de dados.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRegiaoMetropolitana)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Regiao_Metropolitana where Idf_Regiao_Metropolitana in (";

			for (int i = 0; i < idRegiaoMetropolitana.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRegiaoMetropolitana[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRegiaoMetropolitana)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regiao_Metropolitana", SqlDbType.Int, 4));

			parms[0].Value = idRegiaoMetropolitana;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRegiaoMetropolitana, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Regiao_Metropolitana", SqlDbType.Int, 4));

			parms[0].Value = idRegiaoMetropolitana;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Reg.Idf_Regiao_Metropolitana, Reg.Nme_Regiao_Metropolitana, Reg.Nme_Regiao_Metropolitana_Pesquisa, Reg.Idf_Cidade, Reg.Flg_Inativo, Reg.Dta_Cadastro, Reg.Sig_UF, Reg.CID_Regiao_Metropolitana FROM TAB_Regiao_Metropolitana Reg";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RegiaoMetropolitana a partir do banco de dados.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <returns>Instância de RegiaoMetropolitana.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RegiaoMetropolitana LoadObject(int idRegiaoMetropolitana)
		{
			using (IDataReader dr = LoadDataReader(idRegiaoMetropolitana))
			{
				RegiaoMetropolitana objRegiaoMetropolitana = new RegiaoMetropolitana();
				if (SetInstance(dr, objRegiaoMetropolitana))
					return objRegiaoMetropolitana;
			}
			throw (new RecordNotFoundException(typeof(RegiaoMetropolitana)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RegiaoMetropolitana a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRegiaoMetropolitana">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RegiaoMetropolitana.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RegiaoMetropolitana LoadObject(int idRegiaoMetropolitana, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRegiaoMetropolitana, trans))
			{
				RegiaoMetropolitana objRegiaoMetropolitana = new RegiaoMetropolitana();
				if (SetInstance(dr, objRegiaoMetropolitana))
					return objRegiaoMetropolitana;
			}
			throw (new RecordNotFoundException(typeof(RegiaoMetropolitana)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RegiaoMetropolitana a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRegiaoMetropolitana))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RegiaoMetropolitana a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRegiaoMetropolitana, trans))
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
		/// <param name="objRegiaoMetropolitana">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RegiaoMetropolitana objRegiaoMetropolitana)
		{
			try
			{
				if (dr.Read())
				{
					objRegiaoMetropolitana._idRegiaoMetropolitana = Convert.ToInt32(dr["Idf_Regiao_Metropolitana"]);
					objRegiaoMetropolitana._nomeRegiaoMetropolitana = Convert.ToString(dr["Nme_Regiao_Metropolitana"]);
					objRegiaoMetropolitana._nomeRegiaoMetropolitanaPesquisa = Convert.ToString(dr["Nme_Regiao_Metropolitana_Pesquisa"]);
					objRegiaoMetropolitana._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					objRegiaoMetropolitana._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objRegiaoMetropolitana._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Sig_UF"] != DBNull.Value)
						objRegiaoMetropolitana._siglaUF = Convert.ToString(dr["Sig_UF"]);
					if (dr["CID_Regiao_Metropolitana"] != DBNull.Value)
						objRegiaoMetropolitana._CIDRegiaoMetropolitana = Convert.ToString(dr["CID_Regiao_Metropolitana"]);

					objRegiaoMetropolitana._persisted = true;
					objRegiaoMetropolitana._modified = false;

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