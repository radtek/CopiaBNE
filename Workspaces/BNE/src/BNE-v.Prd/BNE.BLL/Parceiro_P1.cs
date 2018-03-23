//-- Data: 25/07/2013 18:01
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Parceiro // Tabela: BNE_Parceiro
	{
		#region Atributos
		private int _idParceiro;
		private TipoParceiro _tipoParceiro;
		private string _descricaoParceiro;
		private bool _flagEmpresa;
		private decimal? _numeroCPF;
		private decimal? _numeroCNPJ;
		private string _descricaoEmail;
		private DateTime _dataCadastro;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdParceiro
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdParceiro
		{
			get
			{
				return this._idParceiro;
			}
		}
		#endregion 

		#region TipoParceiro
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoParceiro TipoParceiro
		{
			get
			{
				return this._tipoParceiro;
			}
			set
			{
				this._tipoParceiro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoParceiro
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoParceiro
		{
			get
			{
				return this._descricaoParceiro;
			}
			set
			{
				this._descricaoParceiro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagEmpresa
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagEmpresa
		{
			get
			{
				return this._flagEmpresa;
			}
			set
			{
				this._flagEmpresa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCPF
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroCPF
		{
			get
			{
				return this._numeroCPF;
			}
			set
			{
				this._numeroCPF = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCNPJ
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroCNPJ
		{
			get
			{
				return this._numeroCNPJ;
			}
			set
			{
				this._numeroCNPJ = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoEmail
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string DescricaoEmail
		{
			get
			{
				return this._descricaoEmail;
			}
			set
			{
				this._descricaoEmail = value;
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
		public Parceiro()
		{
		}
		public Parceiro(int idParceiro)
		{
			this._idParceiro = idParceiro;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Parceiro (Idf_Tipo_Parceiro, Des_Parceiro, Flg_Empresa, Num_CPF, Num_CNPJ, Des_Email, Dta_Cadastro, Flg_Inativo) VALUES (@Idf_Tipo_Parceiro, @Des_Parceiro, @Flg_Empresa, @Num_CPF, @Num_CNPJ, @Des_Email, @Dta_Cadastro, @Flg_Inativo);SET @Idf_Parceiro = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Parceiro SET Idf_Tipo_Parceiro = @Idf_Tipo_Parceiro, Des_Parceiro = @Des_Parceiro, Flg_Empresa = @Flg_Empresa, Num_CPF = @Num_CPF, Num_CNPJ = @Num_CNPJ, Des_Email = @Des_Email, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Parceiro = @Idf_Parceiro";
		private const string SPDELETE = "DELETE FROM BNE_Parceiro WHERE Idf_Parceiro = @Idf_Parceiro";
		private const string SPSELECTID = "SELECT * FROM BNE_Parceiro WITH(NOLOCK) WHERE Idf_Parceiro = @Idf_Parceiro";
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
			parms.Add(new SqlParameter("@Idf_Parceiro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Parceiro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Parceiro", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Flg_Empresa", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Num_CNPJ", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Des_Email", SqlDbType.VarChar, 200));
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
			parms[0].Value = this._idParceiro;
			parms[1].Value = this._tipoParceiro.IdTipoParceiro;
			parms[2].Value = this._descricaoParceiro;
			parms[3].Value = this._flagEmpresa;

			if (this._numeroCPF.HasValue)
				parms[4].Value = this._numeroCPF;
			else
				parms[4].Value = DBNull.Value;


			if (this._numeroCNPJ.HasValue)
				parms[5].Value = this._numeroCNPJ;
			else
				parms[5].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoEmail))
				parms[6].Value = this._descricaoEmail;
			else
				parms[6].Value = DBNull.Value;

			parms[8].Value = this._flagInativo;

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
		/// Método utilizado para inserir uma instância de Parceiro no banco de dados.
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
						this._idParceiro = Convert.ToInt32(cmd.Parameters["@Idf_Parceiro"].Value);
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
		/// Método utilizado para inserir uma instância de Parceiro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idParceiro = Convert.ToInt32(cmd.Parameters["@Idf_Parceiro"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Parceiro no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Parceiro no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Parceiro no banco de dados.
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
		/// Método utilizado para salvar uma instância de Parceiro no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Parceiro no banco de dados.
		/// </summary>
		/// <param name="idParceiro">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idParceiro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parceiro", SqlDbType.Int, 4));

			parms[0].Value = idParceiro;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Parceiro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParceiro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idParceiro, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parceiro", SqlDbType.Int, 4));

			parms[0].Value = idParceiro;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Parceiro no banco de dados.
		/// </summary>
		/// <param name="idParceiro">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idParceiro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Parceiro where Idf_Parceiro in (";

			for (int i = 0; i < idParceiro.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idParceiro[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idParceiro">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idParceiro)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parceiro", SqlDbType.Int, 4));

			parms[0].Value = idParceiro;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParceiro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idParceiro, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parceiro", SqlDbType.Int, 4));

			parms[0].Value = idParceiro;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Par.Idf_Parceiro, Par.Idf_Tipo_Parceiro, Par.Des_Parceiro, Par.Flg_Empresa, Par.Num_CPF, Par.Num_CNPJ, Par.Des_Email, Par.Dta_Cadastro, Par.Flg_Inativo FROM BNE_Parceiro Par";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Parceiro a partir do banco de dados.
		/// </summary>
		/// <param name="idParceiro">Chave do registro.</param>
		/// <returns>Instância de Parceiro.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Parceiro LoadObject(int idParceiro)
		{
			using (IDataReader dr = LoadDataReader(idParceiro))
			{
				Parceiro objParceiro = new Parceiro();
				if (SetInstance(dr, objParceiro))
					return objParceiro;
			}
			throw (new RecordNotFoundException(typeof(Parceiro)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Parceiro a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParceiro">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Parceiro.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Parceiro LoadObject(int idParceiro, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idParceiro, trans))
			{
				Parceiro objParceiro = new Parceiro();
				if (SetInstance(dr, objParceiro))
					return objParceiro;
			}
			throw (new RecordNotFoundException(typeof(Parceiro)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Parceiro a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idParceiro))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Parceiro a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idParceiro, trans))
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
		/// <param name="objParceiro">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Parceiro objParceiro)
		{
			try
			{
				if (dr.Read())
				{
					objParceiro._idParceiro = Convert.ToInt32(dr["Idf_Parceiro"]);
					objParceiro._tipoParceiro = new TipoParceiro(Convert.ToInt32(dr["Idf_Tipo_Parceiro"]));
					objParceiro._descricaoParceiro = Convert.ToString(dr["Des_Parceiro"]);
					objParceiro._flagEmpresa = Convert.ToBoolean(dr["Flg_Empresa"]);
					if (dr["Num_CPF"] != DBNull.Value)
						objParceiro._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
					if (dr["Num_CNPJ"] != DBNull.Value)
						objParceiro._numeroCNPJ = Convert.ToDecimal(dr["Num_CNPJ"]);
					if (dr["Des_Email"] != DBNull.Value)
						objParceiro._descricaoEmail = Convert.ToString(dr["Des_Email"]);
					objParceiro._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objParceiro._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objParceiro._persisted = true;
					objParceiro._modified = false;

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