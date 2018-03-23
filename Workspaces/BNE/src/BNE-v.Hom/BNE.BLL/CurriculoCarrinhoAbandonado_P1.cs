//-- Data: 18/03/2016 09:31
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CurriculoCarrinhoAbandonado // Tabela: BNE_Curriculo_Carrinho_Abandonado
	{
		#region Atributos
		private int _idCurriculoCarrinhoAbandonado;
		private Curriculo _curriculo;
		private CodigoDesconto _codigoDesconto;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCurriculoCarrinhoAbandonado
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCurriculoCarrinhoAbandonado
		{
			get
			{
				return this._idCurriculoCarrinhoAbandonado;
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

		#region CodigoDesconto
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CodigoDesconto CodigoDesconto
		{
			get
			{
				return this._codigoDesconto;
			}
			set
			{
				this._codigoDesconto = value;
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
		public CurriculoCarrinhoAbandonado()
		{
		}
		public CurriculoCarrinhoAbandonado(int idCurriculoCarrinhoAbandonado)
		{
			this._idCurriculoCarrinhoAbandonado = idCurriculoCarrinhoAbandonado;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Curriculo_Carrinho_Abandonado (Idf_Curriculo, Idf_Codigo_Desconto, Dta_Cadastro) VALUES (@Idf_Curriculo, @Idf_Codigo_Desconto, @Dta_Cadastro);SET @Idf_Curriculo_Carrinho_Abandonado = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Curriculo_Carrinho_Abandonado SET Idf_Curriculo = @Idf_Curriculo, Idf_Codigo_Desconto = @Idf_Codigo_Desconto, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Curriculo_Carrinho_Abandonado = @Idf_Curriculo_Carrinho_Abandonado";
		private const string SPDELETE = "DELETE FROM BNE_Curriculo_Carrinho_Abandonado WHERE Idf_Curriculo_Carrinho_Abandonado = @Idf_Curriculo_Carrinho_Abandonado";
		private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Carrinho_Abandonado WITH(NOLOCK) WHERE Idf_Curriculo_Carrinho_Abandonado = @Idf_Curriculo_Carrinho_Abandonado";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Mailson</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Carrinho_Abandonado", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Codigo_Desconto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 3));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Mailson</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idCurriculoCarrinhoAbandonado;
			parms[1].Value = this._curriculo.IdCurriculo;
			parms[2].Value = this._codigoDesconto.IdCodigoDesconto;

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
		/// Método utilizado para inserir uma instância de CurriculoCarrinhoAbandonado no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
						this._idCurriculoCarrinhoAbandonado = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Carrinho_Abandonado"].Value);
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
		/// Método utilizado para inserir uma instância de CurriculoCarrinhoAbandonado no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCurriculoCarrinhoAbandonado = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Carrinho_Abandonado"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CurriculoCarrinhoAbandonado no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para atualizar uma instância de CurriculoCarrinhoAbandonado no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para salvar uma instância de CurriculoCarrinhoAbandonado no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de CurriculoCarrinhoAbandonado no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para excluir uma instância de CurriculoCarrinhoAbandonado no banco de dados.
		/// </summary>
		/// <param name="idCurriculoCarrinhoAbandonado">Chave do registro.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idCurriculoCarrinhoAbandonado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Carrinho_Abandonado", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoCarrinhoAbandonado;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CurriculoCarrinhoAbandonado no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoCarrinhoAbandonado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idCurriculoCarrinhoAbandonado, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Carrinho_Abandonado", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoCarrinhoAbandonado;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CurriculoCarrinhoAbandonado no banco de dados.
		/// </summary>
		/// <param name="idCurriculoCarrinhoAbandonado">Lista de chaves.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(List<int> idCurriculoCarrinhoAbandonado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Curriculo_Carrinho_Abandonado where Idf_Curriculo_Carrinho_Abandonado in (";

			for (int i = 0; i < idCurriculoCarrinhoAbandonado.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCurriculoCarrinhoAbandonado[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurriculoCarrinhoAbandonado">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idCurriculoCarrinhoAbandonado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Carrinho_Abandonado", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoCarrinhoAbandonado;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoCarrinhoAbandonado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idCurriculoCarrinhoAbandonado, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Carrinho_Abandonado", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoCarrinhoAbandonado;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo_Carrinho_Abandonado, Cur.Idf_Curriculo, Cur.Idf_Codigo_Desconto, Cur.Dta_Cadastro FROM BNE_Curriculo_Carrinho_Abandonado Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoCarrinhoAbandonado a partir do banco de dados.
		/// </summary>
		/// <param name="idCurriculoCarrinhoAbandonado">Chave do registro.</param>
		/// <returns>Instância de CurriculoCarrinhoAbandonado.</returns>
		/// <remarks>Mailson</remarks>
		public static CurriculoCarrinhoAbandonado LoadObject(int idCurriculoCarrinhoAbandonado)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoCarrinhoAbandonado))
			{
				CurriculoCarrinhoAbandonado objCurriculoCarrinhoAbandonado = new CurriculoCarrinhoAbandonado();
				if (SetInstance(dr, objCurriculoCarrinhoAbandonado))
					return objCurriculoCarrinhoAbandonado;
			}
			throw (new RecordNotFoundException(typeof(CurriculoCarrinhoAbandonado)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoCarrinhoAbandonado a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoCarrinhoAbandonado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CurriculoCarrinhoAbandonado.</returns>
		/// <remarks>Mailson</remarks>
		public static CurriculoCarrinhoAbandonado LoadObject(int idCurriculoCarrinhoAbandonado, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoCarrinhoAbandonado, trans))
			{
				CurriculoCarrinhoAbandonado objCurriculoCarrinhoAbandonado = new CurriculoCarrinhoAbandonado();
				if (SetInstance(dr, objCurriculoCarrinhoAbandonado))
					return objCurriculoCarrinhoAbandonado;
			}
			throw (new RecordNotFoundException(typeof(CurriculoCarrinhoAbandonado)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoCarrinhoAbandonado a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoCarrinhoAbandonado))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoCarrinhoAbandonado a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoCarrinhoAbandonado, trans))
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
		/// <param name="objCurriculoCarrinhoAbandonado">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, CurriculoCarrinhoAbandonado objCurriculoCarrinhoAbandonado, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objCurriculoCarrinhoAbandonado._idCurriculoCarrinhoAbandonado = Convert.ToInt32(dr["Idf_Curriculo_Carrinho_Abandonado"]);
					objCurriculoCarrinhoAbandonado._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objCurriculoCarrinhoAbandonado._codigoDesconto = new CodigoDesconto(Convert.ToInt32(dr["Idf_Codigo_Desconto"]));
					objCurriculoCarrinhoAbandonado._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objCurriculoCarrinhoAbandonado._persisted = true;
					objCurriculoCarrinhoAbandonado._modified = false;

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