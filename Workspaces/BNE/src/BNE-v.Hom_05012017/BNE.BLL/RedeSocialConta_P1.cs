//-- Data: 28/04/2010 08:41
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RedeSocialConta // Tabela: BNE_Rede_Social_Conta
	{
		#region Atributos
		private int _idRedeSocialConta;
		private bool _flagVaga;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private RedeSocialCS _redeSocialcs;
		private string _descricaoLogin;
		private string _descricaoSenha;
		private string _descricaoComunidade;
		private AreaBNE _areaBNE;
		private Cidade _cidade;
		private FuncaoCategoria _funcaoCategoria;
		private Vaga _vaga;
		private Funcao _funcao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRedeSocialConta
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRedeSocialConta
		{
			get
			{
				return this._idRedeSocialConta;
			}
		}
		#endregion 

		#region FlagVaga
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagVaga
		{
			get
			{
				return this._flagVaga;
			}
			set
			{
				this._flagVaga = value;
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

		#region RedeSocialcs
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public RedeSocialCS RedeSocialcs
		{
			get
			{
				return this._redeSocialcs;
			}
			set
			{
				this._redeSocialcs = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoLogin
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoLogin
		{
			get
			{
				return this._descricaoLogin;
			}
			set
			{
				this._descricaoLogin = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoSenha
		/// <summary>
		/// Tamanho do campo: 255.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoSenha
		{
			get
			{
				return this._descricaoSenha;
			}
			set
			{
				this._descricaoSenha = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoComunidade
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string DescricaoComunidade
		{
			get
			{
				return this._descricaoComunidade;
			}
			set
			{
				this._descricaoComunidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region AreaBNE
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public AreaBNE AreaBNE
		{
			get
			{
				return this._areaBNE;
			}
			set
			{
				this._areaBNE = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Cidade
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Cidade Cidade
		{
			get
			{
				return this._cidade;
			}
			set
			{
				this._cidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FuncaoCategoria
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public FuncaoCategoria FuncaoCategoria
		{
			get
			{
				return this._funcaoCategoria;
			}
			set
			{
				this._funcaoCategoria = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Vaga
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Vaga Vaga
		{
			get
			{
				return this._vaga;
			}
			set
			{
				this._vaga = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Funcao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Funcao Funcao
		{
			get
			{
				return this._funcao;
			}
			set
			{
				this._funcao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public RedeSocialConta()
		{
		}
		public RedeSocialConta(int idRedeSocialConta)
		{
			this._idRedeSocialConta = idRedeSocialConta;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Rede_Social_Conta (Flg_Vaga, Flg_Inativo, Dta_Cadastro, Idf_Rede_Social_cs, Des_Login, Des_Senha, Des_Comunidade, Idf_Area_BNE, Idf_Cidade, Idf_Funcao_Categoria, Idf_Vaga, Idf_Funcao) VALUES (@Flg_Vaga, @Flg_Inativo, @Dta_Cadastro, @Idf_Rede_Social_cs, @Des_Login, @Des_Senha, @Des_Comunidade, @Idf_Area_BNE, @Idf_Cidade, @Idf_Funcao_Categoria, @Idf_Vaga, @Idf_Funcao);SET @Idf_Rede_Social_Conta = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Rede_Social_Conta SET Flg_Vaga = @Flg_Vaga, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Idf_Rede_Social_cs = @Idf_Rede_Social_cs, Des_Login = @Des_Login, Des_Senha = @Des_Senha, Des_Comunidade = @Des_Comunidade, Idf_Area_BNE = @Idf_Area_BNE, Idf_Cidade = @Idf_Cidade, Idf_Funcao_Categoria = @Idf_Funcao_Categoria, Idf_Vaga = @Idf_Vaga, Idf_Funcao = @Idf_Funcao WHERE Idf_Rede_Social_Conta = @Idf_Rede_Social_Conta";
		private const string SPDELETE = "DELETE FROM BNE_Rede_Social_Conta WHERE Idf_Rede_Social_Conta = @Idf_Rede_Social_Conta";
		private const string SPSELECTID = "SELECT * FROM BNE_Rede_Social_Conta WHERE Idf_Rede_Social_Conta = @Idf_Rede_Social_Conta";
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
			parms.Add(new SqlParameter("@Idf_Rede_Social_Conta", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Vaga", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Rede_Social_cs", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Login", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Des_Senha", SqlDbType.VarChar, 255));
			parms.Add(new SqlParameter("@Des_Comunidade", SqlDbType.VarChar, 10));
			parms.Add(new SqlParameter("@Idf_Area_BNE", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao_Categoria", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
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
			parms[0].Value = this._idRedeSocialConta;
			parms[1].Value = this._flagVaga;
			parms[2].Value = this._flagInativo;
			parms[4].Value = this._redeSocialcs.IdRedeSocialCS;
			parms[5].Value = this._descricaoLogin;
			parms[6].Value = this._descricaoSenha;

			if (!String.IsNullOrEmpty(this._descricaoComunidade))
				parms[7].Value = this._descricaoComunidade;
			else
				parms[7].Value = DBNull.Value;


			if (this._areaBNE != null)
				parms[8].Value = this._areaBNE.IdAreaBNE;
			else
				parms[8].Value = DBNull.Value;


			if (this._cidade != null)
				parms[9].Value = this._cidade.IdCidade;
			else
				parms[9].Value = DBNull.Value;


			if (this._funcaoCategoria != null)
				parms[10].Value = this._funcaoCategoria.IdFuncaoCategoria;
			else
				parms[10].Value = DBNull.Value;


			if (this._vaga != null)
				parms[11].Value = this._vaga.IdVaga;
			else
				parms[11].Value = DBNull.Value;


			if (this._funcao != null)
				parms[12].Value = this._funcao.IdFuncao;
			else
				parms[12].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de RedeSocialConta no banco de dados.
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
						this._idRedeSocialConta = Convert.ToInt32(cmd.Parameters["@Idf_Rede_Social_Conta"].Value);
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
		/// Método utilizado para inserir uma instância de RedeSocialConta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRedeSocialConta = Convert.ToInt32(cmd.Parameters["@Idf_Rede_Social_Conta"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RedeSocialConta no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RedeSocialConta no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de RedeSocialConta no banco de dados.
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
		/// Método utilizado para salvar uma instância de RedeSocialConta no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RedeSocialConta no banco de dados.
		/// </summary>
		/// <param name="idRedeSocialConta">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRedeSocialConta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rede_Social_Conta", SqlDbType.Int, 4));

			parms[0].Value = idRedeSocialConta;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RedeSocialConta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRedeSocialConta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRedeSocialConta, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rede_Social_Conta", SqlDbType.Int, 4));

			parms[0].Value = idRedeSocialConta;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RedeSocialConta no banco de dados.
		/// </summary>
		/// <param name="idRedeSocialConta">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRedeSocialConta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Rede_Social_Conta where Idf_Rede_Social_Conta in (";

			for (int i = 0; i < idRedeSocialConta.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRedeSocialConta[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRedeSocialConta">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRedeSocialConta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rede_Social_Conta", SqlDbType.Int, 4));

			parms[0].Value = idRedeSocialConta;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRedeSocialConta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRedeSocialConta, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rede_Social_Conta", SqlDbType.Int, 4));

			parms[0].Value = idRedeSocialConta;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Red.Idf_Rede_Social_Conta, Red.Flg_Vaga, Red.Flg_Inativo, Red.Dta_Cadastro, Red.Idf_Rede_Social_cs, Red.Des_Login, Red.Des_Senha, Red.Des_Comunidade, Red.Idf_Area_BNE, Red.Idf_Cidade, Red.Idf_Funcao_Categoria, Red.Idf_Vaga, Red.Idf_Funcao FROM BNE_Rede_Social_Conta Red";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RedeSocialConta a partir do banco de dados.
		/// </summary>
		/// <param name="idRedeSocialConta">Chave do registro.</param>
		/// <returns>Instância de RedeSocialConta.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RedeSocialConta LoadObject(int idRedeSocialConta)
		{
			using (IDataReader dr = LoadDataReader(idRedeSocialConta))
			{
				RedeSocialConta objRedeSocialConta = new RedeSocialConta();
				if (SetInstance(dr, objRedeSocialConta))
					return objRedeSocialConta;
			}
			throw (new RecordNotFoundException(typeof(RedeSocialConta)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RedeSocialConta a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRedeSocialConta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RedeSocialConta.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RedeSocialConta LoadObject(int idRedeSocialConta, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRedeSocialConta, trans))
			{
				RedeSocialConta objRedeSocialConta = new RedeSocialConta();
				if (SetInstance(dr, objRedeSocialConta))
					return objRedeSocialConta;
			}
			throw (new RecordNotFoundException(typeof(RedeSocialConta)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RedeSocialConta a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRedeSocialConta))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RedeSocialConta a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRedeSocialConta, trans))
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
		/// <param name="objRedeSocialConta">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RedeSocialConta objRedeSocialConta)
		{
			try
			{
				if (dr.Read())
				{
					objRedeSocialConta._idRedeSocialConta = Convert.ToInt32(dr["Idf_Rede_Social_Conta"]);
					objRedeSocialConta._flagVaga = Convert.ToBoolean(dr["Flg_Vaga"]);
					objRedeSocialConta._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objRedeSocialConta._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objRedeSocialConta._redeSocialcs = new RedeSocialCS(Convert.ToInt32(dr["Idf_Rede_Social_cs"]));
					objRedeSocialConta._descricaoLogin = Convert.ToString(dr["Des_Login"]);
					objRedeSocialConta._descricaoSenha = Convert.ToString(dr["Des_Senha"]);
					if (dr["Des_Comunidade"] != DBNull.Value)
						objRedeSocialConta._descricaoComunidade = Convert.ToString(dr["Des_Comunidade"]);
					if (dr["Idf_Area_BNE"] != DBNull.Value)
						objRedeSocialConta._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
					if (dr["Idf_Cidade"] != DBNull.Value)
						objRedeSocialConta._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					if (dr["Idf_Funcao_Categoria"] != DBNull.Value)
						objRedeSocialConta._funcaoCategoria = new FuncaoCategoria(Convert.ToInt32(dr["Idf_Funcao_Categoria"]));
					if (dr["Idf_Vaga"] != DBNull.Value)
						objRedeSocialConta._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					if (dr["Idf_Funcao"] != DBNull.Value)
						objRedeSocialConta._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));

					objRedeSocialConta._persisted = true;
					objRedeSocialConta._modified = false;

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