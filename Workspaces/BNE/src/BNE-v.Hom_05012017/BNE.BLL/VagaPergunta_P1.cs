//-- Data: 09/07/2014 17:41
//-- Autor: ValeriaNeves

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class VagaPergunta // Tabela: BNE_Vaga_Pergunta
	{
		#region Atributos
		private int _idVagaPergunta;
		private string _descricaoVagaPergunta;
		private bool? _flagResposta;
		private Vaga _vaga;
		private DateTime _dataCadastro;
		private bool _flaginativo;
		private TipoResposta _tipoResposta;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdVagaPergunta
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdVagaPergunta
		{
			get
			{
				return this._idVagaPergunta;
			}
		}
		#endregion 

		#region DescricaoVagaPergunta
		/// <summary>
		/// Tamanho do campo: 140.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoVagaPergunta
		{
			get
			{
				return this._descricaoVagaPergunta;
			}
			set
			{
				this._descricaoVagaPergunta = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagResposta
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagResposta
		{
			get
			{
				return this._flagResposta;
			}
			set
			{
				this._flagResposta = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Vaga
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Vaga Vaga
		{
			get
			{
				return this._vaga;
			}
			set
			{
				this._vaga = value;
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

		#region TipoResposta
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoResposta TipoResposta
		{
			get
			{
				return this._tipoResposta;
			}
			set
			{
				this._tipoResposta = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public VagaPergunta()
		{
		}
		public VagaPergunta(int idVagaPergunta)
		{
			this._idVagaPergunta = idVagaPergunta;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Vaga_Pergunta (Des_Vaga_Pergunta, Flg_Resposta, Idf_Vaga, Dta_Cadastro, Flg_inativo, Idf_Tipo_Resposta) VALUES (@Des_Vaga_Pergunta, @Flg_Resposta, @Idf_Vaga, @Dta_Cadastro, @Flg_inativo, @Idf_Tipo_Resposta);SET @Idf_Vaga_Pergunta = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Vaga_Pergunta SET Des_Vaga_Pergunta = @Des_Vaga_Pergunta, Flg_Resposta = @Flg_Resposta, Idf_Vaga = @Idf_Vaga, Dta_Cadastro = @Dta_Cadastro, Flg_inativo = @Flg_inativo, Idf_Tipo_Resposta = @Idf_Tipo_Resposta WHERE Idf_Vaga_Pergunta = @Idf_Vaga_Pergunta";
		private const string SPDELETE = "DELETE FROM BNE_Vaga_Pergunta WHERE Idf_Vaga_Pergunta = @Idf_Vaga_Pergunta";
		private const string SPSELECTID = "SELECT * FROM BNE_Vaga_Pergunta WITH(NOLOCK) WHERE Idf_Vaga_Pergunta = @Idf_Vaga_Pergunta";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>ValeriaNeves</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Pergunta", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Vaga_Pergunta", SqlDbType.VarChar, 140));
			parms.Add(new SqlParameter("@Flg_Resposta", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Tipo_Resposta", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>ValeriaNeves</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idVagaPergunta;
			parms[1].Value = this._descricaoVagaPergunta;

			if (this._flagResposta.HasValue)
				parms[2].Value = this._flagResposta;
			else
				parms[2].Value = DBNull.Value;

			parms[3].Value = this._vaga.IdVaga;
			parms[5].Value = this._flaginativo;
			parms[6].Value = this._tipoResposta.IdTipoResposta;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[4].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de VagaPergunta no banco de dados.
		/// </summary>
		/// <remarks>ValeriaNeves</remarks>
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
						this._idVagaPergunta = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Pergunta"].Value);
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
		/// Método utilizado para inserir uma instância de VagaPergunta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>ValeriaNeves</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idVagaPergunta = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Pergunta"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de VagaPergunta no banco de dados.
		/// </summary>
		/// <remarks>ValeriaNeves</remarks>
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
		/// Método utilizado para atualizar uma instância de VagaPergunta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>ValeriaNeves</remarks>
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
		/// Método utilizado para salvar uma instância de VagaPergunta no banco de dados.
		/// </summary>
		/// <remarks>ValeriaNeves</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de VagaPergunta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>ValeriaNeves</remarks>
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
		/// Método utilizado para excluir uma instância de VagaPergunta no banco de dados.
		/// </summary>
		/// <param name="idVagaPergunta">Chave do registro.</param>
		/// <remarks>ValeriaNeves</remarks>
		public static void Delete(int idVagaPergunta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Pergunta", SqlDbType.Int, 4));

			parms[0].Value = idVagaPergunta;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de VagaPergunta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaPergunta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>ValeriaNeves</remarks>
		public static void Delete(int idVagaPergunta, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Pergunta", SqlDbType.Int, 4));

			parms[0].Value = idVagaPergunta;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de VagaPergunta no banco de dados.
		/// </summary>
		/// <param name="idVagaPergunta">Lista de chaves.</param>
		/// <remarks>ValeriaNeves</remarks>
		public static void Delete(List<int> idVagaPergunta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Vaga_Pergunta where Idf_Vaga_Pergunta in (";

			for (int i = 0; i < idVagaPergunta.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idVagaPergunta[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idVagaPergunta">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>ValeriaNeves</remarks>
		private static IDataReader LoadDataReader(int idVagaPergunta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Pergunta", SqlDbType.Int, 4));

			parms[0].Value = idVagaPergunta;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaPergunta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>ValeriaNeves</remarks>
		private static IDataReader LoadDataReader(int idVagaPergunta, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Pergunta", SqlDbType.Int, 4));

			parms[0].Value = idVagaPergunta;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Vag.Idf_Vaga_Pergunta, Vag.Des_Vaga_Pergunta, Vag.Flg_Resposta, Vag.Idf_Vaga, Vag.Dta_Cadastro, Vag.Flg_inativo, Vag.Idf_Tipo_Resposta FROM BNE_Vaga_Pergunta Vag";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaPergunta a partir do banco de dados.
		/// </summary>
		/// <param name="idVagaPergunta">Chave do registro.</param>
		/// <returns>Instância de VagaPergunta.</returns>
		/// <remarks>ValeriaNeves</remarks>
		public static VagaPergunta LoadObject(int idVagaPergunta)
		{
			using (IDataReader dr = LoadDataReader(idVagaPergunta))
			{
				VagaPergunta objVagaPergunta = new VagaPergunta();
				if (SetInstance(dr, objVagaPergunta))
					return objVagaPergunta;
			}
			throw (new RecordNotFoundException(typeof(VagaPergunta)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaPergunta a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaPergunta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de VagaPergunta.</returns>
		/// <remarks>ValeriaNeves</remarks>
		public static VagaPergunta LoadObject(int idVagaPergunta, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idVagaPergunta, trans))
			{
				VagaPergunta objVagaPergunta = new VagaPergunta();
				if (SetInstance(dr, objVagaPergunta))
					return objVagaPergunta;
			}
			throw (new RecordNotFoundException(typeof(VagaPergunta)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de VagaPergunta a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>ValeriaNeves</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idVagaPergunta))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de VagaPergunta a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>ValeriaNeves</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idVagaPergunta, trans))
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
		/// <param name="objVagaPergunta">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>ValeriaNeves</remarks>
		private static bool SetInstance(IDataReader dr, VagaPergunta objVagaPergunta)
		{
			try
			{
				if (dr.Read())
				{
					objVagaPergunta._idVagaPergunta = Convert.ToInt32(dr["Idf_Vaga_Pergunta"]);
					objVagaPergunta._descricaoVagaPergunta = Convert.ToString(dr["Des_Vaga_Pergunta"]);
					if (dr["Flg_Resposta"] != DBNull.Value)
						objVagaPergunta._flagResposta = Convert.ToBoolean(dr["Flg_Resposta"]);
					objVagaPergunta._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					objVagaPergunta._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objVagaPergunta._flaginativo = Convert.ToBoolean(dr["Flg_inativo"]);
					objVagaPergunta._tipoResposta = new TipoResposta(Convert.ToInt32(dr["Idf_Tipo_Resposta"]));

					objVagaPergunta._persisted = true;
					objVagaPergunta._modified = false;

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