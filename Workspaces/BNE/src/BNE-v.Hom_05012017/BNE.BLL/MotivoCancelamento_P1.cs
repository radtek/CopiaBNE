//-- Data: 18/07/2016 15:29
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class MotivoCancelamento // Tabela: BNE_Motivo_Cancelamento
	{
		#region Atributos
		private int _idMotivoCancelamento;
		private string _descricaoMotivoCancelamento;
		private int? _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdMotivoCancelamento
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdMotivoCancelamento
		{
			get
			{
				return this._idMotivoCancelamento;
			}
		}
		#endregion 

		#region DescricaoMotivoCancelamento
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoMotivoCancelamento
		{
			get
			{
				return this._descricaoMotivoCancelamento;
			}
			set
			{
				this._descricaoMotivoCancelamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagInativo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? FlagInativo
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

		#endregion

		#region Construtores
		public MotivoCancelamento()
		{
		}
		public MotivoCancelamento(int idMotivoCancelamento)
		{
			this._idMotivoCancelamento = idMotivoCancelamento;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Motivo_Cancelamento (Des_Motivo_Cancelamento, Flg_Inativo) VALUES (@Des_Motivo_Cancelamento, @Flg_Inativo);SET @Idf_Motivo_Cancelamento = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Motivo_Cancelamento SET Des_Motivo_Cancelamento = @Des_Motivo_Cancelamento, Flg_Inativo = @Flg_Inativo WHERE Idf_Motivo_Cancelamento = @Idf_Motivo_Cancelamento";
		private const string SPDELETE = "DELETE FROM BNE_Motivo_Cancelamento WHERE Idf_Motivo_Cancelamento = @Idf_Motivo_Cancelamento";
		private const string SPSELECTID = "SELECT * FROM BNE_Motivo_Cancelamento WITH(NOLOCK) WHERE Idf_Motivo_Cancelamento = @Idf_Motivo_Cancelamento";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Mailson</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Cancelamento", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Motivo_Cancelamento", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Mailson</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idMotivoCancelamento;
			parms[1].Value = this._descricaoMotivoCancelamento;

			if (this._flagInativo.HasValue)
				parms[2].Value = this._flagInativo;
			else
				parms[2].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de MotivoCancelamento no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
						this._idMotivoCancelamento = Convert.ToInt32(cmd.Parameters["@Idf_Motivo_Cancelamento"].Value);
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
		/// Método utilizado para inserir uma instância de MotivoCancelamento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idMotivoCancelamento = Convert.ToInt32(cmd.Parameters["@Idf_Motivo_Cancelamento"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de MotivoCancelamento no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para atualizar uma instância de MotivoCancelamento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para salvar uma instância de MotivoCancelamento no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de MotivoCancelamento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para excluir uma instância de MotivoCancelamento no banco de dados.
		/// </summary>
		/// <param name="idMotivoCancelamento">Chave do registro.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idMotivoCancelamento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Cancelamento", SqlDbType.Int, 4));

			parms[0].Value = idMotivoCancelamento;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de MotivoCancelamento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoCancelamento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idMotivoCancelamento, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Cancelamento", SqlDbType.Int, 4));

			parms[0].Value = idMotivoCancelamento;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de MotivoCancelamento no banco de dados.
		/// </summary>
		/// <param name="idMotivoCancelamento">Lista de chaves.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(List<int> idMotivoCancelamento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Motivo_Cancelamento where Idf_Motivo_Cancelamento in (";

			for (int i = 0; i < idMotivoCancelamento.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idMotivoCancelamento[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idMotivoCancelamento">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idMotivoCancelamento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Cancelamento", SqlDbType.Int, 4));

			parms[0].Value = idMotivoCancelamento;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoCancelamento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idMotivoCancelamento, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Cancelamento", SqlDbType.Int, 4));

			parms[0].Value = idMotivoCancelamento;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Mot.Idf_Motivo_Cancelamento, Mot.Des_Motivo_Cancelamento, Mot.Flg_Inativo FROM BNE_Motivo_Cancelamento Mot";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de MotivoCancelamento a partir do banco de dados.
		/// </summary>
		/// <param name="idMotivoCancelamento">Chave do registro.</param>
		/// <returns>Instância de MotivoCancelamento.</returns>
		/// <remarks>Mailson</remarks>
		public static MotivoCancelamento LoadObject(int idMotivoCancelamento)
		{
			using (IDataReader dr = LoadDataReader(idMotivoCancelamento))
			{
				MotivoCancelamento objMotivoCancelamento = new MotivoCancelamento();
				if (SetInstance(dr, objMotivoCancelamento))
					return objMotivoCancelamento;
			}
			throw (new RecordNotFoundException(typeof(MotivoCancelamento)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de MotivoCancelamento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoCancelamento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de MotivoCancelamento.</returns>
		/// <remarks>Mailson</remarks>
		public static MotivoCancelamento LoadObject(int idMotivoCancelamento, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idMotivoCancelamento, trans))
			{
				MotivoCancelamento objMotivoCancelamento = new MotivoCancelamento();
				if (SetInstance(dr, objMotivoCancelamento))
					return objMotivoCancelamento;
			}
			throw (new RecordNotFoundException(typeof(MotivoCancelamento)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de MotivoCancelamento a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idMotivoCancelamento))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de MotivoCancelamento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idMotivoCancelamento, trans))
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
		/// <param name="objMotivoCancelamento">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, MotivoCancelamento objMotivoCancelamento, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objMotivoCancelamento._idMotivoCancelamento = Convert.ToInt32(dr["Idf_Motivo_Cancelamento"]);
					objMotivoCancelamento._descricaoMotivoCancelamento = Convert.ToString(dr["Des_Motivo_Cancelamento"]);
					if (dr["Flg_Inativo"] != DBNull.Value)
						objMotivoCancelamento._flagInativo = Convert.ToInt32(dr["Flg_Inativo"]);

					objMotivoCancelamento._persisted = true;
					objMotivoCancelamento._modified = false;

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
				if (dispose)
					dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}