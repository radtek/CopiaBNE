//-- Data: 21/09/2010 15:33
//-- Autor: Bruno Flammarion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Estatistica // Tabela: BNE_Estatistica
	{
		#region Atributos
		private int _idEstatistica;
		private Int64 _quantidadeCurriculo;
		private DateTime _dataCadastro;
		private Int64? _quantidadeVaga;
		private Int64 _quantidadeEmpresa;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdEstatistica
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdEstatistica
		{
			get
			{
				return this._idEstatistica;
			}
		}
		#endregion 

		#region QuantidadeCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Int64 QuantidadeCurriculo
		{
			get
			{
				return this._quantidadeCurriculo;
			}
			set
			{
				this._quantidadeCurriculo = value;
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

		#region QuantidadeVaga
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int64? QuantidadeVaga
		{
			get
			{
				return this._quantidadeVaga;
			}
			set
			{
				this._quantidadeVaga = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeEmpresa
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Int64 QuantidadeEmpresa
		{
			get
			{
				return this._quantidadeEmpresa;
			}
			set
			{
				this._quantidadeEmpresa = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Estatistica()
		{
		}
		public Estatistica(int idEstatistica)
		{
			this._idEstatistica = idEstatistica;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Estatistica (Qtd_Curriculo, Dta_Cadastro, Qtd_Vaga, Qtd_Empresa) VALUES (@Qtd_Curriculo, @Dta_Cadastro, @Qtd_Vaga, @Qtd_Empresa);SET @Idf_Estatistica = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Estatistica SET Qtd_Curriculo = @Qtd_Curriculo, Dta_Cadastro = @Dta_Cadastro, Qtd_Vaga = @Qtd_Vaga, Qtd_Empresa = @Qtd_Empresa WHERE Idf_Estatistica = @Idf_Estatistica";
		private const string SPDELETE = "DELETE FROM BNE_Estatistica WHERE Idf_Estatistica = @Idf_Estatistica";
		private const string SPSELECTID = "SELECT * FROM BNE_Estatistica WHERE Idf_Estatistica = @Idf_Estatistica";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estatistica", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Qtd_Curriculo", SqlDbType.BigInt, 8));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Qtd_Vaga", SqlDbType.BigInt, 8));
			parms.Add(new SqlParameter("@Qtd_Empresa", SqlDbType.BigInt, 8));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Bruno Flammarion</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idEstatistica;
			parms[1].Value = this._quantidadeCurriculo;

			if (this._quantidadeVaga.HasValue)
				parms[3].Value = this._quantidadeVaga;
			else
				parms[3].Value = DBNull.Value;

			parms[4].Value = this._quantidadeEmpresa;

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
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Estatistica no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
						this._idEstatistica = Convert.ToInt32(cmd.Parameters["@Idf_Estatistica"].Value);
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
		/// Método utilizado para inserir uma instância de Estatistica no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idEstatistica = Convert.ToInt32(cmd.Parameters["@Idf_Estatistica"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Estatistica no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para atualizar uma instância de Estatistica no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para salvar uma instância de Estatistica no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de Estatistica no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para excluir uma instância de Estatistica no banco de dados.
		/// </summary>
		/// <param name="idEstatistica">Chave do registro.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idEstatistica)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estatistica", SqlDbType.Int, 4));

			parms[0].Value = idEstatistica;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Estatistica no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEstatistica">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idEstatistica, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estatistica", SqlDbType.Int, 4));

			parms[0].Value = idEstatistica;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Estatistica no banco de dados.
		/// </summary>
		/// <param name="idEstatistica">Lista de chaves.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(List<int> idEstatistica)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Estatistica where Idf_Estatistica in (";

			for (int i = 0; i < idEstatistica.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idEstatistica[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idEstatistica">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idEstatistica)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estatistica", SqlDbType.Int, 4));

			parms[0].Value = idEstatistica;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEstatistica">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idEstatistica, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estatistica", SqlDbType.Int, 4));

			parms[0].Value = idEstatistica;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Est.Idf_Estatistica, Est.Qtd_Curriculo, Est.Dta_Cadastro, Est.Qtd_Vaga, Est.Qtd_Empresa FROM BNE_Estatistica Est";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Estatistica a partir do banco de dados.
		/// </summary>
		/// <param name="idEstatistica">Chave do registro.</param>
		/// <returns>Instância de Estatistica.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static Estatistica LoadObject(int idEstatistica)
		{
			using (IDataReader dr = LoadDataReader(idEstatistica))
			{
				Estatistica objEstatistica = new Estatistica();
				if (SetInstance(dr, objEstatistica))
					return objEstatistica;
			}
			throw (new RecordNotFoundException(typeof(Estatistica)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Estatistica a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEstatistica">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Estatistica.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static Estatistica LoadObject(int idEstatistica, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idEstatistica, trans))
			{
				Estatistica objEstatistica = new Estatistica();
				if (SetInstance(dr, objEstatistica))
					return objEstatistica;
			}
			throw (new RecordNotFoundException(typeof(Estatistica)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Estatistica a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idEstatistica))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Estatistica a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idEstatistica, trans))
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
		/// <param name="objEstatistica">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static bool SetInstance(IDataReader dr, Estatistica objEstatistica)
		{
			try
			{
				if (dr.Read())
				{
					objEstatistica._idEstatistica = Convert.ToInt32(dr["Idf_Estatistica"]);
					objEstatistica._quantidadeCurriculo = Convert.ToInt64(dr["Qtd_Curriculo"]);
					objEstatistica._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Qtd_Vaga"] != DBNull.Value)
						objEstatistica._quantidadeVaga = Convert.ToInt64(dr["Qtd_Vaga"]);
					objEstatistica._quantidadeEmpresa = Convert.ToInt64(dr["Qtd_Empresa"]);

					objEstatistica._persisted = true;
					objEstatistica._modified = false;

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