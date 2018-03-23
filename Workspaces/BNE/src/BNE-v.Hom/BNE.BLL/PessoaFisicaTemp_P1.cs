//-- Data: 13/05/2010 12:11
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PessoaFisicaTemp // Tabela: TAB_Pessoa_Fisica_Temp
	{
		#region Atributos
		private int _idPessoaFisicaTemp;
		private decimal _numeroCPF;
		private DateTime _dataNascimento;
		private string _numeroDDDCelular;
		private string _numeroCelular;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPessoaFisicaTemp
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPessoaFisicaTemp
		{
			get
			{
				return this._idPessoaFisicaTemp;
			}
		}
		#endregion 

		#region DataNascimento
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataNascimento
		{
			get
			{
				return this._dataNascimento;
			}
			set
			{
				this._dataNascimento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDDDCelular
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo obrigatório.
		/// </summary>
		public string NumeroDDDCelular
		{
			get
			{
				return this._numeroDDDCelular;
			}
			set
			{
				this._numeroDDDCelular = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCelular
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo obrigatório.
		/// </summary>
		public string NumeroCelular
		{
			get
			{
				return this._numeroCelular;
			}
			set
			{
				this._numeroCelular = value;
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
		public PessoaFisicaTemp()
		{
		}
		public PessoaFisicaTemp(int idPessoaFisicaTemp)
		{
			this._idPessoaFisicaTemp = idPessoaFisicaTemp;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pessoa_Fisica_Temp (Num_CPF, Dta_Nascimento, Num_DDD_Celular, Num_Celular, Dta_Cadastro) VALUES (@Num_CPF, @Dta_Nascimento, @Num_DDD_Celular, @Num_Celular, @Dta_Cadastro);SET @Idf_Pessoa_Fisica_Temp = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pessoa_Fisica_Temp SET Num_CPF = @Num_CPF, Dta_Nascimento = @Dta_Nascimento, Num_DDD_Celular = @Num_DDD_Celular, Num_Celular = @Num_Celular, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Pessoa_Fisica_Temp = @Idf_Pessoa_Fisica_Temp";
		private const string SPDELETE = "DELETE FROM TAB_Pessoa_Fisica_Temp WHERE Idf_Pessoa_Fisica_Temp = @Idf_Pessoa_Fisica_Temp";
		private const string SPSELECTID = "SELECT * FROM TAB_Pessoa_Fisica_Temp WHERE Idf_Pessoa_Fisica_Temp = @Idf_Pessoa_Fisica_Temp";
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
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Temp", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Dta_Nascimento", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Num_DDD_Celular", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Celular", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idPessoaFisicaTemp;
			parms[1].Value = this._numeroCPF;
			parms[2].Value = this._dataNascimento;
			parms[3].Value = this._numeroDDDCelular;
			parms[4].Value = this._numeroCelular;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[5].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PessoaFisicaTemp no banco de dados.
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
						this._idPessoaFisicaTemp = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica_Temp"].Value);
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
		/// Método utilizado para inserir uma instância de PessoaFisicaTemp no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPessoaFisicaTemp = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica_Temp"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PessoaFisicaTemp no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PessoaFisicaTemp no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PessoaFisicaTemp no banco de dados.
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
		/// Método utilizado para salvar uma instância de PessoaFisicaTemp no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PessoaFisicaTemp no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaTemp">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPessoaFisicaTemp)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Temp", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaTemp;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PessoaFisicaTemp no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaTemp">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPessoaFisicaTemp, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Temp", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaTemp;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PessoaFisicaTemp no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaTemp">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPessoaFisicaTemp)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pessoa_Fisica_Temp where Idf_Pessoa_Fisica_Temp in (";

			for (int i = 0; i < idPessoaFisicaTemp.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPessoaFisicaTemp[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaTemp">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPessoaFisicaTemp)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Temp", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaTemp;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaTemp">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPessoaFisicaTemp, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Temp", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaTemp;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pessoa_Fisica_Temp, Pes.Num_CPF, Pes.Dta_Nascimento, Pes.Num_DDD_Celular, Pes.Num_Celular, Pes.Dta_Cadastro FROM TAB_Pessoa_Fisica_Temp Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoaFisicaTemp a partir do banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaTemp">Chave do registro.</param>
		/// <returns>Instância de PessoaFisicaTemp.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoaFisicaTemp LoadObject(int idPessoaFisicaTemp)
		{
			using (IDataReader dr = LoadDataReader(idPessoaFisicaTemp))
			{
				PessoaFisicaTemp objPessoaFisicaTemp = new PessoaFisicaTemp();
				if (SetInstance(dr, objPessoaFisicaTemp))
					return objPessoaFisicaTemp;
			}
			throw (new RecordNotFoundException(typeof(PessoaFisicaTemp)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoaFisicaTemp a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaTemp">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PessoaFisicaTemp.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoaFisicaTemp LoadObject(int idPessoaFisicaTemp, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPessoaFisicaTemp, trans))
			{
				PessoaFisicaTemp objPessoaFisicaTemp = new PessoaFisicaTemp();
				if (SetInstance(dr, objPessoaFisicaTemp))
					return objPessoaFisicaTemp;
			}
			throw (new RecordNotFoundException(typeof(PessoaFisicaTemp)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PessoaFisicaTemp a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPessoaFisicaTemp))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PessoaFisicaTemp a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPessoaFisicaTemp, trans))
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
		/// <param name="objPessoaFisicaTemp">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PessoaFisicaTemp objPessoaFisicaTemp)
		{
			try
			{
				if (dr.Read())
				{
					objPessoaFisicaTemp._idPessoaFisicaTemp = Convert.ToInt32(dr["Idf_Pessoa_Fisica_Temp"]);
					objPessoaFisicaTemp._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
					objPessoaFisicaTemp._dataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]);
					objPessoaFisicaTemp._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
					objPessoaFisicaTemp._numeroCelular = Convert.ToString(dr["Num_Celular"]);
					objPessoaFisicaTemp._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objPessoaFisicaTemp._persisted = true;
					objPessoaFisicaTemp._modified = false;

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