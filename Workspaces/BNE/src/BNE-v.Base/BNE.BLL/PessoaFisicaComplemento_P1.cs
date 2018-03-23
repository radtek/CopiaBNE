//-- Data: 28/12/2010 15:29
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PessoaFisicaComplemento // Tabela: TAB_Pessoa_Fisica_Complemento
	{
		#region Atributos
		private PessoaFisica _pessoaFisica;
		private string _numeroHabilitacao;
		private CategoriaHabilitacao _categoriaHabilitacao;
		private DateTime? _dataValidadeHabilitacao;
		private string _numeroTituloEleitoral;
		private string _numeroZonaEleitoral;
		private string _numeroSecaoEleitoral;
		private string _numeroRegistroConselho;
		private int? _idConselho;
		private int? _idTipoConselho;
		private DateTime? _dataValidadeConselho;
		private string _descricaoPaisVisto;
		private string _numeroVisto;
		private DateTime? _dataValidadeVisto;
		private bool? _flagVeiculoProprio;
		private Int16? _anoVeiculo;
		private string _descricaoPlacaVeiculo;
		private string _numeroRenavam;
		private string _numeroDocReservista;
		private bool? _flagAposentado;
		private DateTime? _dataAposentadoria;
		private TipoSanguineo _tipoSanguineo;
		private bool? _flagDoador;
		private DateTime _dataCadastro;
		private DateTime _dataAlteracao;
		private bool _flagImportado;
		private bool? _flagSeguroVeiculo;
		private DateTime? _dataVencimentoSeguro;
		private int? _idMotivoAposentadoria;
		private int? _idCID;
		private string _numeroCEI;
		private decimal? _numeroAltura;
		private decimal? _numeroPeso;
		private TipoVeiculo _tipoVeiculo;
		private int? _numeroRegistroHabilitacao;
		private string _descricaoConhecimento;
		private string _descricaoComplementoDeficiencia;
		private bool? _flagFilhos;
		private bool? _flagViagem;
		private bool? _flagMudanca;
		private byte[] _arquivoAnexo;
		private string _nomeAnexo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region PessoaFisica
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public PessoaFisica PessoaFisica
		{
			get
			{
				return this._pessoaFisica;
			}
			set
			{
				this._pessoaFisica = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroHabilitacao
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
		/// </summary>
		public string NumeroHabilitacao
		{
			get
			{
				return this._numeroHabilitacao;
			}
			set
			{
				this._numeroHabilitacao = value;
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

		#region DataValidadeHabilitacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataValidadeHabilitacao
		{
			get
			{
				return this._dataValidadeHabilitacao;
			}
			set
			{
				this._dataValidadeHabilitacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroTituloEleitoral
		/// <summary>
		/// Tamanho do campo: 16.
		/// Campo opcional.
		/// </summary>
		public string NumeroTituloEleitoral
		{
			get
			{
				return this._numeroTituloEleitoral;
			}
			set
			{
				this._numeroTituloEleitoral = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroZonaEleitoral
		/// <summary>
		/// Tamanho do campo: 4.
		/// Campo opcional.
		/// </summary>
		public string NumeroZonaEleitoral
		{
			get
			{
				return this._numeroZonaEleitoral;
			}
			set
			{
				this._numeroZonaEleitoral = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroSecaoEleitoral
		/// <summary>
		/// Tamanho do campo: 4.
		/// Campo opcional.
		/// </summary>
		public string NumeroSecaoEleitoral
		{
			get
			{
				return this._numeroSecaoEleitoral;
			}
			set
			{
				this._numeroSecaoEleitoral = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroRegistroConselho
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo opcional.
		/// </summary>
		public string NumeroRegistroConselho
		{
			get
			{
				return this._numeroRegistroConselho;
			}
			set
			{
				this._numeroRegistroConselho = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdConselho
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? IdConselho
		{
			get
			{
				return this._idConselho;
			}
			set
			{
				this._idConselho = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdTipoConselho
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? IdTipoConselho
		{
			get
			{
				return this._idTipoConselho;
			}
			set
			{
				this._idTipoConselho = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataValidadeConselho
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataValidadeConselho
		{
			get
			{
				return this._dataValidadeConselho;
			}
			set
			{
				this._dataValidadeConselho = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPaisVisto
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo opcional.
		/// </summary>
		public string DescricaoPaisVisto
		{
			get
			{
				return this._descricaoPaisVisto;
			}
			set
			{
				this._descricaoPaisVisto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroVisto
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
		/// </summary>
		public string NumeroVisto
		{
			get
			{
				return this._numeroVisto;
			}
			set
			{
				this._numeroVisto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataValidadeVisto
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataValidadeVisto
		{
			get
			{
				return this._dataValidadeVisto;
			}
			set
			{
				this._dataValidadeVisto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagVeiculoProprio
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagVeiculoProprio
		{
			get
			{
				return this._flagVeiculoProprio;
			}
			set
			{
				this._flagVeiculoProprio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region AnoVeiculo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? AnoVeiculo
		{
			get
			{
				return this._anoVeiculo;
			}
			set
			{
				this._anoVeiculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPlacaVeiculo
		/// <summary>
		/// Tamanho do campo: 7.
		/// Campo opcional.
		/// </summary>
		public string DescricaoPlacaVeiculo
		{
			get
			{
				return this._descricaoPlacaVeiculo;
			}
			set
			{
				this._descricaoPlacaVeiculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroRenavam
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
		/// </summary>
		public string NumeroRenavam
		{
			get
			{
				return this._numeroRenavam;
			}
			set
			{
				this._numeroRenavam = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDocReservista
		/// <summary>
		/// Tamanho do campo: 12.
		/// Campo opcional.
		/// </summary>
		public string NumeroDocReservista
		{
			get
			{
				return this._numeroDocReservista;
			}
			set
			{
				this._numeroDocReservista = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagAposentado
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagAposentado
		{
			get
			{
				return this._flagAposentado;
			}
			set
			{
				this._flagAposentado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataAposentadoria
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataAposentadoria
		{
			get
			{
				return this._dataAposentadoria;
			}
			set
			{
				this._dataAposentadoria = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoSanguineo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public TipoSanguineo TipoSanguineo
		{
			get
			{
				return this._tipoSanguineo;
			}
			set
			{
				this._tipoSanguineo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagDoador
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagDoador
		{
			get
			{
				return this._flagDoador;
			}
			set
			{
				this._flagDoador = value;
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
		public DateTime DataAlteracao
		{
			get
			{
				return this._dataAlteracao;
			}
		}
		#endregion 

		#region FlagImportado
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagImportado
		{
			get
			{
				return this._flagImportado;
			}
			set
			{
				this._flagImportado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagSeguroVeiculo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagSeguroVeiculo
		{
			get
			{
				return this._flagSeguroVeiculo;
			}
			set
			{
				this._flagSeguroVeiculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataVencimentoSeguro
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataVencimentoSeguro
		{
			get
			{
				return this._dataVencimentoSeguro;
			}
			set
			{
				this._dataVencimentoSeguro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdMotivoAposentadoria
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? IdMotivoAposentadoria
		{
			get
			{
				return this._idMotivoAposentadoria;
			}
			set
			{
				this._idMotivoAposentadoria = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdCID
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? IdCID
		{
			get
			{
				return this._idCID;
			}
			set
			{
				this._idCID = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCEI
		/// <summary>
		/// Tamanho do campo: 12.
		/// Campo opcional.
		/// </summary>
		public string NumeroCEI
		{
			get
			{
				return this._numeroCEI;
			}
			set
			{
				this._numeroCEI = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroAltura
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroAltura
		{
			get
			{
				return this._numeroAltura;
			}
			set
			{
				this._numeroAltura = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroPeso
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroPeso
		{
			get
			{
				return this._numeroPeso;
			}
			set
			{
				this._numeroPeso = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoVeiculo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public TipoVeiculo TipoVeiculo
		{
			get
			{
				return this._tipoVeiculo;
			}
			set
			{
				this._tipoVeiculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroRegistroHabilitacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? NumeroRegistroHabilitacao
		{
			get
			{
				return this._numeroRegistroHabilitacao;
			}
			set
			{
				this._numeroRegistroHabilitacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoConhecimento
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoConhecimento
		{
			get
			{
				return this._descricaoConhecimento;
			}
			set
			{
				this._descricaoConhecimento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoComplementoDeficiencia
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string DescricaoComplementoDeficiencia
		{
			get
			{
				return this._descricaoComplementoDeficiencia;
			}
			set
			{
				this._descricaoComplementoDeficiencia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagFilhos
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagFilhos
		{
			get
			{
				return this._flagFilhos;
			}
			set
			{
				this._flagFilhos = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagViagem
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagViagem
		{
			get
			{
				return this._flagViagem;
			}
			set
			{
				this._flagViagem = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagMudanca
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagMudanca
		{
			get
			{
				return this._flagMudanca;
			}
			set
			{
				this._flagMudanca = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ArquivoAnexo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public byte[] ArquivoAnexo
		{
			get
			{
				return this._arquivoAnexo;
			}
			set
			{
				this._arquivoAnexo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeAnexo
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string NomeAnexo
		{
			get
			{
				return this._nomeAnexo;
			}
			set
			{
				this._nomeAnexo = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PessoaFisicaComplemento()
		{
		}
		public PessoaFisicaComplemento(PessoaFisica pessoaFisica)
		{
			this._pessoaFisica = pessoaFisica;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pessoa_Fisica_Complemento (Idf_Pessoa_Fisica, Num_Habilitacao, Idf_Categoria_Habilitacao, Dta_Validade_Habilitacao, Num_Titulo_Eleitoral, Num_Zona_Eleitoral, Num_Secao_Eleitoral, Num_Registro_Conselho, Idf_Conselho, Idf_Tipo_Conselho, Dta_Validade_Conselho, Des_Pais_Visto, Num_Visto, Dta_Validade_Visto, Flg_Veiculo_Proprio, Ano_Veiculo, Des_Placa_Veiculo, Num_Renavam, Num_Doc_Reservista, Flg_Aposentado, Dta_Aposentadoria, Idf_Tipo_Sanguineo, Flg_Doador, Dta_Cadastro, Dta_Alteracao, Flg_Importado, Flg_Seguro_Veiculo, Dta_Vencimento_Seguro, Idf_Motivo_Aposentadoria, Idf_CID, Num_CEI, Num_Altura, Num_Peso, Idf_Tipo_Veiculo, Num_Registro_Habilitacao, Des_Conhecimento, Des_Complemento_Deficiencia, Flg_Filhos, Flg_Viagem, Flg_Mudanca, Arq_Anexo, Nme_Anexo) VALUES (@Idf_Pessoa_Fisica, @Num_Habilitacao, @Idf_Categoria_Habilitacao, @Dta_Validade_Habilitacao, @Num_Titulo_Eleitoral, @Num_Zona_Eleitoral, @Num_Secao_Eleitoral, @Num_Registro_Conselho, @Idf_Conselho, @Idf_Tipo_Conselho, @Dta_Validade_Conselho, @Des_Pais_Visto, @Num_Visto, @Dta_Validade_Visto, @Flg_Veiculo_Proprio, @Ano_Veiculo, @Des_Placa_Veiculo, @Num_Renavam, @Num_Doc_Reservista, @Flg_Aposentado, @Dta_Aposentadoria, @Idf_Tipo_Sanguineo, @Flg_Doador, @Dta_Cadastro, @Dta_Alteracao, @Flg_Importado, @Flg_Seguro_Veiculo, @Dta_Vencimento_Seguro, @Idf_Motivo_Aposentadoria, @Idf_CID, @Num_CEI, @Num_Altura, @Num_Peso, @Idf_Tipo_Veiculo, @Num_Registro_Habilitacao, @Des_Conhecimento, @Des_Complemento_Deficiencia, @Flg_Filhos, @Flg_Viagem, @Flg_Mudanca, @Arq_Anexo, @Nme_Anexo);";
		private const string SPUPDATE = "UPDATE TAB_Pessoa_Fisica_Complemento SET Num_Habilitacao = @Num_Habilitacao, Idf_Categoria_Habilitacao = @Idf_Categoria_Habilitacao, Dta_Validade_Habilitacao = @Dta_Validade_Habilitacao, Num_Titulo_Eleitoral = @Num_Titulo_Eleitoral, Num_Zona_Eleitoral = @Num_Zona_Eleitoral, Num_Secao_Eleitoral = @Num_Secao_Eleitoral, Num_Registro_Conselho = @Num_Registro_Conselho, Idf_Conselho = @Idf_Conselho, Idf_Tipo_Conselho = @Idf_Tipo_Conselho, Dta_Validade_Conselho = @Dta_Validade_Conselho, Des_Pais_Visto = @Des_Pais_Visto, Num_Visto = @Num_Visto, Dta_Validade_Visto = @Dta_Validade_Visto, Flg_Veiculo_Proprio = @Flg_Veiculo_Proprio, Ano_Veiculo = @Ano_Veiculo, Des_Placa_Veiculo = @Des_Placa_Veiculo, Num_Renavam = @Num_Renavam, Num_Doc_Reservista = @Num_Doc_Reservista, Flg_Aposentado = @Flg_Aposentado, Dta_Aposentadoria = @Dta_Aposentadoria, Idf_Tipo_Sanguineo = @Idf_Tipo_Sanguineo, Flg_Doador = @Flg_Doador, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Flg_Importado = @Flg_Importado, Flg_Seguro_Veiculo = @Flg_Seguro_Veiculo, Dta_Vencimento_Seguro = @Dta_Vencimento_Seguro, Idf_Motivo_Aposentadoria = @Idf_Motivo_Aposentadoria, Idf_CID = @Idf_CID, Num_CEI = @Num_CEI, Num_Altura = @Num_Altura, Num_Peso = @Num_Peso, Idf_Tipo_Veiculo = @Idf_Tipo_Veiculo, Num_Registro_Habilitacao = @Num_Registro_Habilitacao, Des_Conhecimento = @Des_Conhecimento, Des_Complemento_Deficiencia = @Des_Complemento_Deficiencia, Flg_Filhos = @Flg_Filhos, Flg_Viagem = @Flg_Viagem, Flg_Mudanca = @Flg_Mudanca, Arq_Anexo = @Arq_Anexo, Nme_Anexo = @Nme_Anexo WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
		private const string SPDELETE = "DELETE FROM TAB_Pessoa_Fisica_Complemento WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
		private const string SPSELECTID = "SELECT * FROM TAB_Pessoa_Fisica_Complemento WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
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
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_Habilitacao", SqlDbType.VarChar, 15));
			parms.Add(new SqlParameter("@Idf_Categoria_Habilitacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Validade_Habilitacao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Num_Titulo_Eleitoral", SqlDbType.VarChar, 16));
			parms.Add(new SqlParameter("@Num_Zona_Eleitoral", SqlDbType.VarChar, 4));
			parms.Add(new SqlParameter("@Num_Secao_Eleitoral", SqlDbType.VarChar, 4));
			parms.Add(new SqlParameter("@Num_Registro_Conselho", SqlDbType.VarChar, 20));
			parms.Add(new SqlParameter("@Idf_Conselho", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Conselho", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Validade_Conselho", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Pais_Visto", SqlDbType.VarChar, 20));
			parms.Add(new SqlParameter("@Num_Visto", SqlDbType.VarChar, 15));
			parms.Add(new SqlParameter("@Dta_Validade_Visto", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Veiculo_Proprio", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Ano_Veiculo", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Des_Placa_Veiculo", SqlDbType.Char, 7));
			parms.Add(new SqlParameter("@Num_Renavam", SqlDbType.VarChar, 15));
			parms.Add(new SqlParameter("@Num_Doc_Reservista", SqlDbType.VarChar, 12));
			parms.Add(new SqlParameter("@Flg_Aposentado", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Aposentadoria", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Tipo_Sanguineo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Doador", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Importado", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Seguro_Veiculo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Vencimento_Seguro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Motivo_Aposentadoria", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_CID", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_CEI", SqlDbType.Char, 12));
			parms.Add(new SqlParameter("@Num_Altura", SqlDbType.Decimal, 5));
			parms.Add(new SqlParameter("@Num_Peso", SqlDbType.Decimal, 5));
			parms.Add(new SqlParameter("@Idf_Tipo_Veiculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_Registro_Habilitacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Conhecimento", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Des_Complemento_Deficiencia", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Flg_Filhos", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Viagem", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Mudanca", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Arq_Anexo", SqlDbType.VarBinary));
			parms.Add(new SqlParameter("@Nme_Anexo", SqlDbType.VarChar, 100));
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
			parms[0].Value = this._pessoaFisica.IdPessoaFisica;

			if (!String.IsNullOrEmpty(this._numeroHabilitacao))
				parms[1].Value = this._numeroHabilitacao;
			else
				parms[1].Value = DBNull.Value;


			if (this._categoriaHabilitacao != null)
				parms[2].Value = this._categoriaHabilitacao.IdCategoriaHabilitacao;
			else
				parms[2].Value = DBNull.Value;


			if (this._dataValidadeHabilitacao.HasValue)
				parms[3].Value = this._dataValidadeHabilitacao;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroTituloEleitoral))
				parms[4].Value = this._numeroTituloEleitoral;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroZonaEleitoral))
				parms[5].Value = this._numeroZonaEleitoral;
			else
				parms[5].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroSecaoEleitoral))
				parms[6].Value = this._numeroSecaoEleitoral;
			else
				parms[6].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroRegistroConselho))
				parms[7].Value = this._numeroRegistroConselho;
			else
				parms[7].Value = DBNull.Value;


			if (this._idConselho.HasValue)
				parms[8].Value = this._idConselho;
			else
				parms[8].Value = DBNull.Value;


			if (this._idTipoConselho.HasValue)
				parms[9].Value = this._idTipoConselho;
			else
				parms[9].Value = DBNull.Value;


			if (this._dataValidadeConselho.HasValue)
				parms[10].Value = this._dataValidadeConselho;
			else
				parms[10].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoPaisVisto))
				parms[11].Value = this._descricaoPaisVisto;
			else
				parms[11].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroVisto))
				parms[12].Value = this._numeroVisto;
			else
				parms[12].Value = DBNull.Value;


			if (this._dataValidadeVisto.HasValue)
				parms[13].Value = this._dataValidadeVisto;
			else
				parms[13].Value = DBNull.Value;


			if (this._flagVeiculoProprio.HasValue)
				parms[14].Value = this._flagVeiculoProprio;
			else
				parms[14].Value = DBNull.Value;


			if (this._anoVeiculo.HasValue)
				parms[15].Value = this._anoVeiculo;
			else
				parms[15].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoPlacaVeiculo))
				parms[16].Value = this._descricaoPlacaVeiculo;
			else
				parms[16].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroRenavam))
				parms[17].Value = this._numeroRenavam;
			else
				parms[17].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroDocReservista))
				parms[18].Value = this._numeroDocReservista;
			else
				parms[18].Value = DBNull.Value;


			if (this._flagAposentado.HasValue)
				parms[19].Value = this._flagAposentado;
			else
				parms[19].Value = DBNull.Value;


			if (this._dataAposentadoria.HasValue)
				parms[20].Value = this._dataAposentadoria;
			else
				parms[20].Value = DBNull.Value;


			if (this._tipoSanguineo != null)
				parms[21].Value = this._tipoSanguineo.IdTipoSanguineo;
			else
				parms[21].Value = DBNull.Value;


			if (this._flagDoador.HasValue)
				parms[22].Value = this._flagDoador;
			else
				parms[22].Value = DBNull.Value;

			parms[25].Value = this._flagImportado;

			if (this._flagSeguroVeiculo.HasValue)
				parms[26].Value = this._flagSeguroVeiculo;
			else
				parms[26].Value = DBNull.Value;


			if (this._dataVencimentoSeguro.HasValue)
				parms[27].Value = this._dataVencimentoSeguro;
			else
				parms[27].Value = DBNull.Value;


			if (this._idMotivoAposentadoria.HasValue)
				parms[28].Value = this._idMotivoAposentadoria;
			else
				parms[28].Value = DBNull.Value;


			if (this._idCID.HasValue)
				parms[29].Value = this._idCID;
			else
				parms[29].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCEI))
				parms[30].Value = this._numeroCEI;
			else
				parms[30].Value = DBNull.Value;


			if (this._numeroAltura.HasValue)
				parms[31].Value = this._numeroAltura;
			else
				parms[31].Value = DBNull.Value;


			if (this._numeroPeso.HasValue)
				parms[32].Value = this._numeroPeso;
			else
				parms[32].Value = DBNull.Value;


			if (this._tipoVeiculo != null)
				parms[33].Value = this._tipoVeiculo.IdTipoVeiculo;
			else
				parms[33].Value = DBNull.Value;


			if (this._numeroRegistroHabilitacao.HasValue)
				parms[34].Value = this._numeroRegistroHabilitacao;
			else
				parms[34].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoConhecimento))
				parms[35].Value = this._descricaoConhecimento;
			else
				parms[35].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoComplementoDeficiencia))
				parms[36].Value = this._descricaoComplementoDeficiencia;
			else
				parms[36].Value = DBNull.Value;


			if (this._flagFilhos.HasValue)
				parms[37].Value = this._flagFilhos;
			else
				parms[37].Value = DBNull.Value;


			if (this._flagViagem.HasValue)
				parms[38].Value = this._flagViagem;
			else
				parms[38].Value = DBNull.Value;


			if (this._flagMudanca.HasValue)
				parms[39].Value = this._flagMudanca;
			else
				parms[39].Value = DBNull.Value;


			if (this._arquivoAnexo != null)
				parms[40].Value = this._arquivoAnexo;
			else
				parms[40].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._nomeAnexo))
				parms[41].Value = this._nomeAnexo;
			else
				parms[41].Value = DBNull.Value;


			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[23].Value = this._dataCadastro;
			this._dataAlteracao = DateTime.Now;
			parms[24].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PessoaFisicaComplemento no banco de dados.
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
		/// Método utilizado para inserir uma instância de PessoaFisicaComplemento no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de PessoaFisicaComplemento no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PessoaFisicaComplemento no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PessoaFisicaComplemento no banco de dados.
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
		/// Método utilizado para salvar uma instância de PessoaFisicaComplemento no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PessoaFisicaComplemento no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisica">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPessoaFisica)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisica;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PessoaFisicaComplemento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisica">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPessoaFisica, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisica;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PessoaFisicaComplemento no banco de dados.
		/// </summary>
		/// <param name="pessoaFisica">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<PessoaFisica> pessoaFisica)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pessoa_Fisica_Complemento where Idf_Pessoa_Fisica in (";

			for (int i = 0; i < pessoaFisica.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = pessoaFisica[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPessoaFisica">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPessoaFisica)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisica;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisica">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPessoaFisica, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));

			parms[0].Value = idPessoaFisica;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pessoa_Fisica, Pes.Num_Habilitacao, Pes.Idf_Categoria_Habilitacao, Pes.Dta_Validade_Habilitacao, Pes.Num_Titulo_Eleitoral, Pes.Num_Zona_Eleitoral, Pes.Num_Secao_Eleitoral, Pes.Num_Registro_Conselho, Pes.Idf_Conselho, Pes.Idf_Tipo_Conselho, Pes.Dta_Validade_Conselho, Pes.Des_Pais_Visto, Pes.Num_Visto, Pes.Dta_Validade_Visto, Pes.Flg_Veiculo_Proprio, Pes.Ano_Veiculo, Pes.Des_Placa_Veiculo, Pes.Num_Renavam, Pes.Num_Doc_Reservista, Pes.Flg_Aposentado, Pes.Dta_Aposentadoria, Pes.Idf_Tipo_Sanguineo, Pes.Flg_Doador, Pes.Dta_Cadastro, Pes.Dta_Alteracao, Pes.Flg_Importado, Pes.Flg_Seguro_Veiculo, Pes.Dta_Vencimento_Seguro, Pes.Idf_Motivo_Aposentadoria, Pes.Idf_CID, Pes.Num_CEI, Pes.Num_Altura, Pes.Num_Peso, Pes.Idf_Tipo_Veiculo, Pes.Num_Registro_Habilitacao, Pes.Des_Conhecimento, Pes.Des_Complemento_Deficiencia, Pes.Flg_Filhos, Pes.Flg_Viagem, Pes.Flg_Mudanca, Pes.Arq_Anexo, Pes.Nme_Anexo FROM TAB_Pessoa_Fisica_Complemento Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoaFisicaComplemento a partir do banco de dados.
		/// </summary>
		/// <param name="idPessoaFisica">Chave do registro.</param>
		/// <returns>Instância de PessoaFisicaComplemento.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoaFisicaComplemento LoadObject(int idPessoaFisica)
		{
			using (IDataReader dr = LoadDataReader(idPessoaFisica))
			{
				PessoaFisicaComplemento objPessoaFisicaComplemento = new PessoaFisicaComplemento();
				if (SetInstance(dr, objPessoaFisicaComplemento))
					return objPessoaFisicaComplemento;
			}
			throw (new RecordNotFoundException(typeof(PessoaFisicaComplemento)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PessoaFisicaComplemento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPessoaFisica">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PessoaFisicaComplemento.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PessoaFisicaComplemento LoadObject(int idPessoaFisica, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPessoaFisica, trans))
			{
				PessoaFisicaComplemento objPessoaFisicaComplemento = new PessoaFisicaComplemento();
				if (SetInstance(dr, objPessoaFisicaComplemento))
					return objPessoaFisicaComplemento;
			}
			throw (new RecordNotFoundException(typeof(PessoaFisicaComplemento)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PessoaFisicaComplemento a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._pessoaFisica.IdPessoaFisica))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PessoaFisicaComplemento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._pessoaFisica.IdPessoaFisica, trans))
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
		/// <param name="objPessoaFisicaComplemento">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PessoaFisicaComplemento objPessoaFisicaComplemento)
		{
			try
			{
				if (dr.Read())
				{
					objPessoaFisicaComplemento._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
					if (dr["Num_Habilitacao"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroHabilitacao = Convert.ToString(dr["Num_Habilitacao"]);
					if (dr["Idf_Categoria_Habilitacao"] != DBNull.Value)
						objPessoaFisicaComplemento._categoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(dr["Idf_Categoria_Habilitacao"]));
					if (dr["Dta_Validade_Habilitacao"] != DBNull.Value)
						objPessoaFisicaComplemento._dataValidadeHabilitacao = Convert.ToDateTime(dr["Dta_Validade_Habilitacao"]);
					if (dr["Num_Titulo_Eleitoral"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroTituloEleitoral = Convert.ToString(dr["Num_Titulo_Eleitoral"]);
					if (dr["Num_Zona_Eleitoral"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroZonaEleitoral = Convert.ToString(dr["Num_Zona_Eleitoral"]);
					if (dr["Num_Secao_Eleitoral"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroSecaoEleitoral = Convert.ToString(dr["Num_Secao_Eleitoral"]);
					if (dr["Num_Registro_Conselho"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroRegistroConselho = Convert.ToString(dr["Num_Registro_Conselho"]);
					if (dr["Idf_Conselho"] != DBNull.Value)
						objPessoaFisicaComplemento._idConselho = Convert.ToInt32(dr["Idf_Conselho"]);
					if (dr["Idf_Tipo_Conselho"] != DBNull.Value)
						objPessoaFisicaComplemento._idTipoConselho = Convert.ToInt32(dr["Idf_Tipo_Conselho"]);
					if (dr["Dta_Validade_Conselho"] != DBNull.Value)
						objPessoaFisicaComplemento._dataValidadeConselho = Convert.ToDateTime(dr["Dta_Validade_Conselho"]);
					if (dr["Des_Pais_Visto"] != DBNull.Value)
						objPessoaFisicaComplemento._descricaoPaisVisto = Convert.ToString(dr["Des_Pais_Visto"]);
					if (dr["Num_Visto"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroVisto = Convert.ToString(dr["Num_Visto"]);
					if (dr["Dta_Validade_Visto"] != DBNull.Value)
						objPessoaFisicaComplemento._dataValidadeVisto = Convert.ToDateTime(dr["Dta_Validade_Visto"]);
					if (dr["Flg_Veiculo_Proprio"] != DBNull.Value)
						objPessoaFisicaComplemento._flagVeiculoProprio = Convert.ToBoolean(dr["Flg_Veiculo_Proprio"]);
					if (dr["Ano_Veiculo"] != DBNull.Value)
						objPessoaFisicaComplemento._anoVeiculo = Convert.ToInt16(dr["Ano_Veiculo"]);
					if (dr["Des_Placa_Veiculo"] != DBNull.Value)
						objPessoaFisicaComplemento._descricaoPlacaVeiculo = Convert.ToString(dr["Des_Placa_Veiculo"]);
					if (dr["Num_Renavam"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroRenavam = Convert.ToString(dr["Num_Renavam"]);
					if (dr["Num_Doc_Reservista"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroDocReservista = Convert.ToString(dr["Num_Doc_Reservista"]);
					if (dr["Flg_Aposentado"] != DBNull.Value)
						objPessoaFisicaComplemento._flagAposentado = Convert.ToBoolean(dr["Flg_Aposentado"]);
					if (dr["Dta_Aposentadoria"] != DBNull.Value)
						objPessoaFisicaComplemento._dataAposentadoria = Convert.ToDateTime(dr["Dta_Aposentadoria"]);
					if (dr["Idf_Tipo_Sanguineo"] != DBNull.Value)
						objPessoaFisicaComplemento._tipoSanguineo = new TipoSanguineo(Convert.ToInt32(dr["Idf_Tipo_Sanguineo"]));
					if (dr["Flg_Doador"] != DBNull.Value)
						objPessoaFisicaComplemento._flagDoador = Convert.ToBoolean(dr["Flg_Doador"]);
					objPessoaFisicaComplemento._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objPessoaFisicaComplemento._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objPessoaFisicaComplemento._flagImportado = Convert.ToBoolean(dr["Flg_Importado"]);
					if (dr["Flg_Seguro_Veiculo"] != DBNull.Value)
						objPessoaFisicaComplemento._flagSeguroVeiculo = Convert.ToBoolean(dr["Flg_Seguro_Veiculo"]);
					if (dr["Dta_Vencimento_Seguro"] != DBNull.Value)
						objPessoaFisicaComplemento._dataVencimentoSeguro = Convert.ToDateTime(dr["Dta_Vencimento_Seguro"]);
					if (dr["Idf_Motivo_Aposentadoria"] != DBNull.Value)
						objPessoaFisicaComplemento._idMotivoAposentadoria = Convert.ToInt32(dr["Idf_Motivo_Aposentadoria"]);
					if (dr["Idf_CID"] != DBNull.Value)
						objPessoaFisicaComplemento._idCID = Convert.ToInt32(dr["Idf_CID"]);
					if (dr["Num_CEI"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroCEI = Convert.ToString(dr["Num_CEI"]);
					if (dr["Num_Altura"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroAltura = Convert.ToDecimal(dr["Num_Altura"]);
					if (dr["Num_Peso"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroPeso = Convert.ToDecimal(dr["Num_Peso"]);
					if (dr["Idf_Tipo_Veiculo"] != DBNull.Value)
						objPessoaFisicaComplemento._tipoVeiculo = new TipoVeiculo(Convert.ToInt32(dr["Idf_Tipo_Veiculo"]));
					if (dr["Num_Registro_Habilitacao"] != DBNull.Value)
						objPessoaFisicaComplemento._numeroRegistroHabilitacao = Convert.ToInt32(dr["Num_Registro_Habilitacao"]);
					if (dr["Des_Conhecimento"] != DBNull.Value)
						objPessoaFisicaComplemento._descricaoConhecimento = Convert.ToString(dr["Des_Conhecimento"]);
					if (dr["Des_Complemento_Deficiencia"] != DBNull.Value)
						objPessoaFisicaComplemento._descricaoComplementoDeficiencia = Convert.ToString(dr["Des_Complemento_Deficiencia"]);
					if (dr["Flg_Filhos"] != DBNull.Value)
						objPessoaFisicaComplemento._flagFilhos = Convert.ToBoolean(dr["Flg_Filhos"]);
					if (dr["Flg_Viagem"] != DBNull.Value)
						objPessoaFisicaComplemento._flagViagem = Convert.ToBoolean(dr["Flg_Viagem"]);
					if (dr["Flg_Mudanca"] != DBNull.Value)
						objPessoaFisicaComplemento._flagMudanca = Convert.ToBoolean(dr["Flg_Mudanca"]);
					if (dr["Arq_Anexo"] != DBNull.Value)
						objPessoaFisicaComplemento._arquivoAnexo = (byte[])(dr["Arq_Anexo"]);
					if (dr["Nme_Anexo"] != DBNull.Value)
						objPessoaFisicaComplemento._nomeAnexo = Convert.ToString(dr["Nme_Anexo"]);

					objPessoaFisicaComplemento._persisted = true;
					objPessoaFisicaComplemento._modified = false;

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