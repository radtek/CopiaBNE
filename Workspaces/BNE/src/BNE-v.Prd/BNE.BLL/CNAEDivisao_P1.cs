//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CNAEDivisao // Tabela: plataforma.TAB_CNAE_Divisao
	{
		#region Atributos
		private int _idCNAEDivisao;
		private string _codigoCNAEDivisao;
		private string _descricaoCNAEDivisao;
		private CNAESecao _cNAESecao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCNAEDivisao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCNAEDivisao
		{
			get
			{
				return this._idCNAEDivisao;
			}
			set
			{
				this._idCNAEDivisao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoCNAEDivisao
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo obrigatório.
		/// </summary>
		public string CodigoCNAEDivisao
		{
			get
			{
				return this._codigoCNAEDivisao;
			}
			set
			{
				this._codigoCNAEDivisao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCNAEDivisao
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoCNAEDivisao
		{
			get
			{
				return this._descricaoCNAEDivisao;
			}
			set
			{
				this._descricaoCNAEDivisao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CNAESecao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CNAESecao CNAESecao
		{
			get
			{
				return this._cNAESecao;
			}
			set
			{
				this._cNAESecao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CNAEDivisao()
		{
		}
		public CNAEDivisao(int idCNAEDivisao)
		{
			this._idCNAEDivisao = idCNAEDivisao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_CNAE_Divisao (Idf_CNAE_Divisao, Cod_CNAE_Divisao, Des_CNAE_Divisao, Idf_CNAE_Secao) VALUES (@Idf_CNAE_Divisao, @Cod_CNAE_Divisao, @Des_CNAE_Divisao, @Idf_CNAE_Secao);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_CNAE_Divisao SET Cod_CNAE_Divisao = @Cod_CNAE_Divisao, Des_CNAE_Divisao = @Des_CNAE_Divisao, Idf_CNAE_Secao = @Idf_CNAE_Secao WHERE Idf_CNAE_Divisao = @Idf_CNAE_Divisao";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_CNAE_Divisao WHERE Idf_CNAE_Divisao = @Idf_CNAE_Divisao";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_CNAE_Divisao WHERE Idf_CNAE_Divisao = @Idf_CNAE_Divisao";
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
			parms.Add(new SqlParameter("@Idf_CNAE_Divisao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Cod_CNAE_Divisao", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Des_CNAE_Divisao", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Idf_CNAE_Secao", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCNAEDivisao;
			parms[1].Value = this._codigoCNAEDivisao;
			parms[2].Value = this._descricaoCNAEDivisao;
			parms[3].Value = this._cNAESecao.IdCNAESecao;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CNAEDivisao no banco de dados.
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
		/// Método utilizado para inserir uma instância de CNAEDivisao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de CNAEDivisao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CNAEDivisao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CNAEDivisao no banco de dados.
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
		/// Método utilizado para salvar uma instância de CNAEDivisao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CNAEDivisao no banco de dados.
		/// </summary>
		/// <param name="idCNAEDivisao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCNAEDivisao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Divisao", SqlDbType.Int, 4));

			parms[0].Value = idCNAEDivisao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CNAEDivisao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCNAEDivisao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCNAEDivisao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Divisao", SqlDbType.Int, 4));

			parms[0].Value = idCNAEDivisao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CNAEDivisao no banco de dados.
		/// </summary>
		/// <param name="idCNAEDivisao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCNAEDivisao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_CNAE_Divisao where Idf_CNAE_Divisao in (";

			for (int i = 0; i < idCNAEDivisao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCNAEDivisao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCNAEDivisao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCNAEDivisao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Divisao", SqlDbType.Int, 4));

			parms[0].Value = idCNAEDivisao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCNAEDivisao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCNAEDivisao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Divisao", SqlDbType.Int, 4));

			parms[0].Value = idCNAEDivisao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, CNA.Idf_CNAE_Divisao, CNA.Cod_CNAE_Divisao, CNA.Des_CNAE_Divisao, CNA.Idf_CNAE_Secao FROM plataforma.TAB_CNAE_Divisao CNA";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CNAEDivisao a partir do banco de dados.
		/// </summary>
		/// <param name="idCNAEDivisao">Chave do registro.</param>
		/// <returns>Instância de CNAEDivisao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CNAEDivisao LoadObject(int idCNAEDivisao)
		{
			using (IDataReader dr = LoadDataReader(idCNAEDivisao))
			{
				CNAEDivisao objCNAEDivisao = new CNAEDivisao();
				if (SetInstance(dr, objCNAEDivisao))
					return objCNAEDivisao;
			}
			throw (new RecordNotFoundException(typeof(CNAEDivisao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CNAEDivisao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCNAEDivisao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CNAEDivisao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CNAEDivisao LoadObject(int idCNAEDivisao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCNAEDivisao, trans))
			{
				CNAEDivisao objCNAEDivisao = new CNAEDivisao();
				if (SetInstance(dr, objCNAEDivisao))
					return objCNAEDivisao;
			}
			throw (new RecordNotFoundException(typeof(CNAEDivisao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CNAEDivisao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCNAEDivisao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CNAEDivisao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCNAEDivisao, trans))
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
		/// <param name="objCNAEDivisao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CNAEDivisao objCNAEDivisao)
		{
			try
			{
				if (dr.Read())
				{
					objCNAEDivisao._idCNAEDivisao = Convert.ToInt32(dr["Idf_CNAE_Divisao"]);
					objCNAEDivisao._codigoCNAEDivisao = Convert.ToString(dr["Cod_CNAE_Divisao"]);
					objCNAEDivisao._descricaoCNAEDivisao = Convert.ToString(dr["Des_CNAE_Divisao"]);
					objCNAEDivisao._cNAESecao = new CNAESecao(Convert.ToInt32(dr["Idf_CNAE_Secao"]));

					objCNAEDivisao._persisted = true;
					objCNAEDivisao._modified = false;

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