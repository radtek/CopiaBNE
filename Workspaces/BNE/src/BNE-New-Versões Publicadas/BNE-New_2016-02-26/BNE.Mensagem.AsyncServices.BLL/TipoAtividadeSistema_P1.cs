//-- Data: 05/10/2012 14:44
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.ExceptionLog.Exceptions;

namespace BNE.Mensagem.AsyncServices.BLL
{
	public partial class TipoAtividadeSistema // Tabela: atividade.TAB_Tipo_Atividade_Sistema
	{
		#region Atributos
		private short _idTipoAtividadeSistema;
		private string _descricaoTipoAtividadeSistema;
		private bool _flagInativo;
	    private TipoAtividade _tipoAtividade;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTipoAtividade
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdTipoAtividadeSistema
		{
			get
			{
				return this._idTipoAtividadeSistema;
			}
		}
		#endregion 

		#region DescricaoTipoAtividadeSistema
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoTipoAtividadeSistema
		{
			get
			{
				return this._descricaoTipoAtividadeSistema;
			}
			set
			{
				this._descricaoTipoAtividadeSistema = value;
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

        #region TipoAtividade
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public TipoAtividade TipoAtividade
        {
            get
            {
                return this._tipoAtividade;
            }
            set
            {
                this._tipoAtividade = value;
                this._modified = true;
            }
        }
        #endregion

		#endregion

		#region Construtores
        public TipoAtividadeSistema()
		{
		}
        public TipoAtividadeSistema(short idTipoAtividadeSistema)
		{
            this._idTipoAtividadeSistema = idTipoAtividadeSistema;
			this._persisted = true;
		}
		#endregion

		#region Consultas
        private const string SPINSERT = "INSERT INTO atividade.TAB_Tipo_Atividade_Sistema (Des_Tipo_Atividade_Sistema, Flg_Inativo) VALUES (@Des_Tipo_Atividade_Sistema, @Flg_Inativo, @Dta_Cadastro);SET @Idf_Tipo_Atividade = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE atividade.TAB_Tipo_Atividade_Sistema SET Des_Tipo_Atividade_Sistema = @Des_Tipo_Atividade_Sistema, Flg_Inativo = @Flg_Inativo WHERE Idf_Tipo_Atividade = @Idf_Tipo_Atividade";
        private const string SPDELETE = "DELETE FROM atividade.TAB_Tipo_Atividade_Sistema WHERE Idf_Tipo_Atividade_Sistema = @Idf_Tipo_Atividade_Sistema";
        private const string SPSELECTID = "SELECT * FROM atividade.TAB_Tipo_Atividade_Sistema WHERE Idf_Tipo_Atividade_Sistema = @Idf_Tipo_Atividade_Sistema";
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
			parms.Add(new SqlParameter("@Idf_Tipo_Atividade_Sistema", SqlDbType.SmallInt, 2));
            parms.Add(new SqlParameter("@Des_Tipo_Atividade_Sistema", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Tipo_Atividade", SqlDbType.Int, 4));
            return (parms);
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
			parms[0].Value = this._idTipoAtividadeSistema;
			parms[1].Value = this._descricaoTipoAtividadeSistema;
			parms[2].Value = this._flagInativo;
            parms[3].Value = this._tipoAtividade.IdTipoAtividade;

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
		/// Método utilizado para inserir uma instância de TipoAtividade no banco de dados.
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
						this._idTipoAtividadeSistema = Convert.ToInt16(cmd.Parameters["@Idf_Tipo_Atividade_Sistema"].Value);
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
		/// Método utilizado para inserir uma instância de TipoAtividade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idTipoAtividadeSistema = Convert.ToInt16(cmd.Parameters["@Idf_Tipo_Atividade_Sistema"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de TipoAtividade no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TipoAtividade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TipoAtividade no banco de dados.
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
		/// Método utilizado para salvar uma instância de TipoAtividade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TipoAtividade no banco de dados.
		/// </summary>
        /// <param name="idTipoAtividadeSistema">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idTipoAtividadeSistema)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Atividade_Sistema", SqlDbType.Int, 4));

            parms[0].Value = idTipoAtividadeSistema;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoAtividade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoAtividade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoAtividade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Atividade", SqlDbType.Int, 4));

			parms[0].Value = idTipoAtividade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoAtividade no banco de dados.
		/// </summary>
		/// <param name="idTipoAtividade">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idTipoAtividade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Tipo_Atividade where Idf_Tipo_Atividade in (";

			for (int i = 0; i < idTipoAtividade.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTipoAtividade[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
        /// <param name="idTipoAtividadeSistema">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoAtividadeSistema)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Atividade_Sistema", SqlDbType.Int, 4));

            parms[0].Value = idTipoAtividadeSistema;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoAtividade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idTipoAtividadeSistema, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Tipo_Atividade_Sistema", SqlDbType.Int, 4));

            parms[0].Value = idTipoAtividadeSistema;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Atividade, Tip.Des_Tipo_Atividade, Tip.Flg_Inativo, Tip.Num_Dias_Expiracao, Tip.Dta_Cadastro FROM plataforma.TAB_Tipo_Atividade Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoAtividade a partir do banco de dados.
		/// </summary>
        /// <param name="idTipoAtividadeSistema">Chave do registro.</param>
		/// <returns>Instância de TipoAtividade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
        public static TipoAtividadeSistema LoadObject(int idTipoAtividadeSistema)
		{
            using (IDataReader dr = LoadDataReader(idTipoAtividadeSistema))
			{
                TipoAtividadeSistema objTipoAtividadeSistema = new TipoAtividadeSistema();
                if (SetInstance(dr, objTipoAtividadeSistema))
                    return objTipoAtividadeSistema;
			}
            throw (new RecordNotFoundException(typeof(TipoAtividadeSistema)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoAtividade a partir do banco de dados, dentro de uma transação.
		/// </summary>
        /// <param name="idTipoAtividadeSistema">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoAtividade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
        public static TipoAtividadeSistema LoadObject(int idTipoAtividadeSistema, SqlTransaction trans)
		{
            using (IDataReader dr = LoadDataReader(idTipoAtividadeSistema, trans))
			{
                TipoAtividadeSistema objTipoAtividadeSistema = new TipoAtividadeSistema();
                if (SetInstance(dr, objTipoAtividadeSistema))
                    return objTipoAtividadeSistema;
			}
			throw (new RecordNotFoundException(typeof(TipoAtividadeSistema)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoAtividade a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoAtividadeSistema))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoAtividade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoAtividadeSistema, trans))
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
        /// <param name="objTipoAtividadeSistema">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, TipoAtividadeSistema objTipoAtividadeSistema)
		{
			try
			{
				if (dr.Read())
				{
                    objTipoAtividadeSistema._idTipoAtividadeSistema = Convert.ToInt16(dr["Idf_Tipo_Atividade_Sistema"]);
                    objTipoAtividadeSistema._descricaoTipoAtividadeSistema = Convert.ToString(dr["Des_Tipo_Atividade_Sistema"]);
                    objTipoAtividadeSistema._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objTipoAtividadeSistema._tipoAtividade = new TipoAtividade(Convert.ToByte(dr["Idf_Tipo_Atividade"]));

                    objTipoAtividadeSistema._persisted = true;
                    objTipoAtividadeSistema._modified = false;

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