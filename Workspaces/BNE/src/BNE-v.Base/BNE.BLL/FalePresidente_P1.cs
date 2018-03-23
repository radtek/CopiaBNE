//-- Data: 31/01/2011 10:46
//-- Autor: Elias Junior

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class FalePresidente // Tabela: BNE_Fale_Presidente
	{
		#region Atributos
		private int _idFalePresidente;
		private string _nomeUsuario;
		private decimal? _numeroCPF;
		private string _emailPessoa;
		private string _descricaoAssunto;
		private string _descricaoMensagem;
		private DateTime? _dataCadastro;
		private bool? _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdFalePresidente
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdFalePresidente
		{
			get
			{
				return this._idFalePresidente;
			}
		}
		#endregion 

		#region NomeUsuario
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string NomeUsuario
		{
			get
			{
				return this._nomeUsuario;
			}
			set
			{
				this._nomeUsuario = value;
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

		#region EmailPessoa
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string EmailPessoa
		{
			get
			{
				return this._emailPessoa;
			}
			set
			{
				this._emailPessoa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoAssunto
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoAssunto
		{
			get
			{
				return this._descricaoAssunto;
			}
			set
			{
				this._descricaoAssunto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMensagem
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo opcional.
		/// </summary>
		public string DescricaoMensagem
		{
			get
			{
				return this._descricaoMensagem;
			}
			set
			{
				this._descricaoMensagem = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCadastro
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataCadastro
		{
			get
			{
				return this._dataCadastro;
			}
		}
		#endregion 

		#region FlagInativo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagInativo
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
		public FalePresidente()
		{
		}
		public FalePresidente(int idFalePresidente)
		{
			this._idFalePresidente = idFalePresidente;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Fale_Presidente (Nme_Usuario, Num_CPF, Eml_Pessoa, Des_Assunto, Des_Mensagem, Dta_Cadastro, Flg_Inativo) VALUES (@Nme_Usuario, @Num_CPF, @Eml_Pessoa, @Des_Assunto, @Des_Mensagem, @Dta_Cadastro, @Flg_Inativo);SET @Idf_Fale_Presidente = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Fale_Presidente SET Nme_Usuario = @Nme_Usuario, Num_CPF = @Num_CPF, Eml_Pessoa = @Eml_Pessoa, Des_Assunto = @Des_Assunto, Des_Mensagem = @Des_Mensagem, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Fale_Presidente = @Idf_Fale_Presidente";
		private const string SPDELETE = "DELETE FROM BNE_Fale_Presidente WHERE Idf_Fale_Presidente = @Idf_Fale_Presidente";
		private const string SPSELECTID = "SELECT * FROM BNE_Fale_Presidente WHERE Idf_Fale_Presidente = @Idf_Fale_Presidente";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Elias Junior</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Fale_Presidente", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Usuario", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Assunto", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Mensagem", SqlDbType.VarChar, 500));
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
		/// <remarks>Elias Junior</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idFalePresidente;

			if (!String.IsNullOrEmpty(this._nomeUsuario))
				parms[1].Value = this._nomeUsuario;
			else
				parms[1].Value = DBNull.Value;


			if (this._numeroCPF.HasValue)
				parms[2].Value = this._numeroCPF;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._emailPessoa))
				parms[3].Value = this._emailPessoa;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoAssunto))
				parms[4].Value = this._descricaoAssunto;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoMensagem))
				parms[5].Value = this._descricaoMensagem;
			else
				parms[5].Value = DBNull.Value;


			if (this._flagInativo.HasValue)
				parms[7].Value = this._flagInativo;
			else
				parms[7].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[6].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de FalePresidente no banco de dados.
		/// </summary>
		/// <remarks>Elias Junior</remarks>
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
						this._idFalePresidente = Convert.ToInt32(cmd.Parameters["@Idf_Fale_Presidente"].Value);
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
		/// Método utilizado para inserir uma instância de FalePresidente no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias Junior</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idFalePresidente = Convert.ToInt32(cmd.Parameters["@Idf_Fale_Presidente"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de FalePresidente no banco de dados.
		/// </summary>
		/// <remarks>Elias Junior</remarks>
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
		/// Método utilizado para atualizar uma instância de FalePresidente no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias Junior</remarks>
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
		/// Método utilizado para salvar uma instância de FalePresidente no banco de dados.
		/// </summary>
		/// <remarks>Elias Junior</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de FalePresidente no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias Junior</remarks>
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
		/// Método utilizado para excluir uma instância de FalePresidente no banco de dados.
		/// </summary>
		/// <param name="idFalePresidente">Chave do registro.</param>
		/// <remarks>Elias Junior</remarks>
		public static void Delete(int idFalePresidente)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Fale_Presidente", SqlDbType.Int, 4));

			parms[0].Value = idFalePresidente;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de FalePresidente no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFalePresidente">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias Junior</remarks>
		public static void Delete(int idFalePresidente, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Fale_Presidente", SqlDbType.Int, 4));

			parms[0].Value = idFalePresidente;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de FalePresidente no banco de dados.
		/// </summary>
		/// <param name="idFalePresidente">Lista de chaves.</param>
		/// <remarks>Elias Junior</remarks>
		public static void Delete(List<int> idFalePresidente)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Fale_Presidente where Idf_Fale_Presidente in (";

			for (int i = 0; i < idFalePresidente.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idFalePresidente[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFalePresidente">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Elias Junior</remarks>
		private static IDataReader LoadDataReader(int idFalePresidente)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Fale_Presidente", SqlDbType.Int, 4));

			parms[0].Value = idFalePresidente;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFalePresidente">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Elias Junior</remarks>
		private static IDataReader LoadDataReader(int idFalePresidente, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Fale_Presidente", SqlDbType.Int, 4));

			parms[0].Value = idFalePresidente;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Fal.Idf_Fale_Presidente, Fal.Nme_Usuario, Fal.Num_CPF, Fal.Eml_Pessoa, Fal.Des_Assunto, Fal.Des_Mensagem, Fal.Dta_Cadastro, Fal.Flg_Inativo FROM BNE_Fale_Presidente Fal";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de FalePresidente a partir do banco de dados.
		/// </summary>
		/// <param name="idFalePresidente">Chave do registro.</param>
		/// <returns>Instância de FalePresidente.</returns>
		/// <remarks>Elias Junior</remarks>
		public static FalePresidente LoadObject(int idFalePresidente)
		{
			using (IDataReader dr = LoadDataReader(idFalePresidente))
			{
				FalePresidente objFalePresidente = new FalePresidente();
				if (SetInstance(dr, objFalePresidente))
					return objFalePresidente;
			}
			throw (new RecordNotFoundException(typeof(FalePresidente)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de FalePresidente a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFalePresidente">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de FalePresidente.</returns>
		/// <remarks>Elias Junior</remarks>
		public static FalePresidente LoadObject(int idFalePresidente, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idFalePresidente, trans))
			{
				FalePresidente objFalePresidente = new FalePresidente();
				if (SetInstance(dr, objFalePresidente))
					return objFalePresidente;
			}
			throw (new RecordNotFoundException(typeof(FalePresidente)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de FalePresidente a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias Junior</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idFalePresidente))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de FalePresidente a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias Junior</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idFalePresidente, trans))
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
		/// <param name="objFalePresidente">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias Junior</remarks>
		private static bool SetInstance(IDataReader dr, FalePresidente objFalePresidente)
		{
			try
			{
				if (dr.Read())
				{
					objFalePresidente._idFalePresidente = Convert.ToInt32(dr["Idf_Fale_Presidente"]);
					if (dr["Nme_Usuario"] != DBNull.Value)
						objFalePresidente._nomeUsuario = Convert.ToString(dr["Nme_Usuario"]);
					if (dr["Num_CPF"] != DBNull.Value)
						objFalePresidente._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
					if (dr["Eml_Pessoa"] != DBNull.Value)
						objFalePresidente._emailPessoa = Convert.ToString(dr["Eml_Pessoa"]);
					if (dr["Des_Assunto"] != DBNull.Value)
						objFalePresidente._descricaoAssunto = Convert.ToString(dr["Des_Assunto"]);
					if (dr["Des_Mensagem"] != DBNull.Value)
						objFalePresidente._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objFalePresidente._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Flg_Inativo"] != DBNull.Value)
						objFalePresidente._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objFalePresidente._persisted = true;
					objFalePresidente._modified = false;

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