//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CNAEGrupo // Tabela: plataforma.TAB_CNAE_Grupo
	{
		#region Atributos
		private int _idCNAEGrupo;
		private string _codigoCNAEGrupo;
		private string _descricaoCNAEGrupo;
		private CNAEDivisao _cNAEDivisao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCNAEGrupo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCNAEGrupo
		{
			get
			{
				return this._idCNAEGrupo;
			}
			set
			{
				this._idCNAEGrupo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoCNAEGrupo
		/// <summary>
		/// Tamanho do campo: 3.
		/// Campo obrigatório.
		/// </summary>
		public string CodigoCNAEGrupo
		{
			get
			{
				return this._codigoCNAEGrupo;
			}
			set
			{
				this._codigoCNAEGrupo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCNAEGrupo
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoCNAEGrupo
		{
			get
			{
				return this._descricaoCNAEGrupo;
			}
			set
			{
				this._descricaoCNAEGrupo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CNAEDivisao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CNAEDivisao CNAEDivisao
		{
			get
			{
				return this._cNAEDivisao;
			}
			set
			{
				this._cNAEDivisao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CNAEGrupo()
		{
		}
		public CNAEGrupo(int idCNAEGrupo)
		{
			this._idCNAEGrupo = idCNAEGrupo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_CNAE_Grupo (Idf_CNAE_Grupo, Cod_CNAE_Grupo, Des_CNAE_Grupo, Idf_CNAE_Divisao) VALUES (@Idf_CNAE_Grupo, @Cod_CNAE_Grupo, @Des_CNAE_Grupo, @Idf_CNAE_Divisao);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_CNAE_Grupo SET Cod_CNAE_Grupo = @Cod_CNAE_Grupo, Des_CNAE_Grupo = @Des_CNAE_Grupo, Idf_CNAE_Divisao = @Idf_CNAE_Divisao WHERE Idf_CNAE_Grupo = @Idf_CNAE_Grupo";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_CNAE_Grupo WHERE Idf_CNAE_Grupo = @Idf_CNAE_Grupo";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_CNAE_Grupo WHERE Idf_CNAE_Grupo = @Idf_CNAE_Grupo";
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
			parms.Add(new SqlParameter("@Idf_CNAE_Grupo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Cod_CNAE_Grupo", SqlDbType.Char, 3));
			parms.Add(new SqlParameter("@Des_CNAE_Grupo", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Idf_CNAE_Divisao", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCNAEGrupo;
			parms[1].Value = this._codigoCNAEGrupo;
			parms[2].Value = this._descricaoCNAEGrupo;
			parms[3].Value = this._cNAEDivisao.IdCNAEDivisao;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CNAEGrupo no banco de dados.
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
		/// Método utilizado para inserir uma instância de CNAEGrupo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CNAEGrupo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CNAEGrupo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CNAEGrupo no banco de dados.
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
		/// Método utilizado para salvar uma instância de CNAEGrupo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CNAEGrupo no banco de dados.
		/// </summary>
		/// <param name="idCNAEGrupo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCNAEGrupo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Grupo", SqlDbType.Int, 4));

			parms[0].Value = idCNAEGrupo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CNAEGrupo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCNAEGrupo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCNAEGrupo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Grupo", SqlDbType.Int, 4));

			parms[0].Value = idCNAEGrupo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CNAEGrupo no banco de dados.
		/// </summary>
		/// <param name="idCNAEGrupo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCNAEGrupo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_CNAE_Grupo where Idf_CNAE_Grupo in (";

			for (int i = 0; i < idCNAEGrupo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCNAEGrupo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCNAEGrupo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCNAEGrupo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Grupo", SqlDbType.Int, 4));

			parms[0].Value = idCNAEGrupo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCNAEGrupo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCNAEGrupo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_CNAE_Grupo", SqlDbType.Int, 4));

			parms[0].Value = idCNAEGrupo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, CNA.Idf_CNAE_Grupo, CNA.Cod_CNAE_Grupo, CNA.Des_CNAE_Grupo, CNA.Idf_CNAE_Divisao FROM plataforma.TAB_CNAE_Grupo CNA";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CNAEGrupo a partir do banco de dados.
		/// </summary>
		/// <param name="idCNAEGrupo">Chave do registro.</param>
		/// <returns>Instância de CNAEGrupo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CNAEGrupo LoadObject(int idCNAEGrupo)
		{
			using (IDataReader dr = LoadDataReader(idCNAEGrupo))
			{
				CNAEGrupo objCNAEGrupo = new CNAEGrupo();
				if (SetInstance(dr, objCNAEGrupo))
					return objCNAEGrupo;
			}
			throw (new RecordNotFoundException(typeof(CNAEGrupo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CNAEGrupo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCNAEGrupo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CNAEGrupo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CNAEGrupo LoadObject(int idCNAEGrupo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCNAEGrupo, trans))
			{
				CNAEGrupo objCNAEGrupo = new CNAEGrupo();
				if (SetInstance(dr, objCNAEGrupo))
					return objCNAEGrupo;
			}
			throw (new RecordNotFoundException(typeof(CNAEGrupo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CNAEGrupo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCNAEGrupo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CNAEGrupo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCNAEGrupo, trans))
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
		/// <param name="objCNAEGrupo">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CNAEGrupo objCNAEGrupo)
		{
			try
			{
				if (dr.Read())
				{
					objCNAEGrupo._idCNAEGrupo = Convert.ToInt32(dr["Idf_CNAE_Grupo"]);
					objCNAEGrupo._codigoCNAEGrupo = Convert.ToString(dr["Cod_CNAE_Grupo"]);
					objCNAEGrupo._descricaoCNAEGrupo = Convert.ToString(dr["Des_CNAE_Grupo"]);
					objCNAEGrupo._cNAEDivisao = new CNAEDivisao(Convert.ToInt32(dr["Idf_CNAE_Divisao"]));

					objCNAEGrupo._persisted = true;
					objCNAEGrupo._modified = false;

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