//-- Data: 28/02/2013 15:56
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Banco // Tabela: plataforma.TAB_Banco
	{
		#region Atributos
		private int _idBanco;
		private string _nomeBanco;
		private string _apelidoBanco;
		private bool _flagOficial;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private bool _flagVinculaConta;
		private Int16? _numeroHora;
		private Int16? _numeroMinuto;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdBanco
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdBanco
		{
			get
			{
				return this._idBanco;
			}
			set
			{
				this._idBanco = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeBanco
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string NomeBanco
		{
			get
			{
				return this._nomeBanco;
			}
			set
			{
				this._nomeBanco = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ApelidoBanco
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo obrigatório.
		/// </summary>
		public string ApelidoBanco
		{
			get
			{
				return this._apelidoBanco;
			}
			set
			{
				this._apelidoBanco = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagOficial
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagOficial
		{
			get
			{
				return this._flagOficial;
			}
			set
			{
				this._flagOficial = value;
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

		#region FlagVinculaConta
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagVinculaConta
		{
			get
			{
				return this._flagVinculaConta;
			}
			set
			{
				this._flagVinculaConta = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroHora
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? NumeroHora
		{
			get
			{
				return this._numeroHora;
			}
			set
			{
				this._numeroHora = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroMinuto
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? NumeroMinuto
		{
			get
			{
				return this._numeroMinuto;
			}
			set
			{
				this._numeroMinuto = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Banco()
		{
		}
		public Banco(int idBanco)
		{
			this._idBanco = idBanco;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Banco (Idf_Banco, Nme_Banco, Ape_Banco, Flg_Oficial, Flg_Inativo, Dta_Cadastro, Flg_Vincula_Conta, Num_Hora, Num_Minuto) VALUES (@Idf_Banco, @Nme_Banco, @Ape_Banco, @Flg_Oficial, @Flg_Inativo, @Dta_Cadastro, @Flg_Vincula_Conta, @Num_Hora, @Num_Minuto);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Banco SET Nme_Banco = @Nme_Banco, Ape_Banco = @Ape_Banco, Flg_Oficial = @Flg_Oficial, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Flg_Vincula_Conta = @Flg_Vincula_Conta, Num_Hora = @Num_Hora, Num_Minuto = @Num_Minuto WHERE Idf_Banco = @Idf_Banco";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Banco WHERE Idf_Banco = @Idf_Banco";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Banco WITH(NOLOCK) WHERE Idf_Banco = @Idf_Banco";
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
			parms.Add(new SqlParameter("@Idf_Banco", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Banco", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Ape_Banco", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Flg_Oficial", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Vincula_Conta", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Num_Hora", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Num_Minuto", SqlDbType.Int, 2));
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
			parms[0].Value = this._idBanco;
			parms[1].Value = this._nomeBanco;
			parms[2].Value = this._apelidoBanco;
			parms[3].Value = this._flagOficial;
			parms[4].Value = this._flagInativo;
			parms[6].Value = this._flagVinculaConta;

			if (this._numeroHora.HasValue)
				parms[7].Value = this._numeroHora;
			else
				parms[7].Value = DBNull.Value;


			if (this._numeroMinuto.HasValue)
				parms[8].Value = this._numeroMinuto;
			else
				parms[8].Value = DBNull.Value;


			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[5].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Banco no banco de dados.
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
		/// Método utilizado para inserir uma instância de Banco no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de Banco no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Banco no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Banco no banco de dados.
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
		/// Método utilizado para salvar uma instância de Banco no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Banco no banco de dados.
		/// </summary>
		/// <param name="idBanco">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idBanco)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Banco", SqlDbType.Int, 4));

			parms[0].Value = idBanco;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Banco no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idBanco">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idBanco, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Banco", SqlDbType.Int, 4));

			parms[0].Value = idBanco;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Banco no banco de dados.
		/// </summary>
		/// <param name="idBanco">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idBanco)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Banco where Idf_Banco in (";

			for (int i = 0; i < idBanco.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idBanco[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idBanco">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idBanco)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Banco", SqlDbType.Int, 4));

			parms[0].Value = idBanco;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idBanco">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idBanco, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Banco", SqlDbType.Int, 4));

			parms[0].Value = idBanco;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ban.Idf_Banco, Ban.Nme_Banco, Ban.Ape_Banco, Ban.Flg_Oficial, Ban.Flg_Inativo, Ban.Dta_Cadastro, Ban.Flg_Vincula_Conta, Ban.Num_Hora, Ban.Num_Minuto FROM plataforma.TAB_Banco Ban";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Banco a partir do banco de dados.
		/// </summary>
		/// <param name="idBanco">Chave do registro.</param>
		/// <returns>Instância de Banco.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Banco LoadObject(int idBanco)
		{
			using (IDataReader dr = LoadDataReader(idBanco))
			{
				Banco objBanco = new Banco();
				if (SetInstance(dr, objBanco))
					return objBanco;
			}
			throw (new RecordNotFoundException(typeof(Banco)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Banco a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idBanco">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Banco.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Banco LoadObject(int idBanco, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idBanco, trans))
			{
				Banco objBanco = new Banco();
				if (SetInstance(dr, objBanco))
					return objBanco;
			}
			throw (new RecordNotFoundException(typeof(Banco)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Banco a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idBanco))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Banco a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idBanco, trans))
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
		/// <param name="objBanco">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Banco objBanco)
		{
			try
			{
				if (dr.Read())
				{
					objBanco._idBanco = Convert.ToInt32(dr["Idf_Banco"]);
					objBanco._nomeBanco = Convert.ToString(dr["Nme_Banco"]);
					objBanco._apelidoBanco = Convert.ToString(dr["Ape_Banco"]);
					objBanco._flagOficial = Convert.ToBoolean(dr["Flg_Oficial"]);
					objBanco._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objBanco._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objBanco._flagVinculaConta = Convert.ToBoolean(dr["Flg_Vincula_Conta"]);
					if (dr["Num_Hora"] != DBNull.Value)
						objBanco._numeroHora = Convert.ToInt16(dr["Num_Hora"]);
					if (dr["Num_Minuto"] != DBNull.Value)
						objBanco._numeroMinuto = Convert.ToInt16(dr["Num_Minuto"]);

					objBanco._persisted = true;
					objBanco._modified = false;

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