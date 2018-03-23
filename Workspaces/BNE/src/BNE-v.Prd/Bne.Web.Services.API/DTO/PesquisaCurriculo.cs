using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace Bne.Web.Services.API.DTO
{
    [ApiExplorerSettings(IgnoreApi = false)]
    public class PesquisaCurriculo : DTO.Request
    {
        #region RegistrosPorPagina
        /// <summary>
        /// Número de registros a ser retornado por página
        /// </summary>
        public int RegistrosPorPagina
        {
            get;
            set;
        } = 10;
        #endregion RegistrosPorPagina

        #region Pagina
        /// <summary>
        /// Número da página, iniciando em 1
        /// </summary>
        public int Pagina
        {
            get;
            set;
        } = 1;
        #endregion

        #region Funcao
        /// <summary>
        /// Um dos valores presentes da Tabela de Funções
        /// </summary>
        public String Funcao
        {
            get;
            set;
        }
        #endregion

        #region ListaFuncoes
        /// <summary>
        /// Lista com as funcoes desejadas.
        /// Mesma funcionalidade do campo Funcao, podendo indicar uma série de valores.
        /// Indicar valores presentes da Tabela de Funções.
        /// </summary>
        public List<String> ListaFuncoes
        {
            get;
            set;
        }
        #endregion

        #region Cidade
        /// <summary>
        /// Um dos valores presentes da Tabela de Cidades. Enviar no formato nome-da-cidade/UF.
        /// </summary>
        public String Cidade
        {
            get;
            set;
        }
        #endregion

        #region Disponibilidades
        /// <summary>
        /// Lista de disponibilidades
        /// </summary>
        public List<DTO.Enum.Disponibilidade> Disponibilidades { get; set; }
        #endregion Disponibilidades

        #region PalavraChave
        /// <summary>
        /// Palavra chave a ser pesquisada em todo o currículo.
        /// </summary>
        public string PalavraChave
        {
            get;
            set;
        }
        #endregion

        #region ExcluirPalavraChave
        /// <summary>
        /// Currículos com essa(s) palavra(s) chave(s) serão excluídos do resultado.
        /// </summary>
        public string ExcluirPalavraChave
        {
            get;
            set;
        }
        #endregion

        #region PalavraChaveExperiencia
        /// <summary>
        /// Palavra chave a ser pesquisada nas experiências.
        /// </summary>
        public string PalavraChaveExperiencia
        {
            get;
            set;
        }
        #endregion

        #region FuncaoExperiencia
        /// <summary>
        /// Pesquisa a função nas exoeriências do usuário. Recomenda-se informar um dos valores presentes da Tabela de Funções
        /// </summary>
        public string FuncaoExperiencia
        {
            get;
            set;
        }
        #endregion

        #region Estado
        /// <summary>
        /// Sigla de um dos estados brasileiros
        /// </summary>
        public String Estado
        {
            get;
            set;
        }
        #endregion

        #region Escolaridade
        /// <summary>
        /// Um dos valores presentes na Tabela Escolaridades.
        /// </summary>
        public String Escolaridade
        {
            get;
            set;
        }
        #endregion

        #region Sexo
        /// <summary>
        /// Sexo informado no currículo.
        /// </summary>
        public BNE.BLL.Enumeradores.Sexo? Sexo
        {
            get;
            set;
        }
        #endregion

        #region IdadeMinima
        /// <summary>
        /// Idade Mínima informada no currículo.
        /// </summary>
        public int? IdadeMinima
        {
            get;
            set;
        }
        #endregion

        #region IdadeMaxima
        /// <summary>
        /// Idade Máxima informada no currículo.
        /// </summary>
        public int? IdadeMaxima
        {
            get;
            set;
        }
        #endregion

        #region SalarioMinimo
        /// <summary>
        /// Salário Mínimo informada no currículo no formato americano.
        /// </summary>
        public decimal? SalarioMinimo
        {
            get;
            set;
        }
        #endregion

        #region SalarioMaximo
        /// <summary>
        /// Salário Máximo informada no currículo no formato americano.
        /// </summary>
        public decimal? SalarioMaximo
        {
            get;
            set;
        }
        #endregion

        #region QuantidadeExperiencia
        /// <summary>
        /// Quantidade de experiência mínima em meses.
        /// </summary>
        public Int64? QuantidadeExperiencia
        {
            get;
            set;
        }
        #endregion

        #region Idioma
        /// <summary>
        /// Lista com os valores presentes no Enumerador Idiomas
        /// </summary>
        public String[] Idioma
        {
            get;
            set;
        }
        #endregion

        #region CodCPFNome
        /// <summary>
        /// Indicar o código do currículo (BNE), CPF ou Nome. Utilizado para buscar um currículo específico.
        /// </summary>
        public string CodCPFNome
        {
            get;
            set;
        }
        #endregion

        #region EstadoCivil
        /// <summary>
        /// Indicar um dos valores presentes no Enumerador Estado Civil.
        /// </summary>
        public String EstadoCivil
        {
            get;
            set;
        }
        #endregion

        #region Bairro
        /// <summary>
        /// Bairro do candidato.
        /// </summary>
        public string Bairro
        {
            get;
            set;
        }
        #endregion

        #region Logradouro
        /// <summary>
        /// Endereço do candidato.
        /// </summary>
        public string Logradouro
        {
            get;
            set;
        }
        #endregion

        #region CEPMinimo
        /// <summary>
        /// CEP mínimo do endereço do candidato
        /// </summary>
        public string CEPMinimo
        {
            get;
            set;
        }
        #endregion

        #region CEPMaximo
        /// <summary>
        /// CEP máximo do endereço do candidato
        /// </summary>
        public string CEPMaximo
        {
            get;
            set;
        }
        #endregion

        #region CursoTecnicoGraduacao
        /// <summary>
        /// Curso técnico ou de graduação desejado
        /// </summary>
        public String CursoTecnicoGraduacao
        {
            get;
            set;
        }
        #endregion

        #region InstituicaoTecnicoGraduacao
        /// <summary>
        /// Instituição de ensino do curso técnico ou de graduação desejada
        /// </summary>
        public String InstituicaoTecnicoGraduacao
        {
            get;
            set;
        }
        #endregion

        #region CursoOutrosCursos
        /// <summary>
        /// Cursos adicionais.
        /// </summary>
        public String CursoOutrosCursos
        {
            get;
            set;
        }
        #endregion

        #region InstituicaoOutrosCursos
        /// <summary>
        /// Instituição do curso adicional
        /// </summary>
        public String InstituicaoOutrosCursos
        {
            get;
            set;
        }
        #endregion

        #region EmpresaQueJaTrabalhou
        /// <summary>
        /// Empresa presente nas experiências
        /// </summary>
        public string EmpresaQueJaTrabalhou
        {
            get;
            set;
        }
        #endregion

        #region AreaEmpresaQueJaTrabalhou
        /// <summary>
        /// Indicar um dos valores presentes na tabela Areas
        /// </summary>
        public String AreaEmpresaQueJaTrabalhou
        {
            get;
            set;
        }
        #endregion

        #region CategoriaHabilitacao
        /// <summary>
        /// Indicar um dos valores presentes na tabela CategoriasHabilitacao
        /// </summary>
        public String CategoriaHabilitacao
        {
            get;
            set;
        }
        #endregion

        #region DDDTelefone
        /// <summary>
        /// DDD do telefone do candidato
        /// </summary>
        public string DDDTelefone
        {
            get;
            set;
        }
        #endregion

        #region NumeroTelefone
        /// <summary>
        /// Número do telefone do candidato
        /// </summary>
        public string NumeroTelefone
        {
            get;
            set;
        }
        #endregion

        #region Email
        /// <summary>
        /// E-mail do candidato
        /// </summary>
        public string Email
        {
            get;
            set;
        }
        #endregion

        #region Deficiencia
        /// <summary>
        /// Indicar um dos valores presentes na tabela Deficiencias
        /// </summary>
        public String Deficiencia
        {
            get;
            set;
        }
        #endregion

        #region TipoVeiculo
        /// <summary>
        /// Indicar um dos valores presentes na tabela TiposVeiculo
        /// </summary>
        public String TipoVeiculo
        {
            get;
            set;
        }
        #endregion

        #region Raca
        /// <summary>
        /// Indicar um dos valores presentes na tabela Racas
        /// </summary>
        public String Raca
        {
            get;
            set;
        }
        #endregion

        #region PossuiFilhos
        /// <summary>
        /// Indicar true, se desejar filtrar candidatos com filhos
        /// </summary>
        public bool? PossuiFilhos
        {
            get;
            set;
        }
        #endregion

        #region QueroContratarEstagiarios
        /// <summary>
        /// Indicar true, se desejar filtrar candidatos com perfil de estágio.
        /// </summary>
        public bool QueroContratarEstagiarios
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Parametros de pesquisa para formacoes
        /// </summary>
        public PesquisaCurriculoFormacao Formacao { get; set; }
    }

    /// <summary>
    /// Parametros de pesquisa para formacoes
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = false)]
    public class PesquisaCurriculoFormacao
    {
        /// <summary>
        /// Nível do curso. Um dos valores presentes na Tabela Escolaridades.
        /// </summary>
        public string Escolaridade { get; set; }

        /// <summary>
        /// Nome do curso procurado
        /// </summary>
        public string Curso { get; set; }

        /// <summary>
        /// Instituição de ensino
        /// </summary>
        public string Instituicao { get; set; }

        /// <summary>
        /// Ano de conclusao do curso
        /// </summary>
        public int? AnoConclusao { get; set; }

        /// <summary>
        /// Período do curso
        /// </summary>
        public int? Periodo { get; set; }
        
        /// <summary>
        /// Situação do curso de nível Médio Incompleto, Técnico/Pós-Médio Incompleto, Tecnólogo Incompleto ou Superior Incompleto
        /// </summary>
        public Enum.SituacaoCurso? SituacaoCurso { get; set; }

        /// <summary>
        /// Cidade do curso
        /// </summary>
        public string Cidade { get; set; }
    }
}