//-- Data: 05/11/2010 10:41
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class UsuarioFilial // Tabela: BNE_Usuario_Filial
	{
		#region Atributos
		private int _idUsuarioFilial;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private Funcao _funcao;
		private string _descricaoFuncao;
		private string _numeroRamal;
		private string _numeroDDDComercial;
		private string _numeroComercial;
		private string _emailComercial;
		private DateTime? _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdUsuarioFilial
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdUsuarioFilial
		{
			get
			{
				return this._idUsuarioFilial;
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

		#region DescricaoFuncao
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoFuncao
		{
			get
			{
				return this._descricaoFuncao;
			}
			set
			{
				this._descricaoFuncao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroRamal
		/// <summary>
		/// Tamanho do campo: 4.
		/// Campo opcional.
		/// </summary>
		public string NumeroRamal
		{
			get
			{
				return this._numeroRamal;
			}
			set
			{
				this._numeroRamal = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDDDComercial
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo opcional.
		/// </summary>
		public string NumeroDDDComercial
		{
			get
			{
				return this._numeroDDDComercial;
			}
			set
			{
				this._numeroDDDComercial = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroComercial
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string NumeroComercial
		{
			get
			{
				return this._numeroComercial;
			}
			set
			{
				this._numeroComercial = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailComercial
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string EmailComercial
		{
			get
			{
				return this._emailComercial;
			}
			set
			{
				this._emailComercial = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public UsuarioFilial()
		{
		}
		public UsuarioFilial(int idUsuarioFilial)
		{
			this._idUsuarioFilial = idUsuarioFilial;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Usuario_Filial (Idf_Usuario_Filial_Perfil, Idf_Funcao, Des_Funcao, Num_Ramal, Num_DDD_Comercial, Num_Comercial, Eml_Comercial, Dta_Cadastro) VALUES (@Idf_Usuario_Filial_Perfil, @Idf_Funcao, @Des_Funcao, @Num_Ramal, @Num_DDD_Comercial, @Num_Comercial, @Eml_Comercial, @Dta_Cadastro);SET @Idf_Usuario_Filial = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Usuario_Filial SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Funcao = @Idf_Funcao, Des_Funcao = @Des_Funcao, Num_Ramal = @Num_Ramal, Num_DDD_Comercial = @Num_DDD_Comercial, Num_Comercial = @Num_Comercial, Eml_Comercial = @Eml_Comercial, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Usuario_Filial = @Idf_Usuario_Filial";
		private const string SPDELETE = "DELETE FROM BNE_Usuario_Filial WHERE Idf_Usuario_Filial = @Idf_Usuario_Filial";
		private const string SPSELECTID = "SELECT * FROM BNE_Usuario_Filial WHERE Idf_Usuario_Filial = @Idf_Usuario_Filial";
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
			parms.Add(new SqlParameter("@Idf_Usuario_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Funcao", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Num_Ramal", SqlDbType.Char, 4));
			parms.Add(new SqlParameter("@Num_DDD_Comercial", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Comercial", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Eml_Comercial", SqlDbType.VarChar, 100));
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
			parms[0].Value = this._idUsuarioFilial;
			parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;

			if (this._funcao != null)
				parms[2].Value = this._funcao.IdFuncao;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoFuncao))
				parms[3].Value = this._descricaoFuncao;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroRamal))
				parms[4].Value = this._numeroRamal;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroDDDComercial))
				parms[5].Value = this._numeroDDDComercial;
			else
				parms[5].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroComercial))
				parms[6].Value = this._numeroComercial;
			else
				parms[6].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._emailComercial))
				parms[7].Value = this._emailComercial;
			else
				parms[7].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[8].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de UsuarioFilial no banco de dados.
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
						this._idUsuarioFilial = Convert.ToInt32(cmd.Parameters["@Idf_Usuario_Filial"].Value);
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
		/// Método utilizado para inserir uma instância de UsuarioFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idUsuarioFilial = Convert.ToInt32(cmd.Parameters["@Idf_Usuario_Filial"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de UsuarioFilial no banco de dados.
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
		/// Método utilizado para atualizar uma instância de UsuarioFilial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de UsuarioFilial no banco de dados.
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
		/// Método utilizado para salvar uma instância de UsuarioFilial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de UsuarioFilial no banco de dados.
		/// </summary>
		/// <param name="idUsuarioFilial">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idUsuarioFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_Filial", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioFilial;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de UsuarioFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUsuarioFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idUsuarioFilial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_Filial", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioFilial;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de UsuarioFilial no banco de dados.
		/// </summary>
		/// <param name="idUsuarioFilial">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idUsuarioFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Usuario_Filial where Idf_Usuario_Filial in (";

			for (int i = 0; i < idUsuarioFilial.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idUsuarioFilial[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idUsuarioFilial">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idUsuarioFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_Filial", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioFilial;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUsuarioFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idUsuarioFilial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_Filial", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioFilial;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Usu.Idf_Usuario_Filial, Usu.Idf_Usuario_Filial_Perfil, Usu.Idf_Funcao, Usu.Des_Funcao, Usu.Num_Ramal, Usu.Num_DDD_Comercial, Usu.Num_Comercial, Usu.Eml_Comercial, Usu.Dta_Cadastro FROM BNE_Usuario_Filial Usu";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de UsuarioFilial a partir do banco de dados.
		/// </summary>
		/// <param name="idUsuarioFilial">Chave do registro.</param>
		/// <returns>Instância de UsuarioFilial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static UsuarioFilial LoadObject(int idUsuarioFilial)
		{
			using (IDataReader dr = LoadDataReader(idUsuarioFilial))
			{
				UsuarioFilial objUsuarioFilial = new UsuarioFilial();
				if (SetInstance(dr, objUsuarioFilial))
					return objUsuarioFilial;
			}
			throw (new RecordNotFoundException(typeof(UsuarioFilial)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de UsuarioFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUsuarioFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de UsuarioFilial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static UsuarioFilial LoadObject(int idUsuarioFilial, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idUsuarioFilial, trans))
			{
				UsuarioFilial objUsuarioFilial = new UsuarioFilial();
				if (SetInstance(dr, objUsuarioFilial))
					return objUsuarioFilial;
			}
			throw (new RecordNotFoundException(typeof(UsuarioFilial)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de UsuarioFilial a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idUsuarioFilial))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de UsuarioFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idUsuarioFilial, trans))
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
		/// <param name="objUsuarioFilial">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, UsuarioFilial objUsuarioFilial)
		{
			try
			{
				if (dr.Read())
				{
					objUsuarioFilial._idUsuarioFilial = Convert.ToInt32(dr["Idf_Usuario_Filial"]);
					objUsuarioFilial._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					if (dr["Idf_Funcao"] != DBNull.Value)
						objUsuarioFilial._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
					if (dr["Des_Funcao"] != DBNull.Value)
						objUsuarioFilial._descricaoFuncao = Convert.ToString(dr["Des_Funcao"]);
					if (dr["Num_Ramal"] != DBNull.Value)
						objUsuarioFilial._numeroRamal = Convert.ToString(dr["Num_Ramal"]);
					if (dr["Num_DDD_Comercial"] != DBNull.Value)
						objUsuarioFilial._numeroDDDComercial = Convert.ToString(dr["Num_DDD_Comercial"]);
					if (dr["Num_Comercial"] != DBNull.Value)
						objUsuarioFilial._numeroComercial = Convert.ToString(dr["Num_Comercial"]);
					if (dr["Eml_Comercial"] != DBNull.Value)
						objUsuarioFilial._emailComercial = Convert.ToString(dr["Eml_Comercial"]);
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objUsuarioFilial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objUsuarioFilial._persisted = true;
					objUsuarioFilial._modified = false;

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