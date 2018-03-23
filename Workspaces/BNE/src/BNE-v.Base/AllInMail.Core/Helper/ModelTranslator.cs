using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail.Helper
{
    public class ModelTranslator
    {

        public static AllInCurriculoTemplateModel TranslateToAllInCurriculoModel(BNE.BLL.DTO.AllInCurriculo load)
        {
            var model = new AllInCurriculoTemplateModel();

            if (load.Altura.HasValue)
            {
                var text = load.Altura.Value.ToString("N2").Where(a => char.IsNumber(a));
                model.Altura = Convert.ToInt32(new string(text.ToArray()));
            }
            model.Celular = load.NumeroCelular;
            model.Cep = load.NumeroCEP;
            model.Cidade = load.NomeCidade;
            model.Conhecimento_Curriculo = load.OutrosConhecimentos;
            model.CPF = BNE.BLL.Custom.Helper.FormatarCPF(load.NumeroCPF);
            model.DDD_Celular = load.NumeroDDDCelular;
            model.DDD_Telefone = load.NumeroDDDTelefone;
            model.Deficiencia = load.Deficiencia;

            if (load.DisponibilidadesTrabalho.Count > 0)
            {
                var disp = new StringBuilder();
                for (int i = 0; i < load.DisponibilidadesTrabalho.Count; i++)
                {
                    if (i != 0)
                    {
                        disp.Append(" ");
                    }

                    disp.Append(load.DisponibilidadesTrabalho[i].Descricao);
                }

                model.Disponibilidade = disp.ToString();
            }

            model.Dt_Cadastro = load.DataCadastroCurriculo;
            model.Dt_Atualizacao = load.DataAtualizacaoCurriculo;
            model.Dt_Modificacao = load.DataModificacaoCurriculo;

            model.Dt_Inicio_Vip = load.UltimoPlanoVipInicio;
            model.Dt_Fim_Vip = load.UltimoPlanoVipFim;

            model.Dt_Nascimento = load.DataNascimento;
            model.Estado_civil = load.EstadoCivil;
            model.Experiencia_Area_BNE = load.UltimaExperiencia.AreaBNE;
            model.EXPERIENCIA_ATIVIDADE = load.UltimaExperiencia.DescricaoAtividade;
            model.Experiencia_Dta_Adminissao = load.UltimaExperiencia.DataAdmissao;
            model.Experiencia_Dta_Demissao = load.UltimaExperiencia.DataDemissao;
            model.Experiencia_Funcao = load.UltimaExperiencia.Funcao;
            model.Experiencia_Razao_Social = load.UltimaExperiencia.RazaoSocial;

            model.Flag_Filhos = load.TemFilhos;
            model.Flag_Viagens = load.DisponibilidadeViajar;
            model.Flag_Vip = load.VIP;

            if (load.Formacoes.Count > 0)
            {
                var first = load.Formacoes.First();

                model.Formacao_01_Ano_Conclusao = first.AnoConclusao;
                model.Formacao_01_Curso = first.DescricaoCurso;
                model.Formacao_01_Descricao_BNE = first.DescricaoFormacao;
                model.Formacao_01_Nome_Fonte = first.NomeFonte;
                model.Formacao_01_Nome_Periodo = first.Periodo;
                model.Formacao_01_Sigla_Fonte = first.SiglaFonte;
                model.Formacao_01_Situacao_Formacao = first.SituacaoFormacao;

                if (load.Formacoes.Count > 1)
                {
                    var last = load.Formacoes.Skip(1).First();

                    model.Formacao_02_Ano_Conclusao = last.AnoConclusao;
                    model.Formacao_02_Curso = last.DescricaoCurso;
                    model.Formacao_02_Descricao_BNE = last.DescricaoFormacao;
                    model.Formacao_02_Nome_Fonte = last.NomeFonte;
                    model.Formacao_02_Nome_Periodo = last.Periodo;
                    model.Formacao_02_Sigla_Fonte = last.SiglaFonte;
                    model.Formacao_02_Situacao_Formacao = last.SituacaoFormacao;
                }
            }

            if (load.FuncoesPretendidas.Count > 0)
            {
                var f1 = load.FuncoesPretendidas.First();
                model.Funcao_Pretendida_01 = f1.NomeFuncaoPretendida;
                if (load.FuncoesPretendidas.Count > 1)
                {
                    var f2 = load.FuncoesPretendidas.Skip(1).First();
                    model.Funcao_Pretendida_02 = f2.NomeFuncaoPretendida;

                    if (load.FuncoesPretendidas.Count > 2)
                    {
                        var f3 = load.FuncoesPretendidas.Skip(2).First();
                        model.Funcao_Pretendida_03 = f3.NomeFuncaoPretendida;
                    }
                }
            }

            model.Habilitacao = load.CategoriaHabilitacao;
            model.Id_Cadastro = load.IdCurriculo;
            model.Idade = load.Idade;


            if (load.Idiomas.Count > 0)
            {
                var languages = load.Idiomas.OrderByDescending(a => a.DescricaoIdioma.ContainsEx("inglês")
                                        || a.DescricaoIdioma.ContainsEx("ingles") ? 1 : -1)
                .ThenByDescending(a => a.DescricaoIdioma.ContainsEx("espanhol") ? 1 : -1)
                .ThenBy(a => a.DescricaoIdioma)
                .ToArray();

                var i1 = languages.First();
                model.Idioma_Nivel_01 = i1.DescricaoIdioma + " " + i1.NivelIdioma;

                if (languages.Length > 1)
                {
                    var i2 = languages.Skip(1).First();
                    model.Idioma_Nivel_02 = i2.DescricaoIdioma + " " + i2.NivelIdioma;

                    if (languages.Length > 2)
                    {
                        var i3 = languages.Skip(2).First();
                        model.Idioma_Nivel_03 = i3.DescricaoIdioma + " " + i3.NivelIdioma;
                    }
                }
            }

            model.Logradouro = load.Logradouro;
            model.Nm_email = load.Email;
            model.Nome = load.NomeCompleto;
            model.Observacao_Curriculo = load.Observacao;

            if (load.Peso.HasValue)
            {
                var text = load.Peso.Value.ToString("N0").Where(a => char.IsNumber(a));
                model.Peso = Convert.ToInt32(new string(text.ToArray()));
            }
            if (load.ValorPretensaoSalarial.HasValue)
                model.Pretensao_Salarial = Convert.ToInt32(load.ValorPretensaoSalarial.GetValueOrDefault().ToString("F0"));

            //model.Qtd_Qm_Me_Viu = 0;
            model.Raca = load.Raca;
            model.Sexo = load.Sexo;
            model.Situacao_Curriculo = load.SituacaoCurriculo;
            model.Telefone = load.NumeroTelefone;
            model.Tipo_Curriculo = load.TipoCurriculo;

            if (load.ValorUltimoSalario.HasValue)
                model.Ultimo_Salario = Convert.ToInt32(load.ValorUltimoSalario.GetValueOrDefault().ToString("F0"));

            model.UF = load.SiglaEstado;

            return model;
        }

        public static IEnumerable<AllInEmpresaTemplateModel> TranslateToAllInEmpresaModel(BNE.BLL.DTO.AllInFilial load)
        {
            foreach (var item in load.Perfis)
            {
                var obj = PopulateAllInEmpresaTemplate(load, item);

                yield return obj;
            }
        }

        private static AllInEmpresaTemplateModel PopulateAllInEmpresaTemplate(BNE.BLL.DTO.AllInFilial load, BNE.BLL.UsuarioFilialPerfil usuPerf)
        {
            var obj = new AllInEmpresaTemplateModel();
            var pf = usuPerf.PessoaFisica;

            obj.E_PFCPF = pf.NumeroCPF;

            obj.E_PFDDDCel = pf.NumeroDDDCelular;
            obj.E_PFCel = pf.NumeroCelular;
            obj.E_PFDDDtel = load.If(a => a.UsuariosFilial.ContainsKey(usuPerf)).Return(a => a.UsuariosFilial[usuPerf].NumeroDDDComercial, pf.NumeroDDDTelefone);
            obj.E_PFTel = load.If(a => a.UsuariosFilial.ContainsKey(usuPerf)).Return(a => a.UsuariosFilial[usuPerf].NumeroComercial, pf.NumeroTelefone);

            obj.E_PFDtNasc = pf.DataNascimento;
            obj.E_PFDtUltAcc = load.If(a => load.Interacoes.ContainsKey(usuPerf)).Return(a => a.Interacoes[usuPerf].DataUltimaAtividade);
            obj.E_PFDtUltPesqCv =
                load.If(a => load.Pesquisas.ContainsKey(usuPerf)).Return(a => a.Pesquisas[usuPerf].DataCadastro);

            obj.E_PFEmail = load.If(a => a.UsuariosFilial.ContainsKey(usuPerf)).Return(a => a.UsuariosFilial[usuPerf].EmailComercial, pf.EmailPessoa);
            obj.E_PFFuncaoEx = load.If(a => a.UsuariosFilial.ContainsKey(usuPerf)).With(a => a.UsuariosFilial[usuPerf].DescricaoFuncao ?? a.UsuariosFilial[usuPerf].With(b => b.Funcao).Return(b => b.DescricaoFuncao));
            obj.E_PFId = pf.IdPessoaFisica;
            obj.E_PFInativo = pf.FlagInativo.GetValueOrDefault() ||
                              load.If(a => load.Interacoes.ContainsKey(usuPerf))
                                  .Return(a => a.Interacoes[usuPerf].FlagInativo) ||
                              usuPerf.FlagInativo;
            obj.E_PFMaster =
                usuPerf.With(a => a.Perfil)
                    .Return(a => a.IdPerfil == (int)BNE.BLL.Enumeradores.Perfil.AcessoEmpresaMaster);

            obj.E_PFNome = pf.NomeCompleto;
            obj.E_PFSexo = pf.Sexo.Return(a => a.IdSexo == (int)BNE.BLL.Enumeradores.Sexo.Masculino ? "m" : "f");

            obj.E_Bairro = load.Filial.With(a => a.Endereco).Return(a => a.DescricaoBairro);
            obj.E_CepNum = load.Filial.With(a => a.Endereco).With(a => a.NumeroCEP).Return(a => new int?(Convert.ToInt32(a))); /*.Where(char.IsNumber)*/
            obj.E_Cidade = load.Filial.With(a => a.Endereco).With(a => a.Cidade).Return(b => b.NomeCidade);
            obj.E_CodCnae = load.Filial.With(a => a.CNAEPrincipal).Return(a => a.CodigoCNAESubClasse);
            obj.E_DtCad = load.Filial.DataCadastro;

            obj.E_FPlano = load.Plano;
            obj.E_DtInPlano = load.PlanoInicio;
            obj.E_DtFinPlano = load.PlanoFim;

            obj.E_DtMod = load.Filial.DataAlteracao;
            obj.E_DtUltAcc = load.UltimaInteracao;
            obj.E_DtUltPesqCv = load.UltimaPesquisa;

            obj.E_FAceitaEstag = load.AceitaEstag;
            obj.E_FPubVaga = load.PublicaVaga;
            obj.E_FCursos = load.Filial.FlagOfereceCursos;

            obj.E_FilialId = load.Filial.IdFilial;
            obj.E_Inativo = load.Filial.FlagInativo;
            obj.E_NomeFan = load.Filial.NomeFantasia;
            obj.E_RazaoS = load.Filial.RazaoSocial;
            obj.E_Site = load.Filial.EnderecoSite;
            obj.E_Situacao = load.Filial.With(a => a.SituacaoFilial).Return(a => a.DescricaoSituacaoFilial);

            obj.E_UF =
                load.Filial.With(a => a.Endereco).With(a => a.Cidade).With(a => a.Estado).Return(a => a.SiglaEstado);

            var lastVaga = load.With(a => a.UltimaVagaDados);
            obj.E_VGAtrib = lastVaga.Return(a => a.DescricaoAtribuicoes);
            obj.E_VGCid = lastVaga.With(a => a.Cidade).Return(a => a.NomeCidade);
            obj.E_VGUF = lastVaga.With(a => a.Cidade).With(a => a.Estado).Return(a => a.SiglaEstado);
            obj.E_VGCont =
                load.With(a => a.UltimaVagaVinculos)
                    .Return(a => a.Select(b => b.DescricaoTipoVinculo).Aggregate((b, c) => b + " " + c));

            obj.E_VGDt = lastVaga.Return(a => a.DataAbertura);
            obj.E_VGEsc = lastVaga.With(a => a.Escolaridade).Return(a => a.DescricaoGeral);
            obj.E_VGFunc = lastVaga.With(a => a.Funcao).Return(b => b.DescricaoFuncao);
            obj.E_VGSexo = lastVaga.With(a => a.Sexo).Return(a => a.DescricaoSexo);

            return obj;
        }
    }
}
