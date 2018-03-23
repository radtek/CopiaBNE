//-- Data: 25/07/2013 18:01
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TipoCodigoDesconto // Tabela: BNE_Tipo_Codigo_Desconto
	{
		#region Atributos
		private int _idTipoCodigoDesconto;
		private string _descricaoTipoCodigoDesconto;
		private int _numeroPercentualDesconto;
		private int? _numeroDiasValidade;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTipoCodigoDesconto
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdTipoCodigoDesconto
		{
			get
			{
				return this._idTipoCodigoDesconto;
			}
		}
		#endregion 

		#region DescricaoTipoCodigoDesconto
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoTipoCodigoDesconto
		{
			get
			{
				return this._descricaoTipoCodigoDesconto;
			}
			set
			{
				this._descricaoTipoCodigoDesconto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroPercentualDesconto
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int NumeroPercentualDesconto
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

		#region NumeroDiasValidade
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? NumeroDiasValidade
		{
			get
			{
				return this._numeroDiasValidade;
			}
			set
			{
				this._numeroDiasValidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public TipoCodigoDesconto()
		{
		}
		public TipoCodigoDesconto(int idTipoCodigoDesconto)
		{
			this._idTipoCodigoDesconto = idTipoCodigoDesconto;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Tipo_Codigo_Desconto (Des_Tipo_Codigo_Desconto, Num_Percentual_Desconto, Num_Dias_Validade) VALUES (@Des_Tipo_Codigo_Desconto, @Num_Percentual_Desconto, @Num_Dias_Validade);SET @Idf_Tipo_Codigo_Desconto = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Tipo_Codigo_Desconto SET Des_Tipo_Codigo_Desconto = @Des_Tipo_Codigo_Desconto, Num_Percentual_Desconto = @Num_Percentual_Desconto, Num_Dias_Validade = @Num_Dias_Validade WHERE Idf_Tipo_Codigo_Desconto = @Idf_Tipo_Codigo_Desconto";
		private const string SPDELETE = "DELETE FROM BNE_Tipo_Codigo_Desconto WHERE Idf_Tipo_Codigo_Desconto = @Idf_Tipo_Codigo_Desconto";
		private const string SPSELECTID = "SELECT * FROM BNE_Tipo_Codigo_Desconto WITH(NOLOCK) WHERE Idf_Tipo_Codigo_Desconto = @Idf_Tipo_Codigo_Desconto";
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
			parms.Add(new SqlParameter("@Des_Tipo_Codigo_Desconto", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Num_Percentual_Desconto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_Dias_Validade", SqlDbType.Int, 4));
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
			parms[0].Value = this._idTipoCodigoDesconto;
			parms[1].Value = this._descricaoTipoCodigoDesconto;
			parms[2].Value = this._numeroPercentualDesconto;

			if (this._numeroDiasValidade.HasValue)
				parms[3].Value = this._numeroDiasValidade;
			else
				parms[3].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de TipoCodigoDesconto no banco de dados.
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
						this._idTipoCodigoDesconto = Convert.ToInt32(cmd.Parameters["@Idf_Tipo_Codigo_Desconto"].Value);
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
		/// Método utilizado para inserir uma instância de TipoCodigoDesconto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idTipoCodigoDesconto = Convert.ToInt32(cmd.Parameters["@Idf_Tipo_Codigo_Desconto"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de TipoCodigoDesconto no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TipoCodigoDesconto no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TipoCodigoDesconto no banco de dados.
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
		/// Método utilizado para salvar uma instância de TipoCodigoDesconto no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TipoCodigoDesconto no banco de dados.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoCodigoDesconto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Codigo_Desconto", SqlDbType.Int, 4));

			parms[0].Value = idTipoCodigoDesconto;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoCodigoDesconto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoCodigoDesconto, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Codigo_Desconto", SqlDbType.Int, 4));

			parms[0].Value = idTipoCodigoDesconto;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoCodigoDesconto no banco de dados.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idTipoCodigoDesconto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Tipo_Codigo_Desconto where Idf_Tipo_Codigo_Desconto in (";

			for (int i = 0; i < idTipoCodigoDesconto.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTipoCodigoDesconto[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoCodigoDesconto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Codigo_Desconto", SqlDbType.Int, 4));

			parms[0].Value = idTipoCodigoDesconto;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoCodigoDesconto, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Codigo_Desconto", SqlDbType.Int, 4));

			parms[0].Value = idTipoCodigoDesconto;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Codigo_Desconto, Tip.Des_Tipo_Codigo_Desconto, Tip.Num_Percentual_Desconto, Tip.Num_Dias_Validade FROM BNE_Tipo_Codigo_Desconto Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoCodigoDesconto a partir do banco de dados.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <returns>Instância de TipoCodigoDesconto.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoCodigoDesconto LoadObject(int idTipoCodigoDesconto)
		{
			using (IDataReader dr = LoadDataReader(idTipoCodigoDesconto))
			{
				TipoCodigoDesconto objTipoCodigoDesconto = new TipoCodigoDesconto();
				if (SetInstance(dr, objTipoCodigoDesconto))
					return objTipoCodigoDesconto;
			}
			throw (new RecordNotFoundException(typeof(TipoCodigoDesconto)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoCodigoDesconto a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoCodigoDesconto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoCodigoDesconto.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoCodigoDesconto LoadObject(int idTipoCodigoDesconto, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTipoCodigoDesconto, trans))
			{
				TipoCodigoDesconto objTipoCodigoDesconto = new TipoCodigoDesconto();
				if (SetInstance(dr, objTipoCodigoDesconto))
					return objTipoCodigoDesconto;
			}
			throw (new RecordNotFoundException(typeof(TipoCodigoDesconto)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoCodigoDesconto a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoCodigoDesconto))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoCodigoDesconto a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoCodigoDesconto, trans))
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
		/// <param name="objTipoCodigoDesconto">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, TipoCodigoDesconto objTipoCodigoDesconto)
		{
			try
			{
				if (dr.Read())
				{
					objTipoCodigoDesconto._idTipoCodigoDesconto = Convert.ToInt32(dr["Idf_Tipo_Codigo_Desconto"]);
					objTipoCodigoDesconto._descricaoTipoCodigoDesconto = Convert.ToString(dr["Des_Tipo_Codigo_Desconto"]);
					objTipoCodigoDesconto._numeroPercentualDesconto = Convert.ToInt32(dr["Num_Percentual_Desconto"]);
					if (dr["Num_Dias_Validade"] != DBNull.Value)
						objTipoCodigoDesconto._numeroDiasValidade = Convert.ToInt32(dr["Num_Dias_Validade"]);

					objTipoCodigoDesconto._persisted = true;
					objTipoCodigoDesconto._modified = false;

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