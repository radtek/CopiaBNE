//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PessoafisicaIdioma // Tabela: TAB_Pessoa_fisica_Idioma
	{
		#region Atributos
		private int _idPessoaFisicaIdioma;
		private PessoaFisica _pessoaFisica;
		private Idioma _idioma;
		private NivelIdioma _nivelIdioma;
		private DateTime _dataCadastro;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPessoaFisicaIdioma
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPessoaFisicaIdioma
		{
			get
			{
				return this._idPessoaFisicaIdioma;
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

		#region Idioma
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Idioma Idioma
		{
			get
			{
				return this._idioma;
			}
			set
			{
				this._idioma = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NivelIdioma
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public NivelIdioma NivelIdioma
		{
			get
			{
				return this._nivelIdioma;
			}
			set
			{
				this._nivelIdioma = value;
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
		public PessoafisicaIdioma()
		{
		}
		public PessoafisicaIdioma(int idPessoaFisicaIdioma)
		{
			this._idPessoaFisicaIdioma = idPessoaFisicaIdioma;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pessoa_fisica_Idioma (Idf_Pessoa_Fisica, Idf_Idioma, Idf_Nivel_Idioma, Dta_Cadastro, Flg_Inativo) VALUES (@Idf_Pessoa_Fisica, @Idf_Idioma, @Idf_Nivel_Idioma, @Dta_Cadastro, @Flg_Inativo);SET @Idf_Pessoa_Fisica_Idioma = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pessoa_fisica_Idioma SET Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica, Idf_Idioma = @Idf_Idioma, Idf_Nivel_Idioma = @Idf_Nivel_Idioma, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Pessoa_Fisica_Idioma = @Idf_Pessoa_Fisica_Idioma";
		private const string SPDELETE = "DELETE FROM TAB_Pessoa_fisica_Idioma WHERE Idf_Pessoa_Fisica_Idioma = @Idf_Pessoa_Fisica_Idioma";
		private const string SPSELECTID = "SELECT * FROM TAB_Pessoa_fisica_Idioma WHERE Idf_Pessoa_Fisica_Idioma = @Idf_Pessoa_Fisica_Idioma";
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
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Idioma", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Idioma", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Nivel_Idioma", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idPessoaFisicaIdioma;
			parms[1].Value = this._pessoaFisica.IdPessoaFisica;
			parms[2].Value = this._idioma.IdIdioma;
			parms[3].Value = this._nivelIdioma.IdNivelIdioma;
			parms[5].Value = this._flagInativo;

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
		/// Método utilizado para inserir uma instância de PessoafisicaIdioma no banco de dados.
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
						this._idPessoaFisicaIdioma = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica_Idioma"].Value);
						cmd.Parameters.Clear();
						this._persisted = true;
						this._modified = false;
						trans.Commit();
                        Custom.Solr.Buffer.BufferAtualizacaoCurriculoIdioma.Update(new Curriculo(Curriculo.RecuperarIdPorPessoaFisica(this.PessoaFisica)));
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
		/// Método utilizado para inserir uma instância de PessoafisicaIdioma no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPessoaFisicaIdioma = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica_Idioma"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
            Custom.Solr.Buffer.BufferAtualizacaoCurriculoIdioma.Update(new Curriculo(Curriculo.RecuperarIdPorPessoaFisica(this.PessoaFisica)));
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PessoafisicaIdioma no banco de dados.
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
                Custom.Solr.Buffer.BufferAtualizacaoCurriculoIdioma.Update(new Curriculo(Curriculo.RecuperarIdPorPessoaFisica(this.PessoaFisica)));
			}
		}
		/// <summary>
		/// Método utilizado para atualizar uma instância de PessoafisicaIdioma no banco de dados, dentro de uma transação.
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
                Custom.Solr.Buffer.BufferAtualizacaoCurriculoIdioma.Update(new Curriculo(Curriculo.RecuperarIdPorPessoaFisica(this.PessoaFisica)));
			}
		}
		#endregion

		#region Save
		/// <summary>
		/// Método utilizado para salvar uma instância de PessoafisicaIdioma no banco de dados.
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
		/// Método utilizado para salvar uma instância de PessoafisicaIdioma no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PessoafisicaIdioma no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaIdioma">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPessoaFisicaIdioma)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaIdioma;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PessoafisicaIdioma no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaIdioma">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPessoaFisicaIdioma, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaIdioma;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PessoafisicaIdioma no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaIdioma">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPessoaFisicaIdioma)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pessoa_fisica_Idioma where Idf_Pessoa_Fisica_Idioma in (";

			for (int i = 0; i < idPessoaFisicaIdioma.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPessoaFisicaIdioma[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaIdioma">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPessoaFisicaIdioma)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaIdioma;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaIdioma">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPessoaFisicaIdioma, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaIdioma;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pessoa_Fisica_Idioma, Pes.Idf_Pessoa_Fisica, Pes.Idf_Idioma, Pes.Idf_Nivel_Idioma, Pes.Dta_Cadastro, Pes.Flg_Inativo FROM TAB_Pessoa_fisica_Idioma Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoafisicaIdioma a partir do banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaIdioma">Chave do registro.</param>
		/// <returns>Instância de PessoafisicaIdioma.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoafisicaIdioma LoadObject(int idPessoaFisicaIdioma)
		{
			using (IDataReader dr = LoadDataReader(idPessoaFisicaIdioma))
			{
				PessoafisicaIdioma objPessoafisicaIdioma = new PessoafisicaIdioma();
				if (SetInstance(dr, objPessoafisicaIdioma))
					return objPessoafisicaIdioma;
			}
			throw (new RecordNotFoundException(typeof(PessoafisicaIdioma)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoafisicaIdioma a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaIdioma">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PessoafisicaIdioma.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoafisicaIdioma LoadObject(int idPessoaFisicaIdioma, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPessoaFisicaIdioma, trans))
			{
				PessoafisicaIdioma objPessoafisicaIdioma = new PessoafisicaIdioma();
				if (SetInstance(dr, objPessoafisicaIdioma))
					return objPessoafisicaIdioma;
			}
			throw (new RecordNotFoundException(typeof(PessoafisicaIdioma)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PessoafisicaIdioma a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPessoaFisicaIdioma))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PessoafisicaIdioma a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPessoaFisicaIdioma, trans))
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
		/// <param name="objPessoafisicaIdioma">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PessoafisicaIdioma objPessoafisicaIdioma, bool dispose = false)
		{
			try
			{
				if (dr.Read())
				{
					objPessoafisicaIdioma._idPessoaFisicaIdioma = Convert.ToInt32(dr["Idf_Pessoa_Fisica_Idioma"]);
					objPessoafisicaIdioma._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
					objPessoafisicaIdioma._idioma = new Idioma(Convert.ToInt32(dr["Idf_Idioma"]));
					objPessoafisicaIdioma._nivelIdioma = new NivelIdioma(Convert.ToInt32(dr["Idf_Nivel_Idioma"]));
					objPessoafisicaIdioma._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objPessoafisicaIdioma._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objPessoafisicaIdioma._persisted = true;
					objPessoafisicaIdioma._modified = false;

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
                if(dispose)
				    dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}