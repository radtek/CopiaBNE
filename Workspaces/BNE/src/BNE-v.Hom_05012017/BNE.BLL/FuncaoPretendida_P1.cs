//-- Data: 15/04/2010 12:56
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class FuncaoPretendida // Tabela: BNE_Funcao_Pretendida
	{
		#region Atributos
		private int _idFuncaoPretendida;
		private Curriculo _curriculo;
		private Funcao _funcao;
		private Int16? _quantidadeExperiencia;
		private DateTime _dataCadastro;
		private string _descricaoFuncaoPretendida;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdFuncaoPretendida
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdFuncaoPretendida
		{
			get
			{
				return this._idFuncaoPretendida;
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

		#region Funcao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Funcao Funcao
		{
			get
			{
				return this._funcao;
			}
			set
			{
				this._funcao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeExperiencia
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? QuantidadeExperiencia
		{
			get
			{
				return this._quantidadeExperiencia;
			}
			set
			{
				this._quantidadeExperiencia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoFuncaoPretendida
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoFuncaoPretendida
		{
			get
			{
				return this._descricaoFuncaoPretendida;
			}
			set
			{
				this._descricaoFuncaoPretendida = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public FuncaoPretendida()
		{
		}
		public FuncaoPretendida(int idFuncaoPretendida)
		{
			this._idFuncaoPretendida = idFuncaoPretendida;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Funcao_Pretendida (Idf_Curriculo, Idf_Funcao, Qtd_Experiencia, Dta_Cadastro, Des_Funcao_Pretendida) VALUES (@Idf_Curriculo, @Idf_Funcao, @Qtd_Experiencia, @Dta_Cadastro, @Des_Funcao_Pretendida);SET @Idf_Funcao_Pretendida = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Funcao_Pretendida SET Idf_Curriculo = @Idf_Curriculo, Idf_Funcao = @Idf_Funcao, Qtd_Experiencia = @Qtd_Experiencia, Dta_Cadastro = @Dta_Cadastro, Des_Funcao_Pretendida = @Des_Funcao_Pretendida WHERE Idf_Funcao_Pretendida = @Idf_Funcao_Pretendida";
		private const string SPDELETE = "DELETE FROM BNE_Funcao_Pretendida WHERE Idf_Funcao_Pretendida = @Idf_Funcao_Pretendida";
		private const string SPSELECTID = "SELECT * FROM BNE_Funcao_Pretendida WHERE Idf_Funcao_Pretendida = @Idf_Funcao_Pretendida";
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
			parms.Add(new SqlParameter("@Idf_Funcao_Pretendida", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Qtd_Experiencia", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Funcao_Pretendida", SqlDbType.VarChar, 50));
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
			parms[0].Value = this._idFuncaoPretendida;
			parms[1].Value = this._curriculo.IdCurriculo;

			if (this._funcao != null)
				parms[2].Value = this._funcao.IdFuncao;
			else
				parms[2].Value = DBNull.Value;


			if (this._quantidadeExperiencia.HasValue)
				parms[3].Value = this._quantidadeExperiencia;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoFuncaoPretendida))
				parms[5].Value = this._descricaoFuncaoPretendida;
			else
				parms[5].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de FuncaoPretendida no banco de dados.
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
						this._idFuncaoPretendida = Convert.ToInt32(cmd.Parameters["@Idf_Funcao_Pretendida"].Value);
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
		/// Método utilizado para inserir uma instância de FuncaoPretendida no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idFuncaoPretendida = Convert.ToInt32(cmd.Parameters["@Idf_Funcao_Pretendida"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de FuncaoPretendida no banco de dados.
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
		/// Método utilizado para atualizar uma instância de FuncaoPretendida no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de FuncaoPretendida no banco de dados.
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
		/// Método utilizado para salvar uma instância de FuncaoPretendida no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de FuncaoPretendida no banco de dados.
		/// </summary>
		/// <param name="idFuncaoPretendida">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFuncaoPretendida)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Pretendida", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoPretendida;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de FuncaoPretendida no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoPretendida">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFuncaoPretendida, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Pretendida", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoPretendida;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de FuncaoPretendida no banco de dados.
		/// </summary>
		/// <param name="idFuncaoPretendida">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idFuncaoPretendida)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Funcao_Pretendida where Idf_Funcao_Pretendida in (";

			for (int i = 0; i < idFuncaoPretendida.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idFuncaoPretendida[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFuncaoPretendida">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFuncaoPretendida)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Pretendida", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoPretendida;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoPretendida">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFuncaoPretendida, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Pretendida", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoPretendida;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Fun.Idf_Funcao_Pretendida, Fun.Idf_Curriculo, Fun.Idf_Funcao, Fun.Qtd_Experiencia, Fun.Dta_Cadastro, Fun.Des_Funcao_Pretendida FROM BNE_Funcao_Pretendida Fun";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de FuncaoPretendida a partir do banco de dados.
		/// </summary>
		/// <param name="idFuncaoPretendida">Chave do registro.</param>
		/// <returns>Instância de FuncaoPretendida.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static FuncaoPretendida LoadObject(int idFuncaoPretendida)
		{
			using (IDataReader dr = LoadDataReader(idFuncaoPretendida))
			{
				FuncaoPretendida objFuncaoPretendida = new FuncaoPretendida();
				if (SetInstance(dr, objFuncaoPretendida))
					return objFuncaoPretendida;
			}
			throw (new RecordNotFoundException(typeof(FuncaoPretendida)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de FuncaoPretendida a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoPretendida">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de FuncaoPretendida.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static FuncaoPretendida LoadObject(int idFuncaoPretendida, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idFuncaoPretendida, trans))
			{
				FuncaoPretendida objFuncaoPretendida = new FuncaoPretendida();
				if (SetInstance(dr, objFuncaoPretendida))
					return objFuncaoPretendida;
			}
			throw (new RecordNotFoundException(typeof(FuncaoPretendida)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de FuncaoPretendida a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idFuncaoPretendida))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de FuncaoPretendida a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idFuncaoPretendida, trans))
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
		/// <param name="objFuncaoPretendida">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, FuncaoPretendida objFuncaoPretendida)
		{
			try
			{
				if (dr.Read())
				{
					objFuncaoPretendida._idFuncaoPretendida = Convert.ToInt32(dr["Idf_Funcao_Pretendida"]);
					objFuncaoPretendida._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					if (dr["Idf_Funcao"] != DBNull.Value)
						objFuncaoPretendida._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
					if (dr["Qtd_Experiencia"] != DBNull.Value)
						objFuncaoPretendida._quantidadeExperiencia = Convert.ToInt16(dr["Qtd_Experiencia"]);
					objFuncaoPretendida._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Des_Funcao_Pretendida"] != DBNull.Value)
						objFuncaoPretendida._descricaoFuncaoPretendida = Convert.ToString(dr["Des_Funcao_Pretendida"]);

					objFuncaoPretendida._persisted = true;
					objFuncaoPretendida._modified = false;

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