//-- Data: 25/07/2013 18:01
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CodigoDescontoPlano // Tabela: BNE_Codigo_Desconto_Plano
	{
		#region Atributos
		private TipoCodigoDesconto _tipoCodigoDesconto;
		private Plano _plano;
        private int? _numeroPercentualDesconto;
        private int? _quantidadeSMS;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region TipoCodigoDesconto
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public TipoCodigoDesconto TipoCodigoDesconto
		{
			get
			{
				return this._tipoCodigoDesconto;
			}
		}
		#endregion 

        #region Plano
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Plano Plano
        {
            get
            {
                return this._plano;
            }
            set
            {
                this._plano = value;
                this._modified = true;
            }
        }
        #endregion 

        #region NumeroPercentualDesconto
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? NumeroPercentualDesconto
        {
            get
            {
                return this._numeroPercentualDesconto;
            }
            set
            {
                this._numeroPercentualDesconto = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeSMS
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? QuantidadeSMS
        {
            get
            {
                return this._quantidadeSMS;
            }
            set
            {
                this._quantidadeSMS = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

		#region Construtores
		public CodigoDescontoPlano()
		{
		}
		public CodigoDescontoPlano(TipoCodigoDesconto tipoCodigoDesconto, Plano plano)
		{
			this._tipoCodigoDesconto = tipoCodigoDesconto;
			this._plano = plano;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Codigo_Desconto_Plano (Idf_Tipo_Codigo_Desconto, Idf_Plano, Num_Percentual_Desconto, Qtd_SMS) VALUES (@Idf_Tipo_Codigo_Desconto, @Idf_Plano, @Num_Percentual_Desconto, @Qtd_SMS)";
		private const string SPUPDATE = "UPDATE BNE_Codigo_Desconto_Plano SET Idf_Tipo_Codigo_Desconto = @Idf_Tipo_Codigo_Desconto, Idf_Plano = @Idf_Plano, Num_Percentual_Desconto = @Num_Percentual_Desconto, Qtd_SMS = @Qtd_SMS WHERE Idf_Tipo_Codigo_Desconto = @Idf_Tipo_Codigo_Desconto AND Idf_Plano = @Idf_Plano";
		private const string SPDELETE = "DELETE FROM BNE_Codigo_Desconto_Plano WHERE Idf_Tipo_Codigo_Desconto = @Idf_Tipo_Codigo_Desconto AND Idf_Plano = @Idf_Plano";
		private const string SPSELECTID = "SELECT * FROM BNE_Codigo_Desconto_Plano WITH(NOLOCK) WHERE Idf_Tipo_Codigo_Desconto = @Idf_Tipo_Codigo_Desconto AND Idf_Plano = @Idf_Plano";
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
			parms.Add(new SqlParameter("@Idf_Tipo_Codigo_Desconto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Plano", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Percentual_Desconto", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_SMS", SqlDbType.Int, 4));
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
			parms[0].Value = this._tipoCodigoDesconto.IdTipoCodigoDesconto;
			parms[1].Value = this._plano.IdPlano;

            if (_numeroPercentualDesconto.HasValue)
                parms[2].Value = this._numeroPercentualDesconto.Value;
            else
                parms[2].Value = DBNull.Value;

            if (_quantidadeSMS.HasValue)
                parms[3].Value = this._quantidadeSMS.Value;
            else
                parms[3].Value = DBNull.Value;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CodigoDescontoPlano no banco de dados.
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
		/// Método utilizado para inserir uma instância de CodigoDescontoPlano no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de CodigoDescontoPlano no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CodigoDescontoPlano no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CodigoDescontoPlano no banco de dados.
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
		/// Método utilizado para salvar uma instância de CodigoDescontoPlano no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CodigoDescontoPlano no banco de dados.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <param name="idPlano">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoCodigoDesconto, int idPlano)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Codigo_Desconto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Plano", SqlDbType.Int, 4));

			parms[0].Value = idTipoCodigoDesconto;
			parms[1].Value = idPlano;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CodigoDescontoPlano no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <param name="idPlano">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoCodigoDesconto, int idPlano, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Codigo_Desconto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Plano", SqlDbType.Int, 4));

			parms[0].Value = idTipoCodigoDesconto;
			parms[1].Value = idPlano;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <param name="idPlano">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoCodigoDesconto, int idPlano)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Codigo_Desconto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Plano", SqlDbType.Int, 4));

			parms[0].Value = idTipoCodigoDesconto;
			parms[1].Value = idPlano;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <param name="idPlano">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoCodigoDesconto, int idPlano, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Codigo_Desconto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Plano", SqlDbType.Int, 4));

			parms[0].Value = idTipoCodigoDesconto;
			parms[1].Value = idPlano;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cod.Idf_Tipo_Codigo_Desconto, Cod.Idf_Plano, Cod.Num_Percentual_Desconto, Cod.Qtd_SMS FROM BNE_Codigo_Desconto_Plano Cod";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CodigoDescontoPlano a partir do banco de dados.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <param name="idPlano">Chave do registro.</param>
		/// <returns>Instância de CodigoDescontoPlano.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CodigoDescontoPlano LoadObject(int idTipoCodigoDesconto, int idPlano)
		{
			using (IDataReader dr = LoadDataReader(idTipoCodigoDesconto, idPlano))
			{
				CodigoDescontoPlano objCodigoDescontoPlano = new CodigoDescontoPlano();
				if (SetInstance(dr, objCodigoDescontoPlano))
					return objCodigoDescontoPlano;
			}
			throw (new RecordNotFoundException(typeof(CodigoDescontoPlano)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CodigoDescontoPlano a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <param name="idPlano">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CodigoDescontoPlano.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CodigoDescontoPlano LoadObject(int idTipoCodigoDesconto, int idPlano, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTipoCodigoDesconto, idPlano, trans))
			{
				CodigoDescontoPlano objCodigoDescontoPlano = new CodigoDescontoPlano();
				if (SetInstance(dr, objCodigoDescontoPlano))
					return objCodigoDescontoPlano;
			}
			throw (new RecordNotFoundException(typeof(CodigoDescontoPlano)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CodigoDescontoPlano a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._tipoCodigoDesconto.IdTipoCodigoDesconto, this._plano.IdPlano))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CodigoDescontoPlano a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._tipoCodigoDesconto.IdTipoCodigoDesconto, this._plano.IdPlano, trans))
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
		/// <param name="objCodigoDescontoPlano">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CodigoDescontoPlano objCodigoDescontoPlano)
		{
			try
			{
				if (dr.Read())
				{
					objCodigoDescontoPlano._tipoCodigoDesconto = new TipoCodigoDesconto(Convert.ToInt32(dr["Idf_Tipo_Codigo_Desconto"]));
					objCodigoDescontoPlano._plano = new Plano(Convert.ToInt32(dr["Idf_Plano"]));
                    if (dr["Num_Percentual_Desconto"] != DBNull.Value)
                        objCodigoDescontoPlano._numeroPercentualDesconto = Convert.ToInt32(dr["Num_Percentual_Desconto"]);
                    if (dr["Qtd_SMS"] != DBNull.Value)
                        objCodigoDescontoPlano._quantidadeSMS = Convert.ToInt32(dr["Qtd_SMS"]);

					objCodigoDescontoPlano._persisted = true;
					objCodigoDescontoPlano._modified = false;

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