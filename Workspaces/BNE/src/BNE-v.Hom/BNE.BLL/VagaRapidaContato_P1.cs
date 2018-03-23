//-- Data: 13/09/2017 12:21
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class VagaRapidaContato // Tabela: BNE_Vaga_Rapida_Contato
	{
		#region Atributos
		private Vaga _vaga;
		private string _nomeContato;
		private string _numeroDDDContato;
		private string _numeroContato;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region Vaga
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Vaga Vaga
		{
			get
			{
				return this._vaga;
			}
			set
			{
				this._vaga = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeContato
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string NomeContato
		{
			get
			{
				return this._nomeContato;
			}
			set
			{
				this._nomeContato = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDDDContato
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo obrigatório.
		/// </summary>
		public string NumeroDDDContato
		{
			get
			{
				return this._numeroDDDContato;
			}
			set
			{
				this._numeroDDDContato = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroContato
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo obrigatório.
		/// </summary>
		public string NumeroContato
		{
			get
			{
				return this._numeroContato;
			}
			set
			{
				this._numeroContato = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public VagaRapidaContato()
		{
		}
		public VagaRapidaContato(Vaga vaga)
		{
			this._vaga = vaga;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Vaga_Rapida_Contato (Idf_Vaga, Nme_Contato, Num_DDD_Contato, Num_Contato) VALUES (@Idf_Vaga, @Nme_Contato, @Num_DDD_Contato, @Num_Contato);";
		private const string SPUPDATE = "UPDATE BNE_Vaga_Rapida_Contato SET Nme_Contato = @Nme_Contato, Num_DDD_Contato = @Num_DDD_Contato, Num_Contato = @Num_Contato WHERE Idf_Vaga = @Idf_Vaga";
		private const string SPDELETE = "DELETE FROM BNE_Vaga_Rapida_Contato WHERE Idf_Vaga = @Idf_Vaga";
		private const string SPSELECTID = "SELECT * FROM BNE_Vaga_Rapida_Contato WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga";
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
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Contato", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Num_DDD_Contato", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Contato", SqlDbType.Char, 10));
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
			parms[0].Value = this._vaga.IdVaga;
			parms[1].Value = this._nomeContato;
			parms[2].Value = this._numeroDDDContato;
			parms[3].Value = this._numeroContato;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de VagaRapidaContato no banco de dados.
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
		/// Método utilizado para inserir uma instância de VagaRapidaContato no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de VagaRapidaContato no banco de dados.
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
		/// Método utilizado para atualizar uma instância de VagaRapidaContato no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de VagaRapidaContato no banco de dados.
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
		/// Método utilizado para salvar uma instância de VagaRapidaContato no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de VagaRapidaContato no banco de dados.
		/// </summary>
		/// <param name="idVaga">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idVaga;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de VagaRapidaContato no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idVaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idVaga;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de VagaRapidaContato no banco de dados.
		/// </summary>
		/// <param name="vaga">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<Vaga> vaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Vaga_Rapida_Contato where Idf_Vaga in (";

			for (int i = 0; i < vaga.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = vaga[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idVaga">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idVaga;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idVaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idVaga;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Vag.Idf_Vaga, Vag.Nme_Contato, Vag.Num_DDD_Contato, Vag.Num_Contato FROM BNE_Vaga_Rapida_Contato Vag";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaRapidaContato a partir do banco de dados.
		/// </summary>
		/// <param name="idVaga">Chave do registro.</param>
		/// <returns>Instância de VagaRapidaContato.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static VagaRapidaContato LoadObject(int idVaga)
		{
			using (IDataReader dr = LoadDataReader(idVaga))
			{
				VagaRapidaContato objVagaRapidaContato = new VagaRapidaContato();
				if (SetInstance(dr, objVagaRapidaContato))
					return objVagaRapidaContato;
			}
			throw (new RecordNotFoundException(typeof(VagaRapidaContato)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaRapidaContato a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de VagaRapidaContato.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static VagaRapidaContato LoadObject(int idVaga, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idVaga, trans))
			{
				VagaRapidaContato objVagaRapidaContato = new VagaRapidaContato();
				if (SetInstance(dr, objVagaRapidaContato))
					return objVagaRapidaContato;
			}
			throw (new RecordNotFoundException(typeof(VagaRapidaContato)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de VagaRapidaContato a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._vaga.IdVaga))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de VagaRapidaContato a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._vaga.IdVaga, trans))
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
		/// <param name="objVagaRapidaContato">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, VagaRapidaContato objVagaRapidaContato, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objVagaRapidaContato._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					objVagaRapidaContato._nomeContato = Convert.ToString(dr["Nme_Contato"]);
					objVagaRapidaContato._numeroDDDContato = Convert.ToString(dr["Num_DDD_Contato"]);
					objVagaRapidaContato._numeroContato = Convert.ToString(dr["Num_Contato"]);

					objVagaRapidaContato._persisted = true;
					objVagaRapidaContato._modified = false;

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