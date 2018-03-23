//-- Data: 30/06/2010 17:55
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Fonte // Tabela: TAB_Fonte
	{
		#region Atributos
		private int _idFonte;
		private string _siglaFonte;
		private string _nomeFonte;
		private bool _flagAuditada;
		private DateTime _dataCadastro;
		private bool _flagMEC;
		private decimal _numeroCNPJ;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdFonte
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdFonte
		{
			get
			{
				return this._idFonte;
			}
            set
            {
                this._idFonte = value;
                this._modified = true;
            }
		}
		#endregion 

		#region SiglaFonte
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo obrigatório.
		/// </summary>
		public string SiglaFonte
		{
			get
			{
				return this._siglaFonte;
			}
			set
			{
				this._siglaFonte = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeFonte
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string NomeFonte
		{
			get
			{
				return this._nomeFonte;
			}
			set
			{
				this._nomeFonte = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagAuditada
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagAuditada
		{
			get
			{
				return this._flagAuditada;
			}
			set
			{
				this._flagAuditada = value;
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

		#region FlagMEC
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagMEC
		{
			get
			{
				return this._flagMEC;
			}
			set
			{
				this._flagMEC = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCNPJ
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal NumeroCNPJ
		{
			get
			{
				return this._numeroCNPJ;
			}
			set
			{
				this._numeroCNPJ = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Fonte()
		{
		}
		public Fonte(int idFonte)
		{
			this._idFonte = idFonte;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Fonte (Sig_Fonte, Nme_Fonte, Flg_Auditada, Dta_Cadastro, Flg_MEC, Num_CNPJ) VALUES (@Sig_Fonte, @Nme_Fonte, @Flg_Auditada, @Dta_Cadastro, @Flg_MEC, @Num_CNPJ);SET @Idf_Fonte = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Fonte SET Sig_Fonte = @Sig_Fonte, Nme_Fonte = @Nme_Fonte, Flg_Auditada = @Flg_Auditada, Dta_Cadastro = @Dta_Cadastro, Flg_MEC = @Flg_MEC, Num_CNPJ = @Num_CNPJ WHERE Idf_Fonte = @Idf_Fonte";
		private const string SPDELETE = "DELETE FROM TAB_Fonte WHERE Idf_Fonte = @Idf_Fonte";
		private const string SPSELECTID = "SELECT * FROM TAB_Fonte WHERE Idf_Fonte = @Idf_Fonte";
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
			parms.Add(new SqlParameter("@Idf_Fonte", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Sig_Fonte", SqlDbType.VarChar, 20));
			parms.Add(new SqlParameter("@Nme_Fonte", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Auditada", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_MEC", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Num_CNPJ", SqlDbType.Decimal, 9));
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
			parms[0].Value = this._idFonte;
			parms[1].Value = this._siglaFonte;
			parms[2].Value = this._nomeFonte;
			parms[3].Value = this._flagAuditada;
			parms[5].Value = this._flagMEC;
			parms[6].Value = this._numeroCNPJ;

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
		/// Método utilizado para inserir uma instância de Fonte no banco de dados.
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
						this._idFonte = Convert.ToInt32(cmd.Parameters["@Idf_Fonte"].Value);
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
		/// Método utilizado para inserir uma instância de Fonte no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idFonte = Convert.ToInt32(cmd.Parameters["@Idf_Fonte"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Fonte no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Fonte no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Fonte no banco de dados.
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
		/// Método utilizado para salvar uma instância de Fonte no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Fonte no banco de dados.
		/// </summary>
		/// <param name="idFonte">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFonte)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Fonte", SqlDbType.Int, 4));

			parms[0].Value = idFonte;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Fonte no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFonte">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFonte, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Fonte", SqlDbType.Int, 4));

			parms[0].Value = idFonte;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Fonte no banco de dados.
		/// </summary>
		/// <param name="idFonte">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idFonte)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Fonte where Idf_Fonte in (";

			for (int i = 0; i < idFonte.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idFonte[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFonte">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFonte)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Fonte", SqlDbType.Int, 4));

			parms[0].Value = idFonte;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFonte">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFonte, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Fonte", SqlDbType.Int, 4));

			parms[0].Value = idFonte;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Fon.Idf_Fonte, Fon.Sig_Fonte, Fon.Nme_Fonte, Fon.Flg_Auditada, Fon.Dta_Cadastro, Fon.Flg_MEC, Fon.Num_CNPJ FROM TAB_Fonte Fon";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Fonte a partir do banco de dados.
		/// </summary>
		/// <param name="idFonte">Chave do registro.</param>
		/// <returns>Instância de Fonte.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Fonte LoadObject(int idFonte)
		{
			using (IDataReader dr = LoadDataReader(idFonte))
			{
				Fonte objFonte = new Fonte();
				if (SetInstance(dr, objFonte))
					return objFonte;
			}
			throw (new RecordNotFoundException(typeof(Fonte)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Fonte a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFonte">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Fonte.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Fonte LoadObject(int idFonte, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idFonte, trans))
			{
				Fonte objFonte = new Fonte();
				if (SetInstance(dr, objFonte))
					return objFonte;
			}
			throw (new RecordNotFoundException(typeof(Fonte)));
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objFonte">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Fonte objFonte)
		{
			try
			{
				if (dr.Read())
				{
					objFonte._idFonte = Convert.ToInt32(dr["Idf_Fonte"]);
					objFonte._siglaFonte = Convert.ToString(dr["Sig_Fonte"]);
					objFonte._nomeFonte = Convert.ToString(dr["Nme_Fonte"]);
					objFonte._flagAuditada = Convert.ToBoolean(dr["Flg_Auditada"]);
					objFonte._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objFonte._flagMEC = Convert.ToBoolean(dr["Flg_MEC"]);
					objFonte._numeroCNPJ = Convert.ToDecimal(dr["Num_CNPJ"]);

					objFonte._persisted = true;
					objFonte._modified = false;

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