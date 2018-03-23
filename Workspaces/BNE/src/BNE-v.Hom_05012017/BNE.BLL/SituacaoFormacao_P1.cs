//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class SituacaoFormacao // Tabela: BNE_Situacao_Formacao
	{
		#region Atributos
		private Int16 _idSituacaoFormacao;
		private string _descricaoSituacaoFormacao;
		private bool _flagInativo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdSituacaoFormacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Int16 IdSituacaoFormacao
		{
			get
			{
				return this._idSituacaoFormacao;
			}
			set
			{
				this._idSituacaoFormacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoSituacaoFormacao
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoSituacaoFormacao
		{
			get
			{
				return this._descricaoSituacaoFormacao;
			}
			set
			{
				this._descricaoSituacaoFormacao = value;
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

		#endregion

		#region Construtores
		public SituacaoFormacao()
		{
		}
		public SituacaoFormacao(Int16 idSituacaoFormacao)
		{
			this._idSituacaoFormacao = idSituacaoFormacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Situacao_Formacao (Idf_Situacao_Formacao, Des_Situacao_Formacao, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Situacao_Formacao, @Des_Situacao_Formacao, @Flg_Inativo, @Dta_Cadastro);";
		private const string SPUPDATE = "UPDATE BNE_Situacao_Formacao SET Des_Situacao_Formacao = @Des_Situacao_Formacao, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Situacao_Formacao = @Idf_Situacao_Formacao";
		private const string SPDELETE = "DELETE FROM BNE_Situacao_Formacao WHERE Idf_Situacao_Formacao = @Idf_Situacao_Formacao";
		private const string SPSELECTID = "SELECT * FROM BNE_Situacao_Formacao WHERE Idf_Situacao_Formacao = @Idf_Situacao_Formacao";
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
			parms.Add(new SqlParameter("@Idf_Situacao_Formacao", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Des_Situacao_Formacao", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idSituacaoFormacao;
			parms[1].Value = this._descricaoSituacaoFormacao;
			parms[2].Value = this._flagInativo;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de SituacaoFormacao no banco de dados.
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
		/// Método utilizado para inserir uma instância de SituacaoFormacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de SituacaoFormacao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de SituacaoFormacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de SituacaoFormacao no banco de dados.
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
		/// Método utilizado para salvar uma instância de SituacaoFormacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de SituacaoFormacao no banco de dados.
		/// </summary>
		/// <param name="idSituacaoFormacao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(Int16 idSituacaoFormacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Situacao_Formacao", SqlDbType.Int, 2));

			parms[0].Value = idSituacaoFormacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de SituacaoFormacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSituacaoFormacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(Int16 idSituacaoFormacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Situacao_Formacao", SqlDbType.Int, 2));

			parms[0].Value = idSituacaoFormacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de SituacaoFormacao no banco de dados.
		/// </summary>
		/// <param name="idSituacaoFormacao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<Int16> idSituacaoFormacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Situacao_Formacao where Idf_Situacao_Formacao in (";

			for (int i = 0; i < idSituacaoFormacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 2));
				parms[i].Value = idSituacaoFormacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idSituacaoFormacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(Int16 idSituacaoFormacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Situacao_Formacao", SqlDbType.Int, 2));

			parms[0].Value = idSituacaoFormacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSituacaoFormacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(Int16 idSituacaoFormacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Situacao_Formacao", SqlDbType.Int, 2));

			parms[0].Value = idSituacaoFormacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sit.Idf_Situacao_Formacao, Sit.Des_Situacao_Formacao, Sit.Flg_Inativo, Sit.Dta_Cadastro FROM BNE_Situacao_Formacao Sit";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de SituacaoFormacao a partir do banco de dados.
		/// </summary>
		/// <param name="idSituacaoFormacao">Chave do registro.</param>
		/// <returns>Instância de SituacaoFormacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static SituacaoFormacao LoadObject(Int16 idSituacaoFormacao)
		{
			using (IDataReader dr = LoadDataReader(idSituacaoFormacao))
			{
				SituacaoFormacao objSituacaoFormacao = new SituacaoFormacao();
				if (SetInstance(dr, objSituacaoFormacao))
					return objSituacaoFormacao;
			}
			throw (new RecordNotFoundException(typeof(SituacaoFormacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de SituacaoFormacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSituacaoFormacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de SituacaoFormacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static SituacaoFormacao LoadObject(Int16 idSituacaoFormacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idSituacaoFormacao, trans))
			{
				SituacaoFormacao objSituacaoFormacao = new SituacaoFormacao();
				if (SetInstance(dr, objSituacaoFormacao))
					return objSituacaoFormacao;
			}
			throw (new RecordNotFoundException(typeof(SituacaoFormacao)));
		}
		#endregion
        
		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objSituacaoFormacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, SituacaoFormacao objSituacaoFormacao)
		{
			try
			{
				if (dr.Read())
				{
					objSituacaoFormacao._idSituacaoFormacao = Convert.ToInt16(dr["Idf_Situacao_Formacao"]);
					objSituacaoFormacao._descricaoSituacaoFormacao = Convert.ToString(dr["Des_Situacao_Formacao"]);
					objSituacaoFormacao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objSituacaoFormacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objSituacaoFormacao._persisted = true;
					objSituacaoFormacao._modified = false;

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