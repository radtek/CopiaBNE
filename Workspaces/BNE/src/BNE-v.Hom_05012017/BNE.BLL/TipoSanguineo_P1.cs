//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TipoSanguineo // Tabela: plataforma.TAB_Tipo_Sanguineo
	{
		#region Atributos
		private int _idTipoSanguineo;
		private string _descricaoTipoSanguineo;
		private bool _flagInativo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTipoSanguineo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdTipoSanguineo
		{
			get
			{
				return this._idTipoSanguineo;
			}
			set
			{
				this._idTipoSanguineo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoTipoSanguineo
		/// <summary>
		/// Tamanho do campo: 3.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoTipoSanguineo
		{
			get
			{
				return this._descricaoTipoSanguineo;
			}
			set
			{
				this._descricaoTipoSanguineo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagInativo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagInativo
		{
			get
			{
				return this._flagInativo;
			}
			set
			{
				this._flagInativo = value;
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

		#endregion

		#region Construtores
		public TipoSanguineo()
		{
		}
		public TipoSanguineo(int idTipoSanguineo)
		{
			this._idTipoSanguineo = idTipoSanguineo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Tipo_Sanguineo (Idf_Tipo_Sanguineo, Des_Tipo_Sanguineo, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Tipo_Sanguineo, @Des_Tipo_Sanguineo, @Flg_Inativo, @Dta_Cadastro);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Tipo_Sanguineo SET Des_Tipo_Sanguineo = @Des_Tipo_Sanguineo, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Tipo_Sanguineo = @Idf_Tipo_Sanguineo";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Tipo_Sanguineo WHERE Idf_Tipo_Sanguineo = @Idf_Tipo_Sanguineo";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Tipo_Sanguineo WHERE Idf_Tipo_Sanguineo = @Idf_Tipo_Sanguineo";
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
			parms.Add(new SqlParameter("@Idf_Tipo_Sanguineo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Tipo_Sanguineo", SqlDbType.Char, 3));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idTipoSanguineo;
			parms[1].Value = this._descricaoTipoSanguineo;
			parms[2].Value = this._flagInativo;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de TipoSanguineo no banco de dados.
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
		/// Método utilizado para inserir uma instância de TipoSanguineo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de TipoSanguineo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TipoSanguineo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TipoSanguineo no banco de dados.
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
		/// Método utilizado para salvar uma instância de TipoSanguineo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TipoSanguineo no banco de dados.
		/// </summary>
		/// <param name="idTipoSanguineo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoSanguineo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Sanguineo", SqlDbType.Int, 4));

			parms[0].Value = idTipoSanguineo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoSanguineo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoSanguineo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoSanguineo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Sanguineo", SqlDbType.Int, 4));

			parms[0].Value = idTipoSanguineo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoSanguineo no banco de dados.
		/// </summary>
		/// <param name="idTipoSanguineo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idTipoSanguineo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Tipo_Sanguineo where Idf_Tipo_Sanguineo in (";

			for (int i = 0; i < idTipoSanguineo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTipoSanguineo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTipoSanguineo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoSanguineo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Sanguineo", SqlDbType.Int, 4));

			parms[0].Value = idTipoSanguineo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoSanguineo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoSanguineo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Sanguineo", SqlDbType.Int, 4));

			parms[0].Value = idTipoSanguineo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Sanguineo, Tip.Des_Tipo_Sanguineo, Tip.Flg_Inativo, Tip.Dta_Cadastro FROM plataforma.TAB_Tipo_Sanguineo Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoSanguineo a partir do banco de dados.
		/// </summary>
		/// <param name="idTipoSanguineo">Chave do registro.</param>
		/// <returns>Instância de TipoSanguineo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoSanguineo LoadObject(int idTipoSanguineo)
		{
			using (IDataReader dr = LoadDataReader(idTipoSanguineo))
			{
				TipoSanguineo objTipoSanguineo = new TipoSanguineo();
				if (SetInstance(dr, objTipoSanguineo))
					return objTipoSanguineo;
			}
			throw (new RecordNotFoundException(typeof(TipoSanguineo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoSanguineo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoSanguineo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoSanguineo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoSanguineo LoadObject(int idTipoSanguineo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTipoSanguineo, trans))
			{
				TipoSanguineo objTipoSanguineo = new TipoSanguineo();
				if (SetInstance(dr, objTipoSanguineo))
					return objTipoSanguineo;
			}
			throw (new RecordNotFoundException(typeof(TipoSanguineo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoSanguineo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoSanguineo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoSanguineo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoSanguineo, trans))
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
		/// <param name="objTipoSanguineo">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, TipoSanguineo objTipoSanguineo)
		{
			try
			{
				if (dr.Read())
				{
					objTipoSanguineo._idTipoSanguineo = Convert.ToInt32(dr["Idf_Tipo_Sanguineo"]);
					objTipoSanguineo._descricaoTipoSanguineo = Convert.ToString(dr["Des_Tipo_Sanguineo"]);
					objTipoSanguineo._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objTipoSanguineo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objTipoSanguineo._persisted = true;
					objTipoSanguineo._modified = false;

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