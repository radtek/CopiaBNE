//-- Data: 12/07/2011 12:19
//-- Autor: Tiago Franco

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class StatusCurriculoVaga // Tabela: BNE_Status_Curriculo_Vaga
	{
		#region Atributos
		private int _idStatusCurriculoVaga;
		private string _descricaoStatusCurriculoVaga;
		private DateTime _dataCadastro;
		private DateTime _dataAlteracao;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdStatusCurriculoVaga
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdStatusCurriculoVaga
		{
			get
			{
				return this._idStatusCurriculoVaga;
			}
		}
		#endregion 

		#region DescricaoStatusCurriculoVaga
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoStatusCurriculoVaga
		{
			get
			{
				return this._descricaoStatusCurriculoVaga;
			}
			set
			{
				this._descricaoStatusCurriculoVaga = value;
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

		#region DataAlteracao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataAlteracao
		{
			get
			{
				return this._dataAlteracao;
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

		#endregion

		#region Construtores
		public StatusCurriculoVaga()
		{
		}
		public StatusCurriculoVaga(int idStatusCurriculoVaga)
		{
			this._idStatusCurriculoVaga = idStatusCurriculoVaga;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Status_Curriculo_Vaga (Des_Status_Curriculo_Vaga, Dta_Cadastro, Dta_Alteracao, Flg_Inativo) VALUES (@Des_Status_Curriculo_Vaga, @Dta_Cadastro, @Dta_Alteracao, @Flg_Inativo);SET @Idf_Status_Curriculo_Vaga = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Status_Curriculo_Vaga SET Des_Status_Curriculo_Vaga = @Des_Status_Curriculo_Vaga, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Flg_Inativo = @Flg_Inativo WHERE Idf_Status_Curriculo_Vaga = @Idf_Status_Curriculo_Vaga";
		private const string SPDELETE = "DELETE FROM BNE_Status_Curriculo_Vaga WHERE Idf_Status_Curriculo_Vaga = @Idf_Status_Curriculo_Vaga";
		private const string SPSELECTID = "SELECT * FROM BNE_Status_Curriculo_Vaga WHERE Idf_Status_Curriculo_Vaga = @Idf_Status_Curriculo_Vaga";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Tiago Franco</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Curriculo_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Status_Curriculo_Vaga", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Tiago Franco</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idStatusCurriculoVaga;
			parms[1].Value = this._descricaoStatusCurriculoVaga;
			parms[4].Value = this._flagInativo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[2].Value = this._dataCadastro;
			this._dataAlteracao = DateTime.Now;
			parms[3].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de StatusCurriculoVaga no banco de dados.
		/// </summary>
		/// <remarks>Tiago Franco</remarks>
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
						this._idStatusCurriculoVaga = Convert.ToInt32(cmd.Parameters["@Idf_Status_Curriculo_Vaga"].Value);
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
		/// Método utilizado para inserir uma instância de StatusCurriculoVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Tiago Franco</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idStatusCurriculoVaga = Convert.ToInt32(cmd.Parameters["@Idf_Status_Curriculo_Vaga"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de StatusCurriculoVaga no banco de dados.
		/// </summary>
		/// <remarks>Tiago Franco</remarks>
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
		/// Método utilizado para atualizar uma instância de StatusCurriculoVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Tiago Franco</remarks>
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
		/// Método utilizado para salvar uma instância de StatusCurriculoVaga no banco de dados.
		/// </summary>
		/// <remarks>Tiago Franco</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de StatusCurriculoVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Tiago Franco</remarks>
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
		/// Método utilizado para excluir uma instância de StatusCurriculoVaga no banco de dados.
		/// </summary>
		/// <param name="idStatusCurriculoVaga">Chave do registro.</param>
		/// <remarks>Tiago Franco</remarks>
		public static void Delete(int idStatusCurriculoVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Curriculo_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idStatusCurriculoVaga;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de StatusCurriculoVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusCurriculoVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Tiago Franco</remarks>
		public static void Delete(int idStatusCurriculoVaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Curriculo_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idStatusCurriculoVaga;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de StatusCurriculoVaga no banco de dados.
		/// </summary>
		/// <param name="idStatusCurriculoVaga">Lista de chaves.</param>
		/// <remarks>Tiago Franco</remarks>
		public static void Delete(List<int> idStatusCurriculoVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Status_Curriculo_Vaga where Idf_Status_Curriculo_Vaga in (";

			for (int i = 0; i < idStatusCurriculoVaga.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idStatusCurriculoVaga[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idStatusCurriculoVaga">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Tiago Franco</remarks>
		private static IDataReader LoadDataReader(int idStatusCurriculoVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Curriculo_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idStatusCurriculoVaga;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusCurriculoVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Tiago Franco</remarks>
		private static IDataReader LoadDataReader(int idStatusCurriculoVaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Curriculo_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idStatusCurriculoVaga;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sta.Idf_Status_Curriculo_Vaga, Sta.Des_Status_Curriculo_Vaga, Sta.Dta_Cadastro, Sta.Dta_Alteracao, Sta.Flg_Inativo FROM BNE_Status_Curriculo_Vaga Sta";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de StatusCurriculoVaga a partir do banco de dados.
		/// </summary>
		/// <param name="idStatusCurriculoVaga">Chave do registro.</param>
		/// <returns>Instância de StatusCurriculoVaga.</returns>
		/// <remarks>Tiago Franco</remarks>
		public static StatusCurriculoVaga LoadObject(int idStatusCurriculoVaga)
		{
			using (IDataReader dr = LoadDataReader(idStatusCurriculoVaga))
			{
				StatusCurriculoVaga objStatusCurriculoVaga = new StatusCurriculoVaga();
				if (SetInstance(dr, objStatusCurriculoVaga))
					return objStatusCurriculoVaga;
			}
			throw (new RecordNotFoundException(typeof(StatusCurriculoVaga)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de StatusCurriculoVaga a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusCurriculoVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de StatusCurriculoVaga.</returns>
		/// <remarks>Tiago Franco</remarks>
		public static StatusCurriculoVaga LoadObject(int idStatusCurriculoVaga, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idStatusCurriculoVaga, trans))
			{
				StatusCurriculoVaga objStatusCurriculoVaga = new StatusCurriculoVaga();
				if (SetInstance(dr, objStatusCurriculoVaga))
					return objStatusCurriculoVaga;
			}
			throw (new RecordNotFoundException(typeof(StatusCurriculoVaga)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de StatusCurriculoVaga a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Tiago Franco</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idStatusCurriculoVaga))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de StatusCurriculoVaga a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Tiago Franco</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idStatusCurriculoVaga, trans))
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
		/// <param name="objStatusCurriculoVaga">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Tiago Franco</remarks>
		private static bool SetInstance(IDataReader dr, StatusCurriculoVaga objStatusCurriculoVaga)
		{
			try
			{
				if (dr.Read())
				{
					objStatusCurriculoVaga._idStatusCurriculoVaga = Convert.ToInt32(dr["Idf_Status_Curriculo_Vaga"]);
					objStatusCurriculoVaga._descricaoStatusCurriculoVaga = Convert.ToString(dr["Des_Status_Curriculo_Vaga"]);
					objStatusCurriculoVaga._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objStatusCurriculoVaga._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objStatusCurriculoVaga._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objStatusCurriculoVaga._persisted = true;
					objStatusCurriculoVaga._modified = false;

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