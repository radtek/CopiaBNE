//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class ClasseSalarial // Tabela: plataforma.TAB_Classe_Salarial
	{
		#region Atributos
		private int _idClasseSalarial;
		private decimal _valorSalarioMedio;
		private decimal _valorPiso;
		private decimal _valorTeto;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdClasseSalarial
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdClasseSalarial
		{
			get
			{
				return this._idClasseSalarial;
			}
			set
			{
				this._idClasseSalarial = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSalarioMedio
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSalarioMedio
		{
			get
			{
				return this._valorSalarioMedio;
			}
			set
			{
				this._valorSalarioMedio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorPiso
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorPiso
		{
			get
			{
				return this._valorPiso;
			}
			set
			{
				this._valorPiso = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorTeto
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorTeto
		{
			get
			{
				return this._valorTeto;
			}
			set
			{
				this._valorTeto = value;
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
		public ClasseSalarial()
		{
		}
		public ClasseSalarial(int idClasseSalarial)
		{
			this._idClasseSalarial = idClasseSalarial;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Classe_Salarial (Idf_Classe_Salarial, Vlr_Salario_Medio, Vlr_Piso, Vlr_Teto, Dta_Cadastro) VALUES (@Idf_Classe_Salarial, @Vlr_Salario_Medio, @Vlr_Piso, @Vlr_Teto, @Dta_Cadastro);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Classe_Salarial SET Vlr_Salario_Medio = @Vlr_Salario_Medio, Vlr_Piso = @Vlr_Piso, Vlr_Teto = @Vlr_Teto, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Classe_Salarial = @Idf_Classe_Salarial";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Classe_Salarial WHERE Idf_Classe_Salarial = @Idf_Classe_Salarial";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Classe_Salarial WHERE Idf_Classe_Salarial = @Idf_Classe_Salarial";
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
			parms.Add(new SqlParameter("@Idf_Classe_Salarial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Vlr_Salario_Medio", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Vlr_Piso", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Vlr_Teto", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idClasseSalarial;
			parms[1].Value = this._valorSalarioMedio;
			parms[2].Value = this._valorPiso;
			parms[3].Value = this._valorTeto;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[4].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de ClasseSalarial no banco de dados.
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
		/// Método utilizado para inserir uma instância de ClasseSalarial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de ClasseSalarial no banco de dados.
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
		/// Método utilizado para atualizar uma instância de ClasseSalarial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de ClasseSalarial no banco de dados.
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
		/// Método utilizado para salvar uma instância de ClasseSalarial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de ClasseSalarial no banco de dados.
		/// </summary>
		/// <param name="idClasseSalarial">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idClasseSalarial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Classe_Salarial", SqlDbType.Int, 4));

			parms[0].Value = idClasseSalarial;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de ClasseSalarial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idClasseSalarial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idClasseSalarial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Classe_Salarial", SqlDbType.Int, 4));

			parms[0].Value = idClasseSalarial;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de ClasseSalarial no banco de dados.
		/// </summary>
		/// <param name="idClasseSalarial">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idClasseSalarial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Classe_Salarial where Idf_Classe_Salarial in (";

			for (int i = 0; i < idClasseSalarial.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idClasseSalarial[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idClasseSalarial">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idClasseSalarial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Classe_Salarial", SqlDbType.Int, 4));

			parms[0].Value = idClasseSalarial;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idClasseSalarial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idClasseSalarial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Classe_Salarial", SqlDbType.Int, 4));

			parms[0].Value = idClasseSalarial;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cla.Idf_Classe_Salarial, Cla.Vlr_Salario_Medio, Cla.Vlr_Piso, Cla.Vlr_Teto, Cla.Dta_Cadastro FROM plataforma.TAB_Classe_Salarial Cla";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de ClasseSalarial a partir do banco de dados.
		/// </summary>
		/// <param name="idClasseSalarial">Chave do registro.</param>
		/// <returns>Instância de ClasseSalarial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ClasseSalarial LoadObject(int idClasseSalarial)
		{
			using (IDataReader dr = LoadDataReader(idClasseSalarial))
			{
				ClasseSalarial objClasseSalarial = new ClasseSalarial();
				if (SetInstance(dr, objClasseSalarial))
					return objClasseSalarial;
			}
			throw (new RecordNotFoundException(typeof(ClasseSalarial)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de ClasseSalarial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idClasseSalarial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de ClasseSalarial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ClasseSalarial LoadObject(int idClasseSalarial, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idClasseSalarial, trans))
			{
				ClasseSalarial objClasseSalarial = new ClasseSalarial();
				if (SetInstance(dr, objClasseSalarial))
					return objClasseSalarial;
			}
			throw (new RecordNotFoundException(typeof(ClasseSalarial)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de ClasseSalarial a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idClasseSalarial))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de ClasseSalarial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idClasseSalarial, trans))
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
		/// <param name="objClasseSalarial">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, ClasseSalarial objClasseSalarial)
		{
			try
			{
				if (dr.Read())
				{
					objClasseSalarial._idClasseSalarial = Convert.ToInt32(dr["Idf_Classe_Salarial"]);
					objClasseSalarial._valorSalarioMedio = Convert.ToDecimal(dr["Vlr_Salario_Medio"]);
					objClasseSalarial._valorPiso = Convert.ToDecimal(dr["Vlr_Piso"]);
					objClasseSalarial._valorTeto = Convert.ToDecimal(dr["Vlr_Teto"]);
					objClasseSalarial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objClasseSalarial._persisted = true;
					objClasseSalarial._modified = false;

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