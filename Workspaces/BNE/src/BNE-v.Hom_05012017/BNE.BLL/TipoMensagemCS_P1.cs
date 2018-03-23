//-- Data: 09/04/2010 14:12
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TipoMensagemCS // Tabela: BNE_Tipo_Mensagem_CS
	{
		#region Atributos
		private int _idTipoMensagemCS;
		private string _descricaoTipoMensagem;
		private bool _flaginativo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTipoMensagemCS
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdTipoMensagemCS
		{
			get
			{
				return this._idTipoMensagemCS;
			}
			set
			{
				this._idTipoMensagemCS = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoTipoMensagem
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoTipoMensagem
		{
			get
			{
				return this._descricaoTipoMensagem;
			}
			set
			{
				this._descricaoTipoMensagem = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Flaginativo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool Flaginativo
		{
			get
			{
				return this._flaginativo;
			}
			set
			{
				this._flaginativo = value;
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
		public TipoMensagemCS()
		{
		}
		public TipoMensagemCS(int idTipoMensagemCS)
		{
			this._idTipoMensagemCS = idTipoMensagemCS;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Tipo_Mensagem_CS (Idf_Tipo_Mensagem_CS, Des_Tipo_Mensagem, Flg_inativo, Dta_Cadastro) VALUES (@Idf_Tipo_Mensagem_CS, @Des_Tipo_Mensagem, @Flg_inativo, @Dta_Cadastro);";
		private const string SPUPDATE = "UPDATE BNE_Tipo_Mensagem_CS SET Des_Tipo_Mensagem = @Des_Tipo_Mensagem, Flg_inativo = @Flg_inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Tipo_Mensagem_CS = @Idf_Tipo_Mensagem_CS";
		private const string SPDELETE = "DELETE FROM BNE_Tipo_Mensagem_CS WHERE Idf_Tipo_Mensagem_CS = @Idf_Tipo_Mensagem_CS";
		private const string SPSELECTID = "SELECT * FROM BNE_Tipo_Mensagem_CS WHERE Idf_Tipo_Mensagem_CS = @Idf_Tipo_Mensagem_CS";
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
			parms.Add(new SqlParameter("@Idf_Tipo_Mensagem_CS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Tipo_Mensagem", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Flg_inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idTipoMensagemCS;
			parms[1].Value = this._descricaoTipoMensagem;
			parms[2].Value = this._flaginativo;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de TipoMensagemCS no banco de dados.
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
		/// Método utilizado para inserir uma instância de TipoMensagemCS no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de TipoMensagemCS no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TipoMensagemCS no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TipoMensagemCS no banco de dados.
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
		/// Método utilizado para salvar uma instância de TipoMensagemCS no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TipoMensagemCS no banco de dados.
		/// </summary>
		/// <param name="idTipoMensagemCS">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoMensagemCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idTipoMensagemCS;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoMensagemCS no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoMensagemCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoMensagemCS, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idTipoMensagemCS;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoMensagemCS no banco de dados.
		/// </summary>
		/// <param name="idTipoMensagemCS">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idTipoMensagemCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Tipo_Mensagem_CS where Idf_Tipo_Mensagem_CS in (";

			for (int i = 0; i < idTipoMensagemCS.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTipoMensagemCS[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTipoMensagemCS">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoMensagemCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idTipoMensagemCS;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoMensagemCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoMensagemCS, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idTipoMensagemCS;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Mensagem_CS, Tip.Des_Tipo_Mensagem, Tip.Flg_inativo, Tip.Dta_Cadastro FROM BNE_Tipo_Mensagem_CS Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoMensagemCS a partir do banco de dados.
		/// </summary>
		/// <param name="idTipoMensagemCS">Chave do registro.</param>
		/// <returns>Instância de TipoMensagemCS.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoMensagemCS LoadObject(int idTipoMensagemCS)
		{
			using (IDataReader dr = LoadDataReader(idTipoMensagemCS))
			{
				TipoMensagemCS objTipoMensagemCS = new TipoMensagemCS();
				if (SetInstance(dr, objTipoMensagemCS))
					return objTipoMensagemCS;
			}
			throw (new RecordNotFoundException(typeof(TipoMensagemCS)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoMensagemCS a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoMensagemCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoMensagemCS.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoMensagemCS LoadObject(int idTipoMensagemCS, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTipoMensagemCS, trans))
			{
				TipoMensagemCS objTipoMensagemCS = new TipoMensagemCS();
				if (SetInstance(dr, objTipoMensagemCS))
					return objTipoMensagemCS;
			}
			throw (new RecordNotFoundException(typeof(TipoMensagemCS)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoMensagemCS a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoMensagemCS))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoMensagemCS a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoMensagemCS, trans))
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
		/// <param name="objTipoMensagemCS">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, TipoMensagemCS objTipoMensagemCS)
		{
			try
			{
				if (dr.Read())
				{
					objTipoMensagemCS._idTipoMensagemCS = Convert.ToInt32(dr["Idf_Tipo_Mensagem_CS"]);
					objTipoMensagemCS._descricaoTipoMensagem = Convert.ToString(dr["Des_Tipo_Mensagem"]);
					objTipoMensagemCS._flaginativo = Convert.ToBoolean(dr["Flg_inativo"]);
					objTipoMensagemCS._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objTipoMensagemCS._persisted = true;
					objTipoMensagemCS._modified = false;

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