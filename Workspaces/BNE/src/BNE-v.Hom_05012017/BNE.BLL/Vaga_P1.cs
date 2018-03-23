//-- Data: 27/05/2013 17:56
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using Microsoft.SqlServer.Types;
using BNE.BLL.DTO;

namespace BNE.BLL
{
    public partial class Vaga // Tabela: BNE_Vaga
    {
        #region Atributos
        private int _idVaga;
        private Funcao _funcao;
        private Cidade _cidade;
        private string _codigoVaga;
        private decimal? _valorSalarioDe;
        private DateTime? _dataAbertura;
        private DateTime? _dataPrazo;
        private string _emailVaga;
        private string _descricaoRequisito;
        private Int16? _quantidadeVaga;
        private string _nomeEmpresa;
        private bool _flagVagaRapida;
        private DateTime _dataCadastro;
        private bool _flagInativo;
        private Filial _filial;
        private bool _flagConfidencial;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private Escolaridade _escolaridade;
        private Int16? _numeroIdadeMinima;
        private Int16? _numeroIdadeMaxima;
        private Sexo _sexo;
        private string _descricaoBeneficio;
        private string _descricaoAtribuicoes;
        private string _numeroDDD;
        private string _numeroTelefone;
        private bool? _flagReceberCadaCV;
        private bool? _flagReceberTodosCV;
        private string _descricaoFuncao;
        private bool? _flagAuditada;
        private bool _flagBNERecomenda;
        private bool _flagVagaArquivada;
        private bool _flagVagaMassa;
        private Origem _origem;
        private bool? _flagLiberada;
        private Deficiencia _deficiencia;
        private DateTime? _dataAuditoria;
        private decimal? _valorSalarioPara;
        private bool? _flagDeficiencia;
        private bool _flagEmpresaEmAuditoria;
        private SqlGeography _descricaoLocalizacao;
        private Endereco _endereco;
        private string _descDisponibilidades;
        private string _descTiposVinculo;
        private MediaSalarial _mediaSalarial;
        private bool _persisted;
        private bool _modified;
        private int? _idBairro;
        private string _nomeBairro;
        private List<VagaDeficiencia> _vagaDeficiencia;
        private OrigemAnuncioVaga _origemAnuncioVaga;
        #endregion

        #region Propriedades

        #region IdVaga
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdVaga
        {
            get
            {
                return this._idVaga;
            }
        }
        #endregion

        #region Funcao
        /// <summary>
        /// Campo opcional.
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

        #region Cidade
        /// <summary>
        /// Campo obrigatÃ³rio.
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

