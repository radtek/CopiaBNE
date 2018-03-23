//-- Data: 21/01/2014 10:33
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL.CloudTag
{
	public partial class PalavraFuncaoVaga // Tabela: TAB_Palavra_Funcao_Vaga
	{
		#region Atributos
		private int _idPalavraFuncaovaga;
		private int? _idFuncao;
		private int? _idVaga;
		private string _descricaoPalavra;
		private string _nomeFuncao;
		private bool? _flgsine;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPalavraFuncaovaga
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPalavraFuncaovaga
		{
			get
			{
				return this._idPalavraFuncaovaga;
			}
		}
		#endregion 

		#region IdFuncao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? IdFuncao
		{
			get
			{
				return this._idFuncao;
			}
			set
			{
				this._idFuncao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdVaga
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? IdVaga
		{
			get
			{
				return this._idVaga;
			}
			set
			{
				this._idVaga = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPalavra
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo opcional.
		/// </summary>
		public string DescricaoPalavra
		{
			get
			{
				return this._descricaoPalavra;
			}
			set
			{
				this._descricaoPalavra = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeFuncao
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo opcional.
		/// </summary>
		public string NomeFuncao
		{
			get
			{
				return this._nomeFuncao;
			}
			set
			{
				this._nomeFuncao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Flgsine
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? Flgsine
		{
			get
			{
				return this._flgsine;
			}
			set
			{
				this._flgsine = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PalavraFuncaoVaga()
		{
		}
		public PalavraFuncaoVaga(int idPalavraFuncaovaga)
		{
			this._idPalavraFuncaovaga = idPalavraFuncaovaga;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Palavra_Funcao_Vaga (Idf_Funcao, Idf_Vaga, Des_Palavra, Nme_Funcao, flg_sine) VALUES (@Idf_Funcao, @Idf_Vaga, @Des_Palavra, @Nme_Funcao, @flg_sine);SET @Idf_Palavra_Funcao_vaga = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Palavra_Funcao_Vaga SET Idf_Funcao = @Idf_Funcao, Idf_Vaga = @Idf_Vaga, Des_Palavra = @Des_Palavra, Nme_Funcao = @Nme_Funcao, flg_sine = @flg_sine WHERE Idf_Palavra_Funcao_vaga = @Idf_Palavra_Funcao_vaga";
		private const string SPDELETE = "DELETE FROM TAB_Palavra_Funcao_Vaga WHERE Idf_Palavra_Funcao_vaga = @Idf_Palavra_Funcao_vaga";
		private const string SPSELECTID = "SELECT * FROM TAB_Palavra_Funcao_Vaga WITH(NOLOCK) WHERE Idf_Palavra_Funcao_vaga = @Idf_Palavra_Funcao_vaga";
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
			parms.Add(new SqlParameter("@Idf_Palavra_Funcao_vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Palavra", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@Nme_Funcao", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@flg_sine", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idPalavraFuncaovaga;

			if (this._idFuncao.HasValue)
				parms[1].Value = this._idFuncao;
			else
				parms[1].Value = DBNull.Value;


			if (this._idVaga.HasValue)
				parms[2].Value = this._idVaga;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoPalavra))
				parms[3].Value = this._descricaoPalavra;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._nomeFuncao))
				parms[4].Value = this._nomeFuncao;
			else
				parms[4].Value = DBNull.Value;


			if (this._flgsine.HasValue)
				parms[5].Value = this._flgsine;
			else
				parms[5].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de PalavraFuncaoVaga no banco de dados.
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
						this._idPalavraFuncaovaga = Convert.ToInt32(cmd.Parameters["@Idf_Palavra_Funcao_vaga"].Value);
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
		/// Método utilizado para inserir uma instância de PalavraFuncaoVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPalavraFuncaovaga = Convert.ToInt32(cmd.Parameters["@Idf_Palavra_Funcao_vaga"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PalavraFuncaoVaga no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PalavraFuncaoVaga no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PalavraFuncaoVaga no banco de dados.
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
		/// Método utilizado para salvar uma instância de PalavraFuncaoVaga no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PalavraFuncaoVaga no banco de dados.
		/// </summary>
		/// <param name="idPalavraFuncaovaga">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPalavraFuncaovaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Palavra_Funcao_vaga", SqlDbType.Int, 4));

			parms[0].Value = idPalavraFuncaovaga;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PalavraFuncaoVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPalavraFuncaovaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPalavraFuncaovaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Palavra_Funcao_vaga", SqlDbType.Int, 4));

			parms[0].Value = idPalavraFuncaovaga;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PalavraFuncaoVaga no banco de dados.
		/// </summary>
		/// <param name="idPalavraFuncaovaga">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPalavraFuncaovaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Palavra_Funcao_Vaga where Idf_Palavra_Funcao_vaga in (";

			for (int i = 0; i < idPalavraFuncaovaga.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPalavraFuncaovaga[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPalavraFuncaovaga">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPalavraFuncaovaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Palavra_Funcao_vaga", SqlDbType.Int, 4));

			parms[0].Value = idPalavraFuncaovaga;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPalavraFuncaovaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPalavraFuncaovaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Palavra_Funcao_vaga", SqlDbType.Int, 4));

			parms[0].Value = idPalavraFuncaovaga;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pal.Idf_Palavra_Funcao_vaga, Pal.Idf_Funcao, Pal.Idf_Vaga, Pal.Des_Palavra, Pal.Nme_Funcao, Pal.flg_sine FROM TAB_Palavra_Funcao_Vaga Pal";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PalavraFuncaoVaga a partir do banco de dados.
		/// </summary>
		/// <param name="idPalavraFuncaovaga">Chave do registro.</param>
		/// <returns>Instância de PalavraFuncaoVaga.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PalavraFuncaoVaga LoadObject(int idPalavraFuncaovaga)
		{
			using (IDataReader dr = LoadDataReader(idPalavraFuncaovaga))
			{
				PalavraFuncaoVaga objPalavraFuncaoVaga = new PalavraFuncaoVaga();
				if (SetInstance(dr, objPalavraFuncaoVaga))
					return objPalavraFuncaoVaga;
			}
			throw (new RecordNotFoundException(typeof(PalavraFuncaoVaga)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PalavraFuncaoVaga a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPalavraFuncaovaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PalavraFuncaoVaga.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PalavraFuncaoVaga LoadObject(int idPalavraFuncaovaga, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPalavraFuncaovaga, trans))
			{
				PalavraFuncaoVaga objPalavraFuncaoVaga = new PalavraFuncaoVaga();
				if (SetInstance(dr, objPalavraFuncaoVaga))
					return objPalavraFuncaoVaga;
			}
			throw (new RecordNotFoundException(typeof(PalavraFuncaoVaga)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PalavraFuncaoVaga a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPalavraFuncaovaga))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PalavraFuncaoVaga a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPalavraFuncaovaga, trans))
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
		/// <param name="objPalavraFuncaoVaga">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PalavraFuncaoVaga objPalavraFuncaoVaga)
		{
			try
			{
				if (dr.Read())
				{
					objPalavraFuncaoVaga._idPalavraFuncaovaga = Convert.ToInt32(dr["Idf_Palavra_Funcao_vaga"]);
					if (dr["Idf_Funcao"] != DBNull.Value)
						objPalavraFuncaoVaga._idFuncao = Convert.ToInt32(dr["Idf_Funcao"]);
					if (dr["Idf_Vaga"] != DBNull.Value)
						objPalavraFuncaoVaga._idVaga = Convert.ToInt32(dr["Idf_Vaga"]);
					if (dr["Des_Palavra"] != DBNull.Value)
						objPalavraFuncaoVaga._descricaoPalavra = Convert.ToString(dr["Des_Palavra"]);
					if (dr["Nme_Funcao"] != DBNull.Value)
						objPalavraFuncaoVaga._nomeFuncao = Convert.ToString(dr["Nme_Funcao"]);
					if (dr["flg_sine"] != DBNull.Value)
						objPalavraFuncaoVaga._flgsine = Convert.ToBoolean(dr["flg_sine"]);

					objPalavraFuncaoVaga._persisted = true;
					objPalavraFuncaoVaga._modified = false;

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