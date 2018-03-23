//-- Data: 24/06/2013 16:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class SimulacaoR1 // Tabela: BNE_Simulacao_R1
	{
		#region Atributos
		private int _idSimulacaoR1;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private string _nomePessoa;
		private string _numeroDDDTelefone;
		private string _numeroTelefone;
		private string _emailPessoa;
		private Cidade _cidade;
		private Funcao _funcao;
		private decimal _valorSalarioMax;
		private decimal _valorSalarioMin;
		private int _numeroVagas;
		private DateTime _dataCadastro;
		private decimal _valorTaxaAbertura;
		private decimal _valorServicoPrestado;
		private decimal _valorBonusSolicitacaoOnline;
		private decimal _valorTotal;
		private decimal _valorMargemPercentualServico;
		private int _quantidadeDiasAtendimento;
		private ConsultorR1 _consultorR1;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdSimulacaoR1
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdSimulacaoR1
		{
			get
			{
				return this._idSimulacaoR1;
			}
		}
		#endregion 

		#region UsuarioFilialPerfil
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public UsuarioFilialPerfil UsuarioFilialPerfil
		{
			get
			{
				return this._usuarioFilialPerfil;
			}
			set
			{
				this._usuarioFilialPerfil = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomePessoa
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string NomePessoa
		{
			get
			{
				return this._nomePessoa;
			}
			set
			{
				this._nomePessoa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDDDTelefone
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo obrigatório.
		/// </summary>
		public string NumeroDDDTelefone
		{
			get
			{
				return this._numeroDDDTelefone;
			}
			set
			{
				this._numeroDDDTelefone = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroTelefone
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo obrigatório.
		/// </summary>
		public string NumeroTelefone
		{
			get
			{
				return this._numeroTelefone;
			}
			set
			{
				this._numeroTelefone = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailPessoa
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string EmailPessoa
		{
			get
			{
				return this._emailPessoa;
			}
			set
			{
				this._emailPessoa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Cidade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Cidade Cidade
		{
			get
			{
				return this._cidade;
			}
			set
			{
				this._cidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Funcao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Funcao Funcao
		{
			get
			{
				return this._funcao;
			}
			set
			{
				this._funcao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSalarioMax
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSalarioMax
		{
			get
			{
				return this._valorSalarioMax;
			}
			set
			{
				this._valorSalarioMax = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSalarioMin
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSalarioMin
		{
			get
			{
				return this._valorSalarioMin;
			}
			set
			{
				this._valorSalarioMin = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroVagas
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int NumeroVagas
		{
			get
			{
				return this._numeroVagas;
			}
			set
			{
				this._numeroVagas = value;
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

		#region ValorTaxaAbertura
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorTaxaAbertura
		{
			get
			{
				return this._valorTaxaAbertura;
			}
			set
			{
				this._valorTaxaAbertura = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorServicoPrestado
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorServicoPrestado
		{
			get
			{
				return this._valorServicoPrestado;
			}
			set
			{
				this._valorServicoPrestado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorBonusSolicitacaoOnline
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorBonusSolicitacaoOnline
		{
			get
			{
				return this._valorBonusSolicitacaoOnline;
			}
			set
			{
				this._valorBonusSolicitacaoOnline = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorTotal
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorTotal
		{
			get
			{
				return this._valorTotal;
			}
			set
			{
				this._valorTotal = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorMargemPercentualServico
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorMargemPercentualServico
		{
			get
			{
				return this._valorMargemPercentualServico;
			}
			set
			{
				this._valorMargemPercentualServico = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeDiasAtendimento
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int QuantidadeDiasAtendimento
		{
			get
			{
				return this._quantidadeDiasAtendimento;
			}
			set
			{
				this._quantidadeDiasAtendimento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ConsultorR1
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public ConsultorR1 ConsultorR1
		{
			get
			{
				return this._consultorR1;
			}
			set
			{
				this._consultorR1 = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public SimulacaoR1()
		{
		}
		public SimulacaoR1(int idSimulacaoR1)
		{
			this._idSimulacaoR1 = idSimulacaoR1;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Simulacao_R1 (Idf_Usuario_Filial_Perfil, Nme_Pessoa, Num_DDD_Telefone, Num_Telefone, Eml_Pessoa, Idf_Cidade, Idf_Funcao, Vlr_Salario_Max, Vlr_Salario_Min, Num_Vagas, Dta_Cadastro, Vlr_Taxa_Abertura, Vlr_Servico_Prestado, Vlr_Bonus_Solicitacao_Online, Vlr_Total, Vlr_Margem_Percentual_Servico, Qtd_Dias_Atendimento, Idf_Consultor_R1) VALUES (@Idf_Usuario_Filial_Perfil, @Nme_Pessoa, @Num_DDD_Telefone, @Num_Telefone, @Eml_Pessoa, @Idf_Cidade, @Idf_Funcao, @Vlr_Salario_Max, @Vlr_Salario_Min, @Num_Vagas, @Dta_Cadastro, @Vlr_Taxa_Abertura, @Vlr_Servico_Prestado, @Vlr_Bonus_Solicitacao_Online, @Vlr_Total, @Vlr_Margem_Percentual_Servico, @Qtd_Dias_Atendimento, @Idf_Consultor_R1);SET @Idf_Simulacao_R1 = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Simulacao_R1 SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Nme_Pessoa = @Nme_Pessoa, Num_DDD_Telefone = @Num_DDD_Telefone, Num_Telefone = @Num_Telefone, Eml_Pessoa = @Eml_Pessoa, Idf_Cidade = @Idf_Cidade, Idf_Funcao = @Idf_Funcao, Vlr_Salario_Max = @Vlr_Salario_Max, Vlr_Salario_Min = @Vlr_Salario_Min, Num_Vagas = @Num_Vagas, Dta_Cadastro = @Dta_Cadastro, Vlr_Taxa_Abertura = @Vlr_Taxa_Abertura, Vlr_Servico_Prestado = @Vlr_Servico_Prestado, Vlr_Bonus_Solicitacao_Online = @Vlr_Bonus_Solicitacao_Online, Vlr_Total = @Vlr_Total, Vlr_Margem_Percentual_Servico = @Vlr_Margem_Percentual_Servico, Qtd_Dias_Atendimento = @Qtd_Dias_Atendimento, Idf_Consultor_R1 = @Idf_Consultor_R1 WHERE Idf_Simulacao_R1 = @Idf_Simulacao_R1";
		private const string SPDELETE = "DELETE FROM BNE_Simulacao_R1 WHERE Idf_Simulacao_R1 = @Idf_Simulacao_R1";
		private const string SPSELECTID = "SELECT * FROM BNE_Simulacao_R1 WHERE Idf_Simulacao_R1 = @Idf_Simulacao_R1";
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
			parms.Add(new SqlParameter("@Idf_Simulacao_R1", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Num_DDD_Telefone", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Telefone", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Vlr_Salario_Max", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Vlr_Salario_Min", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Num_Vagas", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Vlr_Taxa_Abertura", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Vlr_Servico_Prestado", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Vlr_Bonus_Solicitacao_Online", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Vlr_Total", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Vlr_Margem_Percentual_Servico", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Qtd_Dias_Atendimento", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));
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
			parms[0].Value = this._idSimulacaoR1;

			if (this._usuarioFilialPerfil != null)
				parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
			else
				parms[1].Value = DBNull.Value;

			parms[2].Value = this._nomePessoa;
			parms[3].Value = this._numeroDDDTelefone;
			parms[4].Value = this._numeroTelefone;

			if (!String.IsNullOrEmpty(this._emailPessoa))
				parms[5].Value = this._emailPessoa;
			else
				parms[5].Value = DBNull.Value;

			parms[6].Value = this._cidade.IdCidade;
			parms[7].Value = this._funcao.IdFuncao;
			parms[8].Value = this._valorSalarioMax;
			parms[9].Value = this._valorSalarioMin;
			parms[10].Value = this._numeroVagas;
			parms[12].Value = this._valorTaxaAbertura;
			parms[13].Value = this._valorServicoPrestado;
			parms[14].Value = this._valorBonusSolicitacaoOnline;
			parms[15].Value = this._valorTotal;
			parms[16].Value = this._valorMargemPercentualServico;
			parms[17].Value = this._quantidadeDiasAtendimento;
			parms[18].Value = this._consultorR1.IdConsultorR1;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[11].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de SimulacaoR1 no banco de dados.
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
						this._idSimulacaoR1 = Convert.ToInt32(cmd.Parameters["@Idf_Simulacao_R1"].Value);
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
		/// Método utilizado para inserir uma instância de SimulacaoR1 no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idSimulacaoR1 = Convert.ToInt32(cmd.Parameters["@Idf_Simulacao_R1"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de SimulacaoR1 no banco de dados.
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
		/// Método utilizado para atualizar uma instância de SimulacaoR1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de SimulacaoR1 no banco de dados.
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
		/// Método utilizado para salvar uma instância de SimulacaoR1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de SimulacaoR1 no banco de dados.
		/// </summary>
		/// <param name="idSimulacaoR1">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idSimulacaoR1)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Simulacao_R1", SqlDbType.Int, 4));

			parms[0].Value = idSimulacaoR1;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de SimulacaoR1 no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSimulacaoR1">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idSimulacaoR1, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Simulacao_R1", SqlDbType.Int, 4));

			parms[0].Value = idSimulacaoR1;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de SimulacaoR1 no banco de dados.
		/// </summary>
		/// <param name="idSimulacaoR1">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idSimulacaoR1)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Simulacao_R1 where Idf_Simulacao_R1 in (";

			for (int i = 0; i < idSimulacaoR1.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idSimulacaoR1[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idSimulacaoR1">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idSimulacaoR1)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Simulacao_R1", SqlDbType.Int, 4));

			parms[0].Value = idSimulacaoR1;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSimulacaoR1">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idSimulacaoR1, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Simulacao_R1", SqlDbType.Int, 4));

			parms[0].Value = idSimulacaoR1;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sim.Idf_Simulacao_R1, Sim.Idf_Usuario_Filial_Perfil, Sim.Nme_Pessoa, Sim.Num_DDD_Telefone, Sim.Num_Telefone, Sim.Eml_Pessoa, Sim.Idf_Cidade, Sim.Idf_Funcao, Sim.Vlr_Salario_Max, Sim.Vlr_Salario_Min, Sim.Num_Vagas, Sim.Dta_Cadastro, Sim.Vlr_Taxa_Abertura, Sim.Vlr_Servico_Prestado, Sim.Vlr_Bonus_Solicitacao_Online, Sim.Vlr_Total, Sim.Vlr_Margem_Percentual_Servico, Sim.Qtd_Dias_Atendimento, Sim.Idf_Consultor_R1 FROM BNE_Simulacao_R1 Sim";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de SimulacaoR1 a partir do banco de dados.
		/// </summary>
		/// <param name="idSimulacaoR1">Chave do registro.</param>
		/// <returns>Instância de SimulacaoR1.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static SimulacaoR1 LoadObject(int idSimulacaoR1)
		{
			using (IDataReader dr = LoadDataReader(idSimulacaoR1))
			{
				SimulacaoR1 objSimulacaoR1 = new SimulacaoR1();
				if (SetInstance(dr, objSimulacaoR1))
					return objSimulacaoR1;
			}
			throw (new RecordNotFoundException(typeof(SimulacaoR1)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de SimulacaoR1 a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSimulacaoR1">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de SimulacaoR1.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static SimulacaoR1 LoadObject(int idSimulacaoR1, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idSimulacaoR1, trans))
			{
				SimulacaoR1 objSimulacaoR1 = new SimulacaoR1();
				if (SetInstance(dr, objSimulacaoR1))
					return objSimulacaoR1;
			}
			throw (new RecordNotFoundException(typeof(SimulacaoR1)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de SimulacaoR1 a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idSimulacaoR1))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de SimulacaoR1 a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idSimulacaoR1, trans))
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
		/// <param name="objSimulacaoR1">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, SimulacaoR1 objSimulacaoR1)
		{
			try
			{
				if (dr.Read())
				{
					objSimulacaoR1._idSimulacaoR1 = Convert.ToInt32(dr["Idf_Simulacao_R1"]);
					if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
						objSimulacaoR1._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					objSimulacaoR1._nomePessoa = Convert.ToString(dr["Nme_Pessoa"]);
					objSimulacaoR1._numeroDDDTelefone = Convert.ToString(dr["Num_DDD_Telefone"]);
					objSimulacaoR1._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
					if (dr["Eml_Pessoa"] != DBNull.Value)
						objSimulacaoR1._emailPessoa = Convert.ToString(dr["Eml_Pessoa"]);
					objSimulacaoR1._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					objSimulacaoR1._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
					objSimulacaoR1._valorSalarioMax = Convert.ToDecimal(dr["Vlr_Salario_Max"]);
					objSimulacaoR1._valorSalarioMin = Convert.ToDecimal(dr["Vlr_Salario_Min"]);
					objSimulacaoR1._numeroVagas = Convert.ToInt32(dr["Num_Vagas"]);
					objSimulacaoR1._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objSimulacaoR1._valorTaxaAbertura = Convert.ToDecimal(dr["Vlr_Taxa_Abertura"]);
					objSimulacaoR1._valorServicoPrestado = Convert.ToDecimal(dr["Vlr_Servico_Prestado"]);
					objSimulacaoR1._valorBonusSolicitacaoOnline = Convert.ToDecimal(dr["Vlr_Bonus_Solicitacao_Online"]);
					objSimulacaoR1._valorTotal = Convert.ToDecimal(dr["Vlr_Total"]);
					objSimulacaoR1._valorMargemPercentualServico = Convert.ToDecimal(dr["Vlr_Margem_Percentual_Servico"]);
					objSimulacaoR1._quantidadeDiasAtendimento = Convert.ToInt32(dr["Qtd_Dias_Atendimento"]);
					objSimulacaoR1._consultorR1 = new ConsultorR1(Convert.ToInt32(dr["Idf_Consultor_R1"]));

					objSimulacaoR1._persisted = true;
					objSimulacaoR1._modified = false;

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