//-- Data: 09/03/2010 17:06
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using BNE.EL;

namespace BNE.BLL
{
	public partial class FPASAliquota1 // Tabela: TAB_FPAS_Aliquota1
	{
		#region Atributos
		private int _idFPASAliquota;
		private string _nomeFPASAliquota;
		private Int16 _numeroSomatoria;
		private int _idFPAS;
		private decimal _valorPrevidencia;
		private decimal _valorAutonomo;
		private decimal _valorRiscoAcidente;
		private decimal _valorSalarioEducacao;
		private decimal _valorIncra;
		private decimal _valorSenai;
		private decimal _valorSesi;
		private decimal _valorSenac;
		private decimal _valorSesc;
		private decimal _valorSebrae;
		private decimal _valorDpc;
		private decimal _valorFundoAeroviario;
		private decimal _valorSenar;
		private decimal _valorSest;
		private decimal _valorSenat;
		private decimal _valorSescoop;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdFPASAliquota
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdFPASAliquota
		{
			get
			{
				return this._idFPASAliquota;
			}
			set
			{
				this._idFPASAliquota = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeFPASAliquota
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string NomeFPASAliquota
		{
			get
			{
				return this._nomeFPASAliquota;
			}
			set
			{
				this._nomeFPASAliquota = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroSomatoria
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Int16 NumeroSomatoria
		{
			get
			{
				return this._numeroSomatoria;
			}
			set
			{
				this._numeroSomatoria = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdFPAS
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdFPAS
		{
			get
			{
				return this._idFPAS;
			}
			set
			{
				this._idFPAS = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorPrevidencia
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorPrevidencia
		{
			get
			{
				return this._valorPrevidencia;
			}
			set
			{
				this._valorPrevidencia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorAutonomo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorAutonomo
		{
			get
			{
				return this._valorAutonomo;
			}
			set
			{
				this._valorAutonomo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorRiscoAcidente
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorRiscoAcidente
		{
			get
			{
				return this._valorRiscoAcidente;
			}
			set
			{
				this._valorRiscoAcidente = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSalarioEducacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSalarioEducacao
		{
			get
			{
				return this._valorSalarioEducacao;
			}
			set
			{
				this._valorSalarioEducacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorIncra
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorIncra
		{
			get
			{
				return this._valorIncra;
			}
			set
			{
				this._valorIncra = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSenai
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSenai
		{
			get
			{
				return this._valorSenai;
			}
			set
			{
				this._valorSenai = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSesi
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSesi
		{
			get
			{
				return this._valorSesi;
			}
			set
			{
				this._valorSesi = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSenac
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSenac
		{
			get
			{
				return this._valorSenac;
			}
			set
			{
				this._valorSenac = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSesc
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSesc
		{
			get
			{
				return this._valorSesc;
			}
			set
			{
				this._valorSesc = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSebrae
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSebrae
		{
			get
			{
				return this._valorSebrae;
			}
			set
			{
				this._valorSebrae = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorDpc
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorDpc
		{
			get
			{
				return this._valorDpc;
			}
			set
			{
				this._valorDpc = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorFundoAeroviario
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorFundoAeroviario
		{
			get
			{
				return this._valorFundoAeroviario;
			}
			set
			{
				this._valorFundoAeroviario = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSenar
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSenar
		{
			get
			{
				return this._valorSenar;
			}
			set
			{
				this._valorSenar = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSest
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSest
		{
			get
			{
				return this._valorSest;
			}
			set
			{
				this._valorSest = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSenat
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSenat
		{
			get
			{
				return this._valorSenat;
			}
			set
			{
				this._valorSenat = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSescoop
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSescoop
		{
			get
			{
				return this._valorSescoop;
			}
			set
			{
				this._valorSescoop = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public FPASAliquota1()
		{
		}
		public FPASAliquota1(int idFPASAliquota)
		{
			this._idFPASAliquota = idFPASAliquota;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_FPAS_Aliquota1 (Idf_FPAS_Aliquota, Nme_FPAS_Aliquota, Num_Somatoria, Idf_FPAS, Vlr_Previdencia, Vlr_Autonomo, Vlr_Risco_Acidente, Vlr_Salario_Educacao, Vlr_Incra, Vlr_Senai, Vlr_Sesi, Vlr_Senac, Vlr_Sesc, Vlr_Sebrae, Vlr_Dpc, Vlr_Fundo_Aeroviario, Vlr_Senar, Vlr_Sest, Vlr_Senat, Vlr_Sescoop) VALUES (@Idf_FPAS_Aliquota, @Nme_FPAS_Aliquota, @Num_Somatoria, @Idf_FPAS, @Vlr_Previdencia, @Vlr_Autonomo, @Vlr_Risco_Acidente, @Vlr_Salario_Educacao, @Vlr_Incra, @Vlr_Senai, @Vlr_Sesi, @Vlr_Senac, @Vlr_Sesc, @Vlr_Sebrae, @Vlr_Dpc, @Vlr_Fundo_Aeroviario, @Vlr_Senar, @Vlr_Sest, @Vlr_Senat, @Vlr_Sescoop);";
		private const string SPUPDATE = "UPDATE TAB_FPAS_Aliquota1 SET Nme_FPAS_Aliquota = @Nme_FPAS_Aliquota, Num_Somatoria = @Num_Somatoria, Idf_FPAS = @Idf_FPAS, Vlr_Previdencia = @Vlr_Previdencia, Vlr_Autonomo = @Vlr_Autonomo, Vlr_Risco_Acidente = @Vlr_Risco_Acidente, Vlr_Salario_Educacao = @Vlr_Salario_Educacao, Vlr_Incra = @Vlr_Incra, Vlr_Senai = @Vlr_Senai, Vlr_Sesi = @Vlr_Sesi, Vlr_Senac = @Vlr_Senac, Vlr_Sesc = @Vlr_Sesc, Vlr_Sebrae = @Vlr_Sebrae, Vlr_Dpc = @Vlr_Dpc, Vlr_Fundo_Aeroviario = @Vlr_Fundo_Aeroviario, Vlr_Senar = @Vlr_Senar, Vlr_Sest = @Vlr_Sest, Vlr_Senat = @Vlr_Senat, Vlr_Sescoop = @Vlr_Sescoop WHERE Idf_FPAS_Aliquota = @Idf_FPAS_Aliquota";
		private const string SPDELETE = "DELETE FROM TAB_FPAS_Aliquota1 WHERE Idf_FPAS_Aliquota = @Idf_FPAS_Aliquota";
		private const string SPSELECTID = "SELECT * FROM TAB_FPAS_Aliquota1 WHERE Idf_FPAS_Aliquota = @Idf_FPAS_Aliquota";
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
			parms.Add(new SqlParameter("@Idf_FPAS_Aliquota", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_FPAS_Aliquota", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Num_Somatoria", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Idf_FPAS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Vlr_Previdencia", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Autonomo", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Risco_Acidente", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Salario_Educacao", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Incra", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Senai", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Sesi", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Senac", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Sesc", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Sebrae", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Dpc", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Fundo_Aeroviario", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Senar", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Sest", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Senat", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Vlr_Sescoop", SqlDbType.Float, 8));
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
			parms[0].Value = this._idFPASAliquota;
			parms[1].Value = this._nomeFPASAliquota;
			parms[2].Value = this._numeroSomatoria;
			parms[3].Value = this._idFPAS;
			parms[4].Value = this._valorPrevidencia;
			parms[5].Value = this._valorAutonomo;
			parms[6].Value = this._valorRiscoAcidente;
			parms[7].Value = this._valorSalarioEducacao;
			parms[8].Value = this._valorIncra;
			parms[9].Value = this._valorSenai;
			parms[10].Value = this._valorSesi;
			parms[11].Value = this._valorSenac;
			parms[12].Value = this._valorSesc;
			parms[13].Value = this._valorSebrae;
			parms[14].Value = this._valorDpc;
			parms[15].Value = this._valorFundoAeroviario;
			parms[16].Value = this._valorSenar;
			parms[17].Value = this._valorSest;
			parms[18].Value = this._valorSenat;
			parms[19].Value = this._valorSescoop;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de FPASAliquota1 no banco de dados.
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
		/// Método utilizado para inserir uma instância de FPASAliquota1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de FPASAliquota1 no banco de dados.
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
		/// Método utilizado para atualizar uma instância de FPASAliquota1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de FPASAliquota1 no banco de dados.
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
		/// Método utilizado para salvar uma instância de FPASAliquota1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de FPASAliquota1 no banco de dados.
		/// </summary>
		/// <param name="idFPASAliquota">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFPASAliquota)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_FPAS_Aliquota", SqlDbType.Int, 4));

			parms[0].Value = idFPASAliquota;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de FPASAliquota1 no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFPASAliquota">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFPASAliquota, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_FPAS_Aliquota", SqlDbType.Int, 4));

			parms[0].Value = idFPASAliquota;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de FPASAliquota1 no banco de dados.
		/// </summary>
		/// <param name="idFPASAliquota">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idFPASAliquota)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_FPAS_Aliquota1 where Idf_FPAS_Aliquota in (";

			for (int i = 0; i < idFPASAliquota.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idFPASAliquota[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFPASAliquota">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFPASAliquota)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_FPAS_Aliquota", SqlDbType.Int, 4));

			parms[0].Value = idFPASAliquota;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFPASAliquota">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFPASAliquota, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_FPAS_Aliquota", SqlDbType.Int, 4));

			parms[0].Value = idFPASAliquota;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, FPA.Idf_FPAS_Aliquota, FPA.Nme_FPAS_Aliquota, FPA.Num_Somatoria, FPA.Idf_FPAS, FPA.Vlr_Previdencia, FPA.Vlr_Autonomo, FPA.Vlr_Risco_Acidente, FPA.Vlr_Salario_Educacao, FPA.Vlr_Incra, FPA.Vlr_Senai, FPA.Vlr_Sesi, FPA.Vlr_Senac, FPA.Vlr_Sesc, FPA.Vlr_Sebrae, FPA.Vlr_Dpc, FPA.Vlr_Fundo_Aeroviario, FPA.Vlr_Senar, FPA.Vlr_Sest, FPA.Vlr_Senat, FPA.Vlr_Sescoop FROM TAB_FPAS_Aliquota1 FPA";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de FPASAliquota1 a partir do banco de dados.
		/// </summary>
		/// <param name="idFPASAliquota">Chave do registro.</param>
		/// <returns>Instância de FPASAliquota1.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static FPASAliquota1 LoadObject(int idFPASAliquota)
		{
			using (IDataReader dr = LoadDataReader(idFPASAliquota))
			{
				FPASAliquota1 objFPASAliquota1 = new FPASAliquota1();
				if (SetInstance(dr, objFPASAliquota1))
					return objFPASAliquota1;
			}
			throw (new RecordNotFoundException(typeof(FPASAliquota1)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de FPASAliquota1 a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFPASAliquota">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de FPASAliquota1.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static FPASAliquota1 LoadObject(int idFPASAliquota, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idFPASAliquota, trans))
			{
				FPASAliquota1 objFPASAliquota1 = new FPASAliquota1();
				if (SetInstance(dr, objFPASAliquota1))
					return objFPASAliquota1;
			}
			throw (new RecordNotFoundException(typeof(FPASAliquota1)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de FPASAliquota1 a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idFPASAliquota))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de FPASAliquota1 a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idFPASAliquota, trans))
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
		/// <param name="objFPASAliquota1">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, FPASAliquota1 objFPASAliquota1)
		{
			try
			{
				if (dr.Read())
				{
					objFPASAliquota1._idFPASAliquota = Convert.ToInt32(dr["Idf_FPAS_Aliquota"]);
					objFPASAliquota1._nomeFPASAliquota = Convert.ToString(dr["Nme_FPAS_Aliquota"]);
					objFPASAliquota1._numeroSomatoria = Convert.ToInt16(dr["Num_Somatoria"]);
					objFPASAliquota1._idFPAS = Convert.ToInt32(dr["Idf_FPAS"]);
					objFPASAliquota1._valorPrevidencia = Convert.ToDecimal(dr["Vlr_Previdencia"]);
					objFPASAliquota1._valorAutonomo = Convert.ToDecimal(dr["Vlr_Autonomo"]);
					objFPASAliquota1._valorRiscoAcidente = Convert.ToDecimal(dr["Vlr_Risco_Acidente"]);
					objFPASAliquota1._valorSalarioEducacao = Convert.ToDecimal(dr["Vlr_Salario_Educacao"]);
					objFPASAliquota1._valorIncra = Convert.ToDecimal(dr["Vlr_Incra"]);
					objFPASAliquota1._valorSenai = Convert.ToDecimal(dr["Vlr_Senai"]);
					objFPASAliquota1._valorSesi = Convert.ToDecimal(dr["Vlr_Sesi"]);
					objFPASAliquota1._valorSenac = Convert.ToDecimal(dr["Vlr_Senac"]);
					objFPASAliquota1._valorSesc = Convert.ToDecimal(dr["Vlr_Sesc"]);
					objFPASAliquota1._valorSebrae = Convert.ToDecimal(dr["Vlr_Sebrae"]);
					objFPASAliquota1._valorDpc = Convert.ToDecimal(dr["Vlr_Dpc"]);
					objFPASAliquota1._valorFundoAeroviario = Convert.ToDecimal(dr["Vlr_Fundo_Aeroviario"]);
					objFPASAliquota1._valorSenar = Convert.ToDecimal(dr["Vlr_Senar"]);
					objFPASAliquota1._valorSest = Convert.ToDecimal(dr["Vlr_Sest"]);
					objFPASAliquota1._valorSenat = Convert.ToDecimal(dr["Vlr_Senat"]);
					objFPASAliquota1._valorSescoop = Convert.ToDecimal(dr["Vlr_Sescoop"]);

					objFPASAliquota1._persisted = true;
					objFPASAliquota1._modified = false;

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