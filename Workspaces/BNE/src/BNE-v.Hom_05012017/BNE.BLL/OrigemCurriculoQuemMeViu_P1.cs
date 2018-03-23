//-- Data: 07/04/2016 14:38
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class OrigemCurriculoQuemMeViu // Tabela: BNE_Origem_Curriculo_Quem_Me_Viu
	{
		#region Atributos
		private int _idOrigemCurriculoQuemMeViu;
		private string _descricaoOrigemCurriculoQuemMeViu;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdOrigemCurriculoQuemMeViu
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdOrigemCurriculoQuemMeViu
		{
			get
			{
				return this._idOrigemCurriculoQuemMeViu;
			}
			set
			{
				this._idOrigemCurriculoQuemMeViu = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoOrigemCurriculoQuemMeViu
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoOrigemCurriculoQuemMeViu
		{
			get
			{
				return this._descricaoOrigemCurriculoQuemMeViu;
			}
			set
			{
				this._descricaoOrigemCurriculoQuemMeViu = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public OrigemCurriculoQuemMeViu()
		{
		}
		public OrigemCurriculoQuemMeViu(int idOrigemCurriculoQuemMeViu)
		{
			this._idOrigemCurriculoQuemMeViu = idOrigemCurriculoQuemMeViu;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Origem_Curriculo_Quem_Me_Viu (Idf_Origem_Curriculo_Quem_Me_Viu, Des_Origem_Curriculo_Quem_Me_Viu) VALUES (@Idf_Origem_Curriculo_Quem_Me_Viu, @Des_Origem_Curriculo_Quem_Me_Viu);";
		private const string SPUPDATE = "UPDATE BNE_Origem_Curriculo_Quem_Me_Viu SET Des_Origem_Curriculo_Quem_Me_Viu = @Des_Origem_Curriculo_Quem_Me_Viu WHERE Idf_Origem_Curriculo_Quem_Me_Viu = @Idf_Origem_Curriculo_Quem_Me_Viu";
		private const string SPDELETE = "DELETE FROM BNE_Origem_Curriculo_Quem_Me_Viu WHERE Idf_Origem_Curriculo_Quem_Me_Viu = @Idf_Origem_Curriculo_Quem_Me_Viu";
		private const string SPSELECTID = "SELECT * FROM BNE_Origem_Curriculo_Quem_Me_Viu WITH(NOLOCK) WHERE Idf_Origem_Curriculo_Quem_Me_Viu = @Idf_Origem_Curriculo_Quem_Me_Viu";
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
			parms.Add(new SqlParameter("@Idf_Origem_Curriculo_Quem_Me_Viu", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Origem_Curriculo_Quem_Me_Viu", SqlDbType.VarChar, 50));
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
			parms[0].Value = this._idOrigemCurriculoQuemMeViu;
			parms[1].Value = this._descricaoOrigemCurriculoQuemMeViu;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de OrigemCurriculoQuemMeViu no banco de dados.
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
		/// Método utilizado para inserir uma instância de OrigemCurriculoQuemMeViu no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de OrigemCurriculoQuemMeViu no banco de dados.
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
		/// Método utilizado para atualizar uma instância de OrigemCurriculoQuemMeViu no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de OrigemCurriculoQuemMeViu no banco de dados.
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
		/// Método utilizado para salvar uma instância de OrigemCurriculoQuemMeViu no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de OrigemCurriculoQuemMeViu no banco de dados.
		/// </summary>
		/// <param name="idOrigemCurriculoQuemMeViu">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idOrigemCurriculoQuemMeViu)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Curriculo_Quem_Me_Viu", SqlDbType.Int, 4));

			parms[0].Value = idOrigemCurriculoQuemMeViu;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de OrigemCurriculoQuemMeViu no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemCurriculoQuemMeViu">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idOrigemCurriculoQuemMeViu, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Curriculo_Quem_Me_Viu", SqlDbType.Int, 4));

			parms[0].Value = idOrigemCurriculoQuemMeViu;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de OrigemCurriculoQuemMeViu no banco de dados.
		/// </summary>
		/// <param name="idOrigemCurriculoQuemMeViu">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idOrigemCurriculoQuemMeViu)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Origem_Curriculo_Quem_Me_Viu where Idf_Origem_Curriculo_Quem_Me_Viu in (";

			for (int i = 0; i < idOrigemCurriculoQuemMeViu.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idOrigemCurriculoQuemMeViu[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idOrigemCurriculoQuemMeViu">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idOrigemCurriculoQuemMeViu)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Curriculo_Quem_Me_Viu", SqlDbType.Int, 4));

			parms[0].Value = idOrigemCurriculoQuemMeViu;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemCurriculoQuemMeViu">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idOrigemCurriculoQuemMeViu, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Curriculo_Quem_Me_Viu", SqlDbType.Int, 4));

			parms[0].Value = idOrigemCurriculoQuemMeViu;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ori.Idf_Origem_Curriculo_Quem_Me_Viu, Ori.Des_Origem_Curriculo_Quem_Me_Viu FROM BNE_Origem_Curriculo_Quem_Me_Viu Ori";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de OrigemCurriculoQuemMeViu a partir do banco de dados.
		/// </summary>
		/// <param name="idOrigemCurriculoQuemMeViu">Chave do registro.</param>
		/// <returns>Instância de OrigemCurriculoQuemMeViu.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static OrigemCurriculoQuemMeViu LoadObject(int idOrigemCurriculoQuemMeViu)
		{
			using (IDataReader dr = LoadDataReader(idOrigemCurriculoQuemMeViu))
			{
				OrigemCurriculoQuemMeViu objOrigemCurriculoQuemMeViu = new OrigemCurriculoQuemMeViu();
				if (SetInstance(dr, objOrigemCurriculoQuemMeViu))
					return objOrigemCurriculoQuemMeViu;
			}
			throw (new RecordNotFoundException(typeof(OrigemCurriculoQuemMeViu)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de OrigemCurriculoQuemMeViu a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemCurriculoQuemMeViu">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de OrigemCurriculoQuemMeViu.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static OrigemCurriculoQuemMeViu LoadObject(int idOrigemCurriculoQuemMeViu, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idOrigemCurriculoQuemMeViu, trans))
			{
				OrigemCurriculoQuemMeViu objOrigemCurriculoQuemMeViu = new OrigemCurriculoQuemMeViu();
				if (SetInstance(dr, objOrigemCurriculoQuemMeViu))
					return objOrigemCurriculoQuemMeViu;
			}
			throw (new RecordNotFoundException(typeof(OrigemCurriculoQuemMeViu)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de OrigemCurriculoQuemMeViu a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idOrigemCurriculoQuemMeViu))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de OrigemCurriculoQuemMeViu a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idOrigemCurriculoQuemMeViu, trans))
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
		/// <param name="objOrigemCurriculoQuemMeViu">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, OrigemCurriculoQuemMeViu objOrigemCurriculoQuemMeViu, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objOrigemCurriculoQuemMeViu._idOrigemCurriculoQuemMeViu = Convert.ToInt32(dr["Idf_Origem_Curriculo_Quem_Me_Viu"]);
					objOrigemCurriculoQuemMeViu._descricaoOrigemCurriculoQuemMeViu = Convert.ToString(dr["Des_Origem_Curriculo_Quem_Me_Viu"]);

					objOrigemCurriculoQuemMeViu._persisted = true;
					objOrigemCurriculoQuemMeViu._modified = false;

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