        #region CodigoVaga
        /// <summary>
        /// Tamanho do campo: 10.
        /// Campo opcional.
        /// </summary>
        public string CodigoVaga
        {
            get
            {
                return this._codigoVaga;
            }
            set
            {
                this._codigoVaga = value;
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

        #region DataAbertura
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataAbertura
        {
            get
            {
                return this._dataAbertura;
            }
            set
            {
                this._dataAbertura = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataPrazo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataPrazo
        {
            get
            {
                return this._dataPrazo;
            }
            set
            {
                this._dataPrazo = value;
                this._modified = true;
            }
        }
        #endregion

        #region EmailVaga
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string EmailVaga
        {
            get
            {
                return this._emailVaga;
            }
            set
            {
                this._emailVaga = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoRequisito
        /// <summary>
        /// Tamanho do campo: 2000.
        /// Campo opcional.
        /// </summary>
        public string DescricaoRequisito
        {
            get
            {
                return this._descricaoRequisito;
            }
            set
            {
                this._descricaoRequisito = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeVaga
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Int16? QuantidadeVaga
        {
            get
            {
                return this._quantidadeVaga;
            }
            set
            {
                this._quantidadeVaga = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomeEmpresa
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string NomeEmpresa
        {
            get
            {
                return this._nomeEmpresa;
            }
            set
            {
                this._nomeEmpresa = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagVagaRapida
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagVagaRapida
        {
            get
            {
                return this._flagVagaRapida;
            }
            set
            {
                this._flagVagaRapida = value;
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

        #region FlagInativo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagInativo
        {
            get
            {
                return this._flagInativo;
            }
            set
            {
                this._flagInativo = value;
                this._modified = true;
            }
        }
        #endregion

        #region Filial
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Filial Filial
        {
            get
            {
                return this._filial;
            }
            set
            {
                this._filial = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagConfidencial
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagConfidencial
        {
            get
            {
                return this._flagConfidencial;
            }
            set
            {
                this._flagConfidencial = value;
                this._modified = true;
            }
        }
        #endregion

        #region UsuarioFilialPerfil
        /// <summary>
        /// Campo obrigatório.
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

        #region Escolaridade
        /// <summary>
        /// Campo opcional.
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

        #region DescricaoAtribuicoes
        /// <summary>
        /// Tamanho do campo: 2000.
        /// Campo opcional.
        /// </summary>
        public string DescricaoAtribuicoes
        {
            get
            {
                return this._descricaoAtribuicoes;
            }
            set
            {
                this._descricaoAtribuicoes = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroDDD
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string NumeroDDD
        {
            get
            {
                return this._numeroDDD;
            }
            set
            {
                this._numeroDDD = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroTelefone
        /// <summary>
        /// Tamanho do campo: 10.
        /// Campo opcional.
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

        #region FlagReceberCadaCV
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagReceberCadaCV
        {
            get
            {
                return this._flagReceberCadaCV;
            }
            set
            {
                this._flagReceberCadaCV = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagReceberTodosCV
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagReceberTodosCV
        {
            get
            {
                return this._flagReceberTodosCV;
            }
            set
            {
                this._flagReceberTodosCV = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoFuncao
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo opcional.
        /// </summary>
        public string DescricaoFuncao
        {
            get
            {
                return this._descricaoFuncao;
            }
            set
            {
                this._descricaoFuncao = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagAuditada
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagAuditada
        {
            get
            {
                return this._flagAuditada;
            }
            set
            {
                this._flagAuditada = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagBNERecomenda
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagBNERecomenda
        {
            get
            {
                return this._flagBNERecomenda;
            }
            set
            {
                this._flagBNERecomenda = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagVagaArquivada
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagVagaArquivada
        {
            get
            {
                return this._flagVagaArquivada;
            }
            set
            {
                this._flagVagaArquivada = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagVagaMassa
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagVagaMassa
        {
            get
            {
                return this._flagVagaMassa;
            }
            set
            {
                this._flagVagaMassa = value;
                this._modified = true;
            }
        }
        #endregion

        #region Origem
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Origem Origem
        {
            get
            {
                return this._origem;
            }
            set
            {
                this._origem = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagLiberada
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagLiberada
        {
            get
            {
                return this._flagLiberada;
            }
            set
            {
                this._flagLiberada = value;
                this._modified = true;
            }
        }
        #endregion

        #region Deficiencia
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Deficiencia Deficiencia
        {
            get
            {
                return this._deficiencia;
            }
            set
            {
                this._deficiencia = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataAuditoria
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataAuditoria
        {
            get
            {
                return this._dataAuditoria;
            }
            set
            {
                this._dataAuditoria = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorSalarioPara
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? ValorSalarioPara
        {
            get
            {
                return this._valorSalarioPara;
            }
            set
            {
                this._valorSalarioPara = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagDeficiencia
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagDeficiencia
        {
            get
            {
                return this._flagDeficiencia;
            }
            set
            {
                this._flagDeficiencia = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagEmpresaEmAuditoria
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagEmpresaEmAuditoria
        {
            get
            {
                return this._flagEmpresaEmAuditoria;
            }
            set
            {
                this._flagEmpresaEmAuditoria = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoDisponibilidades
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string DescricaoDisponibilidades
        {
            get
            {
                return this._descDisponibilidades;
            }
            set
            {
                this._descDisponibilidades = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoTiposVinculo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string DescricaoTiposVinculo
        {
            get
            {
                return this._descTiposVinculo;
            }
            set
            {
                this._descTiposVinculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region MediaSalarial
        public MediaSalarial MediaSalarial
        {
            get
            {
                return this._mediaSalarial;
            }
            set
            {
                this._mediaSalarial = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomeBairro
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string NomeBairro
        {
            get
            {
                return this._nomeBairro;
            }
            set
            {
                this._nomeBairro = value;
                this._modified = true;
            }
        }
        #endregion

        #region idBairro
        public int? IdBairro
        {
            get
            {
                return this._idBairro;
            }
            set
            {
                this._idBairro = value;
                this._modified = true;
            }
        }

        #endregion

        #region VagaDeficiencia
        public List<BLL.VagaDeficiencia> VagaDeficiencia
        {
            get
            {
                _vagaDeficiencia = BLL.VagaDeficiencia.listaDeficienciaVaga(this.IdVaga);
                return _vagaDeficiencia;
            }
            set
            {
                this._vagaDeficiencia = value;
                this._modified = true;
            }
        }
        #endregion

        #region OrigemAnuncioVaga
        public OrigemAnuncioVaga OrigemAnuncioVaga
        {
            get
            {
                return this._origemAnuncioVaga;
            }
            set
            {
                this._origemAnuncioVaga = value;
                this._modified = true;
            }
        }
        #endregion
        #endregion

        #region Construtores
        public Vaga()
        {
        }
        public Vaga(int idVaga)
        {
            this._idVaga = idVaga;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPUPDATE = "UPDATE BNE_Vaga SET Idf_Funcao = @Idf_Funcao, Idf_Cidade = @Idf_Cidade, Cod_Vaga = @Cod_Vaga, Vlr_Salario_De = @Vlr_Salario_De, Dta_Abertura = @Dta_Abertura, Dta_Prazo = @Dta_Prazo, Eml_Vaga = @Eml_Vaga, Des_Requisito = @Des_Requisito, Qtd_Vaga = @Qtd_Vaga, Nme_Empresa = @Nme_Empresa, Flg_Vaga_Rapida = @Flg_Vaga_Rapida, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Idf_Filial = @Idf_Filial, Flg_Confidencial = @Flg_Confidencial, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Escolaridade = @Idf_Escolaridade, Num_Idade_Minima = @Num_Idade_Minima, Num_Idade_Maxima = @Num_Idade_Maxima, Idf_Sexo = @Idf_Sexo, Des_Beneficio = @Des_Beneficio, Des_Atribuicoes = @Des_Atribuicoes, Num_DDD = @Num_DDD, Num_Telefone = @Num_Telefone, Flg_Receber_Cada_CV = @Flg_Receber_Cada_CV, Flg_Receber_Todos_CV = @Flg_Receber_Todos_CV, Des_Funcao = @Des_Funcao, Flg_Auditada = @Flg_Auditada, Flg_BNE_Recomenda = @Flg_BNE_Recomenda, Flg_Vaga_Arquivada = @Flg_Vaga_Arquivada, Flg_Vaga_Massa = @Flg_Vaga_Massa, Idf_Origem = @Idf_Origem, Flg_Liberada = @Flg_Liberada, Idf_Deficiencia = @Idf_Deficiencia, Dta_Auditoria = @Dta_Auditoria, Vlr_Salario_Para = @Vlr_Salario_Para, Flg_Deficiencia = @Flg_Deficiencia, Flg_Empresa_Em_Auditoria = @Flg_Empresa_Em_Auditoria,Nme_Bairro = @Nme_Bairro, Idf_Bairro = @Idf_Bairro WHERE Idf_Vaga = @Idf_Vaga";
        private const string SPDELETE = "DELETE FROM BNE_Vaga WHERE Idf_Vaga = @Idf_Vaga";
        private const string SPSELECTID = "SELECT * FROM BNE_Vaga WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga";
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
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Cod_Vaga", SqlDbType.VarChar, 10));
            parms.Add(new SqlParameter("@Vlr_Salario_De", SqlDbType.Money, 8));
            parms.Add(new SqlParameter("@Dta_Abertura", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Prazo", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Eml_Vaga", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Des_Requisito", SqlDbType.VarChar, 2000));
            parms.Add(new SqlParameter("@Qtd_Vaga", SqlDbType.Int, 2));
            parms.Add(new SqlParameter("@Nme_Empresa", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Flg_Vaga_Rapida", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Confidencial", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Idade_Minima", SqlDbType.Int, 2));
            parms.Add(new SqlParameter("@Num_Idade_Maxima", SqlDbType.Int, 2));
            parms.Add(new SqlParameter("@Idf_Sexo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Beneficio", SqlDbType.VarChar, 2000));
            parms.Add(new SqlParameter("@Des_Atribuicoes", SqlDbType.VarChar, 2000));
            parms.Add(new SqlParameter("@Num_DDD", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_Telefone", SqlDbType.VarChar, 10));
            parms.Add(new SqlParameter("@Flg_Receber_Cada_CV", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Receber_Todos_CV", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Des_Funcao", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Flg_Auditada", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_BNE_Recomenda", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Vaga_Arquivada", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Vaga_Massa", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Origem", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Liberada", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Auditoria", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Vlr_Salario_Para", SqlDbType.Money, 8));
            parms.Add(new SqlParameter("@Flg_Deficiencia", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Empresa_Em_Auditoria", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Nme_Bairro", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Idf_Bairro", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Origem_Anuncio_Vaga", SqlDbType.Int, 4));


            return (parms);
        }
        #endregion

        #region Save
        /// <summary>
        /// Método utilizado para salvar uma instância de Vaga no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>

        public void Save(int? idfUsuariofilialPerfil, Enumeradores.VagaLog? Processo)
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update(idfUsuariofilialPerfil, Processo);
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de Vaga no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public void Save(SqlTransaction trans, int? idfUsuariofilialPerfil, Enumeradores.VagaLog? Processo)
        {
            if (!this._persisted)
                this.Insert(trans);
            else
                this.Update(trans, idfUsuariofilialPerfil, Processo);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Método utilizado para excluir uma instância de Vaga no banco de dados.
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
        /// Método utilizado para excluir uma instância de Vaga no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma lista de Vaga no banco de dados.
        /// </summary>
        /// <param name="idVaga">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idVaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Vaga where Idf_Vaga in (";

            for (int i = 0; i < idVaga.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idVaga[i];
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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Vag.Idf_Vaga, Vag.Idf_Funcao, Vag.Cod_Vaga, Vag.Vlr_Salario_De, Vag.Dta_Abertura, Vag.Dta_Prazo, Vag.Eml_Vaga, Vag.Des_Requisito, Vag.Qtd_Vaga, Vag.Nme_Empresa, Vag.Flg_Vaga_Rapida, Vag.Dta_Cadastro, Vag.Flg_Inativo, Vag.Idf_Filial, Vag.Flg_Confidencial, Vag.Idf_Usuario_Filial_Perfil, Vag.Idf_Escolaridade, Vag.Num_Idade_Minima, Vag.Num_Idade_Maxima, Vag.Idf_Sexo, Vag.Des_Beneficio, Vag.Des_Atribuicoes, Vag.Num_DDD, Vag.Num_Telefone, Vag.Flg_Receber_Cada_CV, Vag.Flg_Receber_Todos_CV, Vag.Des_Funcao, Vag.Flg_Auditada, Vag.Flg_BNE_Recomenda, Vag.Flg_Vaga_Arquivada, Vag.Flg_Vaga_Massa, Vag.Idf_Origem, Vag.Flg_Liberada, Vag.Idf_Deficiencia, Vag.Dta_Auditoria, Vag.Vlr_Salario_Para, Vag.Des_Localizacao, Vag.Idf_Endereco, Vag.Idf_Cidade, Vag.Flg_Deficiencia, Vag.Nme_Bairro, Vag.Idf_Bairro FROM BNE_Vaga Vag";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Vaga a partir do banco de dados.
        /// </summary>
        /// <param name="idVaga">Chave do registro.</param>
        /// <returns>Instância de Vaga.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Vaga LoadObject(int idVaga)
        {
            using (IDataReader dr = LoadDataReader(idVaga))
            {
                Vaga objVaga = new Vaga();
                if (SetInstance(dr, objVaga))
                    return objVaga;
            }
            throw (new RecordNotFoundException(typeof(Vaga)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Vaga a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idVaga">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Vaga.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Vaga LoadObject(int idVaga, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idVaga, trans))
            {
                Vaga objVaga = new Vaga();
                if (SetInstance(dr, objVaga))
                    return objVaga;
            }
            throw (new RecordNotFoundException(typeof(Vaga)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Vaga a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idVaga))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Vaga a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idVaga, trans))
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
        /// <param name="objVaga">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, Vaga objVaga)
        {
            try
            {
                if (dr.Read())
                {
                    objVaga._idVaga = Convert.ToInt32(dr["Idf_Vaga"]);
                    if (dr["Idf_Funcao"] != DBNull.Value)
                        objVaga._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
                    if (dr["Idf_Cidade"] != DBNull.Value)
                        objVaga._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
                    if (dr["Cod_Vaga"] != DBNull.Value)
                        objVaga._codigoVaga = Convert.ToString(dr["Cod_Vaga"]);
                    if (dr["Vlr_Salario_De"] != DBNull.Value)
                        objVaga._valorSalarioDe = Convert.ToDecimal(dr["Vlr_Salario_De"]);
                    if (dr["Dta_Abertura"] != DBNull.Value)
                        objVaga._dataAbertura = Convert.ToDateTime(dr["Dta_Abertura"]);
                    if (dr["Dta_Prazo"] != DBNull.Value)
                        objVaga._dataPrazo = Convert.ToDateTime(dr["Dta_Prazo"]);
                    if (dr["Eml_Vaga"] != DBNull.Value)
                        objVaga._emailVaga = Convert.ToString(dr["Eml_Vaga"]);
                    if (dr["Des_Requisito"] != DBNull.Value)
                        objVaga._descricaoRequisito = Convert.ToString(dr["Des_Requisito"]);
                    if (dr["Qtd_Vaga"] != DBNull.Value)
                        objVaga._quantidadeVaga = Convert.ToInt16(dr["Qtd_Vaga"]);
                    if (dr["Nme_Empresa"] != DBNull.Value)
                        objVaga._nomeEmpresa = Convert.ToString(dr["Nme_Empresa"]);
                    objVaga._flagVagaRapida = Convert.ToBoolean(dr["Flg_Vaga_Rapida"]);
                    objVaga._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objVaga._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Idf_Filial"] != DBNull.Value)
                        objVaga._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                    objVaga._flagConfidencial = Convert.ToBoolean(dr["Flg_Confidencial"]);
                    objVaga._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    if (dr["Idf_Escolaridade"] != DBNull.Value)
                        objVaga._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
                    if (dr["Num_Idade_Minima"] != DBNull.Value)
                        objVaga._numeroIdadeMinima = Convert.ToInt16(dr["Num_Idade_Minima"]);
                    if (dr["Num_Idade_Maxima"] != DBNull.Value)
                        objVaga._numeroIdadeMaxima = Convert.ToInt16(dr["Num_Idade_Maxima"]);
                    if (dr["Idf_Sexo"] != DBNull.Value)
                        objVaga._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
                    if (dr["Des_Beneficio"] != DBNull.Value)
                        objVaga._descricaoBeneficio = Convert.ToString(dr["Des_Beneficio"]);
                    if (dr["Des_Atribuicoes"] != DBNull.Value)
                        objVaga._descricaoAtribuicoes = Convert.ToString(dr["Des_Atribuicoes"]);
                    if (dr["Num_DDD"] != DBNull.Value)
                        objVaga._numeroDDD = Convert.ToString(dr["Num_DDD"]);
                    if (dr["Num_Telefone"] != DBNull.Value)
                        objVaga._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
                    if (dr["Flg_Receber_Cada_CV"] != DBNull.Value)
                        objVaga._flagReceberCadaCV = Convert.ToBoolean(dr["Flg_Receber_Cada_CV"]);
                    if (dr["Flg_Receber_Todos_CV"] != DBNull.Value)
                        objVaga._flagReceberTodosCV = Convert.ToBoolean(dr["Flg_Receber_Todos_CV"]);
                    if (dr["Des_Funcao"] != DBNull.Value)
                        objVaga._descricaoFuncao = Convert.ToString(dr["Des_Funcao"]);
                    if (dr["Flg_Auditada"] != DBNull.Value)
                        objVaga._flagAuditada = Convert.ToBoolean(dr["Flg_Auditada"]);
                    objVaga._flagBNERecomenda = Convert.ToBoolean(dr["Flg_BNE_Recomenda"]);
                    objVaga._flagVagaArquivada = Convert.ToBoolean(dr["Flg_Vaga_Arquivada"]);
                    objVaga._flagVagaMassa = Convert.ToBoolean(dr["Flg_Vaga_Massa"]);
                    if (dr["Idf_Origem"] != DBNull.Value)
                        objVaga._origem = new Origem(Convert.ToInt32(dr["Idf_Origem"]));
                    if (dr["Flg_Liberada"] != DBNull.Value)
                        objVaga._flagLiberada = Convert.ToBoolean(dr["Flg_Liberada"]);
                    if (dr["Idf_Deficiencia"] != DBNull.Value)
                        objVaga._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
                    if (dr["Dta_Auditoria"] != DBNull.Value)
                        objVaga._dataAuditoria = Convert.ToDateTime(dr["Dta_Auditoria"]);
                    if (dr["Vlr_Salario_Para"] != DBNull.Value)
                        objVaga._valorSalarioPara = Convert.ToDecimal(dr["Vlr_Salario_Para"]);
                    if (dr["Flg_Deficiencia"] != DBNull.Value)
                        objVaga._flagDeficiencia = Convert.ToBoolean(dr["Flg_Deficiencia"]);
                    objVaga._flagEmpresaEmAuditoria = Convert.ToBoolean(dr["Flg_Empresa_Em_Auditoria"]);
                    if (dr["Nme_Bairro"] != DBNull.Value)
                        objVaga._nomeBairro = Convert.ToString(dr["Nme_Bairro"]);
                    if (dr["Idf_Bairro"] != DBNull.Value)
                        objVaga._idBairro = Convert.ToInt32(dr["Idf_Bairro"]);

                    objVaga._persisted = true;
                    objVaga._modified = false;

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
