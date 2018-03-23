using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanHouse.Business.DTO
{
    public class Curriculo
    {
        public int idPessoaFisica { get; set; }
        public int idCurriculo { get; set; }


        public string nome { get; set; }
        public string email { get; set; }
        public string Cidade { get; set; }
        public string DDDCelular { get; set; }
        public string Celular { get; set; }
        public string DDDTelefone { get; set; }
        public string Telefone { get; set; }
        public string DataNascimento { get; set; }
        public string CPF { get; set; }
        public int? Sexo { get; set; }
        public bool TemDeficiencia { get; set; }
        public int? idDeficiencia { get; set; }
        public decimal PretensaoSalarial { get; set; }
        public string funcao { get; set; }
        public string imgCandidato { get; set; }
        public bool alterouImagem { get; set; }
        public bool salvarTudo { get; set; }

        public int? idEstadoCivil { get; set; }

        public List<DTO.IdiomaCandidato> idiomasCandidato { get; set; }
        public List<DTO.Formacao> formacoes { get; set; }
        public List<DTO.Formacao> cursos { get; set; }
        public IList funcoes { get; set; }
        //public IList experiencias { get; set; }
        public List<DTO.Experiencia> experiencias { get; set; }


        public bool TemExperienciaProfissional { get; set; }

        //última Experiencia
        public int idExperiencia { get; set; }
        public string empresa { get; set; }
        public string funcaoEmpresa { get; set; }
        public string atividades { get; set; }

        public int? idAreaBNE { get; set; }
        public string mesInicio { get; set; }
        public string anoInicio { get; set; }
        public bool empregoAtual { get; set; }

        public string mesSaida { get; set; }
        public string anoSaida { get; set; }

        //penultima Experiencia
        public int idExperienciape { get; set; }
        public string empresape { get; set; }
        public string funcaoEmpresape { get; set; }
        public string atividadespe { get; set; }

        public int? idAreaBNEpe { get; set; }
        public string mesIniciope { get; set; }
        public string anoIniciope { get; set; }
        public string mesSaidape { get; set; }
        public string anoSaidape { get; set; }

        //terceira Experiencia
        public int idExperiencia3 { get; set; }
        public string empresa3 { get; set; }
        public string funcaoEmpresa3 { get; set; }
        public string atividades3 { get; set; }

        public int? idAreaBNE3 { get; set; }
        public string mesInicio3 { get; set; }
        public string anoInicio3 { get; set; }
        public string mesSaida3 { get; set; }
        public string anoSaida3 { get; set; }

        public bool? periodoManha { get; set; }
        public bool? periodoTarde { get; set; }
        public bool? periodoNoite { get; set; }
        public bool? periodoFimdeSemana { get; set; }

        public string Observacao { get; set; }

        // informacoes para vaga
        public int qtdCandidatura { get; set; }
        public bool isVip { get; set; }

        //informações para envio do CV para empresas
        public int qtdEnvioCVEmpresa { get; set; }

        //identifica a origem do CV, qual Lan House ou se é do azulzinho
        public int? idOrigem { get; set; }

    }
}