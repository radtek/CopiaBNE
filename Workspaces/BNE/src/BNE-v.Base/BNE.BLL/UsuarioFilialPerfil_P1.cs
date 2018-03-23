//-- Data: 15/05/2012 16:19
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class UsuarioFilialPerfil // Tabela: TAB_Usuario_Filial_Perfil
	{
		#region Atributos
		private int _idUsuarioFilialPerfil;
		private Filial _filial;
		private string _descricaoIP;
		private DateTime _dataCadastro;
		private bool _flagInativo;
		private PessoaFisica _pessoaFisica;
		private Perfil _perfil;
		private string _senhaUsuarioFilialPerfil;
		private DateTime _dataAlteracao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdUsuarioFilialPerfil
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdUsuarioFilialPerfil
		{
			get
			{
				return this._idUsuarioFilialPerfil;
			}
		}
		#endregion 

		#region Filial
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Filial Filial
		{
			get
			{
				return this._filial;
			}
			set
			{
				this._filial = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoIP
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
		/// </summary>
		public string DescricaoIP
		{
			get
			{
				return this._descricaoIP;
			}
			set
			{
				this._descricaoIP = value;
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

		#region Perfil
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Perfil Perfil
		{
			get
			{
				return this._perfil;
			}
			set
			{
				this._perfil = value;
				this._modified = true;
			}
		}
		#endregion 

		#region SenhaUsuarioFilialPerfil
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo obrigatório.
		/// </summary>
		public string SenhaUsuarioFilialPerfil
		{
			get
			{
				return this._senhaUsuarioFilialPerfil;
			}
			set
			{
				this._senhaUsuarioFilialPerfil = value;
				this._modified = true;
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

		#endregion

		#region Construtores
		public UsuarioFilialPerfil()
		{
		}
		public UsuarioFilialPerfil(int idUsuarioFilialPerfil)
		{
			this._idUsuarioFilialPerfil = idUsuarioFilialPerfil;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Usuario_Filial_Perfil (Idf_Filial, Des_IP, Dta_Cadastro, Flg_Inativo, Idf_Pessoa_Fisica, Idf_Perfil, Sen_Usuario_Filial_Perfil, Dta_Alteracao) VALUES (@Idf_Filial, @Des_IP, @Dta_Cadastro, @Flg_Inativo, @Idf_Pessoa_Fisica, @Idf_Perfil, @Sen_Usuario_Filial_Perfil, @Dta_Alteracao);SET @Idf_Usuario_Filial_Perfil = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Usuario_Filial_Perfil SET Idf_Filial = @Idf_Filial, Des_IP = @Des_IP, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica, Idf_Perfil = @Idf_Perfil, Sen_Usuario_Filial_Perfil = @Sen_Usuario_Filial_Perfil, Dta_Alteracao = @Dta_Alteracao WHERE Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil";
		private const string SPDELETE = "DELETE FROM TAB_Usuario_Filial_Perfil WHERE Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil";
		private const string SPSELECTID = "SELECT * FROM TAB_Usuario_Filial_Perfil WHERE Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil";
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
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_IP", SqlDbType.Char, 15));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Sen_Usuario_Filial_Perfil", SqlDbType.VarChar, 10));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idUsuarioFilialPerfil;

			if (this._filial != null)
				parms[1].Value = this._filial.IdFilial;
			else
				parms[1].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoIP))
				parms[2].Value = this._descricaoIP;
			else
				parms[2].Value = DBNull.Value;

			parms[4].Value = this._flagInativo;
			parms[5].Value = this._pessoaFisica.IdPessoaFisica;

			if (this._perfil != null)
				parms[6].Value = this._perfil.IdPerfil;
			else
				parms[6].Value = DBNull.Value;

			parms[7].Value = this._senhaUsuarioFilialPerfil;

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
			this._dataAlteracao = DateTime.Now;
			parms[8].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de UsuarioFilialPerfil no banco de dados.
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
						this._idUsuarioFilialPerfil = Convert.ToInt32(cmd.Parameters["@Idf_Usuario_Filial_Perfil"].Value);
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
		/// Método utilizado para inserir uma instância de UsuarioFilialPerfil no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idUsuarioFilialPerfil = Convert.ToInt32(cmd.Parameters["@Idf_Usuario_Filial_Perfil"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de UsuarioFilialPerfil no banco de dados.
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
		/// Método utilizado para atualizar uma instância de UsuarioFilialPerfil no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de UsuarioFilialPerfil no banco de dados.
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
		/// Método utilizado para salvar uma instância de UsuarioFilialPerfil no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de UsuarioFilialPerfil no banco de dados.
		/// </summary>
		/// <param name="idUsuarioFilialPerfil">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idUsuarioFilialPerfil)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioFilialPerfil;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de UsuarioFilialPerfil no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUsuarioFilialPerfil">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idUsuarioFilialPerfil, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioFilialPerfil;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de UsuarioFilialPerfil no banco de dados.
		/// </summary>
		/// <param name="idUsuarioFilialPerfil">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idUsuarioFilialPerfil)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Usuario_Filial_Perfil where Idf_Usuario_Filial_Perfil in (";

			for (int i = 0; i < idUsuarioFilialPerfil.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idUsuarioFilialPerfil[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idUsuarioFilialPerfil">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idUsuarioFilialPerfil)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioFilialPerfil;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUsuarioFilialPerfil">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idUsuarioFilialPerfil, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioFilialPerfil;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Usu.Idf_Usuario_Filial_Perfil, Usu.Idf_Filial, Usu.Des_IP, Usu.Dta_Cadastro, Usu.Flg_Inativo, Usu.Idf_Pessoa_Fisica, Usu.Idf_Perfil, Usu.Sen_Usuario_Filial_Perfil, Usu.Dta_Alteracao FROM TAB_Usuario_Filial_Perfil Usu";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de UsuarioFilialPerfil a partir do banco de dados.
		/// </summary>
		/// <param name="idUsuarioFilialPerfil">Chave do registro.</param>
		/// <returns>Instância de UsuarioFilialPerfil.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static UsuarioFilialPerfil LoadObject(int idUsuarioFilialPerfil)
		{
			using (IDataReader dr = LoadDataReader(idUsuarioFilialPerfil))
			{
				UsuarioFilialPerfil objUsuarioFilialPerfil = new UsuarioFilialPerfil();
				if (SetInstance(dr, objUsuarioFilialPerfil))
					return objUsuarioFilialPerfil;
			}
			throw (new RecordNotFoundException(typeof(UsuarioFilialPerfil)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de UsuarioFilialPerfil a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUsuarioFilialPerfil">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de UsuarioFilialPerfil.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static UsuarioFilialPerfil LoadObject(int idUsuarioFilialPerfil, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idUsuarioFilialPerfil, trans))
			{
				UsuarioFilialPerfil objUsuarioFilialPerfil = new UsuarioFilialPerfil();
				if (SetInstance(dr, objUsuarioFilialPerfil))
					return objUsuarioFilialPerfil;
			}
			throw (new RecordNotFoundException(typeof(UsuarioFilialPerfil)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de UsuarioFilialPerfil a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idUsuarioFilialPerfil))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de UsuarioFilialPerfil a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idUsuarioFilialPerfil, trans))
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
		/// <param name="objUsuarioFilialPerfil">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, UsuarioFilialPerfil objUsuarioFilialPerfil)
		{
			try
			{
				if (dr.Read())
				{
					objUsuarioFilialPerfil._idUsuarioFilialPerfil = Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]);
					if (dr["Idf_Filial"] != DBNull.Value)
						objUsuarioFilialPerfil._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
					if (dr["Des_IP"] != DBNull.Value)
						objUsuarioFilialPerfil._descricaoIP = Convert.ToString(dr["Des_IP"]);
					objUsuarioFilialPerfil._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objUsuarioFilialPerfil._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objUsuarioFilialPerfil._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
					if (dr["Idf_Perfil"] != DBNull.Value)
						objUsuarioFilialPerfil._perfil = new Perfil(Convert.ToInt32(dr["Idf_Perfil"]));
					objUsuarioFilialPerfil._senhaUsuarioFilialPerfil = Convert.ToString(dr["Sen_Usuario_Filial_Perfil"]);
					objUsuarioFilialPerfil._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);

					objUsuarioFilialPerfil._persisted = true;
					objUsuarioFilialPerfil._modified = false;

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