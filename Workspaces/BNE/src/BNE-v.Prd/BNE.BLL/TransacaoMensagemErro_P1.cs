//-- Data: 06/03/2013 10:06
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TransacaoMensagemErro // Tabela: TAB_Transacao_Mensagem_Erro
	{
		#region Atributos
		private int _idTransacaoMensagemErro;
		private string _descricaoCodigoErro;
		private string _descricaoDescricaoErro;
		private string _descricaoMensagemAmigavel;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTransacaoMensagemErro
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdTransacaoMensagemErro
		{
			get
			{
				return this._idTransacaoMensagemErro;
			}
		}
		#endregion 

		#region DescricaoCodigoErro
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCodigoErro
		{
			get
			{
				return this._descricaoCodigoErro;
			}
			set
			{
				this._descricaoCodigoErro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoDescricaoErro
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo opcional.
		/// </summary>
		public string DescricaoDescricaoErro
		{
			get
			{
				return this._descricaoDescricaoErro;
			}
			set
			{
				this._descricaoDescricaoErro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMensagemAmigavel
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoMensagemAmigavel
		{
			get
			{
				return this._descricaoMensagemAmigavel;
			}
			set
			{
				this._descricaoMensagemAmigavel = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public TransacaoMensagemErro()
		{
		}
		public TransacaoMensagemErro(int idTransacaoMensagemErro)
		{
			this._idTransacaoMensagemErro = idTransacaoMensagemErro;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Transacao_Mensagem_Erro (Des_Codigo_Erro, Des_Descricao_Erro, Des_Mensagem_Amigavel) VALUES (@Des_Codigo_Erro, @Des_Descricao_Erro, @Des_Mensagem_Amigavel);SET @Idf_Transacao_Mensagem_Erro = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Transacao_Mensagem_Erro SET Des_Codigo_Erro = @Des_Codigo_Erro, Des_Descricao_Erro = @Des_Descricao_Erro, Des_Mensagem_Amigavel = @Des_Mensagem_Amigavel WHERE Idf_Transacao_Mensagem_Erro = @Idf_Transacao_Mensagem_Erro";
		private const string SPDELETE = "DELETE FROM TAB_Transacao_Mensagem_Erro WHERE Idf_Transacao_Mensagem_Erro = @Idf_Transacao_Mensagem_Erro";
		private const string SPSELECTID = "SELECT * FROM TAB_Transacao_Mensagem_Erro WITH(NOLOCK) WHERE Idf_Transacao_Mensagem_Erro = @Idf_Transacao_Mensagem_Erro";
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
			parms.Add(new SqlParameter("@Idf_Transacao_Mensagem_Erro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Codigo_Erro", SqlDbType.VarChar, 10));
			parms.Add(new SqlParameter("@Des_Descricao_Erro", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@Des_Mensagem_Amigavel", SqlDbType.VarChar, 500));
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
			parms[0].Value = this._idTransacaoMensagemErro;

			if (!String.IsNullOrEmpty(this._descricaoCodigoErro))
				parms[1].Value = this._descricaoCodigoErro;
			else
				parms[1].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoDescricaoErro))
				parms[2].Value = this._descricaoDescricaoErro;
			else
				parms[2].Value = DBNull.Value;

			parms[3].Value = this._descricaoMensagemAmigavel;

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
		/// Método utilizado para inserir uma instância de TransacaoMensagemErro no banco de dados.
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
						this._idTransacaoMensagemErro = Convert.ToInt32(cmd.Parameters["@Idf_Transacao_Mensagem_Erro"].Value);
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
		/// Método utilizado para inserir uma instância de TransacaoMensagemErro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idTransacaoMensagemErro = Convert.ToInt32(cmd.Parameters["@Idf_Transacao_Mensagem_Erro"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de TransacaoMensagemErro no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TransacaoMensagemErro no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TransacaoMensagemErro no banco de dados.
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
		/// Método utilizado para salvar uma instância de TransacaoMensagemErro no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TransacaoMensagemErro no banco de dados.
		/// </summary>
		/// <param name="idTransacaoMensagemErro">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTransacaoMensagemErro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Mensagem_Erro", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoMensagemErro;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TransacaoMensagemErro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTransacaoMensagemErro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTransacaoMensagemErro, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Mensagem_Erro", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoMensagemErro;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TransacaoMensagemErro no banco de dados.
		/// </summary>
		/// <param name="idTransacaoMensagemErro">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idTransacaoMensagemErro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Transacao_Mensagem_Erro where Idf_Transacao_Mensagem_Erro in (";

			for (int i = 0; i < idTransacaoMensagemErro.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTransacaoMensagemErro[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTransacaoMensagemErro">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTransacaoMensagemErro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Mensagem_Erro", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoMensagemErro;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTransacaoMensagemErro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTransacaoMensagemErro, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Mensagem_Erro", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoMensagemErro;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tra.Idf_Transacao_Mensagem_Erro, Tra.Des_Codigo_Erro, Tra.Des_Descricao_Erro, Tra.Des_Mensagem_Amigavel FROM TAB_Transacao_Mensagem_Erro Tra";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TransacaoMensagemErro a partir do banco de dados.
		/// </summary>
		/// <param name="idTransacaoMensagemErro">Chave do registro.</param>
		/// <returns>Instância de TransacaoMensagemErro.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TransacaoMensagemErro LoadObject(int idTransacaoMensagemErro)
		{
			using (IDataReader dr = LoadDataReader(idTransacaoMensagemErro))
			{
				TransacaoMensagemErro objTransacaoMensagemErro = new TransacaoMensagemErro();
				if (SetInstance(dr, objTransacaoMensagemErro))
					return objTransacaoMensagemErro;
			}
			throw (new RecordNotFoundException(typeof(TransacaoMensagemErro)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TransacaoMensagemErro a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTransacaoMensagemErro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TransacaoMensagemErro.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TransacaoMensagemErro LoadObject(int idTransacaoMensagemErro, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTransacaoMensagemErro, trans))
			{
				TransacaoMensagemErro objTransacaoMensagemErro = new TransacaoMensagemErro();
				if (SetInstance(dr, objTransacaoMensagemErro))
					return objTransacaoMensagemErro;
			}
			throw (new RecordNotFoundException(typeof(TransacaoMensagemErro)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TransacaoMensagemErro a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTransacaoMensagemErro))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TransacaoMensagemErro a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTransacaoMensagemErro, trans))
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
		/// <param name="objTransacaoMensagemErro">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, TransacaoMensagemErro objTransacaoMensagemErro)
		{
			try
			{
				if (dr.Read())
				{
					objTransacaoMensagemErro._idTransacaoMensagemErro = Convert.ToInt32(dr["Idf_Transacao_Mensagem_Erro"]);
					if (dr["Des_Codigo_Erro"] != DBNull.Value)
						objTransacaoMensagemErro._descricaoCodigoErro = Convert.ToString(dr["Des_Codigo_Erro"]);
					if (dr["Des_Descricao_Erro"] != DBNull.Value)
						objTransacaoMensagemErro._descricaoDescricaoErro = Convert.ToString(dr["Des_Descricao_Erro"]);
					objTransacaoMensagemErro._descricaoMensagemAmigavel = Convert.ToString(dr["Des_Mensagem_Amigavel"]);

					objTransacaoMensagemErro._persisted = true;
					objTransacaoMensagemErro._modified = false;

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