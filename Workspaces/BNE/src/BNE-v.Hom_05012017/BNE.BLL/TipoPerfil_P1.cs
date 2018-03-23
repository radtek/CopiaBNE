//-- Data: 04/11/2010 14:03
//-- Autor: Bruno Flammarion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TipoPerfil // Tabela: BNE_Tipo_Perfil
	{
		#region Atributos
		private int _idTipoPerfil;
		private string _descricaoTipoPerfil;
		private DateTime _dataCadastro;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTipoPerfil
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdTipoPerfil
		{
			get
			{
				return this._idTipoPerfil;
			}
		}
		#endregion 

		#region DescricaoTipoPerfil
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoTipoPerfil
		{
			get
			{
				return this._descricaoTipoPerfil;
			}
			set
			{
				this._descricaoTipoPerfil = value;
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

		#endregion

		#region Construtores
		public TipoPerfil()
		{
		}
		public TipoPerfil(int idTipoPerfil)
		{
			this._idTipoPerfil = idTipoPerfil;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Tipo_Perfil (Des_Tipo_Perfil, Dta_Cadastro, Flg_Inativo) VALUES (@Des_Tipo_Perfil, @Dta_Cadastro, @Flg_Inativo);SET @Idf_Tipo_Perfil = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Tipo_Perfil SET Des_Tipo_Perfil = @Des_Tipo_Perfil, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Tipo_Perfil = @Idf_Tipo_Perfil";
		private const string SPDELETE = "DELETE FROM BNE_Tipo_Perfil WHERE Idf_Tipo_Perfil = @Idf_Tipo_Perfil";
		private const string SPSELECTID = "SELECT * FROM BNE_Tipo_Perfil WHERE Idf_Tipo_Perfil = @Idf_Tipo_Perfil";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Tipo_Perfil", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Bruno Flammarion</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idTipoPerfil;
			parms[1].Value = this._descricaoTipoPerfil;
			parms[3].Value = this._flagInativo;

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
		/// Método utilizado para inserir uma instância de TipoPerfil no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
						this._idTipoPerfil = Convert.ToInt32(cmd.Parameters["@Idf_Tipo_Perfil"].Value);
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
		/// Método utilizado para inserir uma instância de TipoPerfil no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idTipoPerfil = Convert.ToInt32(cmd.Parameters["@Idf_Tipo_Perfil"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de TipoPerfil no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para atualizar uma instância de TipoPerfil no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para salvar uma instância de TipoPerfil no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de TipoPerfil no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para excluir uma instância de TipoPerfil no banco de dados.
		/// </summary>
		/// <param name="idTipoPerfil">Chave do registro.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idTipoPerfil)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Perfil", SqlDbType.Int, 4));

			parms[0].Value = idTipoPerfil;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoPerfil no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoPerfil">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idTipoPerfil, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Perfil", SqlDbType.Int, 4));

			parms[0].Value = idTipoPerfil;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoPerfil no banco de dados.
		/// </summary>
		/// <param name="idTipoPerfil">Lista de chaves.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(List<int> idTipoPerfil)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Tipo_Perfil where Idf_Tipo_Perfil in (";

			for (int i = 0; i < idTipoPerfil.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTipoPerfil[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTipoPerfil">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idTipoPerfil)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Perfil", SqlDbType.Int, 4));

			parms[0].Value = idTipoPerfil;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoPerfil">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idTipoPerfil, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Perfil", SqlDbType.Int, 4));

			parms[0].Value = idTipoPerfil;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Perfil, Tip.Des_Tipo_Perfil, Tip.Dta_Cadastro, Tip.Flg_Inativo FROM BNE_Tipo_Perfil Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoPerfil a partir do banco de dados.
		/// </summary>
		/// <param name="idTipoPerfil">Chave do registro.</param>
		/// <returns>Instância de TipoPerfil.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static TipoPerfil LoadObject(int idTipoPerfil)
		{
			using (IDataReader dr = LoadDataReader(idTipoPerfil))
			{
				TipoPerfil objTipoPerfil = new TipoPerfil();
				if (SetInstance(dr, objTipoPerfil))
					return objTipoPerfil;
			}
			throw (new RecordNotFoundException(typeof(TipoPerfil)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoPerfil a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoPerfil">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoPerfil.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static TipoPerfil LoadObject(int idTipoPerfil, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTipoPerfil, trans))
			{
				TipoPerfil objTipoPerfil = new TipoPerfil();
				if (SetInstance(dr, objTipoPerfil))
					return objTipoPerfil;
			}
			throw (new RecordNotFoundException(typeof(TipoPerfil)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoPerfil a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoPerfil))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoPerfil a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoPerfil, trans))
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
		/// <param name="objTipoPerfil">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static bool SetInstance(IDataReader dr, TipoPerfil objTipoPerfil)
		{
			try
			{
				if (dr.Read())
				{
					objTipoPerfil._idTipoPerfil = Convert.ToInt32(dr["Idf_Tipo_Perfil"]);
					objTipoPerfil._descricaoTipoPerfil = Convert.ToString(dr["Des_Tipo_Perfil"]);
					objTipoPerfil._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objTipoPerfil._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objTipoPerfil._persisted = true;
					objTipoPerfil._modified = false;

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