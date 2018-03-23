//-- Data: 17/03/2011 13:20
//-- Autor: Vinicius Maciel

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Raca // Tabela: plataforma.TAB_Raca
	{
		#region Atributos
		private int _idRaca;
		private string _descricaoRaca;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private string _descricaoRacaBNE;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRaca
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdRaca
		{
			get
			{
				return this._idRaca;
			}
			set
			{
				this._idRaca = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoRaca
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoRaca
		{
			get
			{
				return this._descricaoRaca;
			}
			set
			{
				this._descricaoRaca = value;
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

		#region DescricaoRacaBNE
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo opcional.
		/// </summary>
		public string DescricaoRacaBNE
		{
			get
			{
				return this._descricaoRacaBNE;
			}
			set
			{
				this._descricaoRacaBNE = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Raca()
		{
		}
		public Raca(int idRaca)
		{
			this._idRaca = idRaca;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Raca (Idf_Raca, Des_Raca, Flg_Inativo, Dta_Cadastro, Des_Raca_BNE) VALUES (@Idf_Raca, @Des_Raca, @Flg_Inativo, @Dta_Cadastro, @Des_Raca_BNE);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Raca SET Des_Raca = @Des_Raca, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Des_Raca_BNE = @Des_Raca_BNE WHERE Idf_Raca = @Idf_Raca";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Raca WHERE Idf_Raca = @Idf_Raca";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Raca WHERE Idf_Raca = @Idf_Raca";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Raca", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Raca", SqlDbType.VarChar, 20));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Raca_BNE", SqlDbType.VarChar, 20));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Vinicius Maciel</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idRaca;
			parms[1].Value = this._descricaoRaca;
			parms[2].Value = this._flagInativo;

			if (!String.IsNullOrEmpty(this._descricaoRacaBNE))
				parms[4].Value = this._descricaoRacaBNE;
			else
				parms[4].Value = DBNull.Value;


			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Raca no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para inserir uma instância de Raca no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para atualizar uma instância de Raca no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para atualizar uma instância de Raca no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para salvar uma instância de Raca no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de Raca no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para excluir uma instância de Raca no banco de dados.
		/// </summary>
		/// <param name="idRaca">Chave do registro.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idRaca)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Raca", SqlDbType.Int, 4));

			parms[0].Value = idRaca;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Raca no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRaca">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idRaca, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Raca", SqlDbType.Int, 4));

			parms[0].Value = idRaca;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Raca no banco de dados.
		/// </summary>
		/// <param name="idRaca">Lista de chaves.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(List<int> idRaca)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Raca where Idf_Raca in (";

			for (int i = 0; i < idRaca.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRaca[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRaca">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idRaca)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Raca", SqlDbType.Int, 4));

			parms[0].Value = idRaca;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRaca">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idRaca, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Raca", SqlDbType.Int, 4));

			parms[0].Value = idRaca;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Rac.Idf_Raca, Rac.Des_Raca, Rac.Flg_Inativo, Rac.Dta_Cadastro, Rac.Des_Raca_BNE FROM plataforma.TAB_Raca Rac";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Raca a partir do banco de dados.
		/// </summary>
		/// <param name="idRaca">Chave do registro.</param>
		/// <returns>Instância de Raca.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static Raca LoadObject(int idRaca)
		{
			using (IDataReader dr = LoadDataReader(idRaca))
			{
				Raca objRaca = new Raca();
				if (SetInstance(dr, objRaca))
					return objRaca;
			}
			throw (new RecordNotFoundException(typeof(Raca)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Raca a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRaca">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Raca.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static Raca LoadObject(int idRaca, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRaca, trans))
			{
				Raca objRaca = new Raca();
				if (SetInstance(dr, objRaca))
					return objRaca;
			}
			throw (new RecordNotFoundException(typeof(Raca)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Raca a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRaca))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Raca a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRaca, trans))
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
		/// <param name="objRaca">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static bool SetInstance(IDataReader dr, Raca objRaca)
		{
			try
			{
				if (dr.Read())
				{
					objRaca._idRaca = Convert.ToInt32(dr["Idf_Raca"]);
					objRaca._descricaoRaca = Convert.ToString(dr["Des_Raca"]);
					objRaca._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objRaca._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Des_Raca_BNE"] != DBNull.Value)
						objRaca._descricaoRacaBNE = Convert.ToString(dr["Des_Raca_BNE"]);

					objRaca._persisted = true;
					objRaca._modified = false;

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