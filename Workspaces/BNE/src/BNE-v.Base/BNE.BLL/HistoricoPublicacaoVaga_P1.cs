//-- Data: 17/05/2013 10:15
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class HistoricoPublicacaoVaga // Tabela: BNE_Historico_Publicacao_Vaga
	{
		#region Atributos
		private int _idHistoricoPublicacaoVaga;
		private int _idVaga;
		private string _descricaoHistorico;
		private bool _flagInativo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdHistoricoPublicacaoVaga
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdHistoricoPublicacaoVaga
		{
			get
			{
				return this._idHistoricoPublicacaoVaga;
			}
		}
		#endregion 

		#region IdVaga
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdVaga
		{
			get
			{
				return this._idVaga;
			}
			set
			{
				this._idVaga = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoHistorico
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoHistorico
		{
			get
			{
				return this._descricaoHistorico;
			}
			set
			{
				this._descricaoHistorico = value;
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
		public HistoricoPublicacaoVaga()
		{
		}
		public HistoricoPublicacaoVaga(int idHistoricoPublicacaoVaga)
		{
			this._idHistoricoPublicacaoVaga = idHistoricoPublicacaoVaga;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Historico_Publicacao_Vaga (Idf_Vaga, Des_Historico, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Vaga, @Des_Historico, @Flg_Inativo, @Dta_Cadastro);SET @Idf_Historico_Publicacao_Vaga = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Historico_Publicacao_Vaga SET Idf_Vaga = @Idf_Vaga, Des_Historico = @Des_Historico, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Historico_Publicacao_Vaga = @Idf_Historico_Publicacao_Vaga";
		private const string SPDELETE = "DELETE FROM BNE_Historico_Publicacao_Vaga WHERE Idf_Historico_Publicacao_Vaga = @Idf_Historico_Publicacao_Vaga";
		private const string SPSELECTID = "SELECT * FROM BNE_Historico_Publicacao_Vaga WITH(NOLOCK) WHERE Idf_Historico_Publicacao_Vaga = @Idf_Historico_Publicacao_Vaga";
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
			parms.Add(new SqlParameter("@Idf_Historico_Publicacao_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Historico", SqlDbType.VarChar, 200));
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
		/// <remarks>Francisco Ribas</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idHistoricoPublicacaoVaga;
			parms[1].Value = this._idVaga;
			parms[2].Value = this._descricaoHistorico;
			parms[3].Value = this._flagInativo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[4].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de HistoricoPublicacaoVaga no banco de dados.
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
						this._idHistoricoPublicacaoVaga = Convert.ToInt32(cmd.Parameters["@Idf_Historico_Publicacao_Vaga"].Value);
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
		/// Método utilizado para inserir uma instância de HistoricoPublicacaoVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idHistoricoPublicacaoVaga = Convert.ToInt32(cmd.Parameters["@Idf_Historico_Publicacao_Vaga"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de HistoricoPublicacaoVaga no banco de dados.
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
		/// Método utilizado para atualizar uma instância de HistoricoPublicacaoVaga no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de HistoricoPublicacaoVaga no banco de dados.
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
		/// Método utilizado para salvar uma instância de HistoricoPublicacaoVaga no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de HistoricoPublicacaoVaga no banco de dados.
		/// </summary>
		/// <param name="idHistoricoPublicacaoVaga">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idHistoricoPublicacaoVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Historico_Publicacao_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idHistoricoPublicacaoVaga;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de HistoricoPublicacaoVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idHistoricoPublicacaoVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idHistoricoPublicacaoVaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Historico_Publicacao_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idHistoricoPublicacaoVaga;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de HistoricoPublicacaoVaga no banco de dados.
		/// </summary>
		/// <param name="idHistoricoPublicacaoVaga">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idHistoricoPublicacaoVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Historico_Publicacao_Vaga where Idf_Historico_Publicacao_Vaga in (";

			for (int i = 0; i < idHistoricoPublicacaoVaga.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idHistoricoPublicacaoVaga[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idHistoricoPublicacaoVaga">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idHistoricoPublicacaoVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Historico_Publicacao_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idHistoricoPublicacaoVaga;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idHistoricoPublicacaoVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idHistoricoPublicacaoVaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Historico_Publicacao_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idHistoricoPublicacaoVaga;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, His.Idf_Historico_Publicacao_Vaga, His.Idf_Vaga, His.Des_Historico, His.Flg_Inativo, His.Dta_Cadastro FROM BNE_Historico_Publicacao_Vaga His";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de HistoricoPublicacaoVaga a partir do banco de dados.
		/// </summary>
		/// <param name="idHistoricoPublicacaoVaga">Chave do registro.</param>
		/// <returns>Instância de HistoricoPublicacaoVaga.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static HistoricoPublicacaoVaga LoadObject(int idHistoricoPublicacaoVaga)
		{
			using (IDataReader dr = LoadDataReader(idHistoricoPublicacaoVaga))
			{
				HistoricoPublicacaoVaga objHistoricoPublicacaoVaga = new HistoricoPublicacaoVaga();
				if (SetInstance(dr, objHistoricoPublicacaoVaga))
					return objHistoricoPublicacaoVaga;
			}
			throw (new RecordNotFoundException(typeof(HistoricoPublicacaoVaga)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de HistoricoPublicacaoVaga a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idHistoricoPublicacaoVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de HistoricoPublicacaoVaga.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static HistoricoPublicacaoVaga LoadObject(int idHistoricoPublicacaoVaga, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idHistoricoPublicacaoVaga, trans))
			{
				HistoricoPublicacaoVaga objHistoricoPublicacaoVaga = new HistoricoPublicacaoVaga();
				if (SetInstance(dr, objHistoricoPublicacaoVaga))
					return objHistoricoPublicacaoVaga;
			}
			throw (new RecordNotFoundException(typeof(HistoricoPublicacaoVaga)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de HistoricoPublicacaoVaga a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idHistoricoPublicacaoVaga))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de HistoricoPublicacaoVaga a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idHistoricoPublicacaoVaga, trans))
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
		/// <param name="objHistoricoPublicacaoVaga">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, HistoricoPublicacaoVaga objHistoricoPublicacaoVaga)
		{
			try
			{
				if (dr.Read())
				{
					objHistoricoPublicacaoVaga._idHistoricoPublicacaoVaga = Convert.ToInt32(dr["Idf_Historico_Publicacao_Vaga"]);
					objHistoricoPublicacaoVaga._idVaga = Convert.ToInt32(dr["Idf_Vaga"]);
					objHistoricoPublicacaoVaga._descricaoHistorico = Convert.ToString(dr["Des_Historico"]);
					objHistoricoPublicacaoVaga._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objHistoricoPublicacaoVaga._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objHistoricoPublicacaoVaga._persisted = true;
					objHistoricoPublicacaoVaga._modified = false;

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