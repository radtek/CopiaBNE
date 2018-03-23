//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PlanoTipo // Tabela: BNE_Plano_Tipo
	{
		#region Atributos
		private int _idPlanoTipo;
		private string _descricaoPlanoTipo;
		private DateTime _dataCadasrto;
		private bool _flagImportado;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPlanoTipo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdPlanoTipo
		{
			get
			{
				return this._idPlanoTipo;
			}
			set
			{
				this._idPlanoTipo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPlanoTipo
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoPlanoTipo
		{
			get
			{
				return this._descricaoPlanoTipo;
			}
			set
			{
				this._descricaoPlanoTipo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCadasrto
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataCadasrto
		{
			get
			{
				return this._dataCadasrto;
			}
			set
			{
				this._dataCadasrto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagImportado
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagImportado
		{
			get
			{
				return this._flagImportado;
			}
			set
			{
				this._flagImportado = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PlanoTipo()
		{
		}
		public PlanoTipo(int idPlanoTipo)
		{
			this._idPlanoTipo = idPlanoTipo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Plano_Tipo (Idf_Plano_Tipo, Des_Plano_Tipo, Dta_Cadasrto, Flg_Importado) VALUES (@Idf_Plano_Tipo, @Des_Plano_Tipo, @Dta_Cadasrto, @Flg_Importado);";
		private const string SPUPDATE = "UPDATE BNE_Plano_Tipo SET Des_Plano_Tipo = @Des_Plano_Tipo, Dta_Cadasrto = @Dta_Cadasrto, Flg_Importado = @Flg_Importado WHERE Idf_Plano_Tipo = @Idf_Plano_Tipo";
		private const string SPDELETE = "DELETE FROM BNE_Plano_Tipo WHERE Idf_Plano_Tipo = @Idf_Plano_Tipo";
		private const string SPSELECTID = "SELECT * FROM BNE_Plano_Tipo WHERE Idf_Plano_Tipo = @Idf_Plano_Tipo";
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
			parms.Add(new SqlParameter("@Idf_Plano_Tipo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Plano_Tipo", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Dta_Cadasrto", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Importado", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idPlanoTipo;
			parms[1].Value = this._descricaoPlanoTipo;
			parms[2].Value = this._dataCadasrto;
			parms[3].Value = this._flagImportado;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PlanoTipo no banco de dados.
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
		/// Método utilizado para inserir uma instância de PlanoTipo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de PlanoTipo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PlanoTipo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PlanoTipo no banco de dados.
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
		/// Método utilizado para salvar uma instância de PlanoTipo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PlanoTipo no banco de dados.
		/// </summary>
		/// <param name="idPlanoTipo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPlanoTipo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Tipo", SqlDbType.Int, 4));

			parms[0].Value = idPlanoTipo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PlanoTipo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlanoTipo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPlanoTipo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Tipo", SqlDbType.Int, 4));

			parms[0].Value = idPlanoTipo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PlanoTipo no banco de dados.
		/// </summary>
		/// <param name="idPlanoTipo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPlanoTipo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Plano_Tipo where Idf_Plano_Tipo in (";

			for (int i = 0; i < idPlanoTipo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPlanoTipo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPlanoTipo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPlanoTipo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Tipo", SqlDbType.Int, 4));

			parms[0].Value = idPlanoTipo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlanoTipo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPlanoTipo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Tipo", SqlDbType.Int, 4));

			parms[0].Value = idPlanoTipo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Tipo, Pla.Des_Plano_Tipo, Pla.Dta_Cadasrto, Pla.Flg_Importado FROM BNE_Plano_Tipo Pla";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PlanoTipo a partir do banco de dados.
		/// </summary>
		/// <param name="idPlanoTipo">Chave do registro.</param>
		/// <returns>Instância de PlanoTipo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PlanoTipo LoadObject(int idPlanoTipo)
		{
			using (IDataReader dr = LoadDataReader(idPlanoTipo))
			{
				PlanoTipo objPlanoTipo = new PlanoTipo();
				if (SetInstance(dr, objPlanoTipo))
					return objPlanoTipo;
			}
			throw (new RecordNotFoundException(typeof(PlanoTipo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PlanoTipo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlanoTipo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PlanoTipo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PlanoTipo LoadObject(int idPlanoTipo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPlanoTipo, trans))
			{
				PlanoTipo objPlanoTipo = new PlanoTipo();
				if (SetInstance(dr, objPlanoTipo))
					return objPlanoTipo;
			}
			throw (new RecordNotFoundException(typeof(PlanoTipo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PlanoTipo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPlanoTipo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PlanoTipo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPlanoTipo, trans))
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
		/// <param name="objPlanoTipo">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PlanoTipo objPlanoTipo)
		{
			try
			{
				if (dr.Read())
				{
					objPlanoTipo._idPlanoTipo = Convert.ToInt32(dr["Idf_Plano_Tipo"]);
					objPlanoTipo._descricaoPlanoTipo = Convert.ToString(dr["Des_Plano_Tipo"]);
					objPlanoTipo._dataCadasrto = Convert.ToDateTime(dr["Dta_Cadasrto"]);
					objPlanoTipo._flagImportado = Convert.ToBoolean(dr["Flg_Importado"]);

					objPlanoTipo._persisted = true;
					objPlanoTipo._modified = false;

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