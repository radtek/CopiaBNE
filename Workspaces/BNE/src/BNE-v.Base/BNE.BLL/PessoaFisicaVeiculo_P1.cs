//-- Data: 03/09/2010 17:40
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PessoaFisicaVeiculo // Tabela: TAB_Pessoa_Fisica_Veiculo
	{
		#region Atributos
		private int _idVeiculo;
		private PessoaFisica _pessoaFisica;
		private TipoVeiculo _tipoVeiculo;
		private string _descricaoModelo;
		private Int16 _anoVeiculo;
		private DateTime _dataCadastro;
		private bool _flagInativo;
		private bool? _flagVeiculoProprio;
		private string _descricaoplacaVeiculo;
		private string _numeroRenavam;
		private bool? _flagSeguroVeiculo;
		private DateTime? _dataVencimentoSeguro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdVeiculo
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdVeiculo
		{
			get
			{
				return this._idVeiculo;
			}
		}
		#endregion 

		#region PessoaFisica
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public PessoaFisica PessoaFisica
		{
			get
			{
				return this._pessoaFisica;
			}
			set
			{
				this._pessoaFisica = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoVeiculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoVeiculo TipoVeiculo
		{
			get
			{
				return this._tipoVeiculo;
			}
			set
			{
				this._tipoVeiculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoModelo
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoModelo
		{
			get
			{
				return this._descricaoModelo;
			}
			set
			{
				this._descricaoModelo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region AnoVeiculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Int16 AnoVeiculo
		{
			get
			{
				return this._anoVeiculo;
			}
			set
			{
				this._anoVeiculo = value;
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

		#region FlagVeiculoProprio
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagVeiculoProprio
		{
			get
			{
				return this._flagVeiculoProprio;
			}
			set
			{
				this._flagVeiculoProprio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoplacaVeiculo
		/// <summary>
		/// Tamanho do campo: 7.
		/// Campo opcional.
		/// </summary>
		public string DescricaoplacaVeiculo
		{
			get
			{
				return this._descricaoplacaVeiculo;
			}
			set
			{
				this._descricaoplacaVeiculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroRenavam
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
		/// </summary>
		public string NumeroRenavam
		{
			get
			{
				return this._numeroRenavam;
			}
			set
			{
				this._numeroRenavam = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagSeguroVeiculo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagSeguroVeiculo
		{
			get
			{
				return this._flagSeguroVeiculo;
			}
			set
			{
				this._flagSeguroVeiculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataVencimentoSeguro
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataVencimentoSeguro
		{
			get
			{
				return this._dataVencimentoSeguro;
			}
			set
			{
				this._dataVencimentoSeguro = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PessoaFisicaVeiculo()
		{
		}
		public PessoaFisicaVeiculo(int idVeiculo)
		{
			this._idVeiculo = idVeiculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pessoa_Fisica_Veiculo (Idf_Pessoa_Fisica, Idf_Tipo_Veiculo, Des_Modelo, Ano_Veiculo, Dta_Cadastro, Flg_Inativo, Flg_Veiculo_Proprio, Des_placa_Veiculo, Num_Renavam, Flg_Seguro_Veiculo, Dta_Vencimento_Seguro) VALUES (@Idf_Pessoa_Fisica, @Idf_Tipo_Veiculo, @Des_Modelo, @Ano_Veiculo, @Dta_Cadastro, @Flg_Inativo, @Flg_Veiculo_Proprio, @Des_placa_Veiculo, @Num_Renavam, @Flg_Seguro_Veiculo, @Dta_Vencimento_Seguro);SET @Idf_Veiculo = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pessoa_Fisica_Veiculo SET Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica, Idf_Tipo_Veiculo = @Idf_Tipo_Veiculo, Des_Modelo = @Des_Modelo, Ano_Veiculo = @Ano_Veiculo, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Flg_Veiculo_Proprio = @Flg_Veiculo_Proprio, Des_placa_Veiculo = @Des_placa_Veiculo, Num_Renavam = @Num_Renavam, Flg_Seguro_Veiculo = @Flg_Seguro_Veiculo, Dta_Vencimento_Seguro = @Dta_Vencimento_Seguro WHERE Idf_Veiculo = @Idf_Veiculo";
		private const string SPDELETE = "DELETE FROM TAB_Pessoa_Fisica_Veiculo WHERE Idf_Veiculo = @Idf_Veiculo";
		private const string SPSELECTID = "SELECT * FROM TAB_Pessoa_Fisica_Veiculo WHERE Idf_Veiculo = @Idf_Veiculo";
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
			parms.Add(new SqlParameter("@Idf_Veiculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Veiculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Modelo", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Ano_Veiculo", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Veiculo_Proprio", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_placa_Veiculo", SqlDbType.Char, 7));
			parms.Add(new SqlParameter("@Num_Renavam", SqlDbType.VarChar, 15));
			parms.Add(new SqlParameter("@Flg_Seguro_Veiculo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Vencimento_Seguro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idVeiculo;
			parms[1].Value = this._pessoaFisica.IdPessoaFisica;
			parms[2].Value = this._tipoVeiculo.IdTipoVeiculo;

			if (!String.IsNullOrEmpty(this._descricaoModelo))
				parms[3].Value = this._descricaoModelo;
			else
				parms[3].Value = DBNull.Value;

			parms[4].Value = this._anoVeiculo;
			parms[6].Value = this._flagInativo;

			if (this._flagVeiculoProprio.HasValue)
				parms[7].Value = this._flagVeiculoProprio;
			else
				parms[7].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoplacaVeiculo))
				parms[8].Value = this._descricaoplacaVeiculo;
			else
				parms[8].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroRenavam))
				parms[9].Value = this._numeroRenavam;
			else
				parms[9].Value = DBNull.Value;


			if (this._flagSeguroVeiculo.HasValue)
				parms[10].Value = this._flagSeguroVeiculo;
			else
				parms[10].Value = DBNull.Value;


			if (this._dataVencimentoSeguro.HasValue)
				parms[11].Value = this._dataVencimentoSeguro;
			else
				parms[11].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[5].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PessoaFisicaVeiculo no banco de dados.
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
						this._idVeiculo = Convert.ToInt32(cmd.Parameters["@Idf_Veiculo"].Value);
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
		/// Método utilizado para inserir uma instância de PessoaFisicaVeiculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idVeiculo = Convert.ToInt32(cmd.Parameters["@Idf_Veiculo"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PessoaFisicaVeiculo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PessoaFisicaVeiculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PessoaFisicaVeiculo no banco de dados.
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
		/// Método utilizado para salvar uma instância de PessoaFisicaVeiculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PessoaFisicaVeiculo no banco de dados.
		/// </summary>
		/// <param name="idVeiculo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idVeiculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Veiculo", SqlDbType.Int, 4));

			parms[0].Value = idVeiculo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PessoaFisicaVeiculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVeiculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idVeiculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Veiculo", SqlDbType.Int, 4));

			parms[0].Value = idVeiculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PessoaFisicaVeiculo no banco de dados.
		/// </summary>
		/// <param name="idVeiculo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idVeiculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pessoa_Fisica_Veiculo where Idf_Veiculo in (";

			for (int i = 0; i < idVeiculo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idVeiculo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idVeiculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idVeiculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Veiculo", SqlDbType.Int, 4));

			parms[0].Value = idVeiculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVeiculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idVeiculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Veiculo", SqlDbType.Int, 4));

			parms[0].Value = idVeiculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Veiculo, Pes.Idf_Pessoa_Fisica, Pes.Idf_Tipo_Veiculo, Pes.Des_Modelo, Pes.Ano_Veiculo, Pes.Dta_Cadastro, Pes.Flg_Inativo, Pes.Flg_Veiculo_Proprio, Pes.Des_placa_Veiculo, Pes.Num_Renavam, Pes.Flg_Seguro_Veiculo, Pes.Dta_Vencimento_Seguro FROM TAB_Pessoa_Fisica_Veiculo Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoaFisicaVeiculo a partir do banco de dados.
		/// </summary>
		/// <param name="idVeiculo">Chave do registro.</param>
		/// <returns>Instância de PessoaFisicaVeiculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoaFisicaVeiculo LoadObject(int idVeiculo)
		{
			using (IDataReader dr = LoadDataReader(idVeiculo))
			{
				PessoaFisicaVeiculo objPessoaFisicaVeiculo = new PessoaFisicaVeiculo();
				if (SetInstance(dr, objPessoaFisicaVeiculo))
					return objPessoaFisicaVeiculo;
			}
			throw (new RecordNotFoundException(typeof(PessoaFisicaVeiculo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoaFisicaVeiculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVeiculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PessoaFisicaVeiculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoaFisicaVeiculo LoadObject(int idVeiculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idVeiculo, trans))
			{
				PessoaFisicaVeiculo objPessoaFisicaVeiculo = new PessoaFisicaVeiculo();
				if (SetInstance(dr, objPessoaFisicaVeiculo))
					return objPessoaFisicaVeiculo;
			}
			throw (new RecordNotFoundException(typeof(PessoaFisicaVeiculo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PessoaFisicaVeiculo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idVeiculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PessoaFisicaVeiculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idVeiculo, trans))
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
		/// <param name="objPessoaFisicaVeiculo">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PessoaFisicaVeiculo objPessoaFisicaVeiculo)
		{
			try
			{
				if (dr.Read())
				{
					objPessoaFisicaVeiculo._idVeiculo = Convert.ToInt32(dr["Idf_Veiculo"]);
					objPessoaFisicaVeiculo._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
					objPessoaFisicaVeiculo._tipoVeiculo = new TipoVeiculo(Convert.ToInt32(dr["Idf_Tipo_Veiculo"]));
					if (dr["Des_Modelo"] != DBNull.Value)
						objPessoaFisicaVeiculo._descricaoModelo = Convert.ToString(dr["Des_Modelo"]);
					objPessoaFisicaVeiculo._anoVeiculo = Convert.ToInt16(dr["Ano_Veiculo"]);
					objPessoaFisicaVeiculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objPessoaFisicaVeiculo._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					if (dr["Flg_Veiculo_Proprio"] != DBNull.Value)
						objPessoaFisicaVeiculo._flagVeiculoProprio = Convert.ToBoolean(dr["Flg_Veiculo_Proprio"]);
					if (dr["Des_placa_Veiculo"] != DBNull.Value)
						objPessoaFisicaVeiculo._descricaoplacaVeiculo = Convert.ToString(dr["Des_placa_Veiculo"]);
					if (dr["Num_Renavam"] != DBNull.Value)
						objPessoaFisicaVeiculo._numeroRenavam = Convert.ToString(dr["Num_Renavam"]);
					if (dr["Flg_Seguro_Veiculo"] != DBNull.Value)
						objPessoaFisicaVeiculo._flagSeguroVeiculo = Convert.ToBoolean(dr["Flg_Seguro_Veiculo"]);
					if (dr["Dta_Vencimento_Seguro"] != DBNull.Value)
						objPessoaFisicaVeiculo._dataVencimentoSeguro = Convert.ToDateTime(dr["Dta_Vencimento_Seguro"]);

					objPessoaFisicaVeiculo._persisted = true;
					objPessoaFisicaVeiculo._modified = false;

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