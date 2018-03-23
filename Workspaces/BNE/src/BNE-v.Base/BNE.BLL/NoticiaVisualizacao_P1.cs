//-- Data: 02/02/2011 11:06
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class NoticiaVisualizacao // Tabela: BNE_Noticia_Visualizacao
	{
		#region Atributos
		private int _idNoticiaVisualizacao;
		private Noticia _noticia;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private DateTime? _dataVisualizacao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdNoticiaVisualizacao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdNoticiaVisualizacao
		{
			get
			{
				return this._idNoticiaVisualizacao;
			}
		}
		#endregion 

		#region Noticia
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Noticia Noticia
		{
			get
			{
				return this._noticia;
			}
			set
			{
				this._noticia = value;
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

		#region DataVisualizacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataVisualizacao
		{
			get
			{
				return this._dataVisualizacao;
			}
			set
			{
				this._dataVisualizacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public NoticiaVisualizacao()
		{
		}
		public NoticiaVisualizacao(int idNoticiaVisualizacao)
		{
			this._idNoticiaVisualizacao = idNoticiaVisualizacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Noticia_Visualizacao (Idf_Noticia, Idf_Usuario_Filial_Perfil, Dta_Visualizacao) VALUES (@Idf_Noticia, @Idf_Usuario_Filial_Perfil, @Dta_Visualizacao);SET @Idf_Noticia_Visualizacao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Noticia_Visualizacao SET Idf_Noticia = @Idf_Noticia, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Dta_Visualizacao = @Dta_Visualizacao WHERE Idf_Noticia_Visualizacao = @Idf_Noticia_Visualizacao";
		private const string SPDELETE = "DELETE FROM BNE_Noticia_Visualizacao WHERE Idf_Noticia_Visualizacao = @Idf_Noticia_Visualizacao";
		private const string SPSELECTID = "SELECT * FROM BNE_Noticia_Visualizacao WHERE Idf_Noticia_Visualizacao = @Idf_Noticia_Visualizacao";
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
			parms.Add(new SqlParameter("@Idf_Noticia_Visualizacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Noticia", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Visualizacao", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idNoticiaVisualizacao;
			parms[1].Value = this._noticia.IdNoticia;
			parms[2].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;

			if (this._dataVisualizacao.HasValue)
				parms[3].Value = this._dataVisualizacao;
			else
				parms[3].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de NoticiaVisualizacao no banco de dados.
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
						this._idNoticiaVisualizacao = Convert.ToInt32(cmd.Parameters["@Idf_Noticia_Visualizacao"].Value);
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
		/// Método utilizado para inserir uma instância de NoticiaVisualizacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idNoticiaVisualizacao = Convert.ToInt32(cmd.Parameters["@Idf_Noticia_Visualizacao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de NoticiaVisualizacao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de NoticiaVisualizacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de NoticiaVisualizacao no banco de dados.
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
		/// Método utilizado para salvar uma instância de NoticiaVisualizacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de NoticiaVisualizacao no banco de dados.
		/// </summary>
		/// <param name="idNoticiaVisualizacao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idNoticiaVisualizacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Noticia_Visualizacao", SqlDbType.Int, 4));

			parms[0].Value = idNoticiaVisualizacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de NoticiaVisualizacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNoticiaVisualizacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idNoticiaVisualizacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Noticia_Visualizacao", SqlDbType.Int, 4));

			parms[0].Value = idNoticiaVisualizacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de NoticiaVisualizacao no banco de dados.
		/// </summary>
		/// <param name="idNoticiaVisualizacao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idNoticiaVisualizacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Noticia_Visualizacao where Idf_Noticia_Visualizacao in (";

			for (int i = 0; i < idNoticiaVisualizacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idNoticiaVisualizacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idNoticiaVisualizacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idNoticiaVisualizacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Noticia_Visualizacao", SqlDbType.Int, 4));

			parms[0].Value = idNoticiaVisualizacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNoticiaVisualizacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idNoticiaVisualizacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Noticia_Visualizacao", SqlDbType.Int, 4));

			parms[0].Value = idNoticiaVisualizacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Not.Idf_Noticia_Visualizacao, Not.Idf_Noticia, Not.Idf_Usuario_Filial_Perfil, Not.Dta_Visualizacao FROM BNE_Noticia_Visualizacao Not";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de NoticiaVisualizacao a partir do banco de dados.
		/// </summary>
		/// <param name="idNoticiaVisualizacao">Chave do registro.</param>
		/// <returns>Instância de NoticiaVisualizacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static NoticiaVisualizacao LoadObject(int idNoticiaVisualizacao)
		{
			using (IDataReader dr = LoadDataReader(idNoticiaVisualizacao))
			{
				NoticiaVisualizacao objNoticiaVisualizacao = new NoticiaVisualizacao();
				if (SetInstance(dr, objNoticiaVisualizacao))
					return objNoticiaVisualizacao;
			}
			throw (new RecordNotFoundException(typeof(NoticiaVisualizacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de NoticiaVisualizacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNoticiaVisualizacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de NoticiaVisualizacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static NoticiaVisualizacao LoadObject(int idNoticiaVisualizacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idNoticiaVisualizacao, trans))
			{
				NoticiaVisualizacao objNoticiaVisualizacao = new NoticiaVisualizacao();
				if (SetInstance(dr, objNoticiaVisualizacao))
					return objNoticiaVisualizacao;
			}
			throw (new RecordNotFoundException(typeof(NoticiaVisualizacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de NoticiaVisualizacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idNoticiaVisualizacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de NoticiaVisualizacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idNoticiaVisualizacao, trans))
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
		/// <param name="objNoticiaVisualizacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, NoticiaVisualizacao objNoticiaVisualizacao)
		{
			try
			{
				if (dr.Read())
				{
					objNoticiaVisualizacao._idNoticiaVisualizacao = Convert.ToInt32(dr["Idf_Noticia_Visualizacao"]);
					objNoticiaVisualizacao._noticia = new Noticia(Convert.ToInt32(dr["Idf_Noticia"]));
					objNoticiaVisualizacao._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					if (dr["Dta_Visualizacao"] != DBNull.Value)
						objNoticiaVisualizacao._dataVisualizacao = Convert.ToDateTime(dr["Dta_Visualizacao"]);

					objNoticiaVisualizacao._persisted = true;
					objNoticiaVisualizacao._modified = false;

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