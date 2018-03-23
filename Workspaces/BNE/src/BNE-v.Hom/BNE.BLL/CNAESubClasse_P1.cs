//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CNAESubClasse // Tabela: plataforma.TAB_CNAE_Sub_Classe
	{
		#region Atributos
		private int _idCNAESubClasse;
		private string _codigoCNAESubClasse;
		private string _descricaoCNAESubClasse;
		private CNAEClasse _cNAEClasse;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCNAESubClasse
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCNAESubClasse
		{
			get
			{
				return this._idCNAESubClasse;
			}
			set
			{
				this._idCNAESubClasse = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoCNAESubClasse
		/// <summary>
		/// Tamanho do campo: 7.
		/// Campo obrigatório.
		/// </summary>
		public string CodigoCNAESubClasse
		{
			get
			{
				return this._codigoCNAESubClasse;
			}
			set
			{
				this._codigoCNAESubClasse = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCNAESubClasse
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoCNAESubClasse
		{
			get
			{
				return this._descricaoCNAESubClasse;
			}
			set
			{
				this._descricaoCNAESubClasse = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CNAEClasse
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CNAEClasse CNAEClasse
		{
			get
			{
				return this._cNAEClasse;
			}
			set
			{
				this._cNAEClasse = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CNAESubClasse()
		{
		}
		public CNAESubClasse(int idCNAESubClasse)
		{
			this._idCNAESubClasse = idCNAESubClasse;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_CNAE_Sub_Classe (Idf_CNAE_Sub_Classe, Cod_CNAE_Sub_Classe, Des_CNAE_Sub_Classe, Idf_CNAE_Classe) VALUES (@Idf_CNAE_Sub_Classe, @Cod_CNAE_Sub_Classe, @Des_CNAE_Sub_Classe, @Idf_CNAE_Classe);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_CNAE_Sub_Classe SET Cod_CNAE_Sub_Classe = @Cod_CNAE_Sub_Classe, Des_CNAE_Sub_Classe = @Des_CNAE_Sub_Classe, Idf_CNAE_Classe = @Idf_CNAE_Classe WHERE Idf_CNAE_Sub_Classe = @Idf_CNAE_Sub_Classe";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_CNAE_Sub_Classe WHERE Idf_CNAE_Sub_Classe = @Idf_CNAE_Sub_Classe";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_CNAE_Sub_Classe WHERE Idf_CNAE_Sub_Classe = @Idf_CNAE_Sub_Classe";
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
			parms.Add(new SqlParameter("@Idf_CNAE_Sub_Classe", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Cod_CNAE_Sub_Classe", SqlDbType.Char, 7));
			parms.Add(new SqlParameter("@Des_CNAE_Sub_Classe", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Idf_CNAE_Classe", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCNAESubClasse;
			parms[1].Value = this._codigoCNAESubClasse;
			parms[2].Value = this._descricaoCNAESubClasse;
			parms[3].Value = this._cNAEClasse.IdCNAEClasse;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CNAESubClasse no banco de dados.
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
		/// Método utilizado para inserir uma instância de CNAESubClasse no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de CNAESubClasse no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CNAESubClasse no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CNAESubClasse no banco de dados.
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
		/// Método utilizado para salvar uma instância de CNAESubClasse no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CNAESubClasse no banco de dados.
		/// </summary>
		/// <param name="idCNAESubClasse">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCNAESubClasse)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Sub_Classe", SqlDbType.Int, 4));

			parms[0].Value = idCNAESubClasse;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CNAESubClasse no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCNAESubClasse">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCNAESubClasse, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Sub_Classe", SqlDbType.Int, 4));

			parms[0].Value = idCNAESubClasse;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CNAESubClasse no banco de dados.
		/// </summary>
		/// <param name="idCNAESubClasse">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCNAESubClasse)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_CNAE_Sub_Classe where Idf_CNAE_Sub_Classe in (";

			for (int i = 0; i < idCNAESubClasse.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCNAESubClasse[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCNAESubClasse">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCNAESubClasse)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Sub_Classe", SqlDbType.Int, 4));

			parms[0].Value = idCNAESubClasse;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCNAESubClasse">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCNAESubClasse, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Sub_Classe", SqlDbType.Int, 4));

			parms[0].Value = idCNAESubClasse;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, CNA.Idf_CNAE_Sub_Classe, CNA.Cod_CNAE_Sub_Classe, CNA.Des_CNAE_Sub_Classe, CNA.Idf_CNAE_Classe FROM plataforma.TAB_CNAE_Sub_Classe CNA";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objCNAESubClasse">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CNAESubClasse objCNAESubClasse)
		{
			try
			{
				if (dr.Read())
				{
					objCNAESubClasse._idCNAESubClasse = Convert.ToInt32(dr["Idf_CNAE_Sub_Classe"]);
					objCNAESubClasse._codigoCNAESubClasse = Convert.ToString(dr["Cod_CNAE_Sub_Classe"]);
					objCNAESubClasse._descricaoCNAESubClasse = Convert.ToString(dr["Des_CNAE_Sub_Classe"]);
					objCNAESubClasse._cNAEClasse = new CNAEClasse(Convert.ToInt32(dr["Idf_CNAE_Classe"]));

					objCNAESubClasse._persisted = true;
					objCNAESubClasse._modified = false;

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