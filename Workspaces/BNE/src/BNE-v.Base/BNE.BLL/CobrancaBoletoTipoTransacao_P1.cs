//-- Data: 02/05/2014 17:31
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CobrancaBoletoTipoTransacao // Tabela: plataforma.TAB_Tipo_Transacao
	{
		#region Atributos
		private int _idCobrancaBoletoTipoTransacao;
		private string _descricaoTransacao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCobrancaBoletoTipoTransacao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCobrancaBoletoTipoTransacao
		{
			get
			{
				return this._idCobrancaBoletoTipoTransacao;
			}
		}
		#endregion 

		#region DescricaoTransacao
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoTransacao
		{
			get
			{
				return this._descricaoTransacao;
			}
			set
			{
				this._descricaoTransacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CobrancaBoletoTipoTransacao()
		{
		}
        public CobrancaBoletoTipoTransacao(int idCobrancaBoletoTipoTransacao)
		{
			this._idCobrancaBoletoTipoTransacao = idCobrancaBoletoTipoTransacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Tipo_Transacao (Des_Transacao) VALUES (@Des_Transacao);SET @Idf_Tipo_Transacao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Tipo_Transacao SET Des_Transacao = @Des_Transacao WHERE Idf_Tipo_Transacao = @Idf_Tipo_Transacao";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Tipo_Transacao WHERE Idf_Tipo_Transacao = @Idf_Tipo_Transacao";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Tipo_Transacao WITH(NOLOCK) WHERE Idf_Tipo_Transacao = @Idf_Tipo_Transacao";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Transacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Transacao", SqlDbType.VarChar, 50));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idCobrancaBoletoTipoTransacao;

			if (!String.IsNullOrEmpty(this._descricaoTransacao))
				parms[1].Value = this._descricaoTransacao;
			else
				parms[1].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de CobrancaBoletoTipoTransacao no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
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
						this._idCobrancaBoletoTipoTransacao = Convert.ToInt32(cmd.Parameters["@Idf_Tipo_Transacao"].Value);
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
		/// Método utilizado para inserir uma instância de CobrancaBoletoTipoTransacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCobrancaBoletoTipoTransacao = Convert.ToInt32(cmd.Parameters["@Idf_Tipo_Transacao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CobrancaBoletoTipoTransacao no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para atualizar uma instância de CobrancaBoletoTipoTransacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoTipoTransacao no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de CobrancaBoletoTipoTransacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para excluir uma instância de CobrancaBoletoTipoTransacao no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoTipoTransacao">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoTipoTransacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoTipoTransacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CobrancaBoletoTipoTransacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoTipoTransacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoTipoTransacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoTipoTransacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CobrancaBoletoTipoTransacao no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoTipoTransacao">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idCobrancaBoletoTipoTransacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Tipo_Transacao where Idf_Tipo_Transacao in (";

			for (int i = 0; i < idCobrancaBoletoTipoTransacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCobrancaBoletoTipoTransacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoTipoTransacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoTipoTransacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoTipoTransacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoTipoTransacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoTipoTransacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoTipoTransacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Transacao, Tip.Des_Transacao FROM plataforma.TAB_Tipo_Transacao Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoTipoTransacao a partir do banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoTipoTransacao">Chave do registro.</param>
		/// <returns>Instância de CobrancaBoletoTipoTransacao.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoTipoTransacao LoadObject(int idCobrancaBoletoTipoTransacao)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoTipoTransacao))
			{
				CobrancaBoletoTipoTransacao objCobrancaBoletoTipoTransacao = new CobrancaBoletoTipoTransacao();
				if (SetInstance(dr, objCobrancaBoletoTipoTransacao))
					return objCobrancaBoletoTipoTransacao;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoTipoTransacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoTipoTransacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoTipoTransacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CobrancaBoletoTipoTransacao.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoTipoTransacao LoadObject(int idCobrancaBoletoTipoTransacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoTipoTransacao, trans))
			{
				CobrancaBoletoTipoTransacao objCobrancaBoletoTipoTransacao = new CobrancaBoletoTipoTransacao();
				if (SetInstance(dr, objCobrancaBoletoTipoTransacao))
					return objCobrancaBoletoTipoTransacao;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoTipoTransacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoTipoTransacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoTipoTransacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoTipoTransacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoTipoTransacao, trans))
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
		/// <param name="objCobrancaBoletoTipoTransacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, CobrancaBoletoTipoTransacao objCobrancaBoletoTipoTransacao)
		{
			try
			{
				if (dr.Read())
				{
					objCobrancaBoletoTipoTransacao._idCobrancaBoletoTipoTransacao = Convert.ToInt32(dr["Idf_Tipo_Transacao"]);
					if (dr["Des_Transacao"] != DBNull.Value)
						objCobrancaBoletoTipoTransacao._descricaoTransacao = Convert.ToString(dr["Des_Transacao"]);

					objCobrancaBoletoTipoTransacao._persisted = true;
					objCobrancaBoletoTipoTransacao._modified = false;

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