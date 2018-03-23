//-- Data: 25/03/2015 13:55
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class SolicitacaoR1 // Tabela: BNE_Solicitacao_R1
	{
		#region Atributos
		private int _idSolicitacaoR1;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private string _nomeSolicitante;
		private string _numeroDDDSolicitante;
		private string _numeroTelefoneSolicitante;
		private string _emailSolicitante;
		private Cidade _cidadeSolicitante;
		private Funcao _funcaoSolicitante;
		private Funcao _funcao;
		private int? _quantidadeExperiencia;
		private int _quantidadeVagas;
		private AreaBNE _areaBNE;
		private string _descricaoAtividade;
		private string _descricaoRequisitoObrigatorio;
		private string _descricaoRequisitoDesejavel;
		private Int16? _numeroIdadeMinima;
		private Int16? _numeroIdadeMaxima;
		private Escolaridade _escolaridade;
		private string _descricaoConhecimentoInformatica;
		private decimal? _valorSalarioDe;
		private decimal? _valorSalarioAte;
		private Sexo _sexo;
		private string _descricaoBeneficio;
		private EstadoCivil _estadoCivil;
		private Cidade _cidade;
		private CategoriaHabilitacao _categoriaHabilitacao;
		private string _descricaoInformacaoAdicional;
		private ConsultorR1 _consultorR1;
		private SimulacaoR1 _simulacaoR1;
		private DateTime _dataCadastro;
		private TipoVinculo _tipoVinculo;
		private string _descricaoBairro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdSolicitacaoR1
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdSolicitacaoR1
		{
			get
			{
				return this._idSolicitacaoR1;
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

		#region NomeSolicitante
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string NomeSolicitante
		{
			get
			{
				return this._nomeSolicitante;
			}
			set
			{
				this._nomeSolicitante = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDDDSolicitante
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo obrigatório.
		/// </summary>
		public string NumeroDDDSolicitante
		{
			get
			{
				return this._numeroDDDSolicitante;
			}
			set
			{
				this._numeroDDDSolicitante = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroTelefoneSolicitante
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo obrigatório.
		/// </summary>
		public string NumeroTelefoneSolicitante
		{
			get
			{
				return this._numeroTelefoneSolicitante;
			}
			set
			{
				this._numeroTelefoneSolicitante = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailSolicitante
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string EmailSolicitante
		{
			get
			{
				return this._emailSolicitante;
			}
			set
			{
				this._emailSolicitante = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CidadeSolicitante
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Cidade CidadeSolicitante
		{
			get
			{
				return this._cidadeSolicitante;
			}
			set
			{
				this._cidadeSolicitante = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FuncaoSolicitante
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Funcao FuncaoSolicitante
		{
			get
			{
				return this._funcaoSolicitante;
			}
			set
			{
				this._funcaoSolicitante = value;
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

		#region QuantidadeExperiencia
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? QuantidadeExperiencia
		{
			get
			{
				return this._quantidadeExperiencia;
			}
			set
			{
				this._quantidadeExperiencia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeVagas
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int QuantidadeVagas
		{
			get
			{
				return this._quantidadeVagas;
			}
			set
			{
				this._quantidadeVagas = value;
				this._modified = true;
			}
		}
		#endregion 

		#region AreaBNE
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public AreaBNE AreaBNE
		{
			get
			{
				return this._areaBNE;
			}
			set
			{
				this._areaBNE = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoAtividade
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoAtividade
		{
			get
			{
				return this._descricaoAtividade;
			}
			set
			{
				this._descricaoAtividade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoRequisitoObrigatorio
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoRequisitoObrigatorio
		{
			get
			{
				return this._descricaoRequisitoObrigatorio;
			}
			set
			{
				this._descricaoRequisitoObrigatorio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoRequisitoDesejavel
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoRequisitoDesejavel
		{
			get
			{
				return this._descricaoRequisitoDesejavel;
			}
			set
			{
				this._descricaoRequisitoDesejavel = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroIdadeMinima
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? NumeroIdadeMinima
		{
			get
			{
				return this._numeroIdadeMinima;
			}
			set
			{
				this._numeroIdadeMinima = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroIdadeMaxima
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? NumeroIdadeMaxima
		{
			get
			{
				return this._numeroIdadeMaxima;
			}
			set
			{
				this._numeroIdadeMaxima = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Escolaridade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Escolaridade Escolaridade
		{
			get
			{
				return this._escolaridade;
			}
			set
			{
				this._escolaridade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoConhecimentoInformatica
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoConhecimentoInformatica
		{
			get
			{
				return this._descricaoConhecimentoInformatica;
			}
			set
			{
				this._descricaoConhecimentoInformatica = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSalarioDe
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? ValorSalarioDe
		{
			get
			{
				return this._valorSalarioDe;
			}
			set
			{
				this._valorSalarioDe = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSalarioAte
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? ValorSalarioAte
		{
			get
			{
				return this._valorSalarioAte;
			}
			set
			{
				this._valorSalarioAte = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Sexo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Sexo Sexo
		{
			get
			{
				return this._sexo;
			}
			set
			{
				this._sexo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoBeneficio
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoBeneficio
		{
			get
			{
				return this._descricaoBeneficio;
			}
			set
			{
				this._descricaoBeneficio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EstadoCivil
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public EstadoCivil EstadoCivil
		{
			get
			{
				return this._estadoCivil;
			}
			set
			{
				this._estadoCivil = value;
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

		#region CategoriaHabilitacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public CategoriaHabilitacao CategoriaHabilitacao
		{
			get
			{
				return this._categoriaHabilitacao;
			}
			set
			{
				this._categoriaHabilitacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoInformacaoAdicional
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoInformacaoAdicional
		{
			get
			{
				return this._descricaoInformacaoAdicional;
			}
			set
			{
				this._descricaoInformacaoAdicional = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ConsultorR1
		/// <summary>
		/// Campo opcional.
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

		#region SimulacaoR1
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public SimulacaoR1 SimulacaoR1
		{
			get
			{
				return this._simulacaoR1;
			}
			set
			{
				this._simulacaoR1 = value;
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

        #region TipoVinculo
        /// <summary>
		/// Campo opcional.
		/// </summary>
		public TipoVinculo TipoVinculo
		{
			get
			{
				return this._tipoVinculo;
			}
			set
			{
				this._tipoVinculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoBairro
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string DescricaoBairro
		{
			get
			{
				return this._descricaoBairro;
			}
			set
			{
				this._descricaoBairro = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public SolicitacaoR1()
		{
		}
		public SolicitacaoR1(int idSolicitacaoR1)
		{
			this._idSolicitacaoR1 = idSolicitacaoR1;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Solicitacao_R1 (Idf_Usuario_Filial_Perfil, Nme_Solicitante, Num_DDD_Solicitante, Num_Telefone_Solicitante, Eml_Solicitante, Idf_Cidade_Solicitante, Idf_Funcao_Solicitante, Idf_Funcao, Qtd_Experiencia, Qtd_Vagas, Idf_Area_BNE, Des_Atividade, Des_Requisito_Obrigatorio, Des_Requisito_Desejavel, Num_Idade_Minima, Num_Idade_Maxima, Idf_Escolaridade, Des_Conhecimento_Informatica, Vlr_Salario_De, Vlr_Salario_Ate, Idf_Sexo, Des_Beneficio, Idf_Estado_Civil, Idf_Cidade, Idf_Categoria_Habilitacao, Des_Informacao_Adicional, Idf_Consultor_R1, Idf_Simulacao_R1, Dta_Cadastro, Idf_Tipo_Vinculo, Des_Bairro) VALUES (@Idf_Usuario_Filial_Perfil, @Nme_Solicitante, @Num_DDD_Solicitante, @Num_Telefone_Solicitante, @Eml_Solicitante, @Idf_Cidade_Solicitante, @Idf_Funcao_Solicitante, @Idf_Funcao, @Qtd_Experiencia, @Qtd_Vagas, @Idf_Area_BNE, @Des_Atividade, @Des_Requisito_Obrigatorio, @Des_Requisito_Desejavel, @Num_Idade_Minima, @Num_Idade_Maxima, @Idf_Escolaridade, @Des_Conhecimento_Informatica, @Vlr_Salario_De, @Vlr_Salario_Ate, @Idf_Sexo, @Des_Beneficio, @Idf_Estado_Civil, @Idf_Cidade, @Idf_Categoria_Habilitacao, @Des_Informacao_Adicional, @Idf_Consultor_R1, @Idf_Simulacao_R1, @Dta_Cadastro, @Idf_Tipo_Vinculo, @Des_Bairro);SET @Idf_Solicitacao_R1 = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Solicitacao_R1 SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Nme_Solicitante = @Nme_Solicitante, Num_DDD_Solicitante = @Num_DDD_Solicitante, Num_Telefone_Solicitante = @Num_Telefone_Solicitante, Eml_Solicitante = @Eml_Solicitante, Idf_Cidade_Solicitante = @Idf_Cidade_Solicitante, Idf_Funcao_Solicitante = @Idf_Funcao_Solicitante, Idf_Funcao = @Idf_Funcao, Qtd_Experiencia = @Qtd_Experiencia, Qtd_Vagas = @Qtd_Vagas, Idf_Area_BNE = @Idf_Area_BNE, Des_Atividade = @Des_Atividade, Des_Requisito_Obrigatorio = @Des_Requisito_Obrigatorio, Des_Requisito_Desejavel = @Des_Requisito_Desejavel, Num_Idade_Minima = @Num_Idade_Minima, Num_Idade_Maxima = @Num_Idade_Maxima, Idf_Escolaridade = @Idf_Escolaridade, Des_Conhecimento_Informatica = @Des_Conhecimento_Informatica, Vlr_Salario_De = @Vlr_Salario_De, Vlr_Salario_Ate = @Vlr_Salario_Ate, Idf_Sexo = @Idf_Sexo, Des_Beneficio = @Des_Beneficio, Idf_Estado_Civil = @Idf_Estado_Civil, Idf_Cidade = @Idf_Cidade, Idf_Categoria_Habilitacao = @Idf_Categoria_Habilitacao, Des_Informacao_Adicional = @Des_Informacao_Adicional, Idf_Consultor_R1 = @Idf_Consultor_R1, Idf_Simulacao_R1 = @Idf_Simulacao_R1, Dta_Cadastro = @Dta_Cadastro, Idf_Tipo_Vinculo = @Idf_Tipo_Vinculo, Des_Bairro = @Des_Bairro WHERE Idf_Solicitacao_R1 = @Idf_Solicitacao_R1";
		private const string SPDELETE = "DELETE FROM BNE_Solicitacao_R1 WHERE Idf_Solicitacao_R1 = @Idf_Solicitacao_R1";
		private const string SPSELECTID = "SELECT * FROM BNE_Solicitacao_R1 WITH(NOLOCK) WHERE Idf_Solicitacao_R1 = @Idf_Solicitacao_R1";
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
			parms.Add(new SqlParameter("@Idf_Solicitacao_R1", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Solicitante", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Num_DDD_Solicitante", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Telefone_Solicitante", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Eml_Solicitante", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Idf_Cidade_Solicitante", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao_Solicitante", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Qtd_Experiencia", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Qtd_Vagas", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Area_BNE", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Atividade", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Des_Requisito_Obrigatorio", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Des_Requisito_Desejavel", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Num_Idade_Minima", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Num_Idade_Maxima", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Conhecimento_Informatica", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Vlr_Salario_De", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Vlr_Salario_Ate", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Idf_Sexo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Beneficio", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Categoria_Habilitacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Informacao_Adicional", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Simulacao_R1", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Tipo_Vinculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Bairro", SqlDbType.VarChar, 200));
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
			parms[0].Value = this._idSolicitacaoR1;

			if (this._usuarioFilialPerfil != null)
				parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
			else
				parms[1].Value = DBNull.Value;

			parms[2].Value = this._nomeSolicitante;
			parms[3].Value = this._numeroDDDSolicitante;
			parms[4].Value = this._numeroTelefoneSolicitante;

			if (!String.IsNullOrEmpty(this._emailSolicitante))
				parms[5].Value = this._emailSolicitante;
			else
				parms[5].Value = DBNull.Value;


			if (this._cidadeSolicitante != null)
				parms[6].Value = this._cidadeSolicitante.IdCidade;
			else
				parms[6].Value = DBNull.Value;


			if (this._funcaoSolicitante != null)
				parms[7].Value = this._funcaoSolicitante.IdFuncao;
			else
				parms[7].Value = DBNull.Value;

			parms[8].Value = this._funcao.IdFuncao;

			if (this._quantidadeExperiencia.HasValue)
				parms[9].Value = this._quantidadeExperiencia;
			else
				parms[9].Value = DBNull.Value;

			parms[10].Value = this._quantidadeVagas;

			if (this._areaBNE != null)
				parms[11].Value = this._areaBNE.IdAreaBNE;
			else
				parms[11].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoAtividade))
				parms[12].Value = this._descricaoAtividade;
			else
				parms[12].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoRequisitoObrigatorio))
				parms[13].Value = this._descricaoRequisitoObrigatorio;
			else
				parms[13].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoRequisitoDesejavel))
				parms[14].Value = this._descricaoRequisitoDesejavel;
			else
				parms[14].Value = DBNull.Value;


			if (this._numeroIdadeMinima.HasValue)
				parms[15].Value = this._numeroIdadeMinima;
			else
				parms[15].Value = DBNull.Value;


			if (this._numeroIdadeMaxima.HasValue)
				parms[16].Value = this._numeroIdadeMaxima;
			else
				parms[16].Value = DBNull.Value;

			parms[17].Value = this._escolaridade.IdEscolaridade;

			if (!String.IsNullOrEmpty(this._descricaoConhecimentoInformatica))
				parms[18].Value = this._descricaoConhecimentoInformatica;
			else
				parms[18].Value = DBNull.Value;


			if (this._valorSalarioDe.HasValue)
				parms[19].Value = this._valorSalarioDe;
			else
				parms[19].Value = DBNull.Value;


			if (this._valorSalarioAte.HasValue)
				parms[20].Value = this._valorSalarioAte;
			else
				parms[20].Value = DBNull.Value;


			if (this._sexo != null)
				parms[21].Value = this._sexo.IdSexo;
			else
				parms[21].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoBeneficio))
				parms[22].Value = this._descricaoBeneficio;
			else
				parms[22].Value = DBNull.Value;


			if (this._estadoCivil != null)
				parms[23].Value = this._estadoCivil.IdEstadoCivil;
			else
				parms[23].Value = DBNull.Value;

			parms[24].Value = this._cidade.IdCidade;

			if (this._categoriaHabilitacao != null)
				parms[25].Value = this._categoriaHabilitacao.IdCategoriaHabilitacao;
			else
				parms[25].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoInformacaoAdicional))
				parms[26].Value = this._descricaoInformacaoAdicional;
			else
				parms[26].Value = DBNull.Value;

            if (this._consultorR1 != null)
                parms[27].Value = this._consultorR1.IdConsultorR1;
            else
                parms[27].Value = DBNull.Value;

			if (this._simulacaoR1 != null)
				parms[28].Value = this._simulacaoR1.IdSimulacaoR1;
			else
				parms[28].Value = DBNull.Value;


			if (this.TipoVinculo != null)
				parms[30].Value = this._tipoVinculo.IdTipoVinculo;
			else
				parms[30].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoBairro))
				parms[31].Value = this._descricaoBairro;
			else
				parms[31].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[29].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de SolicitacaoR1 no banco de dados.
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
						this._idSolicitacaoR1 = Convert.ToInt32(cmd.Parameters["@Idf_Solicitacao_R1"].Value);
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
		/// Método utilizado para inserir uma instância de SolicitacaoR1 no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idSolicitacaoR1 = Convert.ToInt32(cmd.Parameters["@Idf_Solicitacao_R1"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de SolicitacaoR1 no banco de dados.
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
		/// Método utilizado para atualizar uma instância de SolicitacaoR1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de SolicitacaoR1 no banco de dados.
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
		/// Método utilizado para salvar uma instância de SolicitacaoR1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de SolicitacaoR1 no banco de dados.
		/// </summary>
		/// <param name="idSolicitacaoR1">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idSolicitacaoR1)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Solicitacao_R1", SqlDbType.Int, 4));

			parms[0].Value = idSolicitacaoR1;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de SolicitacaoR1 no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSolicitacaoR1">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idSolicitacaoR1, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Solicitacao_R1", SqlDbType.Int, 4));

			parms[0].Value = idSolicitacaoR1;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de SolicitacaoR1 no banco de dados.
		/// </summary>
		/// <param name="idSolicitacaoR1">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idSolicitacaoR1)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Solicitacao_R1 where Idf_Solicitacao_R1 in (";

			for (int i = 0; i < idSolicitacaoR1.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idSolicitacaoR1[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idSolicitacaoR1">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idSolicitacaoR1)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Solicitacao_R1", SqlDbType.Int, 4));

			parms[0].Value = idSolicitacaoR1;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSolicitacaoR1">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idSolicitacaoR1, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Solicitacao_R1", SqlDbType.Int, 4));

			parms[0].Value = idSolicitacaoR1;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sol.Idf_Solicitacao_R1, Sol.Idf_Usuario_Filial_Perfil, Sol.Nme_Solicitante, Sol.Num_DDD_Solicitante, Sol.Num_Telefone_Solicitante, Sol.Eml_Solicitante, Sol.Idf_Cidade_Solicitante, Sol.Idf_Funcao_Solicitante, Sol.Idf_Funcao, Sol.Qtd_Experiencia, Sol.Qtd_Vagas, Sol.Idf_Area_BNE, Sol.Des_Atividade, Sol.Des_Requisito_Obrigatorio, Sol.Des_Requisito_Desejavel, Sol.Num_Idade_Minima, Sol.Num_Idade_Maxima, Sol.Idf_Escolaridade, Sol.Des_Conhecimento_Informatica, Sol.Vlr_Salario_De, Sol.Vlr_Salario_Ate, Sol.Idf_Sexo, Sol.Des_Beneficio, Sol.Idf_Estado_Civil, Sol.Idf_Cidade, Sol.Idf_Categoria_Habilitacao, Sol.Des_Informacao_Adicional, Sol.Idf_Consultor_R1, Sol.Idf_Simulacao_R1, Sol.Dta_Cadastro, Sol.Idf_Tipo_Vinculo, Sol.Des_Bairro FROM BNE_Solicitacao_R1 Sol";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de SolicitacaoR1 a partir do banco de dados.
		/// </summary>
		/// <param name="idSolicitacaoR1">Chave do registro.</param>
		/// <returns>Instância de SolicitacaoR1.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static SolicitacaoR1 LoadObject(int idSolicitacaoR1)
		{
			using (IDataReader dr = LoadDataReader(idSolicitacaoR1))
			{
				SolicitacaoR1 objSolicitacaoR1 = new SolicitacaoR1();
				if (SetInstance(dr, objSolicitacaoR1))
					return objSolicitacaoR1;
			}
			throw (new RecordNotFoundException(typeof(SolicitacaoR1)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de SolicitacaoR1 a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idSolicitacaoR1">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de SolicitacaoR1.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static SolicitacaoR1 LoadObject(int idSolicitacaoR1, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idSolicitacaoR1, trans))
			{
				SolicitacaoR1 objSolicitacaoR1 = new SolicitacaoR1();
				if (SetInstance(dr, objSolicitacaoR1))
					return objSolicitacaoR1;
			}
			throw (new RecordNotFoundException(typeof(SolicitacaoR1)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de SolicitacaoR1 a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idSolicitacaoR1))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de SolicitacaoR1 a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idSolicitacaoR1, trans))
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
		/// <param name="objSolicitacaoR1">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, SolicitacaoR1 objSolicitacaoR1)
		{
			try
			{
				if (dr.Read())
				{
					objSolicitacaoR1._idSolicitacaoR1 = Convert.ToInt32(dr["Idf_Solicitacao_R1"]);
					if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
						objSolicitacaoR1._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					objSolicitacaoR1._nomeSolicitante = Convert.ToString(dr["Nme_Solicitante"]);
					objSolicitacaoR1._numeroDDDSolicitante = Convert.ToString(dr["Num_DDD_Solicitante"]);
					objSolicitacaoR1._numeroTelefoneSolicitante = Convert.ToString(dr["Num_Telefone_Solicitante"]);
					if (dr["Eml_Solicitante"] != DBNull.Value)
						objSolicitacaoR1._emailSolicitante = Convert.ToString(dr["Eml_Solicitante"]);
					if (dr["Idf_Cidade_Solicitante"] != DBNull.Value)
						objSolicitacaoR1._cidadeSolicitante = new Cidade(Convert.ToInt32(dr["Idf_Cidade_Solicitante"]));
					if (dr["Idf_Funcao_Solicitante"] != DBNull.Value)
						objSolicitacaoR1._funcaoSolicitante = new Funcao(Convert.ToInt32(dr["Idf_Funcao_Solicitante"]));
					objSolicitacaoR1._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
					if (dr["Qtd_Experiencia"] != DBNull.Value)
						objSolicitacaoR1._quantidadeExperiencia = Convert.ToInt32(dr["Qtd_Experiencia"]);
					objSolicitacaoR1._quantidadeVagas = Convert.ToInt32(dr["Qtd_Vagas"]);
					if (dr["Idf_Area_BNE"] != DBNull.Value)
						objSolicitacaoR1._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
					if (dr["Des_Atividade"] != DBNull.Value)
						objSolicitacaoR1._descricaoAtividade = Convert.ToString(dr["Des_Atividade"]);
					if (dr["Des_Requisito_Obrigatorio"] != DBNull.Value)
						objSolicitacaoR1._descricaoRequisitoObrigatorio = Convert.ToString(dr["Des_Requisito_Obrigatorio"]);
					if (dr["Des_Requisito_Desejavel"] != DBNull.Value)
						objSolicitacaoR1._descricaoRequisitoDesejavel = Convert.ToString(dr["Des_Requisito_Desejavel"]);
					if (dr["Num_Idade_Minima"] != DBNull.Value)
						objSolicitacaoR1._numeroIdadeMinima = Convert.ToInt16(dr["Num_Idade_Minima"]);
					if (dr["Num_Idade_Maxima"] != DBNull.Value)
						objSolicitacaoR1._numeroIdadeMaxima = Convert.ToInt16(dr["Num_Idade_Maxima"]);
					objSolicitacaoR1._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
					if (dr["Des_Conhecimento_Informatica"] != DBNull.Value)
						objSolicitacaoR1._descricaoConhecimentoInformatica = Convert.ToString(dr["Des_Conhecimento_Informatica"]);
					if (dr["Vlr_Salario_De"] != DBNull.Value)
						objSolicitacaoR1._valorSalarioDe = Convert.ToDecimal(dr["Vlr_Salario_De"]);
					if (dr["Vlr_Salario_Ate"] != DBNull.Value)
						objSolicitacaoR1._valorSalarioAte = Convert.ToDecimal(dr["Vlr_Salario_Ate"]);
					if (dr["Idf_Sexo"] != DBNull.Value)
						objSolicitacaoR1._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
					if (dr["Des_Beneficio"] != DBNull.Value)
						objSolicitacaoR1._descricaoBeneficio = Convert.ToString(dr["Des_Beneficio"]);
					if (dr["Idf_Estado_Civil"] != DBNull.Value)
						objSolicitacaoR1._estadoCivil = new EstadoCivil(Convert.ToInt32(dr["Idf_Estado_Civil"]));
					objSolicitacaoR1._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					if (dr["Idf_Categoria_Habilitacao"] != DBNull.Value)
						objSolicitacaoR1._categoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(dr["Idf_Categoria_Habilitacao"]));
					if (dr["Des_Informacao_Adicional"] != DBNull.Value)
						objSolicitacaoR1._descricaoInformacaoAdicional = Convert.ToString(dr["Des_Informacao_Adicional"]);
                    if (dr["Idf_Consultor_R1"] != DBNull.Value)
                        objSolicitacaoR1._consultorR1 = new ConsultorR1(Convert.ToInt32(dr["Idf_Consultor_R1"]));
					if (dr["Idf_Simulacao_R1"] != DBNull.Value)
						objSolicitacaoR1._simulacaoR1 = new SimulacaoR1(Convert.ToInt32(dr["Idf_Simulacao_R1"]));
					objSolicitacaoR1._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Idf_Tipo_Vinculo"] != DBNull.Value)
						objSolicitacaoR1._tipoVinculo = new TipoVinculo(Convert.ToInt32(dr["Idf_Tipo_Vinculo"]));
					if (dr["Des_Bairro"] != DBNull.Value)
						objSolicitacaoR1._descricaoBairro = Convert.ToString(dr["Des_Bairro"]);

					objSolicitacaoR1._persisted = true;
					objSolicitacaoR1._modified = false;

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