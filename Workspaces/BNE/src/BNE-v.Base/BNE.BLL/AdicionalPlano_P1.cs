//-- Data: 09/09/2011 10:35
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class AdicionalPlano // Tabela: BNE_Adicional_Plano
	{
		#region Atributos
		private int _idAdicionalPlano;
		private PlanoAdquirido _planoAdquirido;
		private TipoAdicional _tipoAdicional;
		private int _quantidadeAdicional;
		private DateTime _dataCadastro;
		private DateTime _dataAlteracao;
		private bool _flagInativo;
		private AdicionalPlanoSituacao _adicionalPlanoSituacao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdAdicionalPlano
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdAdicionalPlano
		{
			get
			{
				return this._idAdicionalPlano;
			}
		}
		#endregion 

		#region PlanoAdquirido
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public PlanoAdquirido PlanoAdquirido
		{
			get
			{
				return this._planoAdquirido;
			}
			set
			{
				this._planoAdquirido = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoAdicional
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoAdicional TipoAdicional
		{
			get
			{
				return this._tipoAdicional;
			}
			set
			{
				this._tipoAdicional = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeAdicional
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int QuantidadeAdicional
		{
			get
			{
				return this._quantidadeAdicional;
			}
			set
			{
				this._quantidadeAdicional = value;
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
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataAlteracao
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

		#region AdicionalPlanoSituacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public AdicionalPlanoSituacao AdicionalPlanoSituacao
		{
			get
			{
				return this._adicionalPlanoSituacao;
			}
			set
			{
				this._adicionalPlanoSituacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public AdicionalPlano()
		{
		}
		public AdicionalPlano(int idAdicionalPlano)
		{
			this._idAdicionalPlano = idAdicionalPlano;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Adicional_Plano (Idf_Plano_Adquirido, Idf_Tipo_Adicional, Qtd_Adicional, Dta_Cadastro, Dta_Alteracao, Flg_Inativo, Idf_Adicional_Plano_Situacao) VALUES (@Idf_Plano_Adquirido, @Idf_Tipo_Adicional, @Qtd_Adicional, @Dta_Cadastro, @Dta_Alteracao, @Flg_Inativo, @Idf_Adicional_Plano_Situacao);SET @Idf_Adicional_Plano = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Adicional_Plano SET Idf_Plano_Adquirido = @Idf_Plano_Adquirido, Idf_Tipo_Adicional = @Idf_Tipo_Adicional, Qtd_Adicional = @Qtd_Adicional, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Flg_Inativo = @Flg_Inativo, Idf_Adicional_Plano_Situacao = @Idf_Adicional_Plano_Situacao WHERE Idf_Adicional_Plano = @Idf_Adicional_Plano";
		private const string SPDELETE = "DELETE FROM BNE_Adicional_Plano WHERE Idf_Adicional_Plano = @Idf_Adicional_Plano";
		private const string SPSELECTID = "SELECT * FROM BNE_Adicional_Plano WHERE Idf_Adicional_Plano = @Idf_Adicional_Plano";
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
			parms.Add(new SqlParameter("@Idf_Adicional_Plano", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Adicional", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Qtd_Adicional", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Adicional_Plano_Situacao", SqlDbType.Int, 4));
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
			parms[0].Value = this._idAdicionalPlano;
			parms[1].Value = this._planoAdquirido.IdPlanoAdquirido;
			parms[2].Value = this._tipoAdicional.IdTipoAdicional;
			parms[3].Value = this._quantidadeAdicional;
			parms[6].Value = this._flagInativo;

			if (this._adicionalPlanoSituacao != null)
				parms[7].Value = this._adicionalPlanoSituacao.IdAdicionalPlanoSituacao;
			else
				parms[7].Value = DBNull.Value;


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
			this._dataAlteracao = DateTime.Now;
			parms[5].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de AdicionalPlano no banco de dados.
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
						this._idAdicionalPlano = Convert.ToInt32(cmd.Parameters["@Idf_Adicional_Plano"].Value);
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
		/// Método utilizado para inserir uma instância de AdicionalPlano no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idAdicionalPlano = Convert.ToInt32(cmd.Parameters["@Idf_Adicional_Plano"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de AdicionalPlano no banco de dados.
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
		/// Método utilizado para atualizar uma instância de AdicionalPlano no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de AdicionalPlano no banco de dados.
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
		/// Método utilizado para salvar uma instância de AdicionalPlano no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de AdicionalPlano no banco de dados.
		/// </summary>
		/// <param name="idAdicionalPlano">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idAdicionalPlano)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Adicional_Plano", SqlDbType.Int, 4));

			parms[0].Value = idAdicionalPlano;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de AdicionalPlano no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAdicionalPlano">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idAdicionalPlano, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Adicional_Plano", SqlDbType.Int, 4));

			parms[0].Value = idAdicionalPlano;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de AdicionalPlano no banco de dados.
		/// </summary>
		/// <param name="idAdicionalPlano">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idAdicionalPlano)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Adicional_Plano where Idf_Adicional_Plano in (";

			for (int i = 0; i < idAdicionalPlano.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idAdicionalPlano[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idAdicionalPlano">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idAdicionalPlano)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Adicional_Plano", SqlDbType.Int, 4));

			parms[0].Value = idAdicionalPlano;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAdicionalPlano">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idAdicionalPlano, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Adicional_Plano", SqlDbType.Int, 4));

			parms[0].Value = idAdicionalPlano;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Adi.Idf_Adicional_Plano, Adi.Idf_Plano_Adquirido, Adi.Idf_Tipo_Adicional, Adi.Qtd_Adicional, Adi.Dta_Cadastro, Adi.Dta_Alteracao, Adi.Flg_Inativo, Adi.Idf_Adicional_Plano_Situacao FROM BNE_Adicional_Plano Adi";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de AdicionalPlano a partir do banco de dados.
		/// </summary>
		/// <param name="idAdicionalPlano">Chave do registro.</param>
		/// <returns>Instância de AdicionalPlano.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static AdicionalPlano LoadObject(int idAdicionalPlano)
		{
			using (IDataReader dr = LoadDataReader(idAdicionalPlano))
			{
				AdicionalPlano objAdicionalPlano = new AdicionalPlano();
				if (SetInstance(dr, objAdicionalPlano))
					return objAdicionalPlano;
			}
			throw (new RecordNotFoundException(typeof(AdicionalPlano)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de AdicionalPlano a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAdicionalPlano">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de AdicionalPlano.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static AdicionalPlano LoadObject(int idAdicionalPlano, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idAdicionalPlano, trans))
			{
				AdicionalPlano objAdicionalPlano = new AdicionalPlano();
				if (SetInstance(dr, objAdicionalPlano))
					return objAdicionalPlano;
			}
			throw (new RecordNotFoundException(typeof(AdicionalPlano)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de AdicionalPlano a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idAdicionalPlano))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de AdicionalPlano a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idAdicionalPlano, trans))
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
		/// <param name="objAdicionalPlano">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, AdicionalPlano objAdicionalPlano)
		{
			try
			{
				if (dr.Read())
				{
					objAdicionalPlano._idAdicionalPlano = Convert.ToInt32(dr["Idf_Adicional_Plano"]);
					objAdicionalPlano._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
					objAdicionalPlano._tipoAdicional = new TipoAdicional(Convert.ToInt32(dr["Idf_Tipo_Adicional"]));
					objAdicionalPlano._quantidadeAdicional = Convert.ToInt32(dr["Qtd_Adicional"]);
					objAdicionalPlano._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objAdicionalPlano._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objAdicionalPlano._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					if (dr["Idf_Adicional_Plano_Situacao"] != DBNull.Value)
						objAdicionalPlano._adicionalPlanoSituacao = new AdicionalPlanoSituacao(Convert.ToInt32(dr["Idf_Adicional_Plano_Situacao"]));

					objAdicionalPlano._persisted = true;
					objAdicionalPlano._modified = false;

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