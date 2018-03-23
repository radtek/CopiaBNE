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

            model.Cidade = load.NomeCidade;
            model.CPF = BNE.BLL.Custom.Helper.FormatarCPF(load.NumeroCPF);

            model.Dt_Cadastro = load.DataCadastroCurriculo;
            model.Dt_Atualizacao = load.DataAtualizacaoCurriculo;
            model.Dt_Modificacao = load.DataModificacaoCurriculo;

            model.Dt_Fim_Vip = load.UltimoPlanoVipFim;

            model.Dt_Nascimento = load.DataNascimento;
            model.Experiencia_Dta_Demissao = load.UltimaExperiencia.DataDemissao;
            model.Flag_Vip = load.VIP;

            model.Id_Cadastro = load.IdCurriculo;
            model.Nm_email = load.Email;
            model.Nome = load.NomeCompleto;

            //Ver para retornar do banco
            model.Qtd_Qm_Me_Viu = 0;
            model.Sexo = load.Sexo;
            model.Tipo_Curriculo = load.TipoCurriculo;

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
