//-- Data: 16/04/2013 16:57
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CadastroParceiro // Tabela: BNE_Cadastro_Parceiro
	{
		#region Atributos
		private int _idCadastroParceiro;
		private DateTime _dataCadastro;
		private string _descricaoLogin;
		private string _descricaoSenha;
		private bool _flagInativo;
		private Curriculo _curriculo;
		private ParceiroTecla _parceiroTecla;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCadastroParceiro
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCadastroParceiro
		{
			get
			{
				return this._idCadastroParceiro;
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

		#region DescricaoLogin
		/// <summary>
		/// Tamanho do campo: 100.
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
		/// Tamanho do campo: 100.
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

		#region Curriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Curriculo Curriculo
		{
			get
			{
				return this._curriculo;
			}
			set
			{
				this._curriculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ParceiroTecla
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public ParceiroTecla ParceiroTecla
		{
			get
			{
				return this._parceiroTecla;
			}
			set
			{
				this._parceiroTecla = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CadastroParceiro()
		{
		}
		public CadastroParceiro(int idCadastroParceiro)
		{
			this._idCadastroParceiro = idCadastroParceiro;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Cadastro_Parceiro (Dta_Cadastro, Des_Login, Des_Senha, Flg_Inativo, Idf_Curriculo, Idf_Parceiro_Tecla) VALUES (@Dta_Cadastro, @Des_Login, @Des_Senha, @Flg_Inativo, @Idf_Curriculo, @Idf_Parceiro_Tecla);SET @Idf_Cadastro_Parceiro = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Cadastro_Parceiro SET Dta_Cadastro = @Dta_Cadastro, Des_Login = @Des_Login, Des_Senha = @Des_Senha, Flg_Inativo = @Flg_Inativo, Idf_Curriculo = @Idf_Curriculo, Idf_Parceiro_Tecla = @Idf_Parceiro_Tecla WHERE Idf_Cadastro_Parceiro = @Idf_Cadastro_Parceiro";
		private const string SPDELETE = "DELETE FROM BNE_Cadastro_Parceiro WHERE Idf_Cadastro_Parceiro = @Idf_Cadastro_Parceiro";
		private const string SPSELECTID = "SELECT * FROM BNE_Cadastro_Parceiro WITH(NOLOCK) WHERE Idf_Cadastro_Parceiro = @Idf_Cadastro_Parceiro";
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
			parms.Add(new SqlParameter("@Idf_Cadastro_Parceiro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Login", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Senha", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Parceiro_Tecla", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCadastroParceiro;
			parms[2].Value = this._descricaoLogin;
			parms[3].Value = this._descricaoSenha;
			parms[4].Value = this._flagInativo;
			parms[5].Value = this._curriculo.IdCurriculo;
			parms[6].Value = this._parceiroTecla.IdParceiroTecla;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[1].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CadastroParceiro no banco de dados.
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
						this._idCadastroParceiro = Convert.ToInt32(cmd.Parameters["@Idf_Cadastro_Parceiro"].Value);
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
		/// Método utilizado para inserir uma instância de CadastroParceiro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCadastroParceiro = Convert.ToInt32(cmd.Parameters["@Idf_Cadastro_Parceiro"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CadastroParceiro no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CadastroParceiro no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CadastroParceiro no banco de dados.
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
		/// Método utilizado para salvar uma instância de CadastroParceiro no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CadastroParceiro no banco de dados.
		/// </summary>
		/// <param name="idCadastroParceiro">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCadastroParceiro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cadastro_Parceiro", SqlDbType.Int, 4));

			parms[0].Value = idCadastroParceiro;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CadastroParceiro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCadastroParceiro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCadastroParceiro, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cadastro_Parceiro", SqlDbType.Int, 4));

			parms[0].Value = idCadastroParceiro;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CadastroParceiro no banco de dados.
		/// </summary>
		/// <param name="idCadastroParceiro">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCadastroParceiro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Cadastro_Parceiro where Idf_Cadastro_Parceiro in (";

			for (int i = 0; i < idCadastroParceiro.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCadastroParceiro[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCadastroParceiro">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCadastroParceiro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cadastro_Parceiro", SqlDbType.Int, 4));

			parms[0].Value = idCadastroParceiro;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCadastroParceiro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCadastroParceiro, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cadastro_Parceiro", SqlDbType.Int, 4));

			parms[0].Value = idCadastroParceiro;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cad.Idf_Cadastro_Parceiro, Cad.Dta_Cadastro, Cad.Des_Login, Cad.Des_Senha, Cad.Flg_Inativo, Cad.Idf_Curriculo, Cad.Idf_Parceiro_Tecla FROM BNE_Cadastro_Parceiro Cad";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CadastroParceiro a partir do banco de dados.
		/// </summary>
		/// <param name="idCadastroParceiro">Chave do registro.</param>
		/// <returns>Instância de CadastroParceiro.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CadastroParceiro LoadObject(int idCadastroParceiro)
		{
			using (IDataReader dr = LoadDataReader(idCadastroParceiro))
			{
				CadastroParceiro objCadastroParceiro = new CadastroParceiro();
				if (SetInstance(dr, objCadastroParceiro))
					return objCadastroParceiro;
			}
			throw (new RecordNotFoundException(typeof(CadastroParceiro)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CadastroParceiro a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCadastroParceiro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CadastroParceiro.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CadastroParceiro LoadObject(int idCadastroParceiro, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCadastroParceiro, trans))
			{
				CadastroParceiro objCadastroParceiro = new CadastroParceiro();
				if (SetInstance(dr, objCadastroParceiro))
					return objCadastroParceiro;
			}
			throw (new RecordNotFoundException(typeof(CadastroParceiro)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CadastroParceiro a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCadastroParceiro))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CadastroParceiro a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCadastroParceiro, trans))
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
		/// <param name="objCadastroParceiro">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CadastroParceiro objCadastroParceiro)
		{
			try
			{
				if (dr.Read())
				{
					objCadastroParceiro._idCadastroParceiro = Convert.ToInt32(dr["Idf_Cadastro_Parceiro"]);
					objCadastroParceiro._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objCadastroParceiro._descricaoLogin = Convert.ToString(dr["Des_Login"]);
					objCadastroParceiro._descricaoSenha = Convert.ToString(dr["Des_Senha"]);
					objCadastroParceiro._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objCadastroParceiro._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objCadastroParceiro._parceiroTecla = new ParceiroTecla(Convert.ToInt32(dr["Idf_Parceiro_Tecla"]));

					objCadastroParceiro._persisted = true;
					objCadastroParceiro._modified = false;

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