using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Bne.Web.Services.API.Controllers
{
    /// <summary>
    /// API com as tabelas necessárias para a correta integração com as API's de consulta e cadastro do BNE.
    /// </summary>
    public class TabelasController : ApiController
    {
        /// <summary>
        /// Lista de funções.
        /// </summary>
        /// <param name="nomeParcial">Nome parcial para pesquisa das funções do BNE.</param>
        /// <param name="numeroRegistros">Número de registros a ser retornado.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Funcoes")]
        public string[] Funcoes(String nomeParcial, int numeroRegistros = 10)
        {
            if (numeroRegistros > 50)
                numeroRegistros = 50;
            
            return Funcao.RecuperarFuncoes(nomeParcial, numeroRegistros, null);
        }

        /// <summary>
        /// Lista de Cidades
        /// </summary>
        /// <param name="nomeParcial">Nome parcial para pesquisa de cidades no BNE.</param>
        /// <param name="numeroRegistros">Número de registros a ser retornado.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Cidades")]
        public string[] Cidades(String nomeParcial, int numeroRegistros = 10)
        {
            if (numeroRegistros > 50)
                numeroRegistros = 50;

            List<String> retorno = new List<string>();
            foreach (var item in Cidade.RecuperarNomesCidadesEstado(nomeParcial, null, numeroRegistros))
            {
                retorno.Add(item.Value);
            }

            return retorno.ToArray();
        }

        /// <summary>
        /// Lista de cursos.
        /// </summary>
        /// <param name="nomeParcial">Nome parcial para pesquisa dos cursos do BNE.</param>
        /// <param name="numeroRegistros">Número de registros a ser retornado.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Cursos")]
        public string[] Cursos(String nomeParcial, int numeroRegistros = 10)
        {
            if (numeroRegistros > 50)
                numeroRegistros = 50;

            return BNE.BLL.Curso.RecuperarCursos(nomeParcial, numeroRegistros);
        }

        /// <summary>
        /// Instituições de insino.
        /// </summary>
        /// <param name="nomeParcial">Nome parcial para pesquisa das instituições.</param>
        /// <param name="nivelCurso">Nível do curso a ser considerado. Envie vazio caso seja indiferente.</param>
        /// <param name="numeroRegistros">Número de registros a ser retornado. Default: 10.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Instituicoes")]
        public string[] Instituicoes(string nomeParcial, DTO.Enum.NivelCurso? nivelCurso = null, int numeroRegistros = 10)
        {
            if (numeroRegistros > 50)
                numeroRegistros = 50;

            int? idNivel = null;
            if (nivelCurso.HasValue)
                idNivel = Convert.ToInt32(nivelCurso);

            return BNE.BLL.Fonte.RecuperarSiglaNomeFonteNivelCurso(nomeParcial, idNivel, numeroRegistros);
        }

        /// <summary>
        /// Lista de Escolaridades
        /// </summary>
        [HttpGet]
        [ActionName("Escolaridades")]
        public string[] Escolaridades()
        {
            return new string[]{
                "Ensino Fundamental Incompleto",
                "Ensino Fundamental Completo",
                "Ensino Médio Incompleto",
                "Ensino Médio Completo",
                "Técnico/Pós-Médio Incompleto",
                "Técnico/Pós-Médio Completo",
                "Tecnólogo Incompleto",
                "Superior Incompleto",
                "Tecnólogo Completo",
                "Superior Completo",
                "Pós Graduação / Especialização",
                "Mestrado",
                "Doutorado",
                "Pós-Doutorado"
            };
        }

        /// <summary>
        /// Lista de áreas
        /// </summary>
        [HttpGet]
        [ActionName("Areas")]
        public string[] Areas()
        {
            return new string[]{
                "Acessórios",
                "Administração Pública ",
                "Administrativo",
                "Agronegócios",
                "Água e Esgoto",
                "Alimentos",
                "Arte e Cultura",
                "Associações e Diversos",
                "Bebidas",
                "Comércio",
                "Comunicação",
                "Construção",
                "Consultoria",
                "Contabilidade",
                "Educação",
                "Elétrico",
                "Eletrônico",
                "Energia",
                "Esporte",
                "Extração",
                "Farmacêutico",
                "Financeiro",
                "Forças Armadas",
                "Fumo",
                "Gráfica",
                "Hotelaria e Turismo",
                "Imobiliária",
                "Informática",
                "Internacional",
                "Jurídico",
                "Limpeza",
                "Locação",
                "Logística",
                "Madeira",
                "Manutenção",
                "Marketing",
                "Mecânico",
                "Metal Mecânico",
                "Metalurgia",
                "Minerais",
                "Móveis",
                "Papel ",
                "Petróleo",
                "Plástico",
                "Produção",
                "Químico",
                "Recursos Humanos",
                "Saúde",
                "Segurança",
                "Serviços Domésticos",
                "Serviços Pessoais",
                "Social",
                "Telecomunicações",
                "Têxteis",
                "Veículos"
            };
        }

        /// <summary>
        /// Lista de Categorias de Habilitação
        /// </summary>
        [HttpGet]
        [ActionName("CategoriasHabilitacao")]
        public string[] CategoriasHabilitacao()
        {
            return new string[]{
                "A",
                "AB",
                "AC",
                "AD",
                "AE",
                "B",
                "C",
                "D",
                "E"
            };
        }

        /// <summary>
        /// Lista de Tipos de Veículo
        /// </summary>
        [HttpGet]
        [ActionName("TiposVeiculos")]
        public string[] TiposVeiculos()
        {
            return new string[]{
                "Caminhão",
                "Carro",
                "Motocicleta",
                "Ônibus",
                "Outros",
                "Utilitário"
            };
        }

        /// <summary>
        /// Lista de Deficiências
        /// </summary>
        [HttpGet]
        [ActionName("Deficiencias")]
        public string[] Deficiencias()
        {
            return new string[]{
                "Auditiva",
                "Física",
                "Mental",
                "Múltipla",
                "Nenhuma",
                "Qualquer",
                "Reabilitado",
                "Visual"
            };
        }

        /// <summary>
        /// Lista de Raças
        /// </summary>
        [HttpGet]
        [ActionName("Racas")]
        public string[] Racas()
        {
            return new string[]{
                "Amarela",
                "Branca",
                "Indígena",
                "Não Informado",
                "Negra",
                "Parda"
            };
        }

        /// <summary>
        /// Lista de Estados Civis
        /// </summary>
        [HttpGet]
        [ActionName("EstadosCivis")]
        public string[] EstadosCivis()
        {
            return EstadoCivil.Listar().Select((k, v) => k.Value).ToArray(); 
        }

        /// <summary>
        /// Lista de Idiomas
        /// </summary>
        [HttpGet]
        [ActionName("Idiomas")]
        public string[] Idiomas()
        {
            return new string[]{
                "Alemão",
                "Chinês Mandarim",
                "Espanhol",
                "Francês",
                "Inglês",
                "Italiano",
                "Japonês",
                "Português",
                "Russo"
            };
        }
                
    }
}
