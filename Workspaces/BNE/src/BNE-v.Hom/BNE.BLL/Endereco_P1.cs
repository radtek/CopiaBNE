//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Endereco // Tabela: TAB_Endereco
	{
		#region Atributos
		private int _idEndereco;
		private string _numeroCEP;
		private string _descricaoLogradouro;
		private string _numeroEndereco;
		private string _descricaoComplemento;
		private string _descricaoBairro;
		private Cidade _cidade;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private DateTime? _dataAlteracao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdEndereco
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdEndereco
		{
			get
			{
				return this._idEndereco;
			}
		}
		#endregion 

		#region NumeroCEP
		/// <summary>
		/// Tamanho do campo: 8.
		/// Campo opcional.
		/// </summary>
		public string NumeroCEP
		{
			get
			{
				return this._numeroCEP;
			}
			set
			{
				this._numeroCEP = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoLogradouro
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoLogradouro
		{
			get
			{
				return this._descricaoLogradouro;
			}
			set
			{
				this._descricaoLogradouro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroEndereco
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
		/// </summary>
		public string NumeroEndereco
		{
			get
			{
				return this._numeroEndereco;
			}
			set
			{
				this._numeroEndereco = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoComplemento
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo opcional.
		/// </summary>
		public string DescricaoComplemento
		{
			get
			{
				return this._descricaoComplemento;
			}
			set
			{
				this._descricaoComplemento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoBairro
		/// <summary>
		/// Tamanho do campo: 80.
		/// Campo opcional.
		/// </summary>
		public string DescricaoBairro
		{
			get
			{
				return this._descricaoBairro;
			}
			set
			{
				this._descricaoBairro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Cidade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Cidade Cidade
		{
			get
			{
				return this._cidade;
			}
			set
			{
				this._cidade = value;
				this._modified = true;
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
		public Endereco()
		{
		}
		public Endereco(int idEndereco)
		{
			this._idEndereco = idEndereco;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Endereco (Num_CEP, Des_Logradouro, Num_Endereco, Des_Complemento, Des_Bairro, Idf_Cidade, Flg_Inativo, Dta_Cadastro, Dta_Alteracao) VALUES (@Num_CEP, @Des_Logradouro, @Num_Endereco, @Des_Complemento, @Des_Bairro, @Idf_Cidade, @Flg_Inativo, @Dta_Cadastro, @Dta_Alteracao);SET @Idf_Endereco = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Endereco SET Num_CEP = @Num_CEP, Des_Logradouro = @Des_Logradouro, Num_Endereco = @Num_Endereco, Des_Complemento = @Des_Complemento, Des_Bairro = @Des_Bairro, Idf_Cidade = @Idf_Cidade, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao WHERE Idf_Endereco = @Idf_Endereco";
		private const string SPDELETE = "DELETE FROM TAB_Endereco WHERE Idf_Endereco = @Idf_Endereco";
		private const string SPSELECTID = "SELECT * FROM TAB_Endereco WHERE Idf_Endereco = @Idf_Endereco";
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
			parms.Add(new SqlParameter("@Idf_Endereco", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_CEP", SqlDbType.Char, 8));
			parms.Add(new SqlParameter("@Des_Logradouro", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Num_Endereco", SqlDbType.VarChar, 15));
			parms.Add(new SqlParameter("@Des_Complemento", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Des_Bairro", SqlDbType.VarChar, 80));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idEndereco;

			if (!String.IsNullOrEmpty(this._numeroCEP))
				parms[1].Value = this._numeroCEP;
			else
				parms[1].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoLogradouro))
				parms[2].Value = this._descricaoLogradouro;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroEndereco))
				parms[3].Value = this._numeroEndereco;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoComplemento))
				parms[4].Value = this._descricaoComplemento;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoBairro))
				parms[5].Value = this._descricaoBairro;
			else
				parms[5].Value = DBNull.Value;

			parms[6].Value = this._cidade.IdCidade;
			parms[7].Value = this._flagInativo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[8].Value = this._dataCadastro;
			this._dataAlteracao = DateTime.Now;
			parms[9].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Endereco no banco de dados.
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
						this._idEndereco = Convert.ToInt32(cmd.Parameters["@Idf_Endereco"].Value);
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
		/// Método utilizado para inserir uma instância de Endereco no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idEndereco = Convert.ToInt32(cmd.Parameters["@Idf_Endereco"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Endereco no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Endereco no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Endereco no banco de dados.
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
		/// Método utilizado para salvar uma instância de Endereco no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Endereco no banco de dados.
		/// </summary>
		/// <param name="idEndereco">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEndereco)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Endereco", SqlDbType.Int, 4));

			parms[0].Value = idEndereco;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Endereco no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEndereco">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEndereco, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Endereco", SqlDbType.Int, 4));

			parms[0].Value = idEndereco;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Endereco no banco de dados.
		/// </summary>
		/// <param name="idEndereco">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idEndereco)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Endereco where Idf_Endereco in (";

			for (int i = 0; i < idEndereco.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idEndereco[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idEndereco">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEndereco)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Endereco", SqlDbType.Int, 4));

			parms[0].Value = idEndereco;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEndereco">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEndereco, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Endereco", SqlDbType.Int, 4));

			parms[0].Value = idEndereco;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, End.Idf_Endereco, End.Num_CEP, End.Des_Logradouro, End.Num_Endereco, End.Des_Complemento, End.Des_Bairro, End.Idf_Cidade, End.Flg_Inativo, End.Dta_Cadastro, End.Dta_Alteracao FROM TAB_Endereco End";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Endereco a partir do banco de dados.
		/// </summary>
		/// <param name="idEndereco">Chave do registro.</param>
		/// <returns>Instância de Endereco.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Endereco LoadObject(int idEndereco)
		{
			using (IDataReader dr = LoadDataReader(idEndereco))
			{
				Endereco objEndereco = new Endereco();
				if (SetInstance(dr, objEndereco))
					return objEndereco;
			}
			throw (new RecordNotFoundException(typeof(Endereco)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Endereco a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEndereco">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Endereco.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Endereco LoadObject(int idEndereco, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idEndereco, trans))
			{
				Endereco objEndereco = new Endereco();
				if (SetInstance(dr, objEndereco))
					return objEndereco;
			}
			throw (new RecordNotFoundException(typeof(Endereco)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Endereco a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idEndereco))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Endereco a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idEndereco, trans))
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
		/// <param name="objEndereco">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Endereco objEndereco)
		{
			try
			{
				if (dr.Read())
				{
					objEndereco._idEndereco = Convert.ToInt32(dr["Idf_Endereco"]);
					if (dr["Num_CEP"] != DBNull.Value)
						objEndereco._numeroCEP = Convert.ToString(dr["Num_CEP"]);
					if (dr["Des_Logradouro"] != DBNull.Value)
						objEndereco._descricaoLogradouro = Convert.ToString(dr["Des_Logradouro"]);
					if (dr["Num_Endereco"] != DBNull.Value)
						objEndereco._numeroEndereco = Convert.ToString(dr["Num_Endereco"]);
					if (dr["Des_Complemento"] != DBNull.Value)
						objEndereco._descricaoComplemento = Convert.ToString(dr["Des_Complemento"]);
					if (dr["Des_Bairro"] != DBNull.Value)
						objEndereco._descricaoBairro = Convert.ToString(dr["Des_Bairro"]);
					objEndereco._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					objEndereco._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objEndereco._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Dta_Alteracao"] != DBNull.Value)
						objEndereco._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);

					objEndereco._persisted = true;
					objEndereco._modified = false;

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