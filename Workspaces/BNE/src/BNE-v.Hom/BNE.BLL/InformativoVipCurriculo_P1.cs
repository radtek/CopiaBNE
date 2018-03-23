//-- Data: 13/09/2016 15:35
//-- Autor: Marty Sroka

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class InformativoVipCurriculo // Tabela: BNE_Informativo_Vip_Curriculo
	{
		#region Atributos
		private InformativoVip _informativoVip;
		private Curriculo _curriculo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region InformativoVip
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public InformativoVip InformativoVip
		{
			get
			{
				return this._informativoVip;
			}
			set
			{
				this._informativoVip = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Curriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Curriculo Curriculo
		{
			get
			{
				return this._curriculo;
			}
			set
			{
				this._curriculo = value;
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
		public InformativoVipCurriculo()
		{
		}
		public InformativoVipCurriculo(InformativoVip informativoVip, Curriculo curriculo)
		{
			this._informativoVip = informativoVip;
			this._curriculo = curriculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Informativo_Vip_Curriculo (Idf_Informativo_Vip, Idf_Curriculo, Dta_Cadastro) VALUES (@Idf_Informativo_Vip, @Idf_Curriculo, @Dta_Cadastro);";
		private const string SPUPDATE = "UPDATE BNE_Informativo_Vip_Curriculo SET Dta_Cadastro = @Dta_Cadastro WHERE Idf_Informativo_Vip = @Idf_Informativo_Vip AND Idf_Curriculo = @Idf_Curriculo";
		private const string SPDELETE = "DELETE FROM BNE_Informativo_Vip_Curriculo WHERE Idf_Informativo_Vip = @Idf_Informativo_Vip AND Idf_Curriculo = @Idf_Curriculo";
		private const string SPSELECTID = "SELECT * FROM BNE_Informativo_Vip_Curriculo WITH(NOLOCK) WHERE Idf_Informativo_Vip = @Idf_Informativo_Vip AND Idf_Curriculo = @Idf_Curriculo";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Marty Sroka</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Informativo_Vip", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Marty Sroka</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._informativoVip.IdInformativoVip;
			parms[1].Value = this._curriculo.IdCurriculo;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de InformativoVipCurriculo no banco de dados.
		/// </summary>
		/// <remarks>Marty Sroka</remarks>
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
		/// Método utilizado para inserir uma instância de InformativoVipCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Marty Sroka</remarks>
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
		/// Método utilizado para atualizar uma instância de InformativoVipCurriculo no banco de dados.
		/// </summary>
		/// <remarks>Marty Sroka</remarks>
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
		/// Método utilizado para atualizar uma instância de InformativoVipCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Marty Sroka</remarks>
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
		/// Método utilizado para salvar uma instância de InformativoVipCurriculo no banco de dados.
		/// </summary>
		/// <remarks>Marty Sroka</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de InformativoVipCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Marty Sroka</remarks>
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
		/// Método utilizado para excluir uma instância de InformativoVipCurriculo no banco de dados.
		/// </summary>
		/// <param name="idInformativoVip">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <remarks>Marty Sroka</remarks>
		public static void Delete(int idInformativoVip, int idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Informativo_Vip", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idInformativoVip;
			parms[1].Value = idCurriculo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de InformativoVipCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idInformativoVip">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Marty Sroka</remarks>
		public static void Delete(int idInformativoVip, int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Informativo_Vip", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idInformativoVip;
			parms[1].Value = idCurriculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idInformativoVip">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static IDataReader LoadDataReader(int idInformativoVip, int idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Informativo_Vip", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idInformativoVip;
			parms[1].Value = idCurriculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idInformativoVip">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static IDataReader LoadDataReader(int idInformativoVip, int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Informativo_Vip", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idInformativoVip;
			parms[1].Value = idCurriculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Inf.Idf_Informativo_Vip, Inf.Idf_Curriculo, Inf.Dta_Cadastro FROM BNE_Informativo_Vip_Curriculo Inf";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de InformativoVipCurriculo a partir do banco de dados.
		/// </summary>
		/// <param name="idInformativoVip">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Instância de InformativoVipCurriculo.</returns>
		/// <remarks>Marty Sroka</remarks>
		public static InformativoVipCurriculo LoadObject(int idInformativoVip, int idCurriculo)
		{
			using (IDataReader dr = LoadDataReader(idInformativoVip, idCurriculo))
			{
				InformativoVipCurriculo objInformativoVipCurriculo = new InformativoVipCurriculo();
				if (SetInstance(dr, objInformativoVipCurriculo))
					return objInformativoVipCurriculo;
			}
			throw (new RecordNotFoundException(typeof(InformativoVipCurriculo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de InformativoVipCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idInformativoVip">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de InformativoVipCurriculo.</returns>
		/// <remarks>Marty Sroka</remarks>
		public static InformativoVipCurriculo LoadObject(int idInformativoVip, int idCurriculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idInformativoVip, idCurriculo, trans))
			{
				InformativoVipCurriculo objInformativoVipCurriculo = new InformativoVipCurriculo();
				if (SetInstance(dr, objInformativoVipCurriculo))
					return objInformativoVipCurriculo;
			}
			throw (new RecordNotFoundException(typeof(InformativoVipCurriculo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de InformativoVipCurriculo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._informativoVip.IdInformativoVip, this._curriculo.IdCurriculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de InformativoVipCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._informativoVip.IdInformativoVip, this._curriculo.IdCurriculo, trans))
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
		/// <param name="objInformativoVipCurriculo">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static bool SetInstance(IDataReader dr, InformativoVipCurriculo objInformativoVipCurriculo, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objInformativoVipCurriculo._informativoVip = new InformativoVip(Convert.ToInt32(dr["Idf_Informativo_Vip"]));
					objInformativoVipCurriculo._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objInformativoVipCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objInformativoVipCurriculo._persisted = true;
					objInformativoVipCurriculo._modified = false;

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
				if (dispose)
					dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}