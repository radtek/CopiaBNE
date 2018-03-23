//-- Data: 24/08/2017 12:28
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class SistemaFolhaPgtoPesquisa // Tabela: BNE_Sistema_Folha_Pgto_Pesquisa
	{
		#region Atributos
		private int _idSistemaFolhaPgtoPesquisa;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private SistemaFolhaPgto _sistemaFolhaPgto;
		private string _descricaoSistemaFolhaPgtoPesquisa;
		private DateTime? _dataCadastro;
		private bool _flagResposta;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdSistemaFolhaPgtoPesquisa
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdSistemaFolhaPgtoPesquisa
		{
			get
			{
				return this._idSistemaFolhaPgtoPesquisa;
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

		#region SistemaFolhaPgto
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public SistemaFolhaPgto SistemaFolhaPgto
		{
			get
			{
				return this._sistemaFolhaPgto;
			}
			set
			{
				this._sistemaFolhaPgto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoSistemaFolhaPgtoPesquisa
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoSistemaFolhaPgtoPesquisa
		{
			get
			{
				return this._descricaoSistemaFolhaPgtoPesquisa;
			}
			set
			{
				this._descricaoSistemaFolhaPgtoPesquisa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCadastro
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataCadastro
		{
			get
			{
				return this._dataCadastro;
			}
		}
		#endregion 

		#region FlagResposta
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagResposta
		{
			get
			{
				return this._flagResposta;
			}
			set
			{
				this._flagResposta = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public SistemaFolhaPgtoPesquisa()
		{
		}
		public SistemaFolhaPgtoPesquisa(int idSistemaFolhaPgtoPesquisa)
		{
			this._idSistemaFolhaPgtoPesquisa = idSistemaFolhaPgtoPesquisa;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Sistema_Folha_Pgto_Pesquisa (Idf_Usuario_Filial_Perfil, Idf_Sistema_Folha_Pgto, Des_Sistema_Folha_Pgto_Pesquisa, Dta_Cadastro, Flg_Resposta) VALUES (@Idf_Usuario_Filial_Perfil, @Idf_Sistema_Folha_Pgto, @Des_Sistema_Folha_Pgto_Pesquisa, @Dta_Cadastro, @Flg_Resposta);SET @Idf_Sistema_Folha_Pgto_Pesquisa = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Sistema_Folha_Pgto_Pesquisa SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Sistema_Folha_Pgto = @Idf_Sistema_Folha_Pgto, Des_Sistema_Folha_Pgto_Pesquisa = @Des_Sistema_Folha_Pgto_Pesquisa, Dta_Cadastro = @Dta_Cadastro, Flg_Resposta = @Flg_Resposta WHERE Idf_Sistema_Folha_Pgto_Pesquisa = @Idf_Sistema_Folha_Pgto_Pesquisa";
		private const string SPDELETE = "DELETE FROM BNE_Sistema_Folha_Pgto_Pesquisa WHERE Idf_Sistema_Folha_Pgto_Pesquisa = @Idf_Sistema_Folha_Pgto_Pesquisa";
		private const string SPSELECTID = "SELECT * FROM BNE_Sistema_Folha_Pgto_Pesquisa WITH(NOLOCK) WHERE Idf_Sistema_Folha_Pgto_Pesquisa = @Idf_Sistema_Folha_Pgto_Pesquisa";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Mailson</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Sistema_Folha_Pgto_Pesquisa", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Sistema_Folha_Pgto", SqlDbType.Int, 1));
			parms.Add(new SqlParameter("@Des_Sistema_Folha_Pgto_Pesquisa", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Resposta", SqlDbType.Bit, 1));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Mailson</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idSistemaFolhaPgtoPesquisa;
			parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;

			if (this._sistemaFolhaPgto != null)
				parms[2].Value = this._sistemaFolhaPgto.IdSistemaFolhaPgto;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoSistemaFolhaPgtoPesquisa))
				parms[3].Value = this._descricaoSistemaFolhaPgtoPesquisa;
			else
				parms[3].Value = DBNull.Value;

			parms[5].Value = this._flagResposta;

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
		/// Método utilizado para inserir uma instância de SistemaFolhaPgtoPesquisa no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
						this._idSistemaFolhaPgtoPesquisa = Convert.ToInt32(cmd.Parameters["@Idf_Sistema_Folha_Pgto_Pesquisa"].Value);
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
		/// Método utilizado para inserir uma instância de SistemaFolhaPgtoPesquisa no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idSistemaFolhaPgtoPesquisa = Convert.ToInt32(cmd.Parameters["@Idf_Sistema_Folha_Pgto_Pesquisa"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de SistemaFolhaPgtoPesquisa no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para atualizar uma instância de SistemaFolhaPgtoPesquisa no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para salvar uma instância de SistemaFolhaPgtoPesquisa no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de SistemaFolhaPgtoPesquisa no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para excluir uma instância de SistemaFolhaPgtoPesquisa no banco de dados.
		/// </summary>
		/// <param name="idSistemaFolhaPgtoPesquisa">Chave do registro.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idSistemaFolhaPgtoPesquisa)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Sistema_Folha_Pgto_Pesquisa", SqlDbType.Int, 4));

			parms[0].Value = idSistemaFolhaPgtoPesquisa;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de SistemaFolhaPgtoPesquisa no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSistemaFolhaPgtoPesquisa">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idSistemaFolhaPgtoPesquisa, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Sistema_Folha_Pgto_Pesquisa", SqlDbType.Int, 4));

			parms[0].Value = idSistemaFolhaPgtoPesquisa;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de SistemaFolhaPgtoPesquisa no banco de dados.
		/// </summary>
		/// <param name="idSistemaFolhaPgtoPesquisa">Lista de chaves.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(List<int> idSistemaFolhaPgtoPesquisa)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Sistema_Folha_Pgto_Pesquisa where Idf_Sistema_Folha_Pgto_Pesquisa in (";

			for (int i = 0; i < idSistemaFolhaPgtoPesquisa.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idSistemaFolhaPgtoPesquisa[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idSistemaFolhaPgtoPesquisa">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idSistemaFolhaPgtoPesquisa)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Sistema_Folha_Pgto_Pesquisa", SqlDbType.Int, 4));

			parms[0].Value = idSistemaFolhaPgtoPesquisa;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSistemaFolhaPgtoPesquisa">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idSistemaFolhaPgtoPesquisa, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Sistema_Folha_Pgto_Pesquisa", SqlDbType.Int, 4));

			parms[0].Value = idSistemaFolhaPgtoPesquisa;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sis.Idf_Sistema_Folha_Pgto_Pesquisa, Sis.Idf_Usuario_Filial_Perfil, Sis.Idf_Sistema_Folha_Pgto, Sis.Des_Sistema_Folha_Pgto_Pesquisa, Sis.Dta_Cadastro, Sis.Flg_Resposta FROM BNE_Sistema_Folha_Pgto_Pesquisa Sis";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de SistemaFolhaPgtoPesquisa a partir do banco de dados.
		/// </summary>
		/// <param name="idSistemaFolhaPgtoPesquisa">Chave do registro.</param>
		/// <returns>Instância de SistemaFolhaPgtoPesquisa.</returns>
		/// <remarks>Mailson</remarks>
		public static SistemaFolhaPgtoPesquisa LoadObject(int idSistemaFolhaPgtoPesquisa)
		{
			using (IDataReader dr = LoadDataReader(idSistemaFolhaPgtoPesquisa))
			{
				SistemaFolhaPgtoPesquisa objSistemaFolhaPgtoPesquisa = new SistemaFolhaPgtoPesquisa();
				if (SetInstance(dr, objSistemaFolhaPgtoPesquisa))
					return objSistemaFolhaPgtoPesquisa;
			}
			throw (new RecordNotFoundException(typeof(SistemaFolhaPgtoPesquisa)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de SistemaFolhaPgtoPesquisa a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSistemaFolhaPgtoPesquisa">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de SistemaFolhaPgtoPesquisa.</returns>
		/// <remarks>Mailson</remarks>
		public static SistemaFolhaPgtoPesquisa LoadObject(int idSistemaFolhaPgtoPesquisa, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idSistemaFolhaPgtoPesquisa, trans))
			{
				SistemaFolhaPgtoPesquisa objSistemaFolhaPgtoPesquisa = new SistemaFolhaPgtoPesquisa();
				if (SetInstance(dr, objSistemaFolhaPgtoPesquisa))
					return objSistemaFolhaPgtoPesquisa;
			}
			throw (new RecordNotFoundException(typeof(SistemaFolhaPgtoPesquisa)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de SistemaFolhaPgtoPesquisa a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idSistemaFolhaPgtoPesquisa))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de SistemaFolhaPgtoPesquisa a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idSistemaFolhaPgtoPesquisa, trans))
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
		/// <param name="objSistemaFolhaPgtoPesquisa">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, SistemaFolhaPgtoPesquisa objSistemaFolhaPgtoPesquisa, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objSistemaFolhaPgtoPesquisa._idSistemaFolhaPgtoPesquisa = Convert.ToInt32(dr["Idf_Sistema_Folha_Pgto_Pesquisa"]);
					objSistemaFolhaPgtoPesquisa._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					if (dr["Idf_Sistema_Folha_Pgto"] != DBNull.Value)
						objSistemaFolhaPgtoPesquisa._sistemaFolhaPgto = new SistemaFolhaPgto(Convert.ToInt16(dr["Idf_Sistema_Folha_Pgto"]));
					if (dr["Des_Sistema_Folha_Pgto_Pesquisa"] != DBNull.Value)
						objSistemaFolhaPgtoPesquisa._descricaoSistemaFolhaPgtoPesquisa = Convert.ToString(dr["Des_Sistema_Folha_Pgto_Pesquisa"]);
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objSistemaFolhaPgtoPesquisa._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objSistemaFolhaPgtoPesquisa._flagResposta = Convert.ToBoolean(dr["Flg_Resposta"]);

					objSistemaFolhaPgtoPesquisa._persisted = true;
					objSistemaFolhaPgtoPesquisa._modified = false;

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
				if (dispose)
					dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}