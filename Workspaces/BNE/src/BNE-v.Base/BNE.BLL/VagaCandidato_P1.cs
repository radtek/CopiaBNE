//-- Data: 12/07/2011 12:19
//-- Autor: Tiago Franco

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class VagaCandidato // Tabela: BNE_Vaga_Candidato
	{
		#region Atributos
		private int _idVagaCandidato;
		private Curriculo _curriculo;
		private Vaga _vaga;
		private DateTime _dataCadastro;
		private bool _flagInativo;
		private StatusCurriculoVaga _statusCurriculoVaga;
        private bool? _flagAutoCandidatura;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdVagaCandidato
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdVagaCandidato
		{
			get
			{
				return this._idVagaCandidato;
			}
		}
		#endregion 

		#region Curriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Curriculo Curriculo
		{
			get
			{
				return this._curriculo;
			}
			set
			{
				this._curriculo = value;
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

		#region StatusCurriculoVaga
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public StatusCurriculoVaga StatusCurriculoVaga
		{
			get
			{
				return this._statusCurriculoVaga;
			}
			set
			{
				this._statusCurriculoVaga = value;
				this._modified = true;
			}
		}
		#endregion 

        #region FlagAutoCandidatura
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagAutoCandidatura
		{
			get
			{
				return this._flagAutoCandidatura;
			}
			set
			{
				this._flagAutoCandidatura = value;
				this._modified = true;
			}
		}
		#endregion
        
		#endregion

		#region Construtores
		public VagaCandidato()
		{
		}
		public VagaCandidato(int idVagaCandidato)
		{
			this._idVagaCandidato = idVagaCandidato;
			this._persisted = true;
		}
		#endregion

		#region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Vaga_Candidato (Idf_Curriculo, Idf_Vaga, Dta_Cadastro, Flg_Inativo, Idf_Status_Curriculo_Vaga, Flg_Auto_Candidatura) VALUES (@Idf_Curriculo, @Idf_Vaga, @Dta_Cadastro, @Flg_Inativo, @Idf_Status_Curriculo_Vaga, @Flg_Auto_Candidatura);SET @Idf_Vaga_Candidato = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Vaga_Candidato SET Idf_Curriculo = @Idf_Curriculo, Idf_Vaga = @Idf_Vaga, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Idf_Status_Curriculo_Vaga = @Idf_Status_Curriculo_Vaga, Flg_Auto_Candidatura = @Flg_Auto_Candidatura WHERE Idf_Vaga_Candidato = @Idf_Vaga_Candidato";
		private const string SPDELETE = "DELETE FROM BNE_Vaga_Candidato WHERE Idf_Vaga_Candidato = @Idf_Vaga_Candidato";
		private const string SPSELECTID = "SELECT * FROM BNE_Vaga_Candidato WHERE Idf_Vaga_Candidato = @Idf_Vaga_Candidato";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Tiago Franco</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Status_Curriculo_Vaga", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Auto_Candidatura", SqlDbType.Bit, 1));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Tiago Franco</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idVagaCandidato;
			parms[1].Value = this._curriculo.IdCurriculo;
			parms[2].Value = this._vaga.IdVaga;
			parms[4].Value = this._flagInativo;
			parms[5].Value = this._statusCurriculoVaga.IdStatusCurriculoVaga;

		    if (this._flagAutoCandidatura.HasValue)
		        parms[6].Value = this._flagAutoCandidatura;
		    else
		        parms[6].Value = DBNull.Value;

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
		/// Método utilizado para inserir uma instância de VagaCandidato no banco de dados.
		/// </summary>
		/// <remarks>Tiago Franco</remarks>
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
						this._idVagaCandidato = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Candidato"].Value);
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
		/// Método utilizado para inserir uma instância de VagaCandidato no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Tiago Franco</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idVagaCandidato = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Candidato"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de VagaCandidato no banco de dados.
		/// </summary>
		/// <remarks>Tiago Franco</remarks>
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
		/// Método utilizado para atualizar uma instância de VagaCandidato no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Tiago Franco</remarks>
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
		/// Método utilizado para salvar uma instância de VagaCandidato no banco de dados.
		/// </summary>
		/// <remarks>Tiago Franco</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de VagaCandidato no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Tiago Franco</remarks>
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
		/// Método utilizado para excluir uma instância de VagaCandidato no banco de dados.
		/// </summary>
		/// <param name="idVagaCandidato">Chave do registro.</param>
		/// <remarks>Tiago Franco</remarks>
		public static void Delete(int idVagaCandidato)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato", SqlDbType.Int, 4));

			parms[0].Value = idVagaCandidato;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de VagaCandidato no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaCandidato">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Tiago Franco</remarks>
		public static void Delete(int idVagaCandidato, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato", SqlDbType.Int, 4));

			parms[0].Value = idVagaCandidato;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de VagaCandidato no banco de dados.
		/// </summary>
		/// <param name="idVagaCandidato">Lista de chaves.</param>
		/// <remarks>Tiago Franco</remarks>
		public static void Delete(List<int> idVagaCandidato)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Vaga_Candidato where Idf_Vaga_Candidato in (";

			for (int i = 0; i < idVagaCandidato.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idVagaCandidato[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idVagaCandidato">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Tiago Franco</remarks>
		private static IDataReader LoadDataReader(int idVagaCandidato)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato", SqlDbType.Int, 4));

			parms[0].Value = idVagaCandidato;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaCandidato">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Tiago Franco</remarks>
		private static IDataReader LoadDataReader(int idVagaCandidato, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato", SqlDbType.Int, 4));

			parms[0].Value = idVagaCandidato;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Vag.Idf_Vaga_Candidato, Vag.Idf_Curriculo, Vag.Idf_Vaga, Vag.Dta_Cadastro, Vag.Flg_Inativo, Vag.Idf_Status_Curriculo_Vaga, Vag.Flg_Auto_Candidatura FROM BNE_Vaga_Candidato Vag";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaCandidato a partir do banco de dados.
		/// </summary>
		/// <param name="idVagaCandidato">Chave do registro.</param>
		/// <returns>Instância de VagaCandidato.</returns>
		/// <remarks>Tiago Franco</remarks>
		public static VagaCandidato LoadObject(int idVagaCandidato)
		{
			using (IDataReader dr = LoadDataReader(idVagaCandidato))
			{
				VagaCandidato objVagaCandidato = new VagaCandidato();
				if (SetInstance(dr, objVagaCandidato))
					return objVagaCandidato;
			}
			throw (new RecordNotFoundException(typeof(VagaCandidato)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaCandidato a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaCandidato">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de VagaCandidato.</returns>
		/// <remarks>Tiago Franco</remarks>
		public static VagaCandidato LoadObject(int idVagaCandidato, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idVagaCandidato, trans))
			{
				VagaCandidato objVagaCandidato = new VagaCandidato();
				if (SetInstance(dr, objVagaCandidato))
					return objVagaCandidato;
			}
			throw (new RecordNotFoundException(typeof(VagaCandidato)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de VagaCandidato a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Tiago Franco</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idVagaCandidato))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de VagaCandidato a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Tiago Franco</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idVagaCandidato, trans))
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
		/// <param name="objVagaCandidato">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Tiago Franco</remarks>
		private static bool SetInstance(IDataReader dr, VagaCandidato objVagaCandidato)
		{
			try
			{
				if (dr.Read())
				{
					objVagaCandidato._idVagaCandidato = Convert.ToInt32(dr["Idf_Vaga_Candidato"]);
					objVagaCandidato._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objVagaCandidato._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					objVagaCandidato._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objVagaCandidato._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objVagaCandidato._statusCurriculoVaga = new StatusCurriculoVaga(Convert.ToInt32(dr["Idf_Status_Curriculo_Vaga"]));
                    if (dr["Flg_Auto_Candidatura"] != DBNull.Value)
                        objVagaCandidato._flagAutoCandidatura = Convert.ToBoolean(dr["Flg_Auto_Candidatura"]);
                    else
                        objVagaCandidato._flagAutoCandidatura = false;

					objVagaCandidato._persisted = true;
					objVagaCandidato._modified = false;

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