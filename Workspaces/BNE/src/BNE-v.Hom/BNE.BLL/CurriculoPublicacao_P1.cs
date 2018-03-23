//-- Data: 28/02/2011 09:46
//-- Autor: Elias

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CurriculoPublicacao // Tabela: BNE_Curriculo_Publicacao
	{
		#region Atributos
		private int _idCurriculoPublicacao;
		private DateTime _dataCadastro;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private Curriculo _curriculo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCurriculoPublicacao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCurriculoPublicacao
		{
			get
			{
				return this._idCurriculoPublicacao;
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

		#endregion

		#region Construtores
		public CurriculoPublicacao()
		{
		}
		public CurriculoPublicacao(int idCurriculoPublicacao)
		{
			this._idCurriculoPublicacao = idCurriculoPublicacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Curriculo_Publicacao (Dta_Cadastro, Idf_Usuario_Filial_Perfil, Idf_Curriculo) VALUES (@Dta_Cadastro, @Idf_Usuario_Filial_Perfil, @Idf_Curriculo);SET @Idf_Curriculo_Publicacao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Curriculo_Publicacao SET Dta_Cadastro = @Dta_Cadastro, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Curriculo = @Idf_Curriculo WHERE Idf_Curriculo_Publicacao = @Idf_Curriculo_Publicacao";
		private const string SPDELETE = "DELETE FROM BNE_Curriculo_Publicacao WHERE Idf_Curriculo_Publicacao = @Idf_Curriculo_Publicacao";
		private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Publicacao WHERE Idf_Curriculo_Publicacao = @Idf_Curriculo_Publicacao";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Elias</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Publicacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Elias</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idCurriculoPublicacao;
			parms[2].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
			parms[3].Value = this._curriculo.IdCurriculo;

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
		/// Método utilizado para inserir uma instância de CurriculoPublicacao no banco de dados.
		/// </summary>
		/// <remarks>Elias</remarks>
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
						this._idCurriculoPublicacao = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Publicacao"].Value);
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
		/// Método utilizado para inserir uma instância de CurriculoPublicacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCurriculoPublicacao = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Publicacao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CurriculoPublicacao no banco de dados.
		/// </summary>
		/// <remarks>Elias</remarks>
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
		/// Método utilizado para atualizar uma instância de CurriculoPublicacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
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
		/// Método utilizado para salvar uma instância de CurriculoPublicacao no banco de dados.
		/// </summary>
		/// <remarks>Elias</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de CurriculoPublicacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
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
		/// Método utilizado para excluir uma instância de CurriculoPublicacao no banco de dados.
		/// </summary>
		/// <param name="idCurriculoPublicacao">Chave do registro.</param>
		/// <remarks>Elias</remarks>
		public static void Delete(int idCurriculoPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoPublicacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CurriculoPublicacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
		public static void Delete(int idCurriculoPublicacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoPublicacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CurriculoPublicacao no banco de dados.
		/// </summary>
		/// <param name="idCurriculoPublicacao">Lista de chaves.</param>
		/// <remarks>Elias</remarks>
		public static void Delete(List<int> idCurriculoPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Curriculo_Publicacao where Idf_Curriculo_Publicacao in (";

			for (int i = 0; i < idCurriculoPublicacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCurriculoPublicacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurriculoPublicacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Elias</remarks>
		private static IDataReader LoadDataReader(int idCurriculoPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoPublicacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Elias</remarks>
		private static IDataReader LoadDataReader(int idCurriculoPublicacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoPublicacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo_Publicacao, Cur.Dta_Cadastro, Cur.Idf_Usuario_Filial_Perfil, Cur.Idf_Curriculo FROM BNE_Curriculo_Publicacao Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoPublicacao a partir do banco de dados.
		/// </summary>
		/// <param name="idCurriculoPublicacao">Chave do registro.</param>
		/// <returns>Instância de CurriculoPublicacao.</returns>
		/// <remarks>Elias</remarks>
		public static CurriculoPublicacao LoadObject(int idCurriculoPublicacao)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoPublicacao))
			{
				CurriculoPublicacao objCurriculoPublicacao = new CurriculoPublicacao();
				if (SetInstance(dr, objCurriculoPublicacao))
					return objCurriculoPublicacao;
			}
			throw (new RecordNotFoundException(typeof(CurriculoPublicacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoPublicacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CurriculoPublicacao.</returns>
		/// <remarks>Elias</remarks>
		public static CurriculoPublicacao LoadObject(int idCurriculoPublicacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoPublicacao, trans))
			{
				CurriculoPublicacao objCurriculoPublicacao = new CurriculoPublicacao();
				if (SetInstance(dr, objCurriculoPublicacao))
					return objCurriculoPublicacao;
			}
			throw (new RecordNotFoundException(typeof(CurriculoPublicacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoPublicacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoPublicacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoPublicacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoPublicacao, trans))
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
		/// <param name="objCurriculoPublicacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias</remarks>
		private static bool SetInstance(IDataReader dr, CurriculoPublicacao objCurriculoPublicacao)
		{
			try
			{
				if (dr.Read())
				{
					objCurriculoPublicacao._idCurriculoPublicacao = Convert.ToInt32(dr["Idf_Curriculo_Publicacao"]);
					objCurriculoPublicacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objCurriculoPublicacao._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					objCurriculoPublicacao._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));

					objCurriculoPublicacao._persisted = true;
					objCurriculoPublicacao._modified = false;

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