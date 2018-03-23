//-- Data: 28/04/2010 08:41
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RedeSocialCS // Tabela: BNE_Rede_Social_CS
	{
		#region Atributos
		private int _idRedeSocialCS;
		private string _descricaoRedeSocial;
		private byte[] _imagemRedeSocial;
		private bool _flagInativo;
		private DateTime _dataAlteracao;
		private DateTime _dataCadastro;
		private int _MaxCaracter;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRedeSocialCS
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRedeSocialCS
		{
			get
			{
				return this._idRedeSocialCS;
			}
		}
		#endregion 

		#region DescricaoRedeSocial
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoRedeSocial
		{
			get
			{
				return this._descricaoRedeSocial;
			}
			set
			{
				this._descricaoRedeSocial = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ImagemRedeSocial
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public byte[] ImagemRedeSocial
		{
			get
			{
				return this._imagemRedeSocial;
			}
			set
			{
				this._imagemRedeSocial = value;
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

		#region DataAlteracao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataAlteracao
		{
			get
			{
				return this._dataAlteracao;
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

		#region MaxCaracter
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int MaxCaracter
		{
			get
			{
				return this._MaxCaracter;
			}
			set
			{
				this._MaxCaracter = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public RedeSocialCS()
		{
		}
		public RedeSocialCS(int idRedeSocialCS)
		{
			this._idRedeSocialCS = idRedeSocialCS;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Rede_Social_CS (Des_Rede_Social, Img_Rede_Social, Flg_Inativo, Dta_Alteracao, Dta_Cadastro, Max_Caracter) VALUES (@Des_Rede_Social, @Img_Rede_Social, @Flg_Inativo, @Dta_Alteracao, @Dta_Cadastro, @Max_Caracter);SET @Idf_Rede_Social_CS = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Rede_Social_CS SET Des_Rede_Social = @Des_Rede_Social, Img_Rede_Social = @Img_Rede_Social, Flg_Inativo = @Flg_Inativo, Dta_Alteracao = @Dta_Alteracao, Dta_Cadastro = @Dta_Cadastro, Max_Caracter = @Max_Caracter WHERE Idf_Rede_Social_CS = @Idf_Rede_Social_CS";
		private const string SPDELETE = "DELETE FROM BNE_Rede_Social_CS WHERE Idf_Rede_Social_CS = @Idf_Rede_Social_CS";
		private const string SPSELECTID = "SELECT * FROM BNE_Rede_Social_CS WHERE Idf_Rede_Social_CS = @Idf_Rede_Social_CS";
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
			parms.Add(new SqlParameter("@Idf_Rede_Social_CS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Rede_Social", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Img_Rede_Social", SqlDbType.VarBinary));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Max_Caracter", SqlDbType.Int, 4));
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
			parms[0].Value = this._idRedeSocialCS;
			parms[1].Value = this._descricaoRedeSocial;
			parms[2].Value = this._imagemRedeSocial;
			parms[3].Value = this._flagInativo;
			parms[6].Value = this._MaxCaracter;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			this._dataAlteracao = DateTime.Now;
			parms[4].Value = this._dataAlteracao;
			parms[5].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de RedeSocialCS no banco de dados.
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
						this._idRedeSocialCS = Convert.ToInt32(cmd.Parameters["@Idf_Rede_Social_CS"].Value);
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
		/// Método utilizado para inserir uma instância de RedeSocialCS no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRedeSocialCS = Convert.ToInt32(cmd.Parameters["@Idf_Rede_Social_CS"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RedeSocialCS no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RedeSocialCS no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de RedeSocialCS no banco de dados.
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
		/// Método utilizado para salvar uma instância de RedeSocialCS no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RedeSocialCS no banco de dados.
		/// </summary>
		/// <param name="idRedeSocialCS">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRedeSocialCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rede_Social_CS", SqlDbType.Int, 4));

			parms[0].Value = idRedeSocialCS;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RedeSocialCS no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRedeSocialCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRedeSocialCS, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rede_Social_CS", SqlDbType.Int, 4));

			parms[0].Value = idRedeSocialCS;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RedeSocialCS no banco de dados.
		/// </summary>
		/// <param name="idRedeSocialCS">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRedeSocialCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Rede_Social_CS where Idf_Rede_Social_CS in (";

			for (int i = 0; i < idRedeSocialCS.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRedeSocialCS[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRedeSocialCS">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRedeSocialCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rede_Social_CS", SqlDbType.Int, 4));

			parms[0].Value = idRedeSocialCS;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRedeSocialCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRedeSocialCS, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rede_Social_CS", SqlDbType.Int, 4));

			parms[0].Value = idRedeSocialCS;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Red.Idf_Rede_Social_CS, Red.Des_Rede_Social, Red.Img_Rede_Social, Red.Flg_Inativo, Red.Dta_Alteracao, Red.Dta_Cadastro, Red.Max_Caracter FROM BNE_Rede_Social_CS Red";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RedeSocialCS a partir do banco de dados.
		/// </summary>
		/// <param name="idRedeSocialCS">Chave do registro.</param>
		/// <returns>Instância de RedeSocialCS.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RedeSocialCS LoadObject(int idRedeSocialCS)
		{
			using (IDataReader dr = LoadDataReader(idRedeSocialCS))
			{
				RedeSocialCS objRedeSocialCS = new RedeSocialCS();
				if (SetInstance(dr, objRedeSocialCS))
					return objRedeSocialCS;
			}
			throw (new RecordNotFoundException(typeof(RedeSocialCS)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RedeSocialCS a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRedeSocialCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RedeSocialCS.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RedeSocialCS LoadObject(int idRedeSocialCS, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRedeSocialCS, trans))
			{
				RedeSocialCS objRedeSocialCS = new RedeSocialCS();
				if (SetInstance(dr, objRedeSocialCS))
					return objRedeSocialCS;
			}
			throw (new RecordNotFoundException(typeof(RedeSocialCS)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RedeSocialCS a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRedeSocialCS))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RedeSocialCS a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRedeSocialCS, trans))
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
		/// <param name="objRedeSocialCS">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RedeSocialCS objRedeSocialCS)
		{
			try
			{
				if (dr.Read())
				{
					objRedeSocialCS._idRedeSocialCS = Convert.ToInt32(dr["Idf_Rede_Social_CS"]);
					objRedeSocialCS._descricaoRedeSocial = Convert.ToString(dr["Des_Rede_Social"]);
					objRedeSocialCS._imagemRedeSocial = (byte[])(dr["Img_Rede_Social"]);
					objRedeSocialCS._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objRedeSocialCS._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objRedeSocialCS._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objRedeSocialCS._MaxCaracter = Convert.ToInt32(dr["Max_Caracter"]);

					objRedeSocialCS._persisted = true;
					objRedeSocialCS._modified = false;

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