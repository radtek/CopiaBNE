//-- Data: 17/07/2013 16:53
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PessoaFisicaRedeSocial // Tabela: TAB_Pessoa_Fisica_Rede_Social
	{
		#region Atributos
		private int _idPessoaFisicaRedeSocial;
		private PessoaFisica _pessoaFisica;
		private RedeSocialCS _redeSocialCS;
		private string _codigoIdentificador;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private string _codigoInternoRedeSocial;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPessoaFisicaRedeSocial
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPessoaFisicaRedeSocial
		{
			get
			{
				return this._idPessoaFisicaRedeSocial;
			}
		}
		#endregion 

		#region PessoaFisica
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public PessoaFisica PessoaFisica
		{
			get
			{
				return this._pessoaFisica;
			}
			set
			{
				this._pessoaFisica = value;
				this._modified = true;
			}
		}
		#endregion 

		#region RedeSocialCS
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public RedeSocialCS RedeSocialCS
		{
			get
			{
				return this._redeSocialCS;
			}
			set
			{
				this._redeSocialCS = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoIdentificador
		/// <summary>
		/// Tamanho do campo: 350.
		/// Campo obrigatório.
		/// </summary>
		public string CodigoIdentificador
		{
			get
			{
				return this._codigoIdentificador;
			}
			set
			{
				this._codigoIdentificador = value;
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

		#region CodigoInternoRedeSocial
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo opcional.
		/// </summary>
		public string CodigoInternoRedeSocial
		{
			get
			{
				return this._codigoInternoRedeSocial;
			}
			set
			{
				this._codigoInternoRedeSocial = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PessoaFisicaRedeSocial()
		{
		}
		public PessoaFisicaRedeSocial(int idPessoaFisicaRedeSocial)
		{
			this._idPessoaFisicaRedeSocial = idPessoaFisicaRedeSocial;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pessoa_Fisica_Rede_Social (Idf_Pessoa_Fisica, Idf_Rede_Social_CS, Cod_Identificador, Flg_Inativo, Dta_Cadastro, Cod_Interno_Rede_Social) VALUES (@Idf_Pessoa_Fisica, @Idf_Rede_Social_CS, @Cod_Identificador, @Flg_Inativo, @Dta_Cadastro, @Cod_Interno_Rede_Social);SET @Idf_Pessoa_Fisica_Rede_Social = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pessoa_Fisica_Rede_Social SET Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica, Idf_Rede_Social_CS = @Idf_Rede_Social_CS, Cod_Identificador = @Cod_Identificador, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Cod_Interno_Rede_Social = @Cod_Interno_Rede_Social WHERE Idf_Pessoa_Fisica_Rede_Social = @Idf_Pessoa_Fisica_Rede_Social";
		private const string SPDELETE = "DELETE FROM TAB_Pessoa_Fisica_Rede_Social WHERE Idf_Pessoa_Fisica_Rede_Social = @Idf_Pessoa_Fisica_Rede_Social";
		private const string SPSELECTID = "SELECT * FROM TAB_Pessoa_Fisica_Rede_Social WITH(NOLOCK) WHERE Idf_Pessoa_Fisica_Rede_Social = @Idf_Pessoa_Fisica_Rede_Social";
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
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Rede_Social", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Rede_Social_CS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Cod_Identificador", SqlDbType.VarChar, 350));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Cod_Interno_Rede_Social", SqlDbType.VarChar, 30));
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
			parms[0].Value = this._idPessoaFisicaRedeSocial;
			parms[1].Value = this._pessoaFisica.IdPessoaFisica;
			parms[2].Value = this._redeSocialCS.IdRedeSocialCS;
			parms[3].Value = this._codigoIdentificador;
			parms[4].Value = this._flagInativo;

			if (!String.IsNullOrEmpty(this._codigoInternoRedeSocial))
				parms[6].Value = this._codigoInternoRedeSocial;
			else
				parms[6].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de PessoaFisicaRedeSocial no banco de dados.
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
						this._idPessoaFisicaRedeSocial = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica_Rede_Social"].Value);
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
		/// Método utilizado para inserir uma instância de PessoaFisicaRedeSocial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPessoaFisicaRedeSocial = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica_Rede_Social"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PessoaFisicaRedeSocial no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PessoaFisicaRedeSocial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PessoaFisicaRedeSocial no banco de dados.
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
		/// Método utilizado para salvar uma instância de PessoaFisicaRedeSocial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PessoaFisicaRedeSocial no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaRedeSocial">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPessoaFisicaRedeSocial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Rede_Social", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaRedeSocial;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PessoaFisicaRedeSocial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaRedeSocial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPessoaFisicaRedeSocial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Rede_Social", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaRedeSocial;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PessoaFisicaRedeSocial no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaRedeSocial">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPessoaFisicaRedeSocial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pessoa_Fisica_Rede_Social where Idf_Pessoa_Fisica_Rede_Social in (";

			for (int i = 0; i < idPessoaFisicaRedeSocial.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPessoaFisicaRedeSocial[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaRedeSocial">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPessoaFisicaRedeSocial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Rede_Social", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaRedeSocial;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaRedeSocial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPessoaFisicaRedeSocial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Rede_Social", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaRedeSocial;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pessoa_Fisica_Rede_Social, Pes.Idf_Pessoa_Fisica, Pes.Idf_Rede_Social_CS, Pes.Cod_Identificador, Pes.Flg_Inativo, Pes.Dta_Cadastro, Pes.Cod_Interno_Rede_Social FROM TAB_Pessoa_Fisica_Rede_Social Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoaFisicaRedeSocial a partir do banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaRedeSocial">Chave do registro.</param>
		/// <returns>Instância de PessoaFisicaRedeSocial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoaFisicaRedeSocial LoadObject(int idPessoaFisicaRedeSocial)
		{
			using (IDataReader dr = LoadDataReader(idPessoaFisicaRedeSocial))
			{
				PessoaFisicaRedeSocial objPessoaFisicaRedeSocial = new PessoaFisicaRedeSocial();
				if (SetInstance(dr, objPessoaFisicaRedeSocial))
					return objPessoaFisicaRedeSocial;
			}
			throw (new RecordNotFoundException(typeof(PessoaFisicaRedeSocial)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoaFisicaRedeSocial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaRedeSocial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PessoaFisicaRedeSocial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoaFisicaRedeSocial LoadObject(int idPessoaFisicaRedeSocial, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPessoaFisicaRedeSocial, trans))
			{
				PessoaFisicaRedeSocial objPessoaFisicaRedeSocial = new PessoaFisicaRedeSocial();
				if (SetInstance(dr, objPessoaFisicaRedeSocial))
					return objPessoaFisicaRedeSocial;
			}
			throw (new RecordNotFoundException(typeof(PessoaFisicaRedeSocial)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PessoaFisicaRedeSocial a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPessoaFisicaRedeSocial))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PessoaFisicaRedeSocial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPessoaFisicaRedeSocial, trans))
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
		/// <param name="objPessoaFisicaRedeSocial">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PessoaFisicaRedeSocial objPessoaFisicaRedeSocial)
		{
			try
			{
				if (dr.Read())
				{
					objPessoaFisicaRedeSocial._idPessoaFisicaRedeSocial = Convert.ToInt32(dr["Idf_Pessoa_Fisica_Rede_Social"]);
					objPessoaFisicaRedeSocial._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
					objPessoaFisicaRedeSocial._redeSocialCS = new RedeSocialCS(Convert.ToInt32(dr["Idf_Rede_Social_CS"]));
					objPessoaFisicaRedeSocial._codigoIdentificador = Convert.ToString(dr["Cod_Identificador"]);
					objPessoaFisicaRedeSocial._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objPessoaFisicaRedeSocial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Cod_Interno_Rede_Social"] != DBNull.Value)
						objPessoaFisicaRedeSocial._codigoInternoRedeSocial = Convert.ToString(dr["Cod_Interno_Rede_Social"]);

					objPessoaFisicaRedeSocial._persisted = true;
					objPessoaFisicaRedeSocial._modified = false;

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