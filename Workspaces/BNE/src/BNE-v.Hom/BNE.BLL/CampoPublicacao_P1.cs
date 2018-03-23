//-- Data: 23/05/2013 14:02
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CampoPublicacao // Tabela: BNE_Campo_Publicacao
	{
		#region Atributos
		private int _idCampoPublicacao;
		private string _descricaoCampoPublicacao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCampoPublicacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCampoPublicacao
		{
			get
			{
				return this._idCampoPublicacao;
			}
			set
			{
				this._idCampoPublicacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCampoPublicacao
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoCampoPublicacao
		{
			get
			{
				return this._descricaoCampoPublicacao;
			}
			set
			{
				this._descricaoCampoPublicacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CampoPublicacao()
		{
		}
		public CampoPublicacao(int idCampoPublicacao)
		{
			this._idCampoPublicacao = idCampoPublicacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Campo_Publicacao (Idf_Campo_Publicacao, Des_Campo_Publicacao) VALUES (@Idf_Campo_Publicacao, @Des_Campo_Publicacao);";
		private const string SPUPDATE = "UPDATE BNE_Campo_Publicacao SET Des_Campo_Publicacao = @Des_Campo_Publicacao WHERE Idf_Campo_Publicacao = @Idf_Campo_Publicacao";
		private const string SPDELETE = "DELETE FROM BNE_Campo_Publicacao WHERE Idf_Campo_Publicacao = @Idf_Campo_Publicacao";
		private const string SPSELECTID = "SELECT * FROM BNE_Campo_Publicacao WITH(NOLOCK) WHERE Idf_Campo_Publicacao = @Idf_Campo_Publicacao";
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
			parms.Add(new SqlParameter("@Idf_Campo_Publicacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Campo_Publicacao", SqlDbType.VarChar, 50));
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
			parms[0].Value = this._idCampoPublicacao;
			parms[1].Value = this._descricaoCampoPublicacao;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CampoPublicacao no banco de dados.
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
		/// Método utilizado para inserir uma instância de CampoPublicacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CampoPublicacao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CampoPublicacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CampoPublicacao no banco de dados.
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
		/// Método utilizado para salvar uma instância de CampoPublicacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CampoPublicacao no banco de dados.
		/// </summary>
		/// <param name="idCampoPublicacao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCampoPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idCampoPublicacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CampoPublicacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampoPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCampoPublicacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idCampoPublicacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CampoPublicacao no banco de dados.
		/// </summary>
		/// <param name="idCampoPublicacao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCampoPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Campo_Publicacao where Idf_Campo_Publicacao in (";

			for (int i = 0; i < idCampoPublicacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCampoPublicacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCampoPublicacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCampoPublicacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idCampoPublicacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampoPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCampoPublicacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Publicacao", SqlDbType.Int, 4));

			parms[0].Value = idCampoPublicacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cam.Idf_Campo_Publicacao, Cam.Des_Campo_Publicacao FROM BNE_Campo_Publicacao Cam";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CampoPublicacao a partir do banco de dados.
		/// </summary>
		/// <param name="idCampoPublicacao">Chave do registro.</param>
		/// <returns>Instância de CampoPublicacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CampoPublicacao LoadObject(int idCampoPublicacao)
		{
			using (IDataReader dr = LoadDataReader(idCampoPublicacao))
			{
				CampoPublicacao objCampoPublicacao = new CampoPublicacao();
				if (SetInstance(dr, objCampoPublicacao))
					return objCampoPublicacao;
			}
			throw (new RecordNotFoundException(typeof(CampoPublicacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CampoPublicacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampoPublicacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CampoPublicacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CampoPublicacao LoadObject(int idCampoPublicacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCampoPublicacao, trans))
			{
				CampoPublicacao objCampoPublicacao = new CampoPublicacao();
				if (SetInstance(dr, objCampoPublicacao))
					return objCampoPublicacao;
			}
			throw (new RecordNotFoundException(typeof(CampoPublicacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CampoPublicacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCampoPublicacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CampoPublicacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCampoPublicacao, trans))
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
		/// <param name="objCampoPublicacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CampoPublicacao objCampoPublicacao)
		{
			try
			{
				if (dr.Read())
				{
					objCampoPublicacao._idCampoPublicacao = Convert.ToInt32(dr["Idf_Campo_Publicacao"]);
					objCampoPublicacao._descricaoCampoPublicacao = Convert.ToString(dr["Des_Campo_Publicacao"]);

					objCampoPublicacao._persisted = true;
					objCampoPublicacao._modified = false;

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