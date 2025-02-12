//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class EstadoCivil // Tabela: plataforma.TAB_Estado_Civil
	{
		#region Atributos
		private int _idEstadoCivil;
		private string _descricaoEstadoCivil;
		private bool _flagInativo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdEstadoCivil
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdEstadoCivil
		{
			get
			{
				return this._idEstadoCivil;
			}
			set
			{
				this._idEstadoCivil = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoEstadoCivil
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoEstadoCivil
		{
			get
			{
				return this._descricaoEstadoCivil;
			}
			set
			{
				this._descricaoEstadoCivil = value;
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
		public EstadoCivil()
		{
		}
		public EstadoCivil(int idEstadoCivil)
		{
			this._idEstadoCivil = idEstadoCivil;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Estado_Civil (Idf_Estado_Civil, Des_Estado_Civil, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Estado_Civil, @Des_Estado_Civil, @Flg_Inativo, @Dta_Cadastro);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Estado_Civil SET Des_Estado_Civil = @Des_Estado_Civil, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Estado_Civil = @Idf_Estado_Civil";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Estado_Civil WHERE Idf_Estado_Civil = @Idf_Estado_Civil";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Estado_Civil WHERE Idf_Estado_Civil = @Idf_Estado_Civil";
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
			parms.Add(new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Estado_Civil", SqlDbType.VarChar, 50));
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
			parms[0].Value = this._idEstadoCivil;
			parms[1].Value = this._descricaoEstadoCivil;
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
		/// Método utilizado para inserir uma instância de EstadoCivil no banco de dados.
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
		/// Método utilizado para inserir uma instância de EstadoCivil no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de EstadoCivil no banco de dados.
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
		/// Método utilizado para atualizar uma instância de EstadoCivil no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de EstadoCivil no banco de dados.
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
		/// Método utilizado para salvar uma instância de EstadoCivil no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de EstadoCivil no banco de dados.
		/// </summary>
		/// <param name="idEstadoCivil">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEstadoCivil)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4));

			parms[0].Value = idEstadoCivil;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de EstadoCivil no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEstadoCivil">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEstadoCivil, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4));

			parms[0].Value = idEstadoCivil;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de EstadoCivil no banco de dados.
		/// </summary>
		/// <param name="idEstadoCivil">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idEstadoCivil)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Estado_Civil where Idf_Estado_Civil in (";

			for (int i = 0; i < idEstadoCivil.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idEstadoCivil[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idEstadoCivil">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEstadoCivil)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4));

			parms[0].Value = idEstadoCivil;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEstadoCivil">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEstadoCivil, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4));

			parms[0].Value = idEstadoCivil;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Est.Idf_Estado_Civil, Est.Des_Estado_Civil, Est.Flg_Inativo, Est.Dta_Cadastro FROM plataforma.TAB_Estado_Civil Est";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objEstadoCivil">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, EstadoCivil objEstadoCivil)
		{
			try
			{
				if (dr.Read())
				{
					objEstadoCivil._idEstadoCivil = Convert.ToInt32(dr["Idf_Estado_Civil"]);
					objEstadoCivil._descricaoEstadoCivil = Convert.ToString(dr["Des_Estado_Civil"]);
					objEstadoCivil._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objEstadoCivil._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objEstadoCivil._persisted = true;
					objEstadoCivil._modified = false;

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