using System.Collections.Generic;
using System.Linq;
using BNE.BLL;
using BNE.BLL.Custom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.Solr.Test
{
    [TestClass]
    public class PesquisaCurriculo
    {

        [TestMethod]
        public void BuscaPorMaisDeUmaFuncao()
        {
            int currentIndex = 0;
            int pageSize = 20;
            int idFilial = 171388;
            var funcoes = new List<int> { 3128, 565 };
            var cidade = Cidade.LoadObject(3287);
            var estado = new Estado("PR");
            var email = "grstelmak@gmail.com";
            var sexo = new Sexo(1);
            var raca = new Raca(1);
            var estadoCivil = new EstadoCivil(1);
            var tipoVeiculo = new TipoVeiculo(1);
            var categoriaHabilitacao = new CategoriaHabilitacao(1);
            var areaBNE = new AreaBNE(1);
            var ddd = "41";
            var telefone = "97794110";
            var codigoCPFNome = "04914896923"; //colocar 123456,7895668
            var bairro = "Centro";
            var palavraChave = "palavrachave";
            var filtroExcludente = "bne";
            var zona = "Centro";
            string cepInicial = "83403350";
            string cepFinal = "83403600";
            decimal salarioMinimo = 1500;
            decimal salarioMaximo = 2000;
            short quantidadeExperiencia = 10;
            var idiomas = new List<int> { 1, 2, 3 };
            var disponibilidade = new List<int> { 1, 2, 3 };
            var filhos = true;
            var aprendiz = true;
            var escolaridadesWebEstagios = "1,2,3";
            var logradouro = "Endereço";
            var razaoSocial = "Razão";
            short idadeMinima = 10;
            short idadeMaxima = 20;
            var experiencia = "Descricao Experiencia";
            Funcao funcaoExercida = null;// new Funcao(3128);
            var descricaoFuncaoExercida = "Abalador";

            var cursoTecnicoGraduacao = new Curso(26008);
            var descricaoCursoTecnicoGraduacao = "Curso";
            var fonteTecnicoGraduacao = new Fonte(18670);

            var cursoOutros = new Curso(26008);
            var descricaoCursoOutros = "Outros Cursos";
            var fonteOutros = new Fonte(18670);

            var idOrigem = 1;
            Origem origem = new Origem(idOrigem);
            Origem origemCurriculos = null;


            int numeroRegistosP1;
            decimal mediaSalarialP1;
            string queryP1 = string.Empty;

            var objPesquisaCurriculo = new BLL.PesquisaCurriculo
            {
                Origem = origemCurriculos,
                Cidade = cidade,
                Estado = estado,
                FlagPesquisaAvancada = true,
                EmailPessoa = email,
                Sexo = sexo,
                Raca = raca,
                NumeroDDDTelefone = ddd,
                NumeroTelefone = telefone,
                DescricaoCodCPFNome = codigoCPFNome,
                DescricaoBairro = bairro,
                DescricaoPalavraChave = palavraChave,
                DescricaoFiltroExcludente = filtroExcludente,
                DescricaoZona = zona,
                NumeroSalarioMin = salarioMinimo,
                NumeroSalarioMax = salarioMaximo,
                QuantidadeExperiencia = quantidadeExperiencia,
                EstadoCivil = estadoCivil,
                FlagFilhos = filhos,
                FlagAprendiz = aprendiz,
                IdEscolaridadeWebStagio = escolaridadesWebEstagios,
                TipoVeiculo = tipoVeiculo,
                DescricaoLogradouro = logradouro,
                CategoriaHabilitacao = categoriaHabilitacao,
                AreaBNE = areaBNE,
                RazaoSocial = razaoSocial,
                NumeroIdadeMin = idadeMinima,
                NumeroIdadeMax = idadeMaxima,
                NumeroCEPMin = cepInicial,
                NumeroCEPMax = cepFinal,
                DescricaoExperiencia = experiencia,
                FuncaoExercida = funcaoExercida,
                DescricaoFuncaoExercida = descricaoFuncaoExercida,
                CursoTecnicoGraduacao = cursoTecnicoGraduacao,
                FonteTecnicoGraduacao = fonteTecnicoGraduacao,
                DescricaoCursoTecnicoGraduacao = descricaoCursoTecnicoGraduacao,
                CursoOutrosCursos = cursoOutros,
                FonteOutrosCursos = fonteOutros,
                DescricaoCursoOutrosCursos = descricaoCursoOutros
            };
            var fun = funcoes.Select(x => new PesquisaCurriculoFuncao { Funcao = new Funcao(x) }).ToList();
            var idi = idiomas.Select(x => new PesquisaCurriculoIdioma { Idioma = new Idioma(x) }).ToList();
            var dis = disponibilidade.Select(x => new PesquisaCurriculoDisponibilidade { Disponibilidade = new Disponibilidade(x) }).ToList();
            objPesquisaCurriculo.Salvar(fun, idi, dis, new List<CampoPalavraChavePesquisaCurriculo>());

            BLL.PesquisaCurriculo.BuscaCurriculo(pageSize, currentIndex, idOrigem, idFilial, null, objPesquisaCurriculo, out numeroRegistosP1, out mediaSalarialP1);

            var parametros = new BuscaCurriculo.Parametros
            {
                Origem = origem,
                Filial = new Filial(idFilial),
                Funcoes = funcoes,
                Cidade = cidade,
                Estado = estado,
                Email = email,
                Sexo = sexo,
                Raca = raca,
                DDDTelefone = ddd,
                Telefone = telefone,
                CodigoCPFNome = codigoCPFNome,
                Bairro = bairro,
                PalavraChave = palavraChave,
                FiltroExcludente = filtroExcludente,
                Zona = zona,
                ValorSalarioMinimo = salarioMinimo,
                ValorSalarioMaximo = salarioMaximo,
                QuantidadeExperiencia = quantidadeExperiencia,
                EstadoCivil = estadoCivil,
                Filhos = filhos,
                Aprendiz = aprendiz,
                EscolaridadesWebEstagios = escolaridadesWebEstagios,
                TipoVeiculo = tipoVeiculo,
                Logradouro = logradouro,
                CategoriaHabilitacao = categoriaHabilitacao,
                AreaBNE = areaBNE,
                RazaoSocial = razaoSocial,
                IdadeMinima = idadeMinima,
                IdadeMaxima = idadeMaxima,
                NumeroCEPInicial = cepInicial,
                NumeroCEPFinal = cepFinal,
                Experiencia = experiencia,
                FuncaoExercida = funcaoExercida,
                DescricaoFuncaoExercida = descricaoFuncaoExercida,
                CursoTecnicoGraduacao = cursoTecnicoGraduacao,
                FonteTecnicoGraduacao = fonteTecnicoGraduacao,
                DescricaoCursoTecnicoGraduacao = descricaoCursoTecnicoGraduacao,
                CursoOutros = cursoOutros,
                FonteOutros = fonteOutros,
                DescricaoCursoOutros = descricaoCursoOutros,
                Idiomas = null, //idiomas,
                Disponibilidades = disponibilidade
            };

            // Assert.AreEqual(numeroRegistosP1, p2.response.numFound);
        }

    }
}
