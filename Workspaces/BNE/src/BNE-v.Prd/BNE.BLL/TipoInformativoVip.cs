//-- Data: 13/09/2016 15:35
//-- Autor: Marty Sroka

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public class TipoInformativoVip // Tabela: BNE_Tipo_Informativo_Vip
	{
		#region Atributos
		private Int16 _idTipoInformativoVip;
		private string _descricaoTipoInformativoVip;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTipoInformativoVip
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Int16 IdTipoInformativoVip
		{
			get
			{
				return this._idTipoInformativoVip;
			}
			set
			{
				this._idTipoInformativoVip = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoTipoInformativoVip
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoTipoInformativoVip
		{
			get
			{
				return this._descricaoTipoInformativoVip;
			}
			set
			{
				this._descricaoTipoInformativoVip = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public TipoInformativoVip()
		{
		}
		public TipoInformativoVip(Int16 idTipoInformativoVip)
		{
			this._idTipoInformativoVip = idTipoInformativoVip;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Tipo_Informativo_Vip (Idf_Tipo_Informativo_Vip, Des_Tipo_Informativo_Vip) VALUES (@Idf_Tipo_Informativo_Vip, @Des_Tipo_Informativo_Vip);";
		private const string SPUPDATE = "UPDATE BNE_Tipo_Informativo_Vip SET Des_Tipo_Informativo_Vip = @Des_Tipo_Informativo_Vip WHERE Idf_Tipo_Informativo_Vip = @Idf_Tipo_Informativo_Vip";
		private const string SPDELETE = "DELETE FROM BNE_Tipo_Informativo_Vip WHERE Idf_Tipo_Informativo_Vip = @Idf_Tipo_Informativo_Vip";
		private const string SPSELECTID = "SELECT * FROM BNE_Tipo_Informativo_Vip WITH(NOLOCK) WHERE Idf_Tipo_Informativo_Vip = @Idf_Tipo_Informativo_Vip";
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
			parms.Add(new SqlParameter("@Idf_Tipo_Informativo_Vip", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Des_Tipo_Informativo_Vip", SqlDbType.VarChar, 20));
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
			parms[0].Value = this._idTipoInformativoVip;
			parms[1].Value = this._descricaoTipoInformativoVip;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de TipoInformativoVip no banco de dados.
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
		/// Método utilizado para inserir uma instância de TipoInformativoVip no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de TipoInformativoVip no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TipoInformativoVip no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TipoInformativoVip no banco de dados.
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
		/// Método utilizado para salvar uma instância de TipoInformativoVip no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TipoInformativoVip no banco de dados.
		/// </summary>
		/// <param name="idTipoInformativoVip">Chave do registro.</param>
		/// <remarks>Marty Sroka</remarks>
		public static void Delete(Int16 idTipoInformativoVip)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Informativo_Vip", SqlDbType.Int, 2));

			parms[0].Value = idTipoInformativoVip;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoInformativoVip no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoInformativoVip">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Marty Sroka</remarks>
		public static void Delete(Int16 idTipoInformativoVip, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Informativo_Vip", SqlDbType.Int, 2));

			parms[0].Value = idTipoInformativoVip;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoInformativoVip no banco de dados.
		/// </summary>
		/// <param name="idTipoInformativoVip">Lista de chaves.</param>
		/// <remarks>Marty Sroka</remarks>
		public static void Delete(List<Int16> idTipoInformativoVip)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Tipo_Informativo_Vip where Idf_Tipo_Informativo_Vip in (";

			for (int i = 0; i < idTipoInformativoVip.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 2));
				parms[i].Value = idTipoInformativoVip[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTipoInformativoVip">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static IDataReader LoadDataReader(Int16 idTipoInformativoVip)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Informativo_Vip", SqlDbType.Int, 2));

			parms[0].Value = idTipoInformativoVip;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoInformativoVip">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static IDataReader LoadDataReader(Int16 idTipoInformativoVip, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Informativo_Vip", SqlDbType.Int, 2));

			parms[0].Value = idTipoInformativoVip;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Informativo_Vip, Tip.Des_Tipo_Informativo_Vip FROM BNE_Tipo_Informativo_Vip Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoInformativoVip a partir do banco de dados.
		/// </summary>
		/// <param name="idTipoInformativoVip">Chave do registro.</param>
		/// <returns>Instância de TipoInformativoVip.</returns>
		/// <remarks>Marty Sroka</remarks>
		public static TipoInformativoVip LoadObject(Int16 idTipoInformativoVip)
		{
			using (IDataReader dr = LoadDataReader(idTipoInformativoVip))
			{
				TipoInformativoVip objTipoInformativoVip = new TipoInformativoVip();
				if (SetInstance(dr, objTipoInformativoVip))
					return objTipoInformativoVip;
			}
			throw (new RecordNotFoundException(typeof(TipoInformativoVip)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoInformativoVip a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoInformativoVip">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoInformativoVip.</returns>
		/// <remarks>Marty Sroka</remarks>
		public static TipoInformativoVip LoadObject(Int16 idTipoInformativoVip, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTipoInformativoVip, trans))
			{
				TipoInformativoVip objTipoInformativoVip = new TipoInformativoVip();
				if (SetInstance(dr, objTipoInformativoVip))
					return objTipoInformativoVip;
			}
			throw (new RecordNotFoundException(typeof(TipoInformativoVip)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoInformativoVip a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoInformativoVip))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoInformativoVip a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoInformativoVip, trans))
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
		/// <param name="objTipoInformativoVip">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static bool SetInstance(IDataReader dr, TipoInformativoVip objTipoInformativoVip, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objTipoInformativoVip._idTipoInformativoVip = Convert.ToInt16(dr["Idf_Tipo_Informativo_Vip"]);
					objTipoInformativoVip._descricaoTipoInformativoVip = Convert.ToString(dr["Des_Tipo_Informativo_Vip"]);

					objTipoInformativoVip._persisted = true;
					objTipoInformativoVip._modified = false;

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