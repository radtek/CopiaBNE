//-- Data: 06/06/2014 15:27
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Arquivo // Tabela: BNE_Arquivo
	{
		#region Atributos
		private int _idArquivo;
		private string _nomeArquivo;
		private TipoArquivo _tipoArquivo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdArquivo
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdArquivo
		{
			get
			{
				return this._idArquivo;
			}
		}
		#endregion 

		#region NomeArquivo
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string NomeArquivo
		{
			get
			{
				return this._nomeArquivo;
			}
			set
			{
				this._nomeArquivo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoArquivo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoArquivo TipoArquivo
		{
			get
			{
				return this._tipoArquivo;
			}
			set
			{
				this._tipoArquivo = value;
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
		public Arquivo()
		{
		}
		public Arquivo(int idArquivo)
		{
			this._idArquivo = idArquivo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Arquivo (Nme_Arquivo, Idf_Tipo_Arquivo, Dta_Cadastro) VALUES (@Nme_Arquivo, @Idf_Tipo_Arquivo, @Dta_Cadastro);SET @Idf_Arquivo = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Arquivo SET Nme_Arquivo = @Nme_Arquivo, Idf_Tipo_Arquivo = @Idf_Tipo_Arquivo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Arquivo = @Idf_Arquivo";
		private const string SPDELETE = "DELETE FROM BNE_Arquivo WHERE Idf_Arquivo = @Idf_Arquivo";
		private const string SPSELECTID = "SELECT * FROM BNE_Arquivo WITH(NOLOCK) WHERE Idf_Arquivo = @Idf_Arquivo";
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
			parms.Add(new SqlParameter("@Idf_Arquivo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Arquivo", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Idf_Tipo_Arquivo", SqlDbType.Int, 4));
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
			parms[0].Value = this._idArquivo;
			parms[1].Value = this._nomeArquivo;
			parms[2].Value = this._tipoArquivo.IdTipoArquivo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Arquivo no banco de dados.
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
						this._idArquivo = Convert.ToInt32(cmd.Parameters["@Idf_Arquivo"].Value);
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
		/// Método utilizado para inserir uma instância de Arquivo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idArquivo = Convert.ToInt32(cmd.Parameters["@Idf_Arquivo"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Arquivo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Arquivo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Arquivo no banco de dados.
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
		/// Método utilizado para salvar uma instância de Arquivo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Arquivo no banco de dados.
		/// </summary>
		/// <param name="idArquivo">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idArquivo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idArquivo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Arquivo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idArquivo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idArquivo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idArquivo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Arquivo no banco de dados.
		/// </summary>
		/// <param name="idArquivo">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idArquivo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Arquivo where Idf_Arquivo in (";

			for (int i = 0; i < idArquivo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idArquivo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idArquivo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idArquivo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idArquivo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idArquivo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idArquivo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idArquivo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Arq.Idf_Arquivo, Arq.Nme_Arquivo, Arq.Idf_Tipo_Arquivo, Arq.Dta_Cadastro FROM BNE_Arquivo Arq";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Arquivo a partir do banco de dados.
		/// </summary>
		/// <param name="idArquivo">Chave do registro.</param>
		/// <returns>Instância de Arquivo.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static Arquivo LoadObject(int idArquivo)
		{
			using (IDataReader dr = LoadDataReader(idArquivo))
			{
				Arquivo objArquivo = new Arquivo();
				if (SetInstance(dr, objArquivo))
					return objArquivo;
			}
			throw (new RecordNotFoundException(typeof(Arquivo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Arquivo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idArquivo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Arquivo.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static Arquivo LoadObject(int idArquivo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idArquivo, trans))
			{
				Arquivo objArquivo = new Arquivo();
				if (SetInstance(dr, objArquivo))
					return objArquivo;
			}
			throw (new RecordNotFoundException(typeof(Arquivo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Arquivo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idArquivo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Arquivo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idArquivo, trans))
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
		/// <param name="objArquivo">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, Arquivo objArquivo)
		{
			try
			{
				if (dr.Read())
				{
					objArquivo._idArquivo = Convert.ToInt32(dr["Idf_Arquivo"]);
					objArquivo._nomeArquivo = Convert.ToString(dr["Nme_Arquivo"]);
					objArquivo._tipoArquivo = new TipoArquivo(Convert.ToInt32(dr["Idf_Tipo_Arquivo"]));
					objArquivo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objArquivo._persisted = true;
					objArquivo._modified = false;

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