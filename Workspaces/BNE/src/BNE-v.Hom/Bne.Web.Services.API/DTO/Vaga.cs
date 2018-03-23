using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BNE.BLL;
using Bne.Web.Services.API.DTO.Enum;
using static BNE.Solr.Vagas.VagasSolr;
using System;

namespace Bne.Web.Services.API.DTO
{
    /// <summary>
    /// Representa uma vaga de emprego no BNE.
    /// </summary>
    public class Vaga
    {
        /// <summary>
        /// Identificador da vaga. Considerado somente nas pesquisas. 
        /// Será desconsiderado em inserções e atualizações.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// (Obrigatório) Lista que deve ser composta pelos seguintes valores.
        /// -> Aprendiz
        /// -> Autônomo
        /// -> Efetivo
        /// -> Estágio
        /// -> Freelancer
        /// -> Temporário
        /// </summary>
        public List<string> TipoVinculo { get; set; }

        /// <summary>
        /// Cursos que o estagiário deve estar cursando para a vaga de estágio.
        /// É considerado somente para os tipos de vínculo "Estágio" e "Aprendiz". Para os demais vínculos, será desconsiderado.
        /// Na tabela de Cursos é possível ter as sugestões para os cursos informados mas, embora altamente recomendável por questões de filtros, não é obrigatório que um valor presente naquela tabela seja utilizado.
        /// </summary>
        public List<string> Cursos { get; set; }

        /// <summary>
        /// (Obrigatório) Nome completo da função.
        /// </summary>
        [Required(ErrorMessage = "A função da vaga é obrigatória.")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "A função deve ter no mínimo 4 caracteres e no máximo 200.")]
        public string Funcao { get; set; }

