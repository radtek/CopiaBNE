//-- Data: 28/04/2010 08:41
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Financeiro // Tabela: BNE_Financeiro
	{
		#region Atributos
		private int _idFinanceiro;
		private DateTime _dataImportacao;
		private string _descricaoCaminho;
		private int _idUsuario;
		private bool _flagSituacao;
		private DateTime _dataCadastro;
		private bool _flagInativo;
		private UsuarioFilialPerfil _usuarioFilialPerfil;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdFinanceiro
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdFinanceiro
		{
			get
			{
				return this._idFinanceiro;
			}
		}
		#endregion 

		#region DataImportacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataImportacao
		{
			get
			{
				return this._dataImportacao;
			}
			set
			{
				this._dataImportacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCaminho
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoCaminho
		{
			get
			{
				return this._descricaoCaminho;
			}
			set
			{
				this._descricaoCaminho = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdUsuario
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdUsuario
		{
			get
			{
				return this._idUsuario;
			}
			set
			{
				this._idUsuario = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagSituacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagSituacao
		{
			get
			{
				return this._flagSituacao;
			}
			set
			{
				this._flagSituacao = value;
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

		#region UsuarioFilialPerfil
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public UsuarioFilialPerfil UsuarioFilialPerfil
		{
			get
			{
				return this._usuarioFilialPerfil;
			}
			set
			{
				this._usuarioFilialPerfil = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Financeiro()
		{
		}
		public Financeiro(int idFinanceiro)
		{
			this._idFinanceiro = idFinanceiro;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Financeiro (Dta_Importacao, Des_Caminho, Idf_Usuario, Flg_Situacao, Dta_Cadastro, Flg_Inativo, Idf_Usuario_Filial_Perfil) VALUES (@Dta_Importacao, @Des_Caminho, @Idf_Usuario, @Flg_Situacao, @Dta_Cadastro, @Flg_Inativo, @Idf_Usuario_Filial_Perfil);SET @Idf_Financeiro = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Financeiro SET Dta_Importacao = @Dta_Importacao, Des_Caminho = @Des_Caminho, Idf_Usuario = @Idf_Usuario, Flg_Situacao = @Flg_Situacao, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil WHERE Idf_Financeiro = @Idf_Financeiro";
		private const string SPDELETE = "DELETE FROM BNE_Financeiro WHERE Idf_Financeiro = @Idf_Financeiro";
		private const string SPSELECTID = "SELECT * FROM BNE_Financeiro WHERE Idf_Financeiro = @Idf_Financeiro";
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
			parms.Add(new SqlParameter("@Idf_Financeiro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Importacao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Caminho", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Usuario", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Situacao", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
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
			parms[0].Value = this._idFinanceiro;
			parms[1].Value = this._dataImportacao;
			parms[2].Value = this._descricaoCaminho;
			parms[3].Value = this._idUsuario;
			parms[4].Value = this._flagSituacao;
			parms[6].Value = this._flagInativo;
			parms[7].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;

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
		/// Método utilizado para inserir uma instância de Financeiro no banco de dados.
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
						this._idFinanceiro = Convert.ToInt32(cmd.Parameters["@Idf_Financeiro"].Value);
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
		/// Método utilizado para inserir uma instância de Financeiro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idFinanceiro = Convert.ToInt32(cmd.Parameters["@Idf_Financeiro"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Financeiro no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Financeiro no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Financeiro no banco de dados.
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
		/// Método utilizado para salvar uma instância de Financeiro no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Financeiro no banco de dados.
		/// </summary>
		/// <param name="idFinanceiro">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFinanceiro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Financeiro", SqlDbType.Int, 4));

			parms[0].Value = idFinanceiro;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Financeiro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFinanceiro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFinanceiro, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Financeiro", SqlDbType.Int, 4));

			parms[0].Value = idFinanceiro;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Financeiro no banco de dados.
		/// </summary>
		/// <param name="idFinanceiro">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idFinanceiro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Financeiro where Idf_Financeiro in (";

			for (int i = 0; i < idFinanceiro.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idFinanceiro[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFinanceiro">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFinanceiro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Financeiro", SqlDbType.Int, 4));

			parms[0].Value = idFinanceiro;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFinanceiro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFinanceiro, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Financeiro", SqlDbType.Int, 4));

			parms[0].Value = idFinanceiro;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Fin.Idf_Financeiro, Fin.Dta_Importacao, Fin.Des_Caminho, Fin.Idf_Usuario, Fin.Flg_Situacao, Fin.Dta_Cadastro, Fin.Flg_Inativo, Fin.Idf_Usuario_Filial_Perfil FROM BNE_Financeiro Fin";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Financeiro a partir do banco de dados.
		/// </summary>
		/// <param name="idFinanceiro">Chave do registro.</param>
		/// <returns>Instância de Financeiro.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Financeiro LoadObject(int idFinanceiro)
		{
			using (IDataReader dr = LoadDataReader(idFinanceiro))
			{
				Financeiro objFinanceiro = new Financeiro();
				if (SetInstance(dr, objFinanceiro))
					return objFinanceiro;
			}
			throw (new RecordNotFoundException(typeof(Financeiro)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Financeiro a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFinanceiro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Financeiro.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Financeiro LoadObject(int idFinanceiro, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idFinanceiro, trans))
			{
				Financeiro objFinanceiro = new Financeiro();
				if (SetInstance(dr, objFinanceiro))
					return objFinanceiro;
			}
			throw (new RecordNotFoundException(typeof(Financeiro)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Financeiro a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idFinanceiro))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Financeiro a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idFinanceiro, trans))
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
		/// <param name="objFinanceiro">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Financeiro objFinanceiro)
		{
			try
			{
				if (dr.Read())
				{
					objFinanceiro._idFinanceiro = Convert.ToInt32(dr["Idf_Financeiro"]);
					objFinanceiro._dataImportacao = Convert.ToDateTime(dr["Dta_Importacao"]);
					objFinanceiro._descricaoCaminho = Convert.ToString(dr["Des_Caminho"]);
					objFinanceiro._idUsuario = Convert.ToInt32(dr["Idf_Usuario"]);
					objFinanceiro._flagSituacao = Convert.ToBoolean(dr["Flg_Situacao"]);
					objFinanceiro._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objFinanceiro._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objFinanceiro._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));

					objFinanceiro._persisted = true;
					objFinanceiro._modified = false;

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