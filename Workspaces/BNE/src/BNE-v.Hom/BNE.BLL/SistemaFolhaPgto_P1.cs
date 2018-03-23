//-- Data: 24/08/2017 12:33
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class SistemaFolhaPgto // Tabela: BNE_Sistema_Folha_Pgto
	{
		#region Atributos
		private Int16 _idSistemaFolhaPgto;
		private string _descricaoSistemaFolhaPgto;
		private bool? _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdSistemaFolhaPgto
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public Int16 IdSistemaFolhaPgto
		{
			get
			{
				return this._idSistemaFolhaPgto;
			}
		}
		#endregion 

		#region DescricaoSistemaFolhaPgto
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoSistemaFolhaPgto
		{
			get
			{
				return this._descricaoSistemaFolhaPgto;
			}
			set
			{
				this._descricaoSistemaFolhaPgto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagInativo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagInativo
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
		public SistemaFolhaPgto()
		{
		}
		public SistemaFolhaPgto(Int16 idSistemaFolhaPgto)
		{
			this._idSistemaFolhaPgto = idSistemaFolhaPgto;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Sistema_Folha_Pgto (Des_Sistema_Folha_Pgto, Flg_Inativo) VALUES (@Des_Sistema_Folha_Pgto, @Flg_Inativo);SET @Idf_Sistema_Folha_Pgto = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Sistema_Folha_Pgto SET Des_Sistema_Folha_Pgto = @Des_Sistema_Folha_Pgto, Flg_Inativo = @Flg_Inativo WHERE Idf_Sistema_Folha_Pgto = @Idf_Sistema_Folha_Pgto";
		private const string SPDELETE = "DELETE FROM BNE_Sistema_Folha_Pgto WHERE Idf_Sistema_Folha_Pgto = @Idf_Sistema_Folha_Pgto";
		private const string SPSELECTID = "SELECT * FROM BNE_Sistema_Folha_Pgto WITH(NOLOCK) WHERE Idf_Sistema_Folha_Pgto = @Idf_Sistema_Folha_Pgto";
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
			parms.Add(new SqlParameter("@Idf_Sistema_Folha_Pgto", SqlDbType.Int, 1));
			parms.Add(new SqlParameter("@Des_Sistema_Folha_Pgto", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idSistemaFolhaPgto;

			if (!String.IsNullOrEmpty(this._descricaoSistemaFolhaPgto))
				parms[1].Value = this._descricaoSistemaFolhaPgto;
			else
				parms[1].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de SistemaFolhaPgto no banco de dados.
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
						this._idSistemaFolhaPgto = Convert.ToInt16(cmd.Parameters["@Idf_Sistema_Folha_Pgto"].Value);
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
		/// Método utilizado para inserir uma instância de SistemaFolhaPgto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idSistemaFolhaPgto = Convert.ToInt16(cmd.Parameters["@Idf_Sistema_Folha_Pgto"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de SistemaFolhaPgto no banco de dados.
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
		/// Método utilizado para atualizar uma instância de SistemaFolhaPgto no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de SistemaFolhaPgto no banco de dados.
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
		/// Método utilizado para salvar uma instância de SistemaFolhaPgto no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de SistemaFolhaPgto no banco de dados.
		/// </summary>
		/// <param name="idSistemaFolhaPgto">Chave do registro.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(Int16 idSistemaFolhaPgto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Sistema_Folha_Pgto", SqlDbType.Int, 1));

			parms[0].Value = idSistemaFolhaPgto;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de SistemaFolhaPgto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSistemaFolhaPgto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(Int16 idSistemaFolhaPgto, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Sistema_Folha_Pgto", SqlDbType.Int, 1));

			parms[0].Value = idSistemaFolhaPgto;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de SistemaFolhaPgto no banco de dados.
		/// </summary>
		/// <param name="idSistemaFolhaPgto">Lista de chaves.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(List<Int16> idSistemaFolhaPgto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Sistema_Folha_Pgto where Idf_Sistema_Folha_Pgto in (";

			for (int i = 0; i < idSistemaFolhaPgto.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 1));
				parms[i].Value = idSistemaFolhaPgto[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idSistemaFolhaPgto">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(Int16 idSistemaFolhaPgto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Sistema_Folha_Pgto", SqlDbType.Int, 1));

			parms[0].Value = idSistemaFolhaPgto;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSistemaFolhaPgto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(Int16 idSistemaFolhaPgto, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Sistema_Folha_Pgto", SqlDbType.Int, 1));

			parms[0].Value = idSistemaFolhaPgto;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sis.Idf_Sistema_Folha_Pgto, Sis.Des_Sistema_Folha_Pgto, Sis.Flg_Inativo FROM BNE_Sistema_Folha_Pgto Sis";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de SistemaFolhaPgto a partir do banco de dados.
		/// </summary>
		/// <param name="idSistemaFolhaPgto">Chave do registro.</param>
		/// <returns>Instância de SistemaFolhaPgto.</returns>
		/// <remarks>Mailson</remarks>
		public static SistemaFolhaPgto LoadObject(Int16 idSistemaFolhaPgto)
		{
			using (IDataReader dr = LoadDataReader(idSistemaFolhaPgto))
			{
				SistemaFolhaPgto objSistemaFolhaPgto = new SistemaFolhaPgto();
				if (SetInstance(dr, objSistemaFolhaPgto))
					return objSistemaFolhaPgto;
			}
			throw (new RecordNotFoundException(typeof(SistemaFolhaPgto)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de SistemaFolhaPgto a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSistemaFolhaPgto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de SistemaFolhaPgto.</returns>
		/// <remarks>Mailson</remarks>
		public static SistemaFolhaPgto LoadObject(Int16 idSistemaFolhaPgto, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idSistemaFolhaPgto, trans))
			{
				SistemaFolhaPgto objSistemaFolhaPgto = new SistemaFolhaPgto();
				if (SetInstance(dr, objSistemaFolhaPgto))
					return objSistemaFolhaPgto;
			}
			throw (new RecordNotFoundException(typeof(SistemaFolhaPgto)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de SistemaFolhaPgto a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idSistemaFolhaPgto))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de SistemaFolhaPgto a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idSistemaFolhaPgto, trans))
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
		/// <param name="objSistemaFolhaPgto">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, SistemaFolhaPgto objSistemaFolhaPgto, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objSistemaFolhaPgto._idSistemaFolhaPgto = Convert.ToInt16(dr["Idf_Sistema_Folha_Pgto"]);
					if (dr["Des_Sistema_Folha_Pgto"] != DBNull.Value)
						objSistemaFolhaPgto._descricaoSistemaFolhaPgto = Convert.ToString(dr["Des_Sistema_Folha_Pgto"]);
					if (dr["Flg_Inativo"] != DBNull.Value)
						objSistemaFolhaPgto._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objSistemaFolhaPgto._persisted = true;
					objSistemaFolhaPgto._modified = false;

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