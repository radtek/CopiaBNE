//-- Data: 29/07/2010 11:19
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class ConteudoHTML // Tabela: BNE_Conteudo_HTML
	{
		#region Atributos
		private int _idConteudo;
		private string _nomeConteudo;
		private string _valorConteudo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdConteudo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdConteudo
		{
			get
			{
				return this._idConteudo;
			}
			set
			{
				this._idConteudo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeConteudo
		/// <summary>
		/// Tamanho do campo: 70.
		/// Campo obrigatório.
		/// </summary>
		public string NomeConteudo
		{
			get
			{
				return this._nomeConteudo;
			}
			set
			{
				this._nomeConteudo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorConteudo
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo obrigatório.
		/// </summary>
		public string ValorConteudo
		{
			get
			{
				return this._valorConteudo;
			}
			set
			{
				this._valorConteudo = value;
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

		#region Construtores
		public ConteudoHTML()
		{
		}
		public ConteudoHTML(int idConteudo)
		{
			this._idConteudo = idConteudo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Conteudo_HTML (Idf_Conteudo, Nme_Conteudo, Vlr_Conteudo, Dta_Cadastro) VALUES (@Idf_Conteudo, @Nme_Conteudo, @Vlr_Conteudo, @Dta_Cadastro);";
		private const string SPUPDATE = "UPDATE BNE_Conteudo_HTML SET Nme_Conteudo = @Nme_Conteudo, Vlr_Conteudo = @Vlr_Conteudo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Conteudo = @Idf_Conteudo";
		private const string SPDELETE = "DELETE FROM BNE_Conteudo_HTML WHERE Idf_Conteudo = @Idf_Conteudo";
		private const string SPSELECTID = "SELECT * FROM BNE_Conteudo_HTML WHERE Idf_Conteudo = @Idf_Conteudo";
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
			parms.Add(new SqlParameter("@Idf_Conteudo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Conteudo", SqlDbType.VarChar, 70));
			parms.Add(new SqlParameter("@Vlr_Conteudo", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idConteudo;
			parms[1].Value = this._nomeConteudo;
			parms[2].Value = this._valorConteudo;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de ConteudoHTML no banco de dados.
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
		/// Método utilizado para inserir uma instância de ConteudoHTML no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de ConteudoHTML no banco de dados.
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
		/// Método utilizado para atualizar uma instância de ConteudoHTML no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de ConteudoHTML no banco de dados.
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
		/// Método utilizado para salvar uma instância de ConteudoHTML no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de ConteudoHTML no banco de dados.
		/// </summary>
		/// <param name="idConteudo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idConteudo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Conteudo", SqlDbType.Int, 4));

			parms[0].Value = idConteudo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de ConteudoHTML no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idConteudo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idConteudo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Conteudo", SqlDbType.Int, 4));

			parms[0].Value = idConteudo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de ConteudoHTML no banco de dados.
		/// </summary>
		/// <param name="idConteudo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idConteudo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Conteudo_HTML where Idf_Conteudo in (";

			for (int i = 0; i < idConteudo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idConteudo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idConteudo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idConteudo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Conteudo", SqlDbType.Int, 4));

			parms[0].Value = idConteudo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idConteudo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idConteudo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Conteudo", SqlDbType.Int, 4));

			parms[0].Value = idConteudo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Con.Idf_Conteudo, Con.Nme_Conteudo, Con.Vlr_Conteudo, Con.Dta_Cadastro FROM BNE_Conteudo_HTML Con";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de ConteudoHTML a partir do banco de dados.
		/// </summary>
		/// <param name="idConteudo">Chave do registro.</param>
		/// <returns>Instância de ConteudoHTML.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ConteudoHTML LoadObject(int idConteudo)
		{
			using (IDataReader dr = LoadDataReader(idConteudo))
			{
				ConteudoHTML objConteudoHTML = new ConteudoHTML();
				if (SetInstance(dr, objConteudoHTML))
					return objConteudoHTML;
			}
			throw (new RecordNotFoundException(typeof(ConteudoHTML)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de ConteudoHTML a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idConteudo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de ConteudoHTML.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ConteudoHTML LoadObject(int idConteudo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idConteudo, trans))
			{
				ConteudoHTML objConteudoHTML = new ConteudoHTML();
				if (SetInstance(dr, objConteudoHTML))
					return objConteudoHTML;
			}
			throw (new RecordNotFoundException(typeof(ConteudoHTML)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de ConteudoHTML a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idConteudo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de ConteudoHTML a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idConteudo, trans))
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
		/// <param name="objConteudoHTML">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, ConteudoHTML objConteudoHTML)
		{
			try
			{
				if (dr.Read())
				{
					objConteudoHTML._idConteudo = Convert.ToInt32(dr["Idf_Conteudo"]);
					objConteudoHTML._nomeConteudo = Convert.ToString(dr["Nme_Conteudo"]);
					objConteudoHTML._valorConteudo = Convert.ToString(dr["Vlr_Conteudo"]);
					objConteudoHTML._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objConteudoHTML._persisted = true;
					objConteudoHTML._modified = false;

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