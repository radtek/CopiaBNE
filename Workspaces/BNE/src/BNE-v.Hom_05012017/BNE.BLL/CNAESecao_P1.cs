//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CNAESecao // Tabela: plataforma.TAB_CNAE_Secao
	{
		#region Atributos
		private int _idCNAESecao;
		private char _codigoCNAESecao;
		private string _descricaoCNAESecao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCNAESecao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCNAESecao
		{
			get
			{
				return this._idCNAESecao;
			}
			set
			{
				this._idCNAESecao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoCNAESecao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public char CodigoCNAESecao
		{
			get
			{
				return this._codigoCNAESecao;
			}
			set
			{
				this._codigoCNAESecao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCNAESecao
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoCNAESecao
		{
			get
			{
				return this._descricaoCNAESecao;
			}
			set
			{
				this._descricaoCNAESecao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CNAESecao()
		{
		}
		public CNAESecao(int idCNAESecao)
		{
			this._idCNAESecao = idCNAESecao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_CNAE_Secao (Idf_CNAE_Secao, Cod_CNAE_Secao, Des_CNAE_Secao) VALUES (@Idf_CNAE_Secao, @Cod_CNAE_Secao, @Des_CNAE_Secao);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_CNAE_Secao SET Cod_CNAE_Secao = @Cod_CNAE_Secao, Des_CNAE_Secao = @Des_CNAE_Secao WHERE Idf_CNAE_Secao = @Idf_CNAE_Secao";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_CNAE_Secao WHERE Idf_CNAE_Secao = @Idf_CNAE_Secao";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_CNAE_Secao WHERE Idf_CNAE_Secao = @Idf_CNAE_Secao";
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
			parms.Add(new SqlParameter("@Idf_CNAE_Secao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Cod_CNAE_Secao", SqlDbType.Char, 1));
			parms.Add(new SqlParameter("@Des_CNAE_Secao", SqlDbType.VarChar, 200));
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
			parms[0].Value = this._idCNAESecao;
			parms[1].Value = this._codigoCNAESecao;
			parms[2].Value = this._descricaoCNAESecao;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CNAESecao no banco de dados.
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
		/// Método utilizado para inserir uma instância de CNAESecao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de CNAESecao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CNAESecao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CNAESecao no banco de dados.
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
		/// Método utilizado para salvar uma instância de CNAESecao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CNAESecao no banco de dados.
		/// </summary>
		/// <param name="idCNAESecao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCNAESecao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Secao", SqlDbType.Int, 4));

			parms[0].Value = idCNAESecao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CNAESecao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCNAESecao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCNAESecao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Secao", SqlDbType.Int, 4));

			parms[0].Value = idCNAESecao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CNAESecao no banco de dados.
		/// </summary>
		/// <param name="idCNAESecao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCNAESecao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_CNAE_Secao where Idf_CNAE_Secao in (";

			for (int i = 0; i < idCNAESecao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCNAESecao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCNAESecao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCNAESecao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Secao", SqlDbType.Int, 4));

			parms[0].Value = idCNAESecao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCNAESecao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCNAESecao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Secao", SqlDbType.Int, 4));

			parms[0].Value = idCNAESecao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, CNA.Idf_CNAE_Secao, CNA.Cod_CNAE_Secao, CNA.Des_CNAE_Secao FROM plataforma.TAB_CNAE_Secao CNA";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CNAESecao a partir do banco de dados.
		/// </summary>
		/// <param name="idCNAESecao">Chave do registro.</param>
		/// <returns>Instância de CNAESecao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CNAESecao LoadObject(int idCNAESecao)
		{
			using (IDataReader dr = LoadDataReader(idCNAESecao))
			{
				CNAESecao objCNAESecao = new CNAESecao();
				if (SetInstance(dr, objCNAESecao))
					return objCNAESecao;
			}
			throw (new RecordNotFoundException(typeof(CNAESecao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CNAESecao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCNAESecao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CNAESecao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CNAESecao LoadObject(int idCNAESecao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCNAESecao, trans))
			{
				CNAESecao objCNAESecao = new CNAESecao();
				if (SetInstance(dr, objCNAESecao))
					return objCNAESecao;
			}
			throw (new RecordNotFoundException(typeof(CNAESecao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CNAESecao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCNAESecao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CNAESecao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCNAESecao, trans))
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
		/// <param name="objCNAESecao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CNAESecao objCNAESecao)
		{
			try
			{
				if (dr.Read())
				{
					objCNAESecao._idCNAESecao = Convert.ToInt32(dr["Idf_CNAE_Secao"]);
					objCNAESecao._codigoCNAESecao = Convert.ToChar(dr["Cod_CNAE_Secao"]);
					objCNAESecao._descricaoCNAESecao = Convert.ToString(dr["Des_CNAE_Secao"]);

					objCNAESecao._persisted = true;
					objCNAESecao._modified = false;

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