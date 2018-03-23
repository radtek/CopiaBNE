//-- Data: 28/01/2016 18:54
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RastreadorCurriculoResultado // Tabela: BNE_Rastreador_Curriculo_Resultado
	{
		#region Atributos
		private int _idRastreadorCurriculoResultado;
		private RastreadorCurriculo _rastreadorCurriculo;
		private Curriculo _curriculo;
		private DateTime _dataCadastro;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRastreadorCurriculoResultado
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRastreadorCurriculoResultado
		{
			get
			{
				return this._idRastreadorCurriculoResultado;
			}
		}
		#endregion 

		#region RastreadorCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public RastreadorCurriculo RastreadorCurriculo
		{
			get
			{
				return this._rastreadorCurriculo;
			}
			set
			{
				this._rastreadorCurriculo = value;
				this._modified = true;
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

		#endregion

		#region Construtores
		public RastreadorCurriculoResultado()
		{
		}
		public RastreadorCurriculoResultado(int idRastreadorCurriculoResultado)
		{
			this._idRastreadorCurriculoResultado = idRastreadorCurriculoResultado;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Rastreador_Curriculo_Resultado (Idf_Rastreador_Curriculo, Idf_Curriculo, Dta_Cadastro, Flg_Inativo) VALUES (@Idf_Rastreador_Curriculo, @Idf_Curriculo, @Dta_Cadastro, @Flg_Inativo);SET @Idf_Rastreador_Curriculo_Resultado = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Rastreador_Curriculo_Resultado SET Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo, Idf_Curriculo = @Idf_Curriculo, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Rastreador_Curriculo_Resultado = @Idf_Rastreador_Curriculo_Resultado";
		private const string SPDELETE = "DELETE FROM BNE_Rastreador_Curriculo_Resultado WHERE Idf_Rastreador_Curriculo_Resultado = @Idf_Rastreador_Curriculo_Resultado";
		private const string SPSELECTID = "SELECT * FROM BNE_Rastreador_Curriculo_Resultado WITH(NOLOCK) WHERE Idf_Rastreador_Curriculo_Resultado = @Idf_Rastreador_Curriculo_Resultado";
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
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Resultado", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idRastreadorCurriculoResultado;
			parms[1].Value = this._rastreadorCurriculo.IdRastreadorCurriculo;
			parms[2].Value = this._curriculo.IdCurriculo;
			parms[4].Value = this._flagInativo;

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
		/// Método utilizado para inserir uma instância de RastreadorCurriculoResultado no banco de dados.
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
						this._idRastreadorCurriculoResultado = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Curriculo_Resultado"].Value);
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
		/// Método utilizado para inserir uma instância de RastreadorCurriculoResultado no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRastreadorCurriculoResultado = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Curriculo_Resultado"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RastreadorCurriculoResultado no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RastreadorCurriculoResultado no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de RastreadorCurriculoResultado no banco de dados.
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
		/// Método utilizado para salvar uma instância de RastreadorCurriculoResultado no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RastreadorCurriculoResultado no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoResultado">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorCurriculoResultado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Resultado", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoResultado;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RastreadorCurriculoResultado no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculoResultado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorCurriculoResultado, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Resultado", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoResultado;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RastreadorCurriculoResultado no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoResultado">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRastreadorCurriculoResultado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Rastreador_Curriculo_Resultado where Idf_Rastreador_Curriculo_Resultado in (";

			for (int i = 0; i < idRastreadorCurriculoResultado.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRastreadorCurriculoResultado[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoResultado">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorCurriculoResultado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Resultado", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoResultado;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculoResultado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorCurriculoResultado, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Resultado", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoResultado;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ras.Idf_Rastreador_Curriculo_Resultado, Ras.Idf_Rastreador_Curriculo, Ras.Idf_Curriculo, Ras.Dta_Cadastro, Ras.Flg_Inativo FROM BNE_Rastreador_Curriculo_Resultado Ras";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorCurriculoResultado a partir do banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoResultado">Chave do registro.</param>
		/// <returns>Instância de RastreadorCurriculoResultado.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorCurriculoResultado LoadObject(int idRastreadorCurriculoResultado)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorCurriculoResultado))
			{
				RastreadorCurriculoResultado objRastreadorCurriculoResultado = new RastreadorCurriculoResultado();
				if (SetInstance(dr, objRastreadorCurriculoResultado))
					return objRastreadorCurriculoResultado;
			}
			throw (new RecordNotFoundException(typeof(RastreadorCurriculoResultado)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorCurriculoResultado a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculoResultado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RastreadorCurriculoResultado.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorCurriculoResultado LoadObject(int idRastreadorCurriculoResultado, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorCurriculoResultado, trans))
			{
				RastreadorCurriculoResultado objRastreadorCurriculoResultado = new RastreadorCurriculoResultado();
				if (SetInstance(dr, objRastreadorCurriculoResultado))
					return objRastreadorCurriculoResultado;
			}
			throw (new RecordNotFoundException(typeof(RastreadorCurriculoResultado)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorCurriculoResultado a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorCurriculoResultado))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorCurriculoResultado a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorCurriculoResultado, trans))
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
		/// <param name="objRastreadorCurriculoResultado">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RastreadorCurriculoResultado objRastreadorCurriculoResultado, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objRastreadorCurriculoResultado._idRastreadorCurriculoResultado = Convert.ToInt32(dr["Idf_Rastreador_Curriculo_Resultado"]);
					objRastreadorCurriculoResultado._rastreadorCurriculo = new RastreadorCurriculo(Convert.ToInt32(dr["Idf_Rastreador_Curriculo"]));
					objRastreadorCurriculoResultado._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objRastreadorCurriculoResultado._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objRastreadorCurriculoResultado._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objRastreadorCurriculoResultado._persisted = true;
					objRastreadorCurriculoResultado._modified = false;

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
				if (dispose)
					dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}