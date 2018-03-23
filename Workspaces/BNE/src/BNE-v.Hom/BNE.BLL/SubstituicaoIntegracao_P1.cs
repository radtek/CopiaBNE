//-- Data: 15/05/2013 15:50
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class SubstituicaoIntegracao // Tabela: TAB_Substituicao_Integracao
	{
		#region Atributos
		private int _idSubstituicaoIntegracao;
		private string _descricaoAntiga;
		private string _descricaoNova;
		private RegraSubstituicaoIntegracao _regraSubstituicaoIntegracao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdSubstituicaoIntegracao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdSubstituicaoIntegracao
		{
			get
			{
				return this._idSubstituicaoIntegracao;
			}
		}
		#endregion 

		#region DescricaoAntiga
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoAntiga
		{
			get
			{
				return this._descricaoAntiga;
			}
			set
			{
				this._descricaoAntiga = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoNova
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoNova
		{
			get
			{
				return this._descricaoNova;
			}
			set
			{
				this._descricaoNova = value;
				this._modified = true;
			}
		}
		#endregion 

		#region RegraSubstituicaoIntegracao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public RegraSubstituicaoIntegracao RegraSubstituicaoIntegracao
		{
			get
			{
				return this._regraSubstituicaoIntegracao;
			}
			set
			{
				this._regraSubstituicaoIntegracao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public SubstituicaoIntegracao()
		{
		}
		public SubstituicaoIntegracao(int idSubstituicaoIntegracao)
		{
			this._idSubstituicaoIntegracao = idSubstituicaoIntegracao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Substituicao_Integracao (Des_Antiga, Des_Nova, Idf_Regra_Substituicao_Integracao) VALUES (@Des_Antiga, @Des_Nova, @Idf_Regra_Substituicao_Integracao);SET @Idf_Substituicao_Integracao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Substituicao_Integracao SET Des_Antiga = @Des_Antiga, Des_Nova = @Des_Nova, Idf_Regra_Substituicao_Integracao = @Idf_Regra_Substituicao_Integracao WHERE Idf_Substituicao_Integracao = @Idf_Substituicao_Integracao";
		private const string SPDELETE = "DELETE FROM TAB_Substituicao_Integracao WHERE Idf_Substituicao_Integracao = @Idf_Substituicao_Integracao";
		private const string SPSELECTID = "SELECT * FROM TAB_Substituicao_Integracao WITH(NOLOCK) WHERE Idf_Substituicao_Integracao = @Idf_Substituicao_Integracao";
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
			parms.Add(new SqlParameter("@Idf_Substituicao_Integracao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Antiga", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@Des_Nova", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@Idf_Regra_Substituicao_Integracao", SqlDbType.Int, 4));
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
			parms[0].Value = this._idSubstituicaoIntegracao;
			parms[1].Value = this._descricaoAntiga;
			parms[2].Value = this._descricaoNova;

			if (this._regraSubstituicaoIntegracao != null)
				parms[3].Value = this._regraSubstituicaoIntegracao.IdRegraSubstituicaoIntegracao;
			else
				parms[3].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de SubstituicaoIntegracao no banco de dados.
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
						this._idSubstituicaoIntegracao = Convert.ToInt32(cmd.Parameters["@Idf_Substituicao_Integracao"].Value);
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
		/// Método utilizado para inserir uma instância de SubstituicaoIntegracao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idSubstituicaoIntegracao = Convert.ToInt32(cmd.Parameters["@Idf_Substituicao_Integracao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de SubstituicaoIntegracao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de SubstituicaoIntegracao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de SubstituicaoIntegracao no banco de dados.
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
		/// Método utilizado para salvar uma instância de SubstituicaoIntegracao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de SubstituicaoIntegracao no banco de dados.
		/// </summary>
		/// <param name="idSubstituicaoIntegracao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idSubstituicaoIntegracao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Substituicao_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idSubstituicaoIntegracao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de SubstituicaoIntegracao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSubstituicaoIntegracao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idSubstituicaoIntegracao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Substituicao_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idSubstituicaoIntegracao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de SubstituicaoIntegracao no banco de dados.
		/// </summary>
		/// <param name="idSubstituicaoIntegracao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idSubstituicaoIntegracao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Substituicao_Integracao where Idf_Substituicao_Integracao in (";

			for (int i = 0; i < idSubstituicaoIntegracao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idSubstituicaoIntegracao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idSubstituicaoIntegracao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idSubstituicaoIntegracao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Substituicao_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idSubstituicaoIntegracao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSubstituicaoIntegracao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idSubstituicaoIntegracao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Substituicao_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idSubstituicaoIntegracao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sub.Idf_Substituicao_Integracao, Sub.Des_Antiga, Sub.Des_Nova, Sub.Idf_Regra_Substituicao_Integracao FROM TAB_Substituicao_Integracao Sub";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de SubstituicaoIntegracao a partir do banco de dados.
		/// </summary>
		/// <param name="idSubstituicaoIntegracao">Chave do registro.</param>
		/// <returns>Instância de SubstituicaoIntegracao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static SubstituicaoIntegracao LoadObject(int idSubstituicaoIntegracao)
		{
			using (IDataReader dr = LoadDataReader(idSubstituicaoIntegracao))
			{
				SubstituicaoIntegracao objSubstituicaoIntegracao = new SubstituicaoIntegracao();
				if (SetInstance(dr, objSubstituicaoIntegracao))
					return objSubstituicaoIntegracao;
			}
			throw (new RecordNotFoundException(typeof(SubstituicaoIntegracao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de SubstituicaoIntegracao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSubstituicaoIntegracao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de SubstituicaoIntegracao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static SubstituicaoIntegracao LoadObject(int idSubstituicaoIntegracao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idSubstituicaoIntegracao, trans))
			{
				SubstituicaoIntegracao objSubstituicaoIntegracao = new SubstituicaoIntegracao();
				if (SetInstance(dr, objSubstituicaoIntegracao))
					return objSubstituicaoIntegracao;
			}
			throw (new RecordNotFoundException(typeof(SubstituicaoIntegracao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de SubstituicaoIntegracao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idSubstituicaoIntegracao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de SubstituicaoIntegracao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idSubstituicaoIntegracao, trans))
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
		/// <param name="objSubstituicaoIntegracao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, SubstituicaoIntegracao objSubstituicaoIntegracao)
		{
			try
			{
				if (dr.Read())
				{
					objSubstituicaoIntegracao._idSubstituicaoIntegracao = Convert.ToInt32(dr["Idf_Substituicao_Integracao"]);
					objSubstituicaoIntegracao._descricaoAntiga = Convert.ToString(dr["Des_Antiga"]);
					objSubstituicaoIntegracao._descricaoNova = Convert.ToString(dr["Des_Nova"]);
					if (dr["Idf_Regra_Substituicao_Integracao"] != DBNull.Value)
						objSubstituicaoIntegracao._regraSubstituicaoIntegracao = new RegraSubstituicaoIntegracao(Convert.ToInt32(dr["Idf_Regra_Substituicao_Integracao"]));

					objSubstituicaoIntegracao._persisted = true;
					objSubstituicaoIntegracao._modified = false;

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