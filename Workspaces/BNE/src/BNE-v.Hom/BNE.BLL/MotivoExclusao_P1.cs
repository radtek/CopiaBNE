using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BNE.EL;
using System.Data.SqlClient;
using System.Data;

namespace BNE.BLL
{
    public partial class MotivoExclusao // Tabela: BNE.BNE_Motivo_Exclusao
    {
        #region Atributos
		private int _idMotivoExclusao;
		private string _descricaoMotivoExclusao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdMotivoExclusao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdMotivoExclusao
		{
			get
			{
				return this._idMotivoExclusao;
			}
			set
			{
				this._idMotivoExclusao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMotivoExclusao
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoMotivoExclusao
		{
			get
			{
				return this._descricaoMotivoExclusao;
			}
			set
			{
				this._descricaoMotivoExclusao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public MotivoExclusao()
		{
		}
        public MotivoExclusao(int idMotivoExclusao)
		{
			this._idMotivoExclusao = idMotivoExclusao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE.BNE_Motivo_Exclusao (Idf_Motivo_Exclusao, Des_Motivo_Exclusao) VALUES (@Idf_Motivo_Exclusao, @Des_Motivo_Exclusao);";
		private const string SPUPDATE = "UPDATE BNE.BNE_Motivo_Exclusao SET Des_Motivo_Exclusao = @Des_Motivo_Exclusao WHERE Idf_Motivo_Exclusao = @Idf_Motivo_Exclusao";
		private const string SPDELETE = "DELETE FROM BNE.BNE_Motivo_Exclusao WHERE Idf_Motivo_Exclusao = @Idf_Motivo_Exclusao";
		private const string SPSELECTID = "SELECT * FROM BNE.BNE_Motivo_Exclusao WITH(NOLOCK) WHERE Idf_Motivo_Exclusao = @Idf_Motivo_Exclusao";
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
			parms.Add(new SqlParameter("@Idf_Motivo_Exclusao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Motivo_Exclusao", SqlDbType.VarChar, 100));
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
			parms[0].Value = this._idMotivoExclusao;
			parms[1].Value = this._descricaoMotivoExclusao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de MotivoExclusao no banco de dados.
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
		/// Método utilizado para inserir uma instância de MotivoExclusao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de MotivoExclusao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de MotivoExclusao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de MotivoExclusao no banco de dados.
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
		/// Método utilizado para salvar uma instância de MotivoExclusao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de MotivoExclusao no banco de dados.
		/// </summary>
		/// <param name="idMotivoExclusao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMotivoExclusao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Exclusao", SqlDbType.Int, 4));

			parms[0].Value = idMotivoExclusao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de MotivoExclusao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoExclusao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMotivoExclusao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Exclusao", SqlDbType.Int, 4));

			parms[0].Value = idMotivoExclusao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de MotivoExclusao no banco de dados.
		/// </summary>
		/// <param name="idMotivoExclusao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idMotivoExclusao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE.BNE_Motivo_Exclusao where Idf_Motivo_Exclusao in (";

			for (int i = 0; i < idMotivoExclusao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idMotivoExclusao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idMotivoExclusao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMotivoExclusao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Exclusao", SqlDbType.Int, 4));

			parms[0].Value = idMotivoExclusao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoExclusao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMotivoExclusao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Motivo_Exclusao", SqlDbType.Int, 4));

			parms[0].Value = idMotivoExclusao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Mot.Idf_Motivo_Exclusao, Mot.Des_Motivo_Exclusao FROM BNE.BNE_Motivo_Exclusao Mot";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de MotivoExclusao a partir do banco de dados.
		/// </summary>
		/// <param name="idMotivoExclusao">Chave do registro.</param>
		/// <returns>Instância de MotivoExclusao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static MotivoExclusao LoadObject(int idMotivoExclusao)
		{
			using (IDataReader dr = LoadDataReader(idMotivoExclusao))
			{
				MotivoExclusao objMotivoExclusao = new MotivoExclusao();
				if (SetInstance(dr, objMotivoExclusao))
					return objMotivoExclusao;
			}
			throw (new RecordNotFoundException(typeof(MotivoExclusao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de MotivoExclusao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMotivoExclusao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de MotivoExclusao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static MotivoExclusao LoadObject(int idMotivoExclusao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idMotivoExclusao, trans))
			{
				MotivoExclusao objMotivoExclusao = new MotivoExclusao();
				if (SetInstance(dr, objMotivoExclusao))
					return objMotivoExclusao;
			}
			throw (new RecordNotFoundException(typeof(MotivoExclusao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de MotivoExclusao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idMotivoExclusao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de MotivoExclusao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idMotivoExclusao, trans))
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
		/// <param name="objMotivoExclusao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, MotivoExclusao objMotivoExclusao)
		{
			try
			{
				if (dr.Read())
				{
					objMotivoExclusao._idMotivoExclusao = Convert.ToInt32(dr["Idf_Motivo_Exclusao"]);
					objMotivoExclusao._descricaoMotivoExclusao = Convert.ToString(dr["Des_Motivo_Exclusao"]);

					objMotivoExclusao._persisted = true;
					objMotivoExclusao._modified = false;

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
