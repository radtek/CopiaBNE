//-- Data: 20/02/2013 16:26
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class ParametroBuscaCV // Tabela: BNE_Parametro_Busca_CV
    {
        #region Atributos
        private int _idParametroBuscaCV;
        private bool _flagIdfCurriculo;
        private bool _flagNumCPF;
        private bool _flagEmlPessoa;
        private bool _flagIdfFuncao;
        private bool _flagIdfCidade;
        private bool _flagSigEstado;
        private bool _flagPesoEscolaridade;
        private bool _flagIdfSexo;
        private bool _flagIdadeMin;
        private bool _flagIdadeMax;
        private bool _flagSalarioMin;
        private bool _flagSalarioMax;
        private bool _flagMesesExp;
        private bool _flagNmePessoa;
        private bool _flagDesBairro;
        private bool _flagDesLogradouro;
        private bool _flagNumCEPMin;
        private bool _flagNumCEPMax;
        private bool _flagExperienciaEm;
        private bool _flagDesCurso;
        private bool _flagNmeFonte;
        private bool _flagDesCursoOutros;
        private bool _flagNmeFonteOutros;
        private bool _flagNmeEmpresa;
        private bool _flagIdfAreaBNE;
        private bool _flagIdfCategoriaHabilitacao;
        private bool _flagIdfTipoVeiculo;
        private bool _flagNumDDD;
        private bool _flagNumTelefone;
        private bool _flagIdfDeficiencia;
        private bool _flagDesMetaBusca;
        private bool _flagDesMetabuscaRapida;
        private bool _flagIdfOrigem;
        private bool _flagIdfEstadoCivil;
        private bool _flagIdfFilial;
        private bool _flagIdfsIdioma;
        private bool _flagIdfsDisponibilidade;
        private bool _flagIdfRaca;
        private bool _flagFlgFilhos;
        private bool _flagIdfVaga;
        private bool _flagIdfRastreador;
        private bool _flagInativo;
        private string _nomeSPBusca;
        private bool _flagAprendiz;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdParametroBuscaCV
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdParametroBuscaCV
        {
            get
            {
                return this._idParametroBuscaCV;
            }
        }
        #endregion

        #region FlagIdfCurriculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfCurriculo
        {
            get
            {
                return this._flagIdfCurriculo;
            }
            set
            {
                this._flagIdfCurriculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagNumCPF
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagNumCPF
        {
            get
            {
                return this._flagNumCPF;
            }
            set
            {
                this._flagNumCPF = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagEmlPessoa
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagEmlPessoa
        {
            get
            {
                return this._flagEmlPessoa;
            }
            set
            {
                this._flagEmlPessoa = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfFuncao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfFuncao
        {
            get
            {
                return this._flagIdfFuncao;
            }
            set
            {
                this._flagIdfFuncao = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfCidade
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfCidade
        {
            get
            {
                return this._flagIdfCidade;
            }
            set
            {
                this._flagIdfCidade = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagSigEstado
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagSigEstado
        {
            get
            {
                return this._flagSigEstado;
            }
            set
            {
                this._flagSigEstado = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagPesoEscolaridade
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagPesoEscolaridade
        {
            get
            {
                return this._flagPesoEscolaridade;
            }
            set
            {
                this._flagPesoEscolaridade = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfSexo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfSexo
        {
            get
            {
                return this._flagIdfSexo;
            }
            set
            {
                this._flagIdfSexo = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdadeMin
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdadeMin
        {
            get
            {
                return this._flagIdadeMin;
            }
            set
            {
                this._flagIdadeMin = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdadeMax
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdadeMax
        {
            get
            {
                return this._flagIdadeMax;
            }
            set
            {
                this._flagIdadeMax = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagSalarioMin
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagSalarioMin
        {
            get
            {
                return this._flagSalarioMin;
            }
            set
            {
                this._flagSalarioMin = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagSalarioMax
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagSalarioMax
        {
            get
            {
                return this._flagSalarioMax;
            }
            set
            {
                this._flagSalarioMax = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagMesesExp
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagMesesExp
        {
            get
            {
                return this._flagMesesExp;
            }
            set
            {
                this._flagMesesExp = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagNmePessoa
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagNmePessoa
        {
            get
            {
                return this._flagNmePessoa;
            }
            set
            {
                this._flagNmePessoa = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagDesBairro
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagDesBairro
        {
            get
            {
                return this._flagDesBairro;
            }
            set
            {
                this._flagDesBairro = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagDesLogradouro
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagDesLogradouro
        {
            get
            {
                return this._flagDesLogradouro;
            }
            set
            {
                this._flagDesLogradouro = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagNumCEPMin
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagNumCEPMin
        {
            get
            {
                return this._flagNumCEPMin;
            }
            set
            {
                this._flagNumCEPMin = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagNumCEPMax
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagNumCEPMax
        {
            get
            {
                return this._flagNumCEPMax;
            }
            set
            {
                this._flagNumCEPMax = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagExperienciaEm
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagExperienciaEm
        {
            get
            {
                return this._flagExperienciaEm;
            }
            set
            {
                this._flagExperienciaEm = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagDesCurso
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagDesCurso
        {
            get
            {
                return this._flagDesCurso;
            }
            set
            {
                this._flagDesCurso = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagNmeFonte
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagNmeFonte
        {
            get
            {
                return this._flagNmeFonte;
            }
            set
            {
                this._flagNmeFonte = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagDesCursoOutros
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagDesCursoOutros
        {
            get
            {
                return this._flagDesCursoOutros;
            }
            set
            {
                this._flagDesCursoOutros = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagNmeFonteOutros
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagNmeFonteOutros
        {
            get
            {
                return this._flagNmeFonteOutros;
            }
            set
            {
                this._flagNmeFonteOutros = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagNmeEmpresa
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagNmeEmpresa
        {
            get
            {
                return this._flagNmeEmpresa;
            }
            set
            {
                this._flagNmeEmpresa = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfAreaBNE
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfAreaBNE
        {
            get
            {
                return this._flagIdfAreaBNE;
            }
            set
            {
                this._flagIdfAreaBNE = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfCategoriaHabilitacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfCategoriaHabilitacao
        {
            get
            {
                return this._flagIdfCategoriaHabilitacao;
            }
            set
            {
                this._flagIdfCategoriaHabilitacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfTipoVeiculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfTipoVeiculo
        {
            get
            {
                return this._flagIdfTipoVeiculo;
            }
            set
            {
                this._flagIdfTipoVeiculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagNumDDD
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagNumDDD
        {
            get
            {
                return this._flagNumDDD;
            }
            set
            {
                this._flagNumDDD = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagNumTelefone
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagNumTelefone
        {
            get
            {
                return this._flagNumTelefone;
            }
            set
            {
                this._flagNumTelefone = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfDeficiencia
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfDeficiencia
        {
            get
            {
                return this._flagIdfDeficiencia;
            }
            set
            {
                this._flagIdfDeficiencia = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagDesMetaBusca
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagDesMetaBusca
        {
            get
            {
                return this._flagDesMetaBusca;
            }
            set
            {
                this._flagDesMetaBusca = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagDesMetabuscaRapida
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagDesMetabuscaRapida
        {
            get
            {
                return this._flagDesMetabuscaRapida;
            }
            set
            {
                this._flagDesMetabuscaRapida = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfOrigem
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfOrigem
        {
            get
            {
                return this._flagIdfOrigem;
            }
            set
            {
                this._flagIdfOrigem = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfEstadoCivil
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfEstadoCivil
        {
            get
            {
                return this._flagIdfEstadoCivil;
            }
            set
            {
                this._flagIdfEstadoCivil = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfFilial
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfFilial
        {
            get
            {
                return this._flagIdfFilial;
            }
            set
            {
                this._flagIdfFilial = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfsIdioma
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfsIdioma
        {
            get
            {
                return this._flagIdfsIdioma;
            }
            set
            {
                this._flagIdfsIdioma = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfsDisponibilidade
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfsDisponibilidade
        {
            get
            {
                return this._flagIdfsDisponibilidade;
            }
            set
            {
                this._flagIdfsDisponibilidade = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfRaca
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfRaca
        {
            get
            {
                return this._flagIdfRaca;
            }
            set
            {
                this._flagIdfRaca = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagFlgFilhos
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagFlgFilhos
        {
            get
            {
                return this._flagFlgFilhos;
            }
            set
            {
                this._flagFlgFilhos = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfVaga
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfVaga
        {
            get
            {
                return this._flagIdfVaga;
            }
            set
            {
                this._flagIdfVaga = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIdfRastreador
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIdfRastreador
        {
            get
            {
                return this._flagIdfRastreador;
            }
            set
            {
                this._flagIdfRastreador = value;
                this._modified = true;
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

        #region NomeSPBusca
        /// <summary>
        /// Tamanho do campo: 200.
        /// Campo opcional.
        /// </summary>
        public string NomeSPBusca
        {
            get
            {
                return this._nomeSPBusca;
            }
            set
            {
                this._nomeSPBusca = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagAprendiz
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagAprendiz
        {
            get
            {
                return this._flagFlgFilhos;
            }
            set
            {
                this._flagFlgFilhos = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public ParametroBuscaCV()
        {
        }
        public ParametroBuscaCV(int idParametroBuscaCV)
        {
            this._idParametroBuscaCV = idParametroBuscaCV;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Parametro_Busca_CV (Flg_Idf_Curriculo, Flg_Num_CPF, Flg_Eml_Pessoa, Flg_Idf_Funcao, Flg_Idf_Cidade, Flg_Sig_Estado, Flg_Peso_Escolaridade, Flg_Idf_Sexo, Flg_Idade_Min, Flg_Idade_Max, Flg_Salario_Min, Flg_Salario_Max, Flg_Meses_Exp, Flg_Nme_Pessoa, Flg_Des_Bairro, Flg_Des_Logradouro, Flg_Num_CEP_Min, Flg_Num_CEP_Max, Flg_Experiencia_Em, Flg_Des_Curso, Flg_Nme_Fonte, Flg_Des_Curso_Outros, Flg_Nme_Fonte_Outros, Flg_Nme_Empresa, Flg_Idf_Area_BNE, Flg_Idf_Categoria_Habilitacao, Flg_Idf_Tipo_Veiculo, Flg_Num_DDD, Flg_Num_Telefone, Flg_Idf_Deficiencia, Flg_Des_MetaBusca, Flg_Des_Metabusca_Rapida, Flg_Idf_Origem, Flg_Idf_Estado_Civil, Flg_Idf_Filial, Flg_Idfs_Idioma, Flg_Idfs_Disponibilidade, Flg_Idf_Raca, Flg_Flg_Filhos, Flg_Idf_Vaga, Flg_Idf_Rastreador, Flg_Inativo, Nme_SP_Busca, Flg_Aprendiz) VALUES (@Flg_Idf_Curriculo, @Flg_Num_CPF, @Flg_Eml_Pessoa, @Flg_Idf_Funcao, @Flg_Idf_Cidade, @Flg_Sig_Estado, @Flg_Peso_Escolaridade, @Flg_Idf_Sexo, @Flg_Idade_Min, @Flg_Idade_Max, @Flg_Salario_Min, @Flg_Salario_Max, @Flg_Meses_Exp, @Flg_Nme_Pessoa, @Flg_Des_Bairro, @Flg_Des_Logradouro, @Flg_Num_CEP_Min, @Flg_Num_CEP_Max, @Flg_Experiencia_Em, @Flg_Des_Curso, @Flg_Nme_Fonte, @Flg_Des_Curso_Outros, @Flg_Nme_Fonte_Outros, @Flg_Nme_Empresa, @Flg_Idf_Area_BNE, @Flg_Idf_Categoria_Habilitacao, @Flg_Idf_Tipo_Veiculo, @Flg_Num_DDD, @Flg_Num_Telefone, @Flg_Idf_Deficiencia, @Flg_Des_MetaBusca, @Flg_Des_Metabusca_Rapida, @Flg_Idf_Origem, @Flg_Idf_Estado_Civil, @Flg_Idf_Filial, @Flg_Idfs_Idioma, @Flg_Idfs_Disponibilidade, @Flg_Idf_Raca, @Flg_Flg_Filhos, @Flg_Idf_Vaga, @Flg_Idf_Rastreador, @Flg_Inativo, @Nme_SP_Busca, @Flg_Aprendiz);SET @Idf_Parametro_Busca_CV = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Parametro_Busca_CV SET Flg_Idf_Curriculo = @Flg_Idf_Curriculo, Flg_Num_CPF = @Flg_Num_CPF, Flg_Eml_Pessoa = @Flg_Eml_Pessoa, Flg_Idf_Funcao = @Flg_Idf_Funcao, Flg_Idf_Cidade = @Flg_Idf_Cidade, Flg_Sig_Estado = @Flg_Sig_Estado, Flg_Peso_Escolaridade = @Flg_Peso_Escolaridade, Flg_Idf_Sexo = @Flg_Idf_Sexo, Flg_Idade_Min = @Flg_Idade_Min, Flg_Idade_Max = @Flg_Idade_Max, Flg_Salario_Min = @Flg_Salario_Min, Flg_Salario_Max = @Flg_Salario_Max, Flg_Meses_Exp = @Flg_Meses_Exp, Flg_Nme_Pessoa = @Flg_Nme_Pessoa, Flg_Des_Bairro = @Flg_Des_Bairro, Flg_Des_Logradouro = @Flg_Des_Logradouro, Flg_Num_CEP_Min = @Flg_Num_CEP_Min, Flg_Num_CEP_Max = @Flg_Num_CEP_Max, Flg_Experiencia_Em = @Flg_Experiencia_Em, Flg_Des_Curso = @Flg_Des_Curso, Flg_Nme_Fonte = @Flg_Nme_Fonte, Flg_Des_Curso_Outros = @Flg_Des_Curso_Outros, Flg_Nme_Fonte_Outros = @Flg_Nme_Fonte_Outros, Flg_Nme_Empresa = @Flg_Nme_Empresa, Flg_Idf_Area_BNE = @Flg_Idf_Area_BNE, Flg_Idf_Categoria_Habilitacao = @Flg_Idf_Categoria_Habilitacao, Flg_Idf_Tipo_Veiculo = @Flg_Idf_Tipo_Veiculo, Flg_Num_DDD = @Flg_Num_DDD, Flg_Num_Telefone = @Flg_Num_Telefone, Flg_Idf_Deficiencia = @Flg_Idf_Deficiencia, Flg_Des_MetaBusca = @Flg_Des_MetaBusca, Flg_Des_Metabusca_Rapida = @Flg_Des_Metabusca_Rapida, Flg_Idf_Origem = @Flg_Idf_Origem, Flg_Idf_Estado_Civil = @Flg_Idf_Estado_Civil, Flg_Idf_Filial = @Flg_Idf_Filial, Flg_Idfs_Idioma = @Flg_Idfs_Idioma, Flg_Idfs_Disponibilidade = @Flg_Idfs_Disponibilidade, Flg_Idf_Raca = @Flg_Idf_Raca, Flg_Flg_Filhos = @Flg_Flg_Filhos, Flg_Idf_Vaga = @Flg_Idf_Vaga, Flg_Idf_Rastreador = @Flg_Idf_Rastreador, Flg_Inativo = @Flg_Inativo, Nme_SP_Busca = @Nme_SP_Busca, Flg_Aprendiz = @Flg_Aprendiz WHERE Idf_Parametro_Busca_CV = @Idf_Parametro_Busca_CV";
        private const string SPDELETE = "DELETE FROM BNE_Parametro_Busca_CV WHERE Idf_Parametro_Busca_CV = @Idf_Parametro_Busca_CV";
        private const string SPSELECTID = "SELECT * FROM BNE_Parametro_Busca_CV WITH(NOLOCK) WHERE Idf_Parametro_Busca_CV = @Idf_Parametro_Busca_CV";
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
            parms.Add(new SqlParameter("@Idf_Parametro_Busca_CV", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Idf_Curriculo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Num_CPF", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Eml_Pessoa", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Funcao", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Cidade", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Sig_Estado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Peso_Escolaridade", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Sexo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idade_Min", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idade_Max", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Salario_Min", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Salario_Max", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Meses_Exp", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Nme_Pessoa", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Des_Bairro", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Des_Logradouro", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Num_CEP_Min", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Num_CEP_Max", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Experiencia_Em", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Des_Curso", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Nme_Fonte", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Des_Curso_Outros", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Nme_Fonte_Outros", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Nme_Empresa", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Area_BNE", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Categoria_Habilitacao", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Tipo_Veiculo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Num_DDD", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Num_Telefone", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Deficiencia", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Des_MetaBusca", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Des_Metabusca_Rapida", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Origem", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Estado_Civil", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Filial", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idfs_Idioma", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idfs_Disponibilidade", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Raca", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Flg_Filhos", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Vaga", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Idf_Rastreador", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Nme_SP_Busca", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Flg_Aprendiz", SqlDbType.Bit, 1));
            return (parms);
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
            parms[0].Value = this._idParametroBuscaCV;
            parms[1].Value = this._flagIdfCurriculo;
            parms[2].Value = this._flagNumCPF;
            parms[3].Value = this._flagEmlPessoa;
            parms[4].Value = this._flagIdfFuncao;
            parms[5].Value = this._flagIdfCidade;
            parms[6].Value = this._flagSigEstado;
            parms[7].Value = this._flagPesoEscolaridade;
            parms[8].Value = this._flagIdfSexo;
            parms[9].Value = this._flagIdadeMin;
            parms[10].Value = this._flagIdadeMax;
            parms[11].Value = this._flagSalarioMin;
            parms[12].Value = this._flagSalarioMax;
            parms[13].Value = this._flagMesesExp;
            parms[14].Value = this._flagNmePessoa;
            parms[15].Value = this._flagDesBairro;
            parms[16].Value = this._flagDesLogradouro;
            parms[17].Value = this._flagNumCEPMin;
            parms[18].Value = this._flagNumCEPMax;
            parms[19].Value = this._flagExperienciaEm;
            parms[20].Value = this._flagDesCurso;
            parms[21].Value = this._flagNmeFonte;
            parms[22].Value = this._flagDesCursoOutros;
            parms[23].Value = this._flagNmeFonteOutros;
            parms[24].Value = this._flagNmeEmpresa;
            parms[25].Value = this._flagIdfAreaBNE;
            parms[26].Value = this._flagIdfCategoriaHabilitacao;
            parms[27].Value = this._flagIdfTipoVeiculo;
            parms[28].Value = this._flagNumDDD;
            parms[29].Value = this._flagNumTelefone;
            parms[30].Value = this._flagIdfDeficiencia;
            parms[31].Value = this._flagDesMetaBusca;
            parms[32].Value = this._flagDesMetabuscaRapida;
            parms[33].Value = this._flagIdfOrigem;
            parms[34].Value = this._flagIdfEstadoCivil;
            parms[35].Value = this._flagIdfFilial;
            parms[36].Value = this._flagIdfsIdioma;
            parms[37].Value = this._flagIdfsDisponibilidade;
            parms[38].Value = this._flagIdfRaca;
            parms[39].Value = this._flagFlgFilhos;
            parms[40].Value = this._flagIdfVaga;
            parms[41].Value = this._flagIdfRastreador;
            parms[42].Value = this._flagInativo;

            if (!String.IsNullOrEmpty(this._nomeSPBusca))
                parms[43].Value = this._nomeSPBusca;
            else
                parms[43].Value = DBNull.Value;

            parms[44].Value = this._flagAprendiz;

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
        /// Método utilizado para inserir uma instância de ParametroBuscaCV no banco de dados.
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
                        this._idParametroBuscaCV = Convert.ToInt32(cmd.Parameters["@Idf_Parametro_Busca_CV"].Value);
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
        /// Método utilizado para inserir uma instância de ParametroBuscaCV no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idParametroBuscaCV = Convert.ToInt32(cmd.Parameters["@Idf_Parametro_Busca_CV"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de ParametroBuscaCV no banco de dados.
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
        /// Método utilizado para atualizar uma instância de ParametroBuscaCV no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de ParametroBuscaCV no banco de dados.
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
        /// Método utilizado para salvar uma instância de ParametroBuscaCV no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de ParametroBuscaCV no banco de dados.
        /// </summary>
        /// <param name="idParametroBuscaCV">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idParametroBuscaCV)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Parametro_Busca_CV", SqlDbType.Int, 4));

            parms[0].Value = idParametroBuscaCV;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de ParametroBuscaCV no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idParametroBuscaCV">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idParametroBuscaCV, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Parametro_Busca_CV", SqlDbType.Int, 4));

            parms[0].Value = idParametroBuscaCV;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de ParametroBuscaCV no banco de dados.
        /// </summary>
        /// <param name="idParametroBuscaCV">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idParametroBuscaCV)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Parametro_Busca_CV where Idf_Parametro_Busca_CV in (";

            for (int i = 0; i < idParametroBuscaCV.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idParametroBuscaCV[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idParametroBuscaCV">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idParametroBuscaCV)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Parametro_Busca_CV", SqlDbType.Int, 4));

            parms[0].Value = idParametroBuscaCV;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idParametroBuscaCV">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idParametroBuscaCV, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Parametro_Busca_CV", SqlDbType.Int, 4));

            parms[0].Value = idParametroBuscaCV;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Par.Idf_Parametro_Busca_CV, Par.Flg_Idf_Curriculo, Par.Flg_Num_CPF, Par.Flg_Eml_Pessoa, Par.Flg_Idf_Funcao, Par.Flg_Idf_Cidade, Par.Flg_Sig_Estado, Par.Flg_Peso_Escolaridade, Par.Flg_Idf_Sexo, Par.Flg_Idade_Min, Par.Flg_Idade_Max, Par.Flg_Salario_Min, Par.Flg_Salario_Max, Par.Flg_Meses_Exp, Par.Flg_Nme_Pessoa, Par.Flg_Des_Bairro, Par.Flg_Des_Logradouro, Par.Flg_Num_CEP_Min, Par.Flg_Num_CEP_Max, Par.Flg_Experiencia_Em, Par.Flg_Des_Curso, Par.Flg_Nme_Fonte, Par.Flg_Des_Curso_Outros, Par.Flg_Nme_Fonte_Outros, Par.Flg_Nme_Empresa, Par.Flg_Idf_Area_BNE, Par.Flg_Idf_Categoria_Habilitacao, Par.Flg_Idf_Tipo_Veiculo, Par.Flg_Num_DDD, Par.Flg_Num_Telefone, Par.Flg_Idf_Deficiencia, Par.Flg_Des_MetaBusca, Par.Flg_Des_Metabusca_Rapida, Par.Flg_Idf_Origem, Par.Flg_Idf_Estado_Civil, Par.Flg_Idf_Filial, Par.Flg_Idfs_Idioma, Par.Flg_Idfs_Disponibilidade, Par.Flg_Idf_Raca, Par.Flg_Flg_Filhos, Par.Flg_Idf_Vaga, Par.Flg_Idf_Rastreador, Par.Flg_Inativo, Par.Nme_SP_Busca FROM BNE_Parametro_Busca_CV Par";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de ParametroBuscaCV a partir do banco de dados.
        /// </summary>
        /// <param name="idParametroBuscaCV">Chave do registro.</param>
        /// <returns>Instância de ParametroBuscaCV.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static ParametroBuscaCV LoadObject(int idParametroBuscaCV)
        {
            using (IDataReader dr = LoadDataReader(idParametroBuscaCV))
            {
                ParametroBuscaCV objParametroBuscaCV = new ParametroBuscaCV();
                if (SetInstance(dr, objParametroBuscaCV))
                    return objParametroBuscaCV;
            }
            throw (new RecordNotFoundException(typeof(ParametroBuscaCV)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de ParametroBuscaCV a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idParametroBuscaCV">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de ParametroBuscaCV.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static ParametroBuscaCV LoadObject(int idParametroBuscaCV, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idParametroBuscaCV, trans))
            {
                ParametroBuscaCV objParametroBuscaCV = new ParametroBuscaCV();
                if (SetInstance(dr, objParametroBuscaCV))
                    return objParametroBuscaCV;
            }
            throw (new RecordNotFoundException(typeof(ParametroBuscaCV)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de ParametroBuscaCV a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idParametroBuscaCV))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de ParametroBuscaCV a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idParametroBuscaCV, trans))
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
        /// <param name="objParametroBuscaCV">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, ParametroBuscaCV objParametroBuscaCV)
        {
            try
            {
                if (dr.Read())
                {
                    objParametroBuscaCV._idParametroBuscaCV = Convert.ToInt32(dr["Idf_Parametro_Busca_CV"]);
                    objParametroBuscaCV._flagIdfCurriculo = Convert.ToBoolean(dr["Flg_Idf_Curriculo"]);
                    objParametroBuscaCV._flagNumCPF = Convert.ToBoolean(dr["Flg_Num_CPF"]);
                    objParametroBuscaCV._flagEmlPessoa = Convert.ToBoolean(dr["Flg_Eml_Pessoa"]);
                    objParametroBuscaCV._flagIdfFuncao = Convert.ToBoolean(dr["Flg_Idf_Funcao"]);
                    objParametroBuscaCV._flagIdfCidade = Convert.ToBoolean(dr["Flg_Idf_Cidade"]);
                    objParametroBuscaCV._flagSigEstado = Convert.ToBoolean(dr["Flg_Sig_Estado"]);
                    objParametroBuscaCV._flagPesoEscolaridade = Convert.ToBoolean(dr["Flg_Peso_Escolaridade"]);
                    objParametroBuscaCV._flagIdfSexo = Convert.ToBoolean(dr["Flg_Idf_Sexo"]);
                    objParametroBuscaCV._flagIdadeMin = Convert.ToBoolean(dr["Flg_Idade_Min"]);
                    objParametroBuscaCV._flagIdadeMax = Convert.ToBoolean(dr["Flg_Idade_Max"]);
                    objParametroBuscaCV._flagSalarioMin = Convert.ToBoolean(dr["Flg_Salario_Min"]);
                    objParametroBuscaCV._flagSalarioMax = Convert.ToBoolean(dr["Flg_Salario_Max"]);
                    objParametroBuscaCV._flagMesesExp = Convert.ToBoolean(dr["Flg_Meses_Exp"]);
                    objParametroBuscaCV._flagNmePessoa = Convert.ToBoolean(dr["Flg_Nme_Pessoa"]);
                    objParametroBuscaCV._flagDesBairro = Convert.ToBoolean(dr["Flg_Des_Bairro"]);
                    objParametroBuscaCV._flagDesLogradouro = Convert.ToBoolean(dr["Flg_Des_Logradouro"]);
                    objParametroBuscaCV._flagNumCEPMin = Convert.ToBoolean(dr["Flg_Num_CEP_Min"]);
                    objParametroBuscaCV._flagNumCEPMax = Convert.ToBoolean(dr["Flg_Num_CEP_Max"]);
                    objParametroBuscaCV._flagExperienciaEm = Convert.ToBoolean(dr["Flg_Experiencia_Em"]);
                    objParametroBuscaCV._flagDesCurso = Convert.ToBoolean(dr["Flg_Des_Curso"]);
                    objParametroBuscaCV._flagNmeFonte = Convert.ToBoolean(dr["Flg_Nme_Fonte"]);
                    objParametroBuscaCV._flagDesCursoOutros = Convert.ToBoolean(dr["Flg_Des_Curso_Outros"]);
                    objParametroBuscaCV._flagNmeFonteOutros = Convert.ToBoolean(dr["Flg_Nme_Fonte_Outros"]);
                    objParametroBuscaCV._flagNmeEmpresa = Convert.ToBoolean(dr["Flg_Nme_Empresa"]);
                    objParametroBuscaCV._flagIdfAreaBNE = Convert.ToBoolean(dr["Flg_Idf_Area_BNE"]);
                    objParametroBuscaCV._flagIdfCategoriaHabilitacao = Convert.ToBoolean(dr["Flg_Idf_Categoria_Habilitacao"]);
                    objParametroBuscaCV._flagIdfTipoVeiculo = Convert.ToBoolean(dr["Flg_Idf_Tipo_Veiculo"]);
                    objParametroBuscaCV._flagNumDDD = Convert.ToBoolean(dr["Flg_Num_DDD"]);
                    objParametroBuscaCV._flagNumTelefone = Convert.ToBoolean(dr["Flg_Num_Telefone"]);
                    objParametroBuscaCV._flagIdfDeficiencia = Convert.ToBoolean(dr["Flg_Idf_Deficiencia"]);
                    objParametroBuscaCV._flagDesMetaBusca = Convert.ToBoolean(dr["Flg_Des_MetaBusca"]);
                    objParametroBuscaCV._flagDesMetabuscaRapida = Convert.ToBoolean(dr["Flg_Des_Metabusca_Rapida"]);
                    objParametroBuscaCV._flagIdfOrigem = Convert.ToBoolean(dr["Flg_Idf_Origem"]);
                    objParametroBuscaCV._flagIdfEstadoCivil = Convert.ToBoolean(dr["Flg_Idf_Estado_Civil"]);
                    objParametroBuscaCV._flagIdfFilial = Convert.ToBoolean(dr["Flg_Idf_Filial"]);
                    objParametroBuscaCV._flagIdfsIdioma = Convert.ToBoolean(dr["Flg_Idfs_Idioma"]);
                    objParametroBuscaCV._flagIdfsDisponibilidade = Convert.ToBoolean(dr["Flg_Idfs_Disponibilidade"]);
                    objParametroBuscaCV._flagIdfRaca = Convert.ToBoolean(dr["Flg_Idf_Raca"]);
                    objParametroBuscaCV._flagFlgFilhos = Convert.ToBoolean(dr["Flg_Flg_Filhos"]);
                    objParametroBuscaCV._flagIdfVaga = Convert.ToBoolean(dr["Flg_Idf_Vaga"]);
                    objParametroBuscaCV._flagIdfRastreador = Convert.ToBoolean(dr["Flg_Idf_Rastreador"]);
                    objParametroBuscaCV._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Nme_SP_Busca"] != DBNull.Value)
                        objParametroBuscaCV._nomeSPBusca = Convert.ToString(dr["Nme_SP_Busca"]);
                    objParametroBuscaCV._flagAprendiz = Convert.ToBoolean(dr["Flg_Aprendiz"]);

                    objParametroBuscaCV._persisted = true;
                    objParametroBuscaCV._modified = false;

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