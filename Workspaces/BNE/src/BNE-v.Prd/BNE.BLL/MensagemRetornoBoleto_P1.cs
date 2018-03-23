//-- Data: 28/04/2014 09:56
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class MensagemRetornoBoleto // Tabela: GLO_Mensagem_Retorno_Boleto
	{
		#region Atributos
		private int _idMensagemRetornoBoleto;
		private string _descricaoStatus;
		private string _codigoStatus;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdMensagemRetornoBoleto
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdMensagemRetornoBoleto
		{
			get
			{
				return this._idMensagemRetornoBoleto;
			}
		}
		#endregion 

		#region DescricaoStatus
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoStatus
		{
			get
			{
				return this._descricaoStatus;
			}
			set
			{
				this._descricaoStatus = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoStatus
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string CodigoStatus
		{
			get
			{
				return this._codigoStatus;
			}
			set
			{
				this._codigoStatus = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public MensagemRetornoBoleto()
		{
		}
		public MensagemRetornoBoleto(int idMensagemRetornoBoleto)
		{
			this._idMensagemRetornoBoleto = idMensagemRetornoBoleto;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO GLO_Mensagem_Retorno_Boleto (Des_Status, Cod_Status) VALUES (@Des_Status, @Cod_Status);SET @Idf_Mensagem_Retorno_Boleto = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE GLO_Mensagem_Retorno_Boleto SET Des_Status = @Des_Status, Cod_Status = @Cod_Status WHERE Idf_Mensagem_Retorno_Boleto = @Idf_Mensagem_Retorno_Boleto";
		private const string SPDELETE = "DELETE FROM GLO_Mensagem_Retorno_Boleto WHERE Idf_Mensagem_Retorno_Boleto = @Idf_Mensagem_Retorno_Boleto";
		private const string SPSELECTID = "SELECT * FROM GLO_Mensagem_Retorno_Boleto WITH(NOLOCK) WHERE Idf_Mensagem_Retorno_Boleto = @Idf_Mensagem_Retorno_Boleto";
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
			parms.Add(new SqlParameter("@Idf_Mensagem_Retorno_Boleto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Status", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Cod_Status", SqlDbType.VarChar, 10));
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
			parms[0].Value = this._idMensagemRetornoBoleto;

			if (!String.IsNullOrEmpty(this._descricaoStatus))
				parms[1].Value = this._descricaoStatus;
			else
				parms[1].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._codigoStatus))
				parms[2].Value = this._codigoStatus;
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
		/// Método utilizado para inserir uma instância de MensagemRetornoBoleto no banco de dados.
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
						this._idMensagemRetornoBoleto = Convert.ToInt32(cmd.Parameters["@Idf_Mensagem_Retorno_Boleto"].Value);
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
		/// Método utilizado para inserir uma instância de MensagemRetornoBoleto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idMensagemRetornoBoleto = Convert.ToInt32(cmd.Parameters["@Idf_Mensagem_Retorno_Boleto"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de MensagemRetornoBoleto no banco de dados.
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
		/// Método utilizado para atualizar uma instância de MensagemRetornoBoleto no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de MensagemRetornoBoleto no banco de dados.
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
		/// Método utilizado para salvar uma instância de MensagemRetornoBoleto no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de MensagemRetornoBoleto no banco de dados.
		/// </summary>
		/// <param name="idMensagemRetornoBoleto">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idMensagemRetornoBoleto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_Retorno_Boleto", SqlDbType.Int, 4));

			parms[0].Value = idMensagemRetornoBoleto;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de MensagemRetornoBoleto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagemRetornoBoleto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idMensagemRetornoBoleto, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_Retorno_Boleto", SqlDbType.Int, 4));

			parms[0].Value = idMensagemRetornoBoleto;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de MensagemRetornoBoleto no banco de dados.
		/// </summary>
		/// <param name="idMensagemRetornoBoleto">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idMensagemRetornoBoleto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from GLO_Mensagem_Retorno_Boleto where Idf_Mensagem_Retorno_Boleto in (";

			for (int i = 0; i < idMensagemRetornoBoleto.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idMensagemRetornoBoleto[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idMensagemRetornoBoleto">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idMensagemRetornoBoleto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_Retorno_Boleto", SqlDbType.Int, 4));

			parms[0].Value = idMensagemRetornoBoleto;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagemRetornoBoleto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idMensagemRetornoBoleto, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_Retorno_Boleto", SqlDbType.Int, 4));

			parms[0].Value = idMensagemRetornoBoleto;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Men.Idf_Mensagem_Retorno_Boleto, Men.Des_Status, Men.Cod_Status FROM GLO_Mensagem_Retorno_Boleto Men";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de MensagemRetornoBoleto a partir do banco de dados.
		/// </summary>
		/// <param name="idMensagemRetornoBoleto">Chave do registro.</param>
		/// <returns>Instância de MensagemRetornoBoleto.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static MensagemRetornoBoleto LoadObject(int idMensagemRetornoBoleto)
		{
			using (IDataReader dr = LoadDataReader(idMensagemRetornoBoleto))
			{
				MensagemRetornoBoleto objMensagemRetornoBoleto = new MensagemRetornoBoleto();
				if (SetInstance(dr, objMensagemRetornoBoleto))
					return objMensagemRetornoBoleto;
			}
			throw (new RecordNotFoundException(typeof(MensagemRetornoBoleto)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de MensagemRetornoBoleto a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagemRetornoBoleto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de MensagemRetornoBoleto.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static MensagemRetornoBoleto LoadObject(int idMensagemRetornoBoleto, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idMensagemRetornoBoleto, trans))
			{
				MensagemRetornoBoleto objMensagemRetornoBoleto = new MensagemRetornoBoleto();
				if (SetInstance(dr, objMensagemRetornoBoleto))
					return objMensagemRetornoBoleto;
			}
			throw (new RecordNotFoundException(typeof(MensagemRetornoBoleto)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de MensagemRetornoBoleto a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idMensagemRetornoBoleto))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de MensagemRetornoBoleto a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idMensagemRetornoBoleto, trans))
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
		/// <param name="objMensagemRetornoBoleto">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, MensagemRetornoBoleto objMensagemRetornoBoleto)
		{
			try
			{
				if (dr.Read())
				{
					objMensagemRetornoBoleto._idMensagemRetornoBoleto = Convert.ToInt32(dr["Idf_Mensagem_Retorno_Boleto"]);
					if (dr["Des_Status"] != DBNull.Value)
						objMensagemRetornoBoleto._descricaoStatus = Convert.ToString(dr["Des_Status"]);
					if (dr["Cod_Status"] != DBNull.Value)
						objMensagemRetornoBoleto._codigoStatus = Convert.ToString(dr["Cod_Status"]);

					objMensagemRetornoBoleto._persisted = true;
					objMensagemRetornoBoleto._modified = false;

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