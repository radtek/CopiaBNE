//-- Data: 31/05/2011 15:15
//-- Autor: Vinicius Maciel

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class VagaCandidatoPergunta // Tabela: BNE_Vaga_Candidato_Pergunta
	{
		#region Atributos
		private int _idVagaCandidatoPergunta;
		private VagaPergunta _vagaPergunta;
		private VagaCandidato _vagaCandidato;
		private bool _flagResposta;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdVagaCandidatoPergunta
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdVagaCandidatoPergunta
		{
			get
			{
				return this._idVagaCandidatoPergunta;
			}
		}
		#endregion 

		#region VagaPergunta
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public VagaPergunta VagaPergunta
		{
			get
			{
				return this._vagaPergunta;
			}
			set
			{
				this._vagaPergunta = value;
				this._modified = true;
			}
		}
		#endregion 

		#region VagaCandidato
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public VagaCandidato VagaCandidato
		{
			get
			{
				return this._vagaCandidato;
			}
			set
			{
				this._vagaCandidato = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagResposta
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagResposta
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

		#endregion

		#region Construtores
		public VagaCandidatoPergunta()
		{
		}
		public VagaCandidatoPergunta(int idVagaCandidatoPergunta)
		{
			this._idVagaCandidatoPergunta = idVagaCandidatoPergunta;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Vaga_Candidato_Pergunta (Idf_Vaga_Pergunta, Idf_Vaga_Candidato, Flg_Resposta) VALUES (@Idf_Vaga_Pergunta, @Idf_Vaga_Candidato, @Flg_Resposta);SET @Idf_Vaga_Candidato_Pergunta = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Vaga_Candidato_Pergunta SET Idf_Vaga_Pergunta = @Idf_Vaga_Pergunta, Idf_Vaga_Candidato = @Idf_Vaga_Candidato, Flg_Resposta = @Flg_Resposta WHERE Idf_Vaga_Candidato_Pergunta = @Idf_Vaga_Candidato_Pergunta";
		private const string SPDELETE = "DELETE FROM BNE_Vaga_Candidato_Pergunta WHERE Idf_Vaga_Candidato_Pergunta = @Idf_Vaga_Candidato_Pergunta";
		private const string SPSELECTID = "SELECT * FROM BNE_Vaga_Candidato_Pergunta WHERE Idf_Vaga_Candidato_Pergunta = @Idf_Vaga_Candidato_Pergunta";
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
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato_Pergunta", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga_Pergunta", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Resposta", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idVagaCandidatoPergunta;
			parms[1].Value = this._vagaPergunta.IdVagaPergunta;
			parms[2].Value = this._vagaCandidato.IdVagaCandidato;
			parms[3].Value = this._flagResposta;

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
		/// Método utilizado para inserir uma instância de VagaCandidatoPergunta no banco de dados.
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
						this._idVagaCandidatoPergunta = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Candidato_Pergunta"].Value);
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
		/// Método utilizado para inserir uma instância de VagaCandidatoPergunta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idVagaCandidatoPergunta = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Candidato_Pergunta"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de VagaCandidatoPergunta no banco de dados.
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
		/// Método utilizado para atualizar uma instância de VagaCandidatoPergunta no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de VagaCandidatoPergunta no banco de dados.
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
		/// Método utilizado para salvar uma instância de VagaCandidatoPergunta no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de VagaCandidatoPergunta no banco de dados.
		/// </summary>
		/// <param name="idVagaCandidatoPergunta">Chave do registro.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idVagaCandidatoPergunta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato_Pergunta", SqlDbType.Int, 4));

			parms[0].Value = idVagaCandidatoPergunta;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de VagaCandidatoPergunta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaCandidatoPergunta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idVagaCandidatoPergunta, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato_Pergunta", SqlDbType.Int, 4));

			parms[0].Value = idVagaCandidatoPergunta;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de VagaCandidatoPergunta no banco de dados.
		/// </summary>
		/// <param name="idVagaCandidatoPergunta">Lista de chaves.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(List<int> idVagaCandidatoPergunta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Vaga_Candidato_Pergunta where Idf_Vaga_Candidato_Pergunta in (";

			for (int i = 0; i < idVagaCandidatoPergunta.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idVagaCandidatoPergunta[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idVagaCandidatoPergunta">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idVagaCandidatoPergunta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato_Pergunta", SqlDbType.Int, 4));

			parms[0].Value = idVagaCandidatoPergunta;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaCandidatoPergunta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idVagaCandidatoPergunta, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato_Pergunta", SqlDbType.Int, 4));

			parms[0].Value = idVagaCandidatoPergunta;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Vag.Idf_Vaga_Candidato_Pergunta, Vag.Idf_Vaga_Pergunta, Vag.Idf_Vaga_Candidato, Vag.Flg_Resposta FROM BNE_Vaga_Candidato_Pergunta Vag";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaCandidatoPergunta a partir do banco de dados.
		/// </summary>
		/// <param name="idVagaCandidatoPergunta">Chave do registro.</param>
		/// <returns>Instância de VagaCandidatoPergunta.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static VagaCandidatoPergunta LoadObject(int idVagaCandidatoPergunta)
		{
			using (IDataReader dr = LoadDataReader(idVagaCandidatoPergunta))
			{
				VagaCandidatoPergunta objVagaCandidatoPergunta = new VagaCandidatoPergunta();
				if (SetInstance(dr, objVagaCandidatoPergunta))
					return objVagaCandidatoPergunta;
			}
			throw (new RecordNotFoundException(typeof(VagaCandidatoPergunta)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaCandidatoPergunta a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaCandidatoPergunta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de VagaCandidatoPergunta.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static VagaCandidatoPergunta LoadObject(int idVagaCandidatoPergunta, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idVagaCandidatoPergunta, trans))
			{
				VagaCandidatoPergunta objVagaCandidatoPergunta = new VagaCandidatoPergunta();
				if (SetInstance(dr, objVagaCandidatoPergunta))
					return objVagaCandidatoPergunta;
			}
			throw (new RecordNotFoundException(typeof(VagaCandidatoPergunta)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de VagaCandidatoPergunta a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idVagaCandidatoPergunta))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de VagaCandidatoPergunta a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idVagaCandidatoPergunta, trans))
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
		/// <param name="objVagaCandidatoPergunta">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static bool SetInstance(IDataReader dr, VagaCandidatoPergunta objVagaCandidatoPergunta)
		{
			try
			{
				if (dr.Read())
				{
					objVagaCandidatoPergunta._idVagaCandidatoPergunta = Convert.ToInt32(dr["Idf_Vaga_Candidato_Pergunta"]);
					objVagaCandidatoPergunta._vagaPergunta = new VagaPergunta(Convert.ToInt32(dr["Idf_Vaga_Pergunta"]));
					objVagaCandidatoPergunta._vagaCandidato = new VagaCandidato(Convert.ToInt32(dr["Idf_Vaga_Candidato"]));
					objVagaCandidatoPergunta._flagResposta = Convert.ToBoolean(dr["Flg_Resposta"]);

					objVagaCandidatoPergunta._persisted = true;
					objVagaCandidatoPergunta._modified = false;

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