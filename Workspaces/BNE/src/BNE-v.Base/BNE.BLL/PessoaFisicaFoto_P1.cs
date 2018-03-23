//-- Data: 06/09/2013 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PessoaFisicaFoto // Tabela: TAB_Pessoa_Fisica_Foto
	{
		#region Atributos
		private int _idPessoaFisicaFoto;
		private PessoaFisica _pessoaFisica;
		private byte[] _imagemPessoa;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private DateTime _dataAlteracao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPessoaFisicaFoto
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPessoaFisicaFoto
		{
			get
			{
				return this._idPessoaFisicaFoto;
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

		#region ImagemPessoa
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public byte[] ImagemPessoa
		{
			get
			{
				return this._imagemPessoa;
			}
			set
			{
				this._imagemPessoa = value;
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

		#endregion

		#region Construtores
		public PessoaFisicaFoto()
		{
		}
		public PessoaFisicaFoto(int idPessoaFisicaFoto)
		{
			this._idPessoaFisicaFoto = idPessoaFisicaFoto;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pessoa_Fisica_Foto (Idf_Pessoa_Fisica, Img_Pessoa, Flg_Inativo, Dta_Cadastro, Dta_Alteracao) VALUES (@Idf_Pessoa_Fisica, @Img_Pessoa, @Flg_Inativo, @Dta_Cadastro, @Dta_Alteracao);SET @Idf_Pessoa_Fisica_Foto = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pessoa_Fisica_Foto SET Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica, Img_Pessoa = @Img_Pessoa, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao WHERE Idf_Pessoa_Fisica_Foto = @Idf_Pessoa_Fisica_Foto";
		private const string SPDELETE = "DELETE FROM TAB_Pessoa_Fisica_Foto WHERE Idf_Pessoa_Fisica_Foto = @Idf_Pessoa_Fisica_Foto";
		private const string SPSELECTID = "SELECT * FROM TAB_Pessoa_Fisica_Foto WHERE Idf_Pessoa_Fisica_Foto = @Idf_Pessoa_Fisica_Foto";
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
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Foto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Img_Pessoa", SqlDbType.VarBinary));
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
			parms[0].Value = this._idPessoaFisicaFoto;
			parms[1].Value = this._pessoaFisica.IdPessoaFisica;
            if (this._imagemPessoa != null)
                parms[2].Value = this._imagemPessoa;
            else
                parms[2].Value = DBNull.Value;
			parms[3].Value = this._flagInativo;

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
			this._dataAlteracao = DateTime.Now;
			parms[5].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PessoaFisicaFoto no banco de dados.
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
						this._idPessoaFisicaFoto = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica_Foto"].Value);
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
		/// Método utilizado para inserir uma instância de PessoaFisicaFoto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPessoaFisicaFoto = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica_Foto"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PessoaFisicaFoto no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PessoaFisicaFoto no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PessoaFisicaFoto no banco de dados.
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
		/// Método utilizado para salvar uma instância de PessoaFisicaFoto no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PessoaFisicaFoto no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaFoto">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPessoaFisicaFoto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Foto", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaFoto;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PessoaFisicaFoto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaFoto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPessoaFisicaFoto, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Foto", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaFoto;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PessoaFisicaFoto no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaFoto">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPessoaFisicaFoto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pessoa_Fisica_Foto where Idf_Pessoa_Fisica_Foto in (";

			for (int i = 0; i < idPessoaFisicaFoto.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPessoaFisicaFoto[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaFoto">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPessoaFisicaFoto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Foto", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaFoto;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaFoto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPessoaFisicaFoto, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica_Foto", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisicaFoto;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pessoa_Fisica_Foto, Pes.Idf_Pessoa_Fisica, Pes.Img_Pessoa, Pes.Flg_Inativo, Pes.Dta_Cadastro, Pes.Dta_Alteracao FROM TAB_Pessoa_Fisica_Foto Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoaFisicaFoto a partir do banco de dados.
		/// </summary>
		/// <param name="idPessoaFisicaFoto">Chave do registro.</param>
		/// <returns>Instância de PessoaFisicaFoto.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoaFisicaFoto LoadObject(int idPessoaFisicaFoto)
		{
			using (IDataReader dr = LoadDataReader(idPessoaFisicaFoto))
			{
				PessoaFisicaFoto objPessoaFisicaFoto = new PessoaFisicaFoto();
				if (SetInstance(dr, objPessoaFisicaFoto))
					return objPessoaFisicaFoto;
			}
			throw (new RecordNotFoundException(typeof(PessoaFisicaFoto)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoaFisicaFoto a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisicaFoto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PessoaFisicaFoto.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoaFisicaFoto LoadObject(int idPessoaFisicaFoto, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPessoaFisicaFoto, trans))
			{
				PessoaFisicaFoto objPessoaFisicaFoto = new PessoaFisicaFoto();
				if (SetInstance(dr, objPessoaFisicaFoto))
					return objPessoaFisicaFoto;
			}
			throw (new RecordNotFoundException(typeof(PessoaFisicaFoto)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PessoaFisicaFoto a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPessoaFisicaFoto))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PessoaFisicaFoto a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPessoaFisicaFoto, trans))
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
		/// <param name="objPessoaFisicaFoto">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PessoaFisicaFoto objPessoaFisicaFoto)
		{
			try
			{
				if (dr.Read())
				{
					objPessoaFisicaFoto._idPessoaFisicaFoto = Convert.ToInt32(dr["Idf_Pessoa_Fisica_Foto"]);
					objPessoaFisicaFoto._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
                    if (dr["Img_Pessoa"] != DBNull.Value)
					    objPessoaFisicaFoto._imagemPessoa = (byte[])(dr["Img_Pessoa"]);
					objPessoaFisicaFoto._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objPessoaFisicaFoto._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objPessoaFisicaFoto._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);

					objPessoaFisicaFoto._persisted = true;
					objPessoaFisicaFoto._modified = false;

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