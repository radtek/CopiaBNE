//-- Data: 18/11/2010 13:28
//-- Autor: Vinicius Maciel

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class VagaTipoVinculo // Tabela: BNE_Vaga_Tipo_Vinculo
	{
		#region Atributos
		private int _idVagaTipoVinculo;
		private TipoVinculo _tipoVinculo;
		private Vaga _vaga;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdVagaTipoVinculo
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdVagaTipoVinculo
		{
			get
			{
				return this._idVagaTipoVinculo;
			}
		}
		#endregion 

		#region TipoVinculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoVinculo TipoVinculo
		{
			get
			{
				return this._tipoVinculo;
			}
			set
			{
				this._tipoVinculo = value;
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

		#endregion

		#region Construtores
		public VagaTipoVinculo()
		{
		}
		public VagaTipoVinculo(int idVagaTipoVinculo)
		{
			this._idVagaTipoVinculo = idVagaTipoVinculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Vaga_Tipo_Vinculo (Idf_Tipo_Vinculo, Idf_Vaga, Dta_Cadastro) VALUES (@Idf_Tipo_Vinculo, @Idf_Vaga, @Dta_Cadastro);SET @Idf_Vaga_Tipo_Vinculo = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Vaga_Tipo_Vinculo SET Idf_Tipo_Vinculo = @Idf_Tipo_Vinculo, Idf_Vaga = @Idf_Vaga, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Vaga_Tipo_Vinculo = @Idf_Vaga_Tipo_Vinculo";
		private const string SPDELETE = "DELETE FROM BNE_Vaga_Tipo_Vinculo WHERE Idf_Vaga_Tipo_Vinculo = @Idf_Vaga_Tipo_Vinculo";
		private const string SPSELECTID = "SELECT * FROM BNE_Vaga_Tipo_Vinculo WHERE Idf_Vaga_Tipo_Vinculo = @Idf_Vaga_Tipo_Vinculo";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Tipo_Vinculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Vinculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Vinicius Maciel</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idVagaTipoVinculo;
			parms[1].Value = this._tipoVinculo.IdTipoVinculo;
			parms[2].Value = this._vaga.IdVaga;

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
		/// Método utilizado para inserir uma instância de VagaTipoVinculo no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
						this._idVagaTipoVinculo = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Tipo_Vinculo"].Value);
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
		/// Método utilizado para inserir uma instância de VagaTipoVinculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idVagaTipoVinculo = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Tipo_Vinculo"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de VagaTipoVinculo no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para atualizar uma instância de VagaTipoVinculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para salvar uma instância de VagaTipoVinculo no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de VagaTipoVinculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para excluir uma instância de VagaTipoVinculo no banco de dados.
		/// </summary>
		/// <param name="idVagaTipoVinculo">Chave do registro.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idVagaTipoVinculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Tipo_Vinculo", SqlDbType.Int, 4));

			parms[0].Value = idVagaTipoVinculo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de VagaTipoVinculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaTipoVinculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idVagaTipoVinculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Tipo_Vinculo", SqlDbType.Int, 4));

			parms[0].Value = idVagaTipoVinculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de VagaTipoVinculo no banco de dados.
		/// </summary>
		/// <param name="idVagaTipoVinculo">Lista de chaves.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(List<int> idVagaTipoVinculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Vaga_Tipo_Vinculo where Idf_Vaga_Tipo_Vinculo in (";

			for (int i = 0; i < idVagaTipoVinculo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idVagaTipoVinculo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idVagaTipoVinculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idVagaTipoVinculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Tipo_Vinculo", SqlDbType.Int, 4));

			parms[0].Value = idVagaTipoVinculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaTipoVinculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idVagaTipoVinculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Tipo_Vinculo", SqlDbType.Int, 4));

			parms[0].Value = idVagaTipoVinculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Vag.Idf_Vaga_Tipo_Vinculo, Vag.Idf_Tipo_Vinculo, Vag.Idf_Vaga, Vag.Dta_Cadastro FROM BNE_Vaga_Tipo_Vinculo Vag";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaTipoVinculo a partir do banco de dados.
		/// </summary>
		/// <param name="idVagaTipoVinculo">Chave do registro.</param>
		/// <returns>Instância de VagaTipoVinculo.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static VagaTipoVinculo LoadObject(int idVagaTipoVinculo)
		{
			using (IDataReader dr = LoadDataReader(idVagaTipoVinculo))
			{
				VagaTipoVinculo objVagaTipoVinculo = new VagaTipoVinculo();
				if (SetInstance(dr, objVagaTipoVinculo))
					return objVagaTipoVinculo;
			}
			throw (new RecordNotFoundException(typeof(VagaTipoVinculo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaTipoVinculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaTipoVinculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de VagaTipoVinculo.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static VagaTipoVinculo LoadObject(int idVagaTipoVinculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idVagaTipoVinculo, trans))
			{
				VagaTipoVinculo objVagaTipoVinculo = new VagaTipoVinculo();
				if (SetInstance(dr, objVagaTipoVinculo))
					return objVagaTipoVinculo;
			}
			throw (new RecordNotFoundException(typeof(VagaTipoVinculo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de VagaTipoVinculo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idVagaTipoVinculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de VagaTipoVinculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idVagaTipoVinculo, trans))
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
		/// <param name="objVagaTipoVinculo">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static bool SetInstance(IDataReader dr, VagaTipoVinculo objVagaTipoVinculo, bool disposeDataReader = true)
		{
			try
			{
				if (dr.Read())
				{
					objVagaTipoVinculo._idVagaTipoVinculo = Convert.ToInt32(dr["Idf_Vaga_Tipo_Vinculo"]);
					objVagaTipoVinculo._tipoVinculo = new TipoVinculo(Convert.ToInt32(dr["Idf_Tipo_Vinculo"]));
				    if (dr["Des_Tipo_Vinculo"] != null)
				        objVagaTipoVinculo._tipoVinculo.DescricaoTipoVinculo = dr["Des_Tipo_Vinculo"].ToString();
                    if (dr["Cod_Grau_Tipo_Vinculo"] != null)
                        objVagaTipoVinculo._tipoVinculo.CodigoGrauTipoVinculo = Convert.ToInt32(dr["Cod_Grau_Tipo_Vinculo"]);
                    objVagaTipoVinculo._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					objVagaTipoVinculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objVagaTipoVinculo._persisted = true;
					objVagaTipoVinculo._modified = false;

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
                if (disposeDataReader)
				    dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}