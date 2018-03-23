//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PesquisaSalarial // Tabela: TAB_Pesquisa_Salarial
	{
		#region Atributos
		private int _idPesquisaSalarial;
		private Funcao _funcao;
		private Estado _estado;
		private decimal _valorMedia;
		private decimal _valorMaximo;
		private decimal _valorMinimo;
		private DateTime _dataAtualizacao;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPesquisaSalarial
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPesquisaSalarial
		{
			get
			{
				return this._idPesquisaSalarial;
			}
		}
		#endregion 

		#region Funcao
		/// <summary>
		/// Campo obrigatório.
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

		#region Estado
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Estado Estado
		{
			get
			{
				return this._estado;
			}
			set
			{
				this._estado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorMedia
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorMedia
		{
			get
			{
				return this._valorMedia;
			}
			set
			{
				this._valorMedia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorMaximo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorMaximo
		{
			get
			{
				return this._valorMaximo;
			}
			set
			{
				this._valorMaximo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorMinimo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorMinimo
		{
			get
			{
				return this._valorMinimo;
			}
			set
			{
				this._valorMinimo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataAtualizacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataAtualizacao
		{
			get
			{
				return this._dataAtualizacao;
			}
			set
			{
				this._dataAtualizacao = value;
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
		public PesquisaSalarial()
		{
		}
		public PesquisaSalarial(int idPesquisaSalarial)
		{
			this._idPesquisaSalarial = idPesquisaSalarial;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pesquisa_Salarial (Idf_Funcao, Idf_Estado, Vlr_Media, Vlr_Maximo, Vlr_Minimo, Dta_Atualizacao, Dta_Cadastro) VALUES (@Idf_Funcao, @Idf_Estado, @Vlr_Media, @Vlr_Maximo, @Vlr_Minimo, @Dta_Atualizacao, @Dta_Cadastro);SET @Idf_Pesquisa_Salarial = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pesquisa_Salarial SET Idf_Funcao = @Idf_Funcao, Idf_Estado = @Idf_Estado, Vlr_Media = @Vlr_Media, Vlr_Maximo = @Vlr_Maximo, Vlr_Minimo = @Vlr_Minimo, Dta_Atualizacao = @Dta_Atualizacao, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Pesquisa_Salarial = @Idf_Pesquisa_Salarial";
		private const string SPDELETE = "DELETE FROM TAB_Pesquisa_Salarial WHERE Idf_Pesquisa_Salarial = @Idf_Pesquisa_Salarial";
		private const string SPSELECTID = "SELECT * FROM TAB_Pesquisa_Salarial WHERE Idf_Pesquisa_Salarial = @Idf_Pesquisa_Salarial";
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
			parms.Add(new SqlParameter("@Idf_Pesquisa_Salarial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Estado", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Vlr_Media", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Vlr_Maximo", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Vlr_Minimo", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Dta_Atualizacao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idPesquisaSalarial;
			parms[1].Value = this._funcao.IdFuncao;
			parms[2].Value = this._estado.IdEstado;
			parms[3].Value = this._valorMedia;
			parms[4].Value = this._valorMaximo;
			parms[5].Value = this._valorMinimo;
			parms[6].Value = this._dataAtualizacao;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[7].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PesquisaSalarial no banco de dados.
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
						this._idPesquisaSalarial = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Salarial"].Value);
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
		/// Método utilizado para inserir uma instância de PesquisaSalarial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPesquisaSalarial = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Salarial"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PesquisaSalarial no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PesquisaSalarial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PesquisaSalarial no banco de dados.
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
		/// Método utilizado para salvar uma instância de PesquisaSalarial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PesquisaSalarial no banco de dados.
		/// </summary>
		/// <param name="idPesquisaSalarial">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPesquisaSalarial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Salarial", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaSalarial;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PesquisaSalarial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaSalarial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPesquisaSalarial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Salarial", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaSalarial;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PesquisaSalarial no banco de dados.
		/// </summary>
		/// <param name="idPesquisaSalarial">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPesquisaSalarial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pesquisa_Salarial where Idf_Pesquisa_Salarial in (";

			for (int i = 0; i < idPesquisaSalarial.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPesquisaSalarial[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPesquisaSalarial">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPesquisaSalarial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Salarial", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaSalarial;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaSalarial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPesquisaSalarial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Salarial", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaSalarial;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pesquisa_Salarial, Pes.Idf_Funcao, Pes.Idf_Estado, Pes.Vlr_Media, Pes.Vlr_Maximo, Pes.Vlr_Minimo, Pes.Dta_Atualizacao, Pes.Dta_Cadastro FROM TAB_Pesquisa_Salarial Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaSalarial a partir do banco de dados.
		/// </summary>
		/// <param name="idPesquisaSalarial">Chave do registro.</param>
		/// <returns>Instância de PesquisaSalarial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PesquisaSalarial LoadObject(int idPesquisaSalarial)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaSalarial))
			{
				PesquisaSalarial objPesquisaSalarial = new PesquisaSalarial();
				if (SetInstance(dr, objPesquisaSalarial))
					return objPesquisaSalarial;
			}
			throw (new RecordNotFoundException(typeof(PesquisaSalarial)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaSalarial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaSalarial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PesquisaSalarial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PesquisaSalarial LoadObject(int idPesquisaSalarial, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaSalarial, trans))
			{
				PesquisaSalarial objPesquisaSalarial = new PesquisaSalarial();
				if (SetInstance(dr, objPesquisaSalarial))
					return objPesquisaSalarial;
			}
			throw (new RecordNotFoundException(typeof(PesquisaSalarial)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaSalarial a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaSalarial))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaSalarial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaSalarial, trans))
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
		/// <param name="objPesquisaSalarial">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PesquisaSalarial objPesquisaSalarial)
		{
			try
			{
				if (dr.Read())
				{
					objPesquisaSalarial._idPesquisaSalarial = Convert.ToInt32(dr["Idf_Pesquisa_Salarial"]);
					objPesquisaSalarial._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
                    if (!string.IsNullOrEmpty(dr["Idf_Estado"].ToString())) 
                            objPesquisaSalarial._estado = new Estado(Convert.ToInt32(dr["Idf_Estado"]));                   
					
                    objPesquisaSalarial._valorMedia = Convert.ToDecimal(dr["Vlr_Media"]);
					objPesquisaSalarial._valorMaximo = Convert.ToDecimal(dr["Vlr_Maximo"]);
					objPesquisaSalarial._valorMinimo = Convert.ToDecimal(dr["Vlr_Minimo"]);
					objPesquisaSalarial._dataAtualizacao = Convert.ToDateTime(dr["Dta_Atualizacao"]);
					objPesquisaSalarial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objPesquisaSalarial._persisted = true;
					objPesquisaSalarial._modified = false;

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