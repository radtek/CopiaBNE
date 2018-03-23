//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CurriculoBusca // Tabela: BNE_Curriculo_Busca
	{
		#region Atributos
		private int _idCurriculoBusca;
		private string _descricaoBusca;
		private DateTime _dataCadastro;
		private int _idUsuarioFilialPerfil;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCurriculoBusca
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCurriculoBusca
		{
			get
			{
				return this._idCurriculoBusca;
			}
		}
		#endregion 

		#region DescricaoBusca
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoBusca
		{
			get
			{
				return this._descricaoBusca;
			}
			set
			{
				this._descricaoBusca = value;
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

		#region IdUsuarioFilialPerfil
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdUsuarioFilialPerfil
		{
			get
			{
				return this._idUsuarioFilialPerfil;
			}
			set
			{
				this._idUsuarioFilialPerfil = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CurriculoBusca()
		{
		}
		public CurriculoBusca(int idCurriculoBusca)
		{
			this._idCurriculoBusca = idCurriculoBusca;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Curriculo_Busca (Des_Busca, Dta_Cadastro, Idf_Usuario_Filial_Perfil) VALUES (@Des_Busca, @Dta_Cadastro, @Idf_Usuario_Filial_Perfil);SET @Idf_Curriculo_Busca = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Curriculo_Busca SET Des_Busca = @Des_Busca, Dta_Cadastro = @Dta_Cadastro, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil WHERE Idf_Curriculo_Busca = @Idf_Curriculo_Busca";
		private const string SPDELETE = "DELETE FROM BNE_Curriculo_Busca WHERE Idf_Curriculo_Busca = @Idf_Curriculo_Busca";
		private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Busca WHERE Idf_Curriculo_Busca = @Idf_Curriculo_Busca";
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
			parms.Add(new SqlParameter("@Idf_Curriculo_Busca", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Busca", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idCurriculoBusca;
			parms[1].Value = this._descricaoBusca;
			parms[3].Value = this._idUsuarioFilialPerfil;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CurriculoBusca no banco de dados.
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
						this._idCurriculoBusca = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Busca"].Value);
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
		/// Método utilizado para inserir uma instância de CurriculoBusca no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCurriculoBusca = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Busca"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CurriculoBusca no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CurriculoBusca no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CurriculoBusca no banco de dados.
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
		/// Método utilizado para salvar uma instância de CurriculoBusca no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CurriculoBusca no banco de dados.
		/// </summary>
		/// <param name="idCurriculoBusca">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCurriculoBusca)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Busca", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoBusca;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CurriculoBusca no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoBusca">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCurriculoBusca, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Busca", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoBusca;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CurriculoBusca no banco de dados.
		/// </summary>
		/// <param name="idCurriculoBusca">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCurriculoBusca)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Curriculo_Busca where Idf_Curriculo_Busca in (";

			for (int i = 0; i < idCurriculoBusca.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCurriculoBusca[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurriculoBusca">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCurriculoBusca)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Busca", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoBusca;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoBusca">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCurriculoBusca, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Busca", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoBusca;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo_Busca, Cur.Des_Busca, Cur.Dta_Cadastro, Cur.Idf_Usuario_Filial_Perfil FROM BNE_Curriculo_Busca Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoBusca a partir do banco de dados.
		/// </summary>
		/// <param name="idCurriculoBusca">Chave do registro.</param>
		/// <returns>Instância de CurriculoBusca.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CurriculoBusca LoadObject(int idCurriculoBusca)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoBusca))
			{
				CurriculoBusca objCurriculoBusca = new CurriculoBusca();
				if (SetInstance(dr, objCurriculoBusca))
					return objCurriculoBusca;
			}
			throw (new RecordNotFoundException(typeof(CurriculoBusca)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoBusca a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoBusca">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CurriculoBusca.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CurriculoBusca LoadObject(int idCurriculoBusca, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoBusca, trans))
			{
				CurriculoBusca objCurriculoBusca = new CurriculoBusca();
				if (SetInstance(dr, objCurriculoBusca))
					return objCurriculoBusca;
			}
			throw (new RecordNotFoundException(typeof(CurriculoBusca)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoBusca a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoBusca))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoBusca a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoBusca, trans))
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
		/// <param name="objCurriculoBusca">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CurriculoBusca objCurriculoBusca)
		{
			try
			{
				if (dr.Read())
				{
					objCurriculoBusca._idCurriculoBusca = Convert.ToInt32(dr["Idf_Curriculo_Busca"]);
					objCurriculoBusca._descricaoBusca = Convert.ToString(dr["Des_Busca"]);
					objCurriculoBusca._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objCurriculoBusca._idUsuarioFilialPerfil = Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]);

					objCurriculoBusca._persisted = true;
					objCurriculoBusca._modified = false;

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