//-- Data: 28/02/2013 11:38
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class BandeiraCartao // Tabela: BNE_Bandeira_Cartao
	{
		#region Atributos
		private int _idBandeiraCartao;
		private string _descricaoBandeiraCartao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdBandeiraCartao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdBandeiraCartao
		{
			get
			{
				return this._idBandeiraCartao;
			}
		}
		#endregion 

		#region DescricaoBandeiraCartao
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoBandeiraCartao
		{
			get
			{
				return this._descricaoBandeiraCartao;
			}
			set
			{
				this._descricaoBandeiraCartao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public BandeiraCartao()
		{
		}
		public BandeiraCartao(int idBandeiraCartao)
		{
			this._idBandeiraCartao = idBandeiraCartao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Bandeira_Cartao (Des_Bandeira_Cartao) VALUES (@Des_Bandeira_Cartao);SET @Idf_Bandeira_Cartao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Bandeira_Cartao SET Des_Bandeira_Cartao = @Des_Bandeira_Cartao WHERE Idf_Bandeira_Cartao = @Idf_Bandeira_Cartao";
		private const string SPDELETE = "DELETE FROM BNE_Bandeira_Cartao WHERE Idf_Bandeira_Cartao = @Idf_Bandeira_Cartao";
		private const string SPSELECTID = "SELECT * FROM BNE_Bandeira_Cartao WITH(NOLOCK) WHERE Idf_Bandeira_Cartao = @Idf_Bandeira_Cartao";
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
			parms.Add(new SqlParameter("@Idf_Bandeira_Cartao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Bandeira_Cartao", SqlDbType.VarChar, 50));
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
			parms[0].Value = this._idBandeiraCartao;
			parms[1].Value = this._descricaoBandeiraCartao;

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
		/// Método utilizado para inserir uma instância de BandeiraCartao no banco de dados.
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
						this._idBandeiraCartao = Convert.ToInt32(cmd.Parameters["@Idf_Bandeira_Cartao"].Value);
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
		/// Método utilizado para inserir uma instância de BandeiraCartao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idBandeiraCartao = Convert.ToInt32(cmd.Parameters["@Idf_Bandeira_Cartao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de BandeiraCartao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de BandeiraCartao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de BandeiraCartao no banco de dados.
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
		/// Método utilizado para salvar uma instância de BandeiraCartao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de BandeiraCartao no banco de dados.
		/// </summary>
		/// <param name="idBandeiraCartao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idBandeiraCartao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Bandeira_Cartao", SqlDbType.Int, 4));

			parms[0].Value = idBandeiraCartao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de BandeiraCartao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idBandeiraCartao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idBandeiraCartao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Bandeira_Cartao", SqlDbType.Int, 4));

			parms[0].Value = idBandeiraCartao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de BandeiraCartao no banco de dados.
		/// </summary>
		/// <param name="idBandeiraCartao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idBandeiraCartao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Bandeira_Cartao where Idf_Bandeira_Cartao in (";

			for (int i = 0; i < idBandeiraCartao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idBandeiraCartao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idBandeiraCartao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idBandeiraCartao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Bandeira_Cartao", SqlDbType.Int, 4));

			parms[0].Value = idBandeiraCartao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idBandeiraCartao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idBandeiraCartao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Bandeira_Cartao", SqlDbType.Int, 4));

			parms[0].Value = idBandeiraCartao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ban.Idf_Bandeira_Cartao, Ban.Des_Bandeira_Cartao FROM BNE_Bandeira_Cartao Ban";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de BandeiraCartao a partir do banco de dados.
		/// </summary>
		/// <param name="idBandeiraCartao">Chave do registro.</param>
		/// <returns>Instância de BandeiraCartao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static BandeiraCartao LoadObject(int idBandeiraCartao)
		{
			using (IDataReader dr = LoadDataReader(idBandeiraCartao))
			{
				BandeiraCartao objBandeiraCartao = new BandeiraCartao();
				if (SetInstance(dr, objBandeiraCartao))
					return objBandeiraCartao;
			}
			throw (new RecordNotFoundException(typeof(BandeiraCartao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de BandeiraCartao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idBandeiraCartao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de BandeiraCartao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static BandeiraCartao LoadObject(int idBandeiraCartao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idBandeiraCartao, trans))
			{
				BandeiraCartao objBandeiraCartao = new BandeiraCartao();
				if (SetInstance(dr, objBandeiraCartao))
					return objBandeiraCartao;
			}
			throw (new RecordNotFoundException(typeof(BandeiraCartao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de BandeiraCartao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idBandeiraCartao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de BandeiraCartao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idBandeiraCartao, trans))
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
		/// <param name="objBandeiraCartao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, BandeiraCartao objBandeiraCartao)
		{
			try
			{
				if (dr.Read())
				{
					objBandeiraCartao._idBandeiraCartao = Convert.ToInt32(dr["Idf_Bandeira_Cartao"]);
					objBandeiraCartao._descricaoBandeiraCartao = Convert.ToString(dr["Des_Bandeira_Cartao"]);

					objBandeiraCartao._persisted = true;
					objBandeiraCartao._modified = false;

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