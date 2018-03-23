//-- Data: 18/03/2011 10:58
//-- Autor: Elias

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RespostaAutomatica // Tabela: BNE_Resposta_Automatica
	{
		#region Atributos
		private int _idRespostaAutomatica;
		private string _descricaoRespostaAutomatica;
		private DateTime _dataCadastro;
		private DateTime? _dataAlteracao;
		private bool _flagInativo;
        private string _tituloRespostaAutomatica;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRespostaAutomatica
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRespostaAutomatica
		{
			get
			{
				return this._idRespostaAutomatica;
			}
		}
		#endregion 

		#region DescricaoRespostaAutomatica
		/// <summary>
		/// Tamanho do campo: 1000.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoRespostaAutomatica
		{
			get
			{
				return this._descricaoRespostaAutomatica;
			}
			set
			{
				this._descricaoRespostaAutomatica = value;
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

		#region DataAlteracao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataAlteracao
		{
			get
			{
				return this._dataAlteracao;
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

        #region TituloRespostaAutomatica
        /// <summary>
        /// 
        /// </summary>
        public string TituloRespostaAutomatica
        {
            get { return _tituloRespostaAutomatica; }
            set { _tituloRespostaAutomatica = value; }
        }
        #endregion

        #endregion

        #region Construtores
        public RespostaAutomatica()
		{
		}
		public RespostaAutomatica(int idRespostaAutomatica)
		{
			this._idRespostaAutomatica = idRespostaAutomatica;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Resposta_Automatica (Des_Resposta_Automatica, Dta_Cadastro, Dta_Alteracao, Flg_Inativo, Nme_Resposta_Automatica) VALUES (@Des_Resposta_Automatica, @Dta_Cadastro, @Dta_Alteracao, @Flg_Inativo, @Nme_Resposta_Automatica);SET @Idf_Resposta_Automatica = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Resposta_Automatica SET Des_Resposta_Automatica = @Des_Resposta_Automatica, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Flg_Inativo = @Flg_Inativo , Nme_Resposta_Automatica = @Nme_Resposta_Automatica WHERE Idf_Resposta_Automatica = @Idf_Resposta_Automatica";
		private const string SPDELETE = "DELETE FROM BNE_Resposta_Automatica WHERE Idf_Resposta_Automatica = @Idf_Resposta_Automatica";
		private const string SPSELECTID = "SELECT * FROM BNE_Resposta_Automatica WHERE Idf_Resposta_Automatica = @Idf_Resposta_Automatica";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Elias</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Resposta_Automatica", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Resposta_Automatica", SqlDbType.VarChar, 1000));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Nme_Resposta_Automatica", SqlDbType.VarChar, 255));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Elias</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idRespostaAutomatica;
			parms[1].Value = this._descricaoRespostaAutomatica;
			parms[4].Value = this._flagInativo;
            parms[5].Value = this._tituloRespostaAutomatica;

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
			this._dataAlteracao = DateTime.Now;
			parms[3].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de RespostaAutomatica no banco de dados.
		/// </summary>
		/// <remarks>Elias</remarks>
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
						this._idRespostaAutomatica = Convert.ToInt32(cmd.Parameters["@Idf_Resposta_Automatica"].Value);
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
		/// Método utilizado para inserir uma instância de RespostaAutomatica no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRespostaAutomatica = Convert.ToInt32(cmd.Parameters["@Idf_Resposta_Automatica"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RespostaAutomatica no banco de dados.
		/// </summary>
		/// <remarks>Elias</remarks>
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
		/// Método utilizado para atualizar uma instância de RespostaAutomatica no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
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
		/// Método utilizado para salvar uma instância de RespostaAutomatica no banco de dados.
		/// </summary>
		/// <remarks>Elias</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de RespostaAutomatica no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
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
		/// Método utilizado para excluir uma instância de RespostaAutomatica no banco de dados.
		/// </summary>
		/// <param name="idRespostaAutomatica">Chave do registro.</param>
		/// <remarks>Elias</remarks>
		public static void Delete(int idRespostaAutomatica)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Resposta_Automatica", SqlDbType.Int, 4));

			parms[0].Value = idRespostaAutomatica;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RespostaAutomatica no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRespostaAutomatica">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
		public static void Delete(int idRespostaAutomatica, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Resposta_Automatica", SqlDbType.Int, 4));

			parms[0].Value = idRespostaAutomatica;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RespostaAutomatica no banco de dados.
		/// </summary>
		/// <param name="idRespostaAutomatica">Lista de chaves.</param>
		/// <remarks>Elias</remarks>
		public static void Delete(List<int> idRespostaAutomatica)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Resposta_Automatica where Idf_Resposta_Automatica in (";

			for (int i = 0; i < idRespostaAutomatica.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRespostaAutomatica[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRespostaAutomatica">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Elias</remarks>
		private static IDataReader LoadDataReader(int idRespostaAutomatica)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Resposta_Automatica", SqlDbType.Int, 4));

			parms[0].Value = idRespostaAutomatica;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRespostaAutomatica">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Elias</remarks>
		private static IDataReader LoadDataReader(int idRespostaAutomatica, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Resposta_Automatica", SqlDbType.Int, 4));

			parms[0].Value = idRespostaAutomatica;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Res.Idf_Resposta_Automatica, Res.Des_Resposta_Automatica, Res.Dta_Cadastro, Res.Dta_Alteracao, Res.Flg_Inativo FROM BNE_Resposta_Automatica Res";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RespostaAutomatica a partir do banco de dados.
		/// </summary>
		/// <param name="idRespostaAutomatica">Chave do registro.</param>
		/// <returns>Instância de RespostaAutomatica.</returns>
		/// <remarks>Elias</remarks>
		public static RespostaAutomatica LoadObject(int idRespostaAutomatica)
		{
			using (IDataReader dr = LoadDataReader(idRespostaAutomatica))
			{
				RespostaAutomatica objRespostaAutomatica = new RespostaAutomatica();
				if (SetInstance(dr, objRespostaAutomatica))
					return objRespostaAutomatica;
			}
			throw (new RecordNotFoundException(typeof(RespostaAutomatica)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RespostaAutomatica a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRespostaAutomatica">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RespostaAutomatica.</returns>
		/// <remarks>Elias</remarks>
		public static RespostaAutomatica LoadObject(int idRespostaAutomatica, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRespostaAutomatica, trans))
			{
				RespostaAutomatica objRespostaAutomatica = new RespostaAutomatica();
				if (SetInstance(dr, objRespostaAutomatica))
					return objRespostaAutomatica;
			}
			throw (new RecordNotFoundException(typeof(RespostaAutomatica)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RespostaAutomatica a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRespostaAutomatica))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RespostaAutomatica a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRespostaAutomatica, trans))
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
		/// <param name="objRespostaAutomatica">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias</remarks>
		private static bool SetInstance(IDataReader dr, RespostaAutomatica objRespostaAutomatica)
		{
			try
			{
				if (dr.Read())
				{
					objRespostaAutomatica._idRespostaAutomatica = Convert.ToInt32(dr["Idf_Resposta_Automatica"]);
					objRespostaAutomatica._descricaoRespostaAutomatica = Convert.ToString(dr["Des_Resposta_Automatica"]);
					objRespostaAutomatica._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Dta_Alteracao"] != DBNull.Value)
						objRespostaAutomatica._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objRespostaAutomatica._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objRespostaAutomatica._tituloRespostaAutomatica = Convert.ToString(dr["Nme_Resposta_Automatica"]);

					objRespostaAutomatica._persisted = true;
					objRespostaAutomatica._modified = false;

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