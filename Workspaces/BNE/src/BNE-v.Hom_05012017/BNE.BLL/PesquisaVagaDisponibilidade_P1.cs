//-- Data: 12/01/2012 15:07
//-- Autor: kaio

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	[Serializable]
	public partial class PesquisaVagaDisponibilidade // Tabela: BNE.TAB_Pesquisa_Vaga_Disponibilidade
	{
		#region Atributos
		private int _idPesquisaVagaDisponibilidade;
		private Disponibilidade _disponibilidade;
		private PesquisaVaga _pesquisaVaga;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPesquisaVagaDisponibilidade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdPesquisaVagaDisponibilidade
		{
			get
			{
				return this._idPesquisaVagaDisponibilidade;
			}
			set
			{
				this._idPesquisaVagaDisponibilidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Disponibilidade
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Disponibilidade Disponibilidade
		{
			get
			{
				return this._disponibilidade;
			}
			set
			{
				this._disponibilidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region PesquisaVaga
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public PesquisaVaga PesquisaVaga
		{
			get
			{
				return this._pesquisaVaga;
			}
			set
			{
				this._pesquisaVaga = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PesquisaVagaDisponibilidade()
		{
		}
		public PesquisaVagaDisponibilidade(int idPesquisaVagaDisponibilidade)
		{
			this._idPesquisaVagaDisponibilidade = idPesquisaVagaDisponibilidade;
			this._persisted = true;
		}
		#endregion

		#region Consultas
        private const string SPINSERT = "INSERT INTO BNE.TAB_Pesquisa_Vaga_Disponibilidade (Idf_Disponibilidade, Idf_Pesquisa_Vaga) VALUES (@Idf_Disponibilidade, @Idf_Pesquisa_Vaga); SET @Idf_Pesquisa_Vaga_Disponibilidade = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE.TAB_Pesquisa_Vaga_Disponibilidade SET Idf_Disponibilidade = @Idf_Disponibilidade, Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga WHERE Idf_Pesquisa_Vaga_Disponibilidade = @Idf_Pesquisa_Vaga_Disponibilidade";
		private const string SPDELETE = "DELETE FROM BNE.TAB_Pesquisa_Vaga_Disponibilidade WHERE Idf_Pesquisa_Vaga_Disponibilidade = @Idf_Pesquisa_Vaga_Disponibilidade";
		private const string SPSELECTID = "SELECT * FROM BNE.TAB_Pesquisa_Vaga_Disponibilidade WHERE Idf_Pesquisa_Vaga_Disponibilidade = @Idf_Pesquisa_Vaga_Disponibilidade";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>kaio</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Disponibilidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Disponibilidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>kaio</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idPesquisaVagaDisponibilidade;

			if (this._disponibilidade != null)
				parms[1].Value = this._disponibilidade.IdDisponibilidade;
			else
				parms[1].Value = DBNull.Value;

			if (this._pesquisaVaga != null)
				parms[2].Value = this._pesquisaVaga.IdPesquisaVaga;
			else
				parms[2].Value = DBNull.Value;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de TABPesquisaVagaDisponibilidade no banco de dados.
		/// </summary>
		/// <remarks>kaio</remarks>
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
                        this._idPesquisaVagaDisponibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga_Disponibilidade"].Value);
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
		/// Método utilizado para inserir uma instância de TABPesquisaVagaDisponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>kaio</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPesquisaVagaDisponibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga_Disponibilidade"].Value);
            cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de TABPesquisaVagaDisponibilidade no banco de dados.
		/// </summary>
		/// <remarks>kaio</remarks>
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
		/// Método utilizado para atualizar uma instância de TABPesquisaVagaDisponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>kaio</remarks>
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
		/// Método utilizado para salvar uma instância de PesquisaVagaDisponibilidade no banco de dados.
		/// </summary>
		/// <remarks>kaio</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de PesquisaVagaDisponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>kaio</remarks>
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
		/// Método utilizado para excluir uma instância de TABPesquisaVagaDisponibilidade no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
		/// <remarks>kaio</remarks>
		public static void Delete(int idPesquisaVagaDisponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaDisponibilidade;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TABPesquisaVagaDisponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>kaio</remarks>
		public static void Delete(int idPesquisaVagaDisponibilidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaDisponibilidade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TABPesquisaVagaDisponibilidade no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaDisponibilidade">Lista de chaves.</param>
		/// <remarks>kaio</remarks>
		public static void Delete(List<int> idPesquisaVagaDisponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE.TAB_Pesquisa_Vaga_Disponibilidade where Idf_Pesquisa_Vaga_Disponibilidade in (";

			for (int i = 0; i < idPesquisaVagaDisponibilidade.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPesquisaVagaDisponibilidade[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>kaio</remarks>
		private static IDataReader LoadDataReader(int idPesquisaVagaDisponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaDisponibilidade;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>kaio</remarks>
		private static IDataReader LoadDataReader(int idPesquisaVagaDisponibilidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaDisponibilidade;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, TAB.Idf_Pesquisa_Vaga_Disponibilidade, TAB.Idf_Disponibilidade, TAB.Idf_Pesquisa_Vaga FROM BNE.TAB_Pesquisa_Vaga_Disponibilidade TAB";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TABPesquisaVagaDisponibilidade a partir do banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
		/// <returns>Instância de TABPesquisaVagaDisponibilidade.</returns>
		/// <remarks>kaio</remarks>
		public static PesquisaVagaDisponibilidade LoadObject(int idPesquisaVagaDisponibilidade)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaVagaDisponibilidade))
			{
				PesquisaVagaDisponibilidade objPesquisaVagaDisponibilidade = new PesquisaVagaDisponibilidade();
				if (SetInstance(dr, objPesquisaVagaDisponibilidade))
					return objPesquisaVagaDisponibilidade;
			}
			throw (new RecordNotFoundException(typeof(PesquisaVagaDisponibilidade)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TABPesquisaVagaDisponibilidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TABPesquisaVagaDisponibilidade.</returns>
		/// <remarks>kaio</remarks>
		public static PesquisaVagaDisponibilidade LoadObject(int idPesquisaVagaDisponibilidade, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaVagaDisponibilidade, trans))
			{
				PesquisaVagaDisponibilidade objPesquisaVagaDisponibilidade = new PesquisaVagaDisponibilidade();
				if (SetInstance(dr, objPesquisaVagaDisponibilidade))
					return objPesquisaVagaDisponibilidade;
			}
			throw (new RecordNotFoundException(typeof(PesquisaVagaDisponibilidade)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TABPesquisaVagaDisponibilidade a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>kaio</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaVagaDisponibilidade))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TABPesquisaVagaDisponibilidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>kaio</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaVagaDisponibilidade, trans))
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
		/// <param name="objTABPesquisaVagaDisponibilidade">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>kaio</remarks>
		private static bool SetInstance(IDataReader dr, PesquisaVagaDisponibilidade objPesquisaVagaDisponibilidade)
		{
			try
			{
				if (dr.Read())
				{
                    objPesquisaVagaDisponibilidade._idPesquisaVagaDisponibilidade = Convert.ToInt32(dr["Idf_Pesquisa_Vaga_Disponibilidade"]);
					if (dr["Idf_Disponibilidade"] != DBNull.Value)
                        objPesquisaVagaDisponibilidade._disponibilidade = new Disponibilidade(Convert.ToInt32(dr["Idf_Disponibilidade"]));
					if (dr["Idf_Pesquisa_Vaga"] != DBNull.Value)
                        objPesquisaVagaDisponibilidade._pesquisaVaga = new PesquisaVaga(Convert.ToInt32(dr["Idf_Pesquisa_Vaga"]));

                    objPesquisaVagaDisponibilidade._persisted = true;
                    objPesquisaVagaDisponibilidade._modified = false;

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