        /// <summary>
        /// (Obrigatório) Nome completo da cidade seguido de barra mais a sigla do estado. Ex.:”Montes Claros/MG”.
        /// </summary>
        [Required(ErrorMessage = "A cidade da vaga é obrigatória.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "A cidade deve ter no mínimo 5 caracteres e no máximo 200.")]
        public string Cidade { get; set; }

        /// <summary>
        /// (Obrigatório) Número de vagas ofertadas.
        /// </summary>
        //[Required(ErrorMessage = "A quantidade de vagas é obrigatória.")]
        //[Range(1, Double.MaxValue, ErrorMessage = "O valor mínimo para a quantidade é 1.")]
        public short? Quantidade { get; set; }

        /// <summary>
        /// Algum dos itens listados:
        /// -> Ensino Fundamental Incompleto
        /// -> Ensino Fundamental Completo
        /// -> Ensino Médio Incompleto
        /// -> Ensino Médio Completo
        /// -> Técnico/Pós-Médio Incompleto
        /// -> Técnico/Pós-Médio Completo
        /// -> Tecnólogo Incompleto
        /// -> Superior Incompleto
        /// -> Tecnólogo Completo
        /// -> Superior Completo
        /// -> Pós Graduação / Especialização
        /// -> Mestrado
        /// -> Doutorado
        /// </summary>
        public string Escolaridade { get; set; }

        /// <summary>
        /// (Opcional) Início da faixa salarial ofertada.
        /// </summary>
        public decimal? SalarioMin { get; set; }

        /// <summary>
        /// (Opcional) Final da faixa salarial ofertada.
        /// </summary>
        public decimal? SalarioMax { get; set; }

        /// <summary>
        /// (Opcional) Descrição dos benefícios oferecidos.
        /// </summary>
        public string Beneficios { get; set; }

        /// <summary>
        /// (Opcional) Requisitos desejados para a vaga.
        /// </summary>
        public string Requisitos { get; set; }

        /// <summary>
        /// (Opcional) Atribuições desejadas para a vaga.
        /// </summary>
        public string Atribuicoes { get; set; }

        /// <summary>
        /// (Opcional) Utilize os itens listados para compor a lista:
        /// -> Manhã
        /// -> Tarde
        /// -> Noite
        /// -> Sábado
        /// -> Domingo
        /// -> Viagem
        ///</summary>
        public List<string> Disponibilidade { get; set; }

        /// <summary>
        /// Nome fantasia da empresa.
        /// </summary>
        //[Required(ErrorMessage = "Nome fantasia é obrigatório.")]
        public string NomeFantasia { get; set; }

        /// <summary>
        /// (Obrigatório) Indica se as informações da empresa são confidênciais.
        /// </summary>
        //[Required(ErrorMessage = "Não foi informado a confidencialidade da vaga.")]
        public bool Confidencial { get; set; }

        /// <summary>
        /// (Opcional) Palavras chaves que auxiliarão na busca de vagas. Essas palavras devem ser separadas por vírgula.
        /// </summary>
        //public string PalavrasChave { get; set; }

        /// <summary>
        /// (Opcional) Lista de objetos do tipo Pergunta.
        /// </summary>
        /// <seealso cref="Bne.Web.Services.API.DTO.Pergunta"/>
        public List<Pergunta> Perguntas { get; set; }

        /// <summary>
        /// (Opcional) Se a vaga é para PCD é necessário alguns dos itens:
        /// -> Auditiva
        /// -> Física
        /// -> Mental
        /// -> Múltipla
        /// -> Nenhuma
        /// -> Qualquer
        /// -> Reabilitado
        /// -> Visual
        /// </summary>
        public string Deficiencia { get; set; }

        /// <summary>
        /// Status atual da vaga
        /// </summary>
        public StatusVaga Status { get; set; }

        /// <summary>
        /// Data de abertura da vaga
        /// </summary>
        public DateTime DataCadastro { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public Vaga() { }



        /// <summary>
        /// 
        /// </summary>
        public string SiglaEstado { get; set; }

        /// <summary>
        /// Data que a vaga começou a aparecer nas pesquisas.
        /// </summary>
        public DateTime DataAnuncio { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DesOrigem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TipoOrigem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool BNERecomenda { get; set; }
        /// <summary>
        /// Codigo da vaga.
        /// </summary>
        public string CodigoVaga { get; set; }
        /// <summary>
        /// Empresa que anunciou a vaga
        /// </summary>
        public int IdFilial { get; set; }
        /// <summary>
        /// /Bairro da vaga
        /// </summary>
        public string Bairro { get; set; }

        /// <summary>
        /// Area da vaga.
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Url da vaga (BNE)
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Plano { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Oferece_Cursos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Campanha { get; set; }
        /// <summary>
        /// Contrutor para uma instancia baseado em uma instância DocVaga
        /// </summary>
        /// <param name="docVaga">Instância com as informações para o novo objeto</param>
        public Vaga(DocVaga docVaga)
        {
            Id = docVaga.Id;
            Atribuicoes = docVaga.Des_Atribuicoes;
            Beneficios = docVaga.Des_Beneficio;
            if (docVaga.Nme_Cidade != null)
                Cidade = $"{docVaga.Nme_Cidade}/{docVaga.Sig_Estado}";

            Confidencial = docVaga.Flg_Confidencial;
            Deficiencia = docVaga.Des_Deficiencia;
            Escolaridade = docVaga.Des_BNE;

            Funcao = docVaga.Des_Funcao;
            NomeFantasia = docVaga.Nme_Empresa;
            Quantidade = (short)docVaga.Qtd_Vaga;
            Requisitos = docVaga.Des_Requisito;
            SalarioMin = docVaga.Vlr_Salario_De;
            SalarioMax = docVaga.Vlr_Salario_Para;
            DataCadastro = docVaga.Dta_Cadastro;
            TipoVinculo = docVaga.Des_Tipo_Vinculo?.ToList();
            Cursos = docVaga.Des_Curso?.ToList();
            SiglaEstado = docVaga.Sig_Estado;
            DataAnuncio = docVaga.Dta_Abertura;
            DesOrigem = docVaga.Des_Origem;
            TipoOrigem = docVaga.Idf_Origem;
            BNERecomenda = docVaga.Flg_BNE_Recomenda;
            CodigoVaga = docVaga.Cod_Vaga;
            IdFilial = docVaga.Idf_Filial;
            this.Bairro = docVaga.Nme_Bairro;
            this.Area = docVaga.Des_Area_BNE;
            this.Url = docVaga.Url_Vaga;
            this.Plano = docVaga.Flg_Plano;
            this.Oferece_Cursos = docVaga.Flg_Oferece_Cursos;
            this.Campanha = docVaga.Campanha;

            Status = StatusVaga.Ativa;
            if (docVaga.Flg_Inativo)
                Status = StatusVaga.Inativa;
            else if (docVaga.Flg_Vaga_Arquivada)
                Status = StatusVaga.Arquivada;
            else if (!docVaga.Flg_Auditada)
                Status = StatusVaga.EmPublicacao;

            // @todo Aguardar indexação das perguntas e disponibilidades no solr
            var perguntas = BNE.BLL.VagaPergunta.RecuperarListaPerguntas(docVaga.Id, null);
            Perguntas = new List<Pergunta>();
            foreach (var vagaPergunta in perguntas)
            {
                Perguntas.Add(new Pergunta()
                {
                    IdPergunta = vagaPergunta.IdVagaPergunta,
                    Texto = vagaPergunta.DescricaoVagaPergunta,
                });
            }

            var disponibilidades = BNE.BLL.VagaDisponibilidade.ListarDisponibilidadesPorVaga(docVaga.Id) ??
                                   new List<VagaDisponibilidade>();
            Disponibilidade = disponibilidades
                                .Where(d => !string.IsNullOrEmpty(d.Disponibilidade.DescricaoDisponibilidade))
                                .Select(d => d.Disponibilidade.DescricaoDisponibilidade).ToList();

        }

        /// <summary>
        /// Contrutor para uma instancia baseado em uma instância BNE.BLL.Vaga
        /// </summary>
        /// <param name="objVaga">Instância com as informações para o novo objeto</param>
        public Vaga(BNE.BLL.Vaga objVaga)
        {
            Id = objVaga.IdVaga;
            Atribuicoes = objVaga.DescricaoAtribuicoes;
            Beneficios = objVaga.DescricaoBeneficio;
            if (objVaga.Cidade != null)
            {
                if (string.IsNullOrEmpty(objVaga.Cidade.NomeCidade))
                    objVaga.Cidade.CompleteObject();
                Cidade = $"{objVaga.Cidade.NomeCidade}/{objVaga.Cidade.Estado.SiglaEstado}";
            }

            Confidencial = objVaga.FlagConfidencial;

            if (objVaga.Deficiencia != null)
            {
                if (string.IsNullOrEmpty(objVaga.Deficiencia.DescricaoDeficiencia))
                    objVaga.Deficiencia.CompleteObject();
                Deficiencia = objVaga.Deficiencia.DescricaoDeficiencia;
            }

            var disponibilidades = BNE.BLL.VagaDisponibilidade.ListarDisponibilidadesPorVaga(objVaga) ??
                                   new List<VagaDisponibilidade>();
            Disponibilidade = disponibilidades
                                .Where(d => !string.IsNullOrEmpty(d.Disponibilidade.DescricaoDisponibilidade))
                                .Select(d => d.Disponibilidade.DescricaoDisponibilidade).ToList();

            if (objVaga.Escolaridade != null)
            {
                if (string.IsNullOrEmpty(objVaga.Escolaridade.DescricaoBNE))
                    objVaga.Escolaridade.CompleteObject();
                Escolaridade = objVaga.Escolaridade.DescricaoBNE;
            }

            DataCadastro = objVaga.DataCadastro;
            Funcao = objVaga.DescricaoFuncao;
            NomeFantasia = objVaga.NomeEmpresa;
            Quantidade = objVaga.QuantidadeVaga;
            Requisitos = objVaga.DescricaoRequisito;
            SalarioMin = objVaga.ValorSalarioDe;
            SalarioMax = objVaga.ValorSalarioPara;
            SiglaEstado = objVaga.Cidade.Estado.SiglaEstado;
            DataAnuncio = objVaga.DataAbertura.Value;
            DesOrigem = objVaga.Origem.DescricaoOrigem;
            TipoOrigem = objVaga.Origem.IdOrigem;
            BNERecomenda = objVaga.FlagBNERecomenda;
            CodigoVaga = objVaga.CodigoVaga;
            IdFilial = objVaga.Filial.IdFilial;

            Bairro = objVaga.IdBairro.HasValue ? BNE.BLL.Bairro.LoadObject(objVaga.IdBairro.Value).NomeBairro : objVaga.NomeBairro;
            var vinculos = BNE.BLL.VagaTipoVinculo.ListarTipoVinculoPorVaga(objVaga.IdVaga);
            TipoVinculo = vinculos
                            .Where(d => !string.IsNullOrEmpty(d.TipoVinculo.DescricaoTipoVinculo))
                            .Select(d => d.TipoVinculo.DescricaoTipoVinculo).ToList();

            // Carregando cursos da vaga
            Cursos = new List<string>();
            VagaCurso.ListarCursoPorVaga(objVaga.IdVaga).ForEach(cv =>
            {
                Cursos.Add(cv.Curso == null ? cv.DescricaoCurso : cv.Curso.DescricaoCurso);
            });

            Status = StatusVaga.Ativa;
            if (objVaga.FlagInativo)
                Status = StatusVaga.Inativa;
            else if (objVaga.FlagVagaArquivada)
                Status = StatusVaga.Arquivada;
            else if (!objVaga.FlagAuditada.HasValue || !objVaga.FlagAuditada.Value)
                Status = StatusVaga.EmPublicacao;

            var perguntas = BNE.BLL.VagaPergunta.RecuperarListaPerguntas(objVaga.IdVaga, null);
            Perguntas = new List<Pergunta>();
            foreach (var vagaPergunta in perguntas)
            {
                Perguntas.Add(new Pergunta()
                {
                    IdPergunta = vagaPergunta.IdVagaPergunta,
                    Texto = vagaPergunta.DescricaoVagaPergunta,
                });
            }
        }
    }
}