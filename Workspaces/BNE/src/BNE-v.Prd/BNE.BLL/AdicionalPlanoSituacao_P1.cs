//-- Data: 09/09/2011 10:34
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using System.ComponentModel.DataAnnotations;

namespace BNE.BLL
{
	public partial class AdicionalPlanoSituacao // Tabela: BNE_Adicional_Plano_Situacao
	{
		#region Atributos
		private int _idAdicionalPlanoSituacao;
		private string _descricaoAdicionalPlanoSituacao;
		private DateTime _dataCadastro;
		private DateTime _dataAlteracao;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdAdicionalPlanoSituacao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdAdicionalPlanoSituacao
		{
			get
			{
				return this._idAdicionalPlanoSituacao;
			}
		}
		#endregion 

		#region DescricaoAdicionalPlanoSituacao
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoAdicionalPlanoSituacao
		{
			get
			{
				return this._descricaoAdicionalPlanoSituacao;
			}
			set
			{
				this._descricaoAdicionalPlanoSituacao = value;
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
        [Display(Name = "IgnoreData")]
		public DateTime DataAlteracao
		{
			get
			{
				return this._dataAlteracao;
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
		public AdicionalPlanoSituacao()
		{
		}
		public AdicionalPlanoSituacao(int idAdicionalPlanoSituacao)
		{
			this._idAdicionalPlanoSituacao = idAdicionalPlanoSituacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Adicional_Plano_Situacao (Des_Adicional_Plano_Situacao, Dta_Cadastro, Dta_Alteracao, Flg_Inativo) VALUES (@Des_Adicional_Plano_Situacao, @Dta_Cadastro, @Dta_Alteracao, @Flg_Inativo);SET @Idf_Adicional_Plano_Situacao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Adicional_Plano_Situacao SET Des_Adicional_Plano_Situacao = @Des_Adicional_Plano_Situacao, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Flg_Inativo = @Flg_Inativo WHERE Idf_Adicional_Plano_Situacao = @Idf_Adicional_Plano_Situacao";
		private const string SPDELETE = "DELETE FROM BNE_Adicional_Plano_Situacao WHERE Idf_Adicional_Plano_Situacao = @Idf_Adicional_Plano_Situacao";
		private const string SPSELECTID = "SELECT * FROM BNE_Adicional_Plano_Situacao WHERE Idf_Adicional_Plano_Situacao = @Idf_Adicional_Plano_Situacao";
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
			parms.Add(new SqlParameter("@Idf_Adicional_Plano_Situacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Adicional_Plano_Situacao", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idAdicionalPlanoSituacao;
			parms[1].Value = this._descricaoAdicionalPlanoSituacao;
			parms[4].Value = this._flagInativo;

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
			this._dataAlteracao = DateTime.Now;
			parms[3].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de AdicionalPlanoSituacao no banco de dados.
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
						this._idAdicionalPlanoSituacao = Convert.ToInt32(cmd.Parameters["@Idf_Adicional_Plano_Situacao"].Value);
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
		/// Método utilizado para inserir uma instância de AdicionalPlanoSituacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idAdicionalPlanoSituacao = Convert.ToInt32(cmd.Parameters["@Idf_Adicional_Plano_Situacao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de AdicionalPlanoSituacao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de AdicionalPlanoSituacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de AdicionalPlanoSituacao no banco de dados.
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
		/// Método utilizado para salvar uma instância de AdicionalPlanoSituacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de AdicionalPlanoSituacao no banco de dados.
		/// </summary>
		/// <param name="idAdicionalPlanoSituacao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idAdicionalPlanoSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Adicional_Plano_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idAdicionalPlanoSituacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de AdicionalPlanoSituacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAdicionalPlanoSituacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idAdicionalPlanoSituacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Adicional_Plano_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idAdicionalPlanoSituacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de AdicionalPlanoSituacao no banco de dados.
		/// </summary>
		/// <param name="idAdicionalPlanoSituacao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idAdicionalPlanoSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Adicional_Plano_Situacao where Idf_Adicional_Plano_Situacao in (";

			for (int i = 0; i < idAdicionalPlanoSituacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idAdicionalPlanoSituacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idAdicionalPlanoSituacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idAdicionalPlanoSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Adicional_Plano_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idAdicionalPlanoSituacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAdicionalPlanoSituacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idAdicionalPlanoSituacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Adicional_Plano_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idAdicionalPlanoSituacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Adi.Idf_Adicional_Plano_Situacao, Adi.Des_Adicional_Plano_Situacao, Adi.Dta_Cadastro, Adi.Dta_Alteracao, Adi.Flg_Inativo FROM BNE_Adicional_Plano_Situacao Adi";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de AdicionalPlanoSituacao a partir do banco de dados.
		/// </summary>
		/// <param name="idAdicionalPlanoSituacao">Chave do registro.</param>
		/// <returns>Instância de AdicionalPlanoSituacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static AdicionalPlanoSituacao LoadObject(int idAdicionalPlanoSituacao)
		{
			using (IDataReader dr = LoadDataReader(idAdicionalPlanoSituacao))
			{
				AdicionalPlanoSituacao objAdicionalPlanoSituacao = new AdicionalPlanoSituacao();
				if (SetInstance(dr, objAdicionalPlanoSituacao))
					return objAdicionalPlanoSituacao;
			}
			throw (new RecordNotFoundException(typeof(AdicionalPlanoSituacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de AdicionalPlanoSituacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAdicionalPlanoSituacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de AdicionalPlanoSituacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static AdicionalPlanoSituacao LoadObject(int idAdicionalPlanoSituacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idAdicionalPlanoSituacao, trans))
			{
				AdicionalPlanoSituacao objAdicionalPlanoSituacao = new AdicionalPlanoSituacao();
				if (SetInstance(dr, objAdicionalPlanoSituacao))
					return objAdicionalPlanoSituacao;
			}
			throw (new RecordNotFoundException(typeof(AdicionalPlanoSituacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de AdicionalPlanoSituacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idAdicionalPlanoSituacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de AdicionalPlanoSituacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idAdicionalPlanoSituacao, trans))
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
		/// <param name="objAdicionalPlanoSituacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, AdicionalPlanoSituacao objAdicionalPlanoSituacao)
		{
			try
			{
				if (dr.Read())
				{
					objAdicionalPlanoSituacao._idAdicionalPlanoSituacao = Convert.ToInt32(dr["Idf_Adicional_Plano_Situacao"]);
					objAdicionalPlanoSituacao._descricaoAdicionalPlanoSituacao = Convert.ToString(dr["Des_Adicional_Plano_Situacao"]);
					objAdicionalPlanoSituacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objAdicionalPlanoSituacao._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objAdicionalPlanoSituacao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objAdicionalPlanoSituacao._persisted = true;
					objAdicionalPlanoSituacao._modified = false;

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