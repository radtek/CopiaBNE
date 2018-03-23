//-- Data: 24/06/2013 16:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class ConsultorR1 // Tabela: BNE_Consultor_R1
	{
		#region Atributos
		private int _idConsultorR1;
		private string _nomeConsultor;
		private string _descricaoEmail;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdConsultorR1
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdConsultorR1
		{
			get
			{
				return this._idConsultorR1;
			}
		}
		#endregion 

		#region NomeConsultor
		/// <summary>
		/// Tamanho do campo: 150.
		/// Campo obrigatório.
		/// </summary>
		public string NomeConsultor
		{
			get
			{
				return this._nomeConsultor;
			}
			set
			{
				this._nomeConsultor = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoEmail
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoEmail
		{
			get
			{
				return this._descricaoEmail;
			}
			set
			{
				this._descricaoEmail = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public ConsultorR1()
		{
		}
		public ConsultorR1(int idConsultorR1)
		{
			this._idConsultorR1 = idConsultorR1;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Consultor_R1 (Nme_Consultor, Des_Email) VALUES (@Nme_Consultor, @Des_Email);SET @Idf_Consultor_R1 = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Consultor_R1 SET Nme_Consultor = @Nme_Consultor, Des_Email = @Des_Email WHERE Idf_Consultor_R1 = @Idf_Consultor_R1";
		private const string SPDELETE = "DELETE FROM BNE_Consultor_R1 WHERE Idf_Consultor_R1 = @Idf_Consultor_R1";
		private const string SPSELECTID = "SELECT * FROM BNE_Consultor_R1 WHERE Idf_Consultor_R1 = @Idf_Consultor_R1";
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
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Consultor", SqlDbType.VarChar, 150));
			parms.Add(new SqlParameter("@Des_Email", SqlDbType.VarChar, 100));
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
			parms[0].Value = this._idConsultorR1;
			parms[1].Value = this._nomeConsultor;
			parms[2].Value = this._descricaoEmail;

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
		/// Método utilizado para inserir uma instância de ConsultorR1 no banco de dados.
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
						this._idConsultorR1 = Convert.ToInt32(cmd.Parameters["@Idf_Consultor_R1"].Value);
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
		/// Método utilizado para inserir uma instância de ConsultorR1 no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idConsultorR1 = Convert.ToInt32(cmd.Parameters["@Idf_Consultor_R1"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de ConsultorR1 no banco de dados.
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
		/// Método utilizado para atualizar uma instância de ConsultorR1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de ConsultorR1 no banco de dados.
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
		/// Método utilizado para salvar uma instância de ConsultorR1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de ConsultorR1 no banco de dados.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idConsultorR1)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));

			parms[0].Value = idConsultorR1;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de ConsultorR1 no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idConsultorR1, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));

			parms[0].Value = idConsultorR1;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de ConsultorR1 no banco de dados.
		/// </summary>
		/// <param name="idConsultorR1">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idConsultorR1)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Consultor_R1 where Idf_Consultor_R1 in (";

			for (int i = 0; i < idConsultorR1.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idConsultorR1[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idConsultorR1)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));

			parms[0].Value = idConsultorR1;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idConsultorR1, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));

			parms[0].Value = idConsultorR1;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Con.Idf_Consultor_R1, Con.Nme_Consultor, Con.Des_Email FROM BNE_Consultor_R1 Con";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de ConsultorR1 a partir do banco de dados.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <returns>Instância de ConsultorR1.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ConsultorR1 LoadObject(int idConsultorR1)
		{
			using (IDataReader dr = LoadDataReader(idConsultorR1))
			{
				ConsultorR1 objConsultorR1 = new ConsultorR1();
				if (SetInstance(dr, objConsultorR1))
					return objConsultorR1;
			}
			throw (new RecordNotFoundException(typeof(ConsultorR1)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de ConsultorR1 a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de ConsultorR1.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ConsultorR1 LoadObject(int idConsultorR1, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idConsultorR1, trans))
			{
				ConsultorR1 objConsultorR1 = new ConsultorR1();
				if (SetInstance(dr, objConsultorR1))
					return objConsultorR1;
			}
			throw (new RecordNotFoundException(typeof(ConsultorR1)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de ConsultorR1 a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idConsultorR1))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de ConsultorR1 a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idConsultorR1, trans))
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
		/// <param name="objConsultorR1">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, ConsultorR1 objConsultorR1)
		{
			try
			{
				if (dr.Read())
				{
					objConsultorR1._idConsultorR1 = Convert.ToInt32(dr["Idf_Consultor_R1"]);
					objConsultorR1._nomeConsultor = Convert.ToString(dr["Nme_Consultor"]);
					objConsultorR1._descricaoEmail = Convert.ToString(dr["Des_Email"]);

					objConsultorR1._persisted = true;
					objConsultorR1._modified = false;

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