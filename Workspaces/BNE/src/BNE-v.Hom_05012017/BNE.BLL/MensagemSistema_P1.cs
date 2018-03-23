//-- Data: 25/01/2011 14:10
//-- Autor: Vinicius Maciel

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class MensagemSistema // Tabela: BNE_Mensagem_Sistema
	{
		#region Atributos
		private int _idMensagemSistema;
		private string _descricaoMensagemSistema;
		private bool _flagInativo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdMensagemSistema
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdMensagemSistema
		{
			get
			{
				return this._idMensagemSistema;
			}
		}
		#endregion 

		#region DescricaoMensagemSistema
		/// <summary>
		/// Tamanho do campo: 40.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoMensagemSistema
		{
			get
			{
				return this._descricaoMensagemSistema;
			}
			set
			{
				this._descricaoMensagemSistema = value;
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
		public MensagemSistema()
		{
		}
		public MensagemSistema(int idMensagemSistema)
		{
			this._idMensagemSistema = idMensagemSistema;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Mensagem_Sistema (Des_Mensagem_Sistema, Flg_Inativo, Dta_Cadastro) VALUES (@Des_Mensagem_Sistema, @Flg_Inativo, @Dta_Cadastro);SET @Idf_Mensagem_Sistema = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Mensagem_Sistema SET Des_Mensagem_Sistema = @Des_Mensagem_Sistema, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Mensagem_Sistema = @Idf_Mensagem_Sistema";
		private const string SPDELETE = "DELETE FROM BNE_Mensagem_Sistema WHERE Idf_Mensagem_Sistema = @Idf_Mensagem_Sistema";
		private const string SPSELECTID = "SELECT * FROM BNE_Mensagem_Sistema WHERE Idf_Mensagem_Sistema = @Idf_Mensagem_Sistema";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_Sistema", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Mensagem_Sistema", SqlDbType.VarChar, 40));
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
		/// <remarks>Vinicius Maciel</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idMensagemSistema;
			parms[1].Value = this._descricaoMensagemSistema;
			parms[2].Value = this._flagInativo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de MensagemSistema no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
						this._idMensagemSistema = Convert.ToInt32(cmd.Parameters["@Idf_Mensagem_Sistema"].Value);
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
		/// Método utilizado para inserir uma instância de MensagemSistema no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idMensagemSistema = Convert.ToInt32(cmd.Parameters["@Idf_Mensagem_Sistema"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de MensagemSistema no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para atualizar uma instância de MensagemSistema no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para salvar uma instância de MensagemSistema no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de MensagemSistema no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para excluir uma instância de MensagemSistema no banco de dados.
		/// </summary>
		/// <param name="idMensagemSistema">Chave do registro.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idMensagemSistema)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_Sistema", SqlDbType.Int, 4));

			parms[0].Value = idMensagemSistema;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de MensagemSistema no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagemSistema">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idMensagemSistema, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_Sistema", SqlDbType.Int, 4));

			parms[0].Value = idMensagemSistema;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de MensagemSistema no banco de dados.
		/// </summary>
		/// <param name="idMensagemSistema">Lista de chaves.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(List<int> idMensagemSistema)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Mensagem_Sistema where Idf_Mensagem_Sistema in (";

			for (int i = 0; i < idMensagemSistema.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idMensagemSistema[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idMensagemSistema">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idMensagemSistema)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_Sistema", SqlDbType.Int, 4));

			parms[0].Value = idMensagemSistema;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagemSistema">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idMensagemSistema, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_Sistema", SqlDbType.Int, 4));

			parms[0].Value = idMensagemSistema;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Men.Idf_Mensagem_Sistema, Men.Des_Mensagem_Sistema, Men.Flg_Inativo, Men.Dta_Cadastro FROM BNE_Mensagem_Sistema Men";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de MensagemSistema a partir do banco de dados.
		/// </summary>
		/// <param name="idMensagemSistema">Chave do registro.</param>
		/// <returns>Instância de MensagemSistema.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static MensagemSistema LoadObject(int idMensagemSistema)
		{
			using (IDataReader dr = LoadDataReader(idMensagemSistema))
			{
				MensagemSistema objMensagemSistema = new MensagemSistema();
				if (SetInstance(dr, objMensagemSistema))
					return objMensagemSistema;
			}
			throw (new RecordNotFoundException(typeof(MensagemSistema)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de MensagemSistema a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagemSistema">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de MensagemSistema.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static MensagemSistema LoadObject(int idMensagemSistema, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idMensagemSistema, trans))
			{
				MensagemSistema objMensagemSistema = new MensagemSistema();
				if (SetInstance(dr, objMensagemSistema))
					return objMensagemSistema;
			}
			throw (new RecordNotFoundException(typeof(MensagemSistema)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de MensagemSistema a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idMensagemSistema))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de MensagemSistema a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idMensagemSistema, trans))
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
		/// <param name="objMensagemSistema">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static bool SetInstance(IDataReader dr, MensagemSistema objMensagemSistema)
		{
			try
			{
				if (dr.Read())
				{
					objMensagemSistema._idMensagemSistema = Convert.ToInt32(dr["Idf_Mensagem_Sistema"]);
					objMensagemSistema._descricaoMensagemSistema = Convert.ToString(dr["Des_Mensagem_Sistema"]);
					objMensagemSistema._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objMensagemSistema._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objMensagemSistema._persisted = true;
					objMensagemSistema._modified = false;

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