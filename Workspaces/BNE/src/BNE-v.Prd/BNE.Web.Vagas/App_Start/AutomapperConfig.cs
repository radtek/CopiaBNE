using System;
using System.Data;
using AutoMapper;
using BNE.BLL.Custom;
using BNE.Web.Vagas.Models;

namespace BNE.Web.Vagas.App_Start
{
    public class AutomapperConfig
    {
        public static void RegisterMaps()
        {
            var map = Mapper.CreateMap<DataRow, Vaga>();
            map.ForAllMembers(opt => opt.Ignore());
            map.ForMember(vaga => vaga.NomeCidade, opt => opt.MapFrom(datarow => datarow["Nme_Cidade"].ToString()));
            map.ForMember(vaga => vaga.SiglaEstado, opt => opt.MapFrom(datarow => datarow["Sig_Estado"].ToString()));
            map.ForMember(vaga => vaga.Cidade, opt => opt.MapFrom(datarow => Helper.FormatarCidade(datarow["Nme_Cidade"].ToString(), datarow["Sig_Estado"].ToString())));
            map.ForMember(vaga => vaga.Codigo, opt => opt.MapFrom(datarow => datarow["Cod_Vaga"]));
            map.ForMember(vaga => vaga.Funcao, opt => opt.MapFrom(datarow => datarow["Des_Funcao"]));
            map.ForMember(vaga => vaga.SalarioInicial, opt => opt.MapFrom(datarow => datarow["Vlr_Salario_De"] != DBNull.Value ? datarow["Vlr_Salario_De"] : null));
            map.ForMember(vaga => vaga.SalarioFinal, opt => opt.MapFrom(datarow => datarow["Vlr_Salario_Para"] != DBNull.Value ? datarow["Vlr_Salario_Para"] : null));
            map.ForMember(vaga => vaga.QuantidadeVaga, opt => opt.MapFrom(datarow => Convert.ToInt32(datarow["Qtd_Vaga"])));
            map.ForMember(vaga => vaga.Atribuicao, opt => opt.MapFrom(datarow => datarow["Des_Atribuicoes"]));
            map.ForMember(vaga => vaga.Beneficio, opt => opt.MapFrom(datarow => datarow["Des_Beneficio"]));
            map.ForMember(vaga => vaga.Requisito, opt => opt.MapFrom(datarow => datarow["Des_Requisito"]));
            map.ForMember(vaga => vaga.Escolaridade, opt => opt.MapFrom(datarow => datarow["Des_Escolaridade"]));
            map.ForMember(vaga => vaga.DataAnuncio, opt => opt.MapFrom(datarow => Convert.IsDBNull(datarow["Dta_Abertura"]) ? default(DateTime) : Convert.ToDateTime(datarow["Dta_Abertura"])));
            map.ForMember(vaga => vaga.URL, opt => opt.MapFrom(datarow => datarow["Url_Vaga"]));
            map.ForMember(vaga => vaga.IdentificadorDeficiencia, opt => opt.MapFrom(datarow => datarow["Idf_Deficiencia"] != DBNull.Value ? datarow["Idf_Deficiencia"] : null));
            map.ForMember(vaga => vaga.DescricaoDeficiencia, opt => opt.MapFrom(datarow => datarow["Des_Deficiencia"]));
            map.ForMember(vaga => vaga.Disponibilidade, opt => opt.MapFrom(datarow => datarow["Des_Disponibilidade"]));
            map.ForMember(vaga => vaga.TipoVinculo, opt => opt.MapFrom(datarow => datarow["Des_Tipo_Vinculo"]));
            map.ForMember(vaga => vaga.IdentificadorOrigem, opt => opt.MapFrom(datarow => Convert.ToInt32(datarow["Idf_Origem"])));
            map.ForMember(vaga => vaga.TipoOrigem, opt => opt.MapFrom(datarow => Convert.ToInt32(datarow["idf_tipo_origem"])));
            map.ForMember(vaga => vaga.BNERecomenda, opt => opt.MapFrom(datarow => Convert.ToBoolean(datarow["Flg_BNE_Recomenda"])));
            map.ForMember(vaga => vaga.Candidatou, opt => opt.MapFrom(datarow => Convert.ToBoolean(datarow["Flg_Candidatou"])));
            map.ForMember(vaga => vaga.IndicadoPeloBNE, opt => opt.MapFrom(datarow => Convert.ToBoolean(datarow["Flg_Auto_Candidatura"])));
            map.ForMember(vaga => vaga.IdentificadorVaga, opt => opt.MapFrom(datarow => Convert.ToInt32(datarow["Idf_Vaga"])));
            map.ForMember(vaga => vaga.AreaBNE, opt => opt.MapFrom(datarow => datarow["Des_Area_BNE"]));
            map.ForMember(vaga => vaga.FlagVagaArquivada, opt => opt.MapFrom(dataRow => Convert.ToBoolean(dataRow["Flg_Vaga_Arquivada"])));
            map.ForMember(vaga => vaga.IdFilial, opt => opt.MapFrom(dataRow => Convert.ToInt32(dataRow["Idf_Filial"])));
            map.ForMember(vaga => vaga.Bairro, opt => opt.MapFrom(dataRow => Helper.AjustarString(dataRow["Nme_Bairro"].ToString())));
            map.ForMember(vaga => vaga.Curso, opt => opt.MapFrom(dataRow => dataRow["Des_Curso"].ToString()));

            var map2 = Mapper.CreateMap<BLL.Vaga, Vaga>();
            map2.ForAllMembers(opt => opt.Ignore());
            map2.ForMember(vaga => vaga.NomeCidade, opt => opt.MapFrom(vaga => vaga.Cidade.NomeCidade));
            map2.ForMember(vaga => vaga.SiglaEstado, opt => opt.MapFrom(vaga => vaga.Cidade.Estado.SiglaEstado));
            map2.ForMember(vaga => vaga.NomeEstado, opt => opt.MapFrom(vaga => vaga.Cidade.Estado.NomeEstado));
            map2.ForMember(vaga => vaga.Cidade, opt => opt.MapFrom(vaga => Helper.FormatarCidade(vaga.Cidade.NomeCidade, vaga.Cidade.Estado.SiglaEstado)));
            map2.ForMember(vaga => vaga.Codigo, opt => opt.MapFrom(vaga => vaga.CodigoVaga));
            map2.ForMember(vaga => vaga.Funcao, opt => opt.MapFrom(vaga => vaga.Funcao.DescricaoFuncao));
            map2.ForMember(vaga => vaga.SalarioInicial, opt => opt.MapFrom(vaga => vaga.ValorSalarioDe.HasValue ? vaga.ValorSalarioDe.Value : (decimal?)null));
            map2.ForMember(vaga => vaga.SalarioFinal, opt => opt.MapFrom(vaga => vaga.ValorSalarioPara.HasValue ? vaga.ValorSalarioPara.Value : (decimal?)null));
            map2.ForMember(vaga => vaga.QuantidadeVaga, opt => opt.MapFrom(vaga => vaga.QuantidadeVaga));
            map2.ForMember(vaga => vaga.Atribuicao, opt => opt.MapFrom(vaga => vaga.DescricaoAtribuicoes));
            map2.ForMember(vaga => vaga.Beneficio, opt => opt.MapFrom(vaga => vaga.DescricaoBeneficio));
            map2.ForMember(vaga => vaga.Requisito, opt => opt.MapFrom(vaga => vaga.DescricaoRequisito));
            map2.ForMember(vaga => vaga.Escolaridade, opt => opt.MapFrom(vaga => vaga.Escolaridade.DescricaoBNE));
            map2.ForMember(vaga => vaga.DataAnuncio, opt => opt.MapFrom(vaga => vaga.DataAbertura));
            map2.ForMember(vaga => vaga.IdentificadorDeficiencia, opt => opt.MapFrom(vaga => vaga.Deficiencia != null ? vaga.Deficiencia.IdDeficiencia : (int?)null));
            map2.ForMember(vaga => vaga.DescricaoDeficiencia, opt => opt.MapFrom(vaga => vaga.Deficiencia != null ? vaga.Deficiencia.DescricaoDeficiencia : string.Empty));
            map2.ForMember(vaga => vaga.IdentificadorOrigem, opt => opt.MapFrom(vaga => vaga.Origem.IdOrigem));
            map2.ForMember(vaga => vaga.TipoOrigem, opt => opt.MapFrom(vaga => vaga.Origem.TipoOrigem.IdTipoOrigem));
            map2.ForMember(vaga => vaga.BNERecomenda, opt => opt.MapFrom(vaga => vaga.FlagBNERecomenda));
            map2.ForMember(vaga => vaga.IdentificadorVaga, opt => opt.MapFrom(vaga => vaga.IdVaga));
            map2.ForMember(vaga => vaga.AreaBNE, opt => opt.MapFrom(vaga => vaga.Funcao.AreaBNE.DescricaoAreaBNE));
            map2.ForMember(vaga => vaga.FlagInativo, opt => opt.MapFrom(vaga => vaga.FlagInativo));
            map2.ForMember(vaga => vaga.FlagVagaArquivada, opt => opt.MapFrom(vaga => vaga.FlagVagaArquivada));



            var map3 = Mapper.CreateMap<BLL.ExperienciaProfissional, ExperienciaProfissional>();

            map3.ForMember(exp => exp.Identificador, opt => opt.MapFrom(exp => exp.IdExperienciaProfissional));
            map3.ForMember(exp => exp.RazSocial, opt => opt.MapFrom(exp => exp.RazaoSocial));
            map3.ForMember(exp => exp.DesAtividade, opt => opt.MapFrom(exp => exp.DescricaoAtividade));
            map3.ForMember(exp => exp.DtaCadastro, opt => opt.MapFrom(exp => exp.DataCadastro));
            map3.ForMember(exp => exp.IdentificadorVaga, opt => opt.MapFrom(exp => exp.IdExperienciaProfissional));
            map3.ForMember(exp => exp.IdPerguntaHistoricoExp, opt => opt.MapFrom(exp => exp.IdExperienciaProfissional));

            var map4 = Mapper.CreateMap<BLL.PessoaFisica, PerguntaEmail>();

            map4.ForMember(per => per.Identificador, opt => opt.MapFrom(per => per.IdPessoaFisica));
            map4.ForMember(per => per.Email, opt => opt.MapFrom(per => per.EmailPessoa));
            map4.ForMember(per => per.IdentificadorVaga, opt => opt.MapFrom(per => per.IdPessoaFisica));
            map4.ForMember(per => per.IdPerguntaHistorico, opt => opt.MapFrom(per => per.IdPessoaFisica));
            map4.ForMember(per => per.EmailAntigo, opt => opt.MapFrom(per => per.EmailPessoa));

            var map5 = Mapper.CreateMap<BLL.PessoaFisica, PerguntaCelular>();

            map5.ForMember(per => per.Identificador, opt => opt.MapFrom(per => per.IdPessoaFisica));
            map5.ForMember(per => per.NumeroDDDCelular, opt => opt.MapFrom(per => per.NumeroDDDCelular));
            map5.ForMember(per => per.NumeroDDDCelularAntigo, opt => opt.MapFrom(per => per.NumeroDDDCelular));
            map5.ForMember(per => per.NumeroCelular, opt => opt.MapFrom(per => per.NumeroCelular));
            map5.ForMember(per => per.IdentificadorVaga, opt => opt.MapFrom(per => per.IdPessoaFisica));
            map5.ForMember(per => per.IdPerguntaHistorico, opt => opt.MapFrom(per => per.IdPessoaFisica));
            map5.ForMember(per => per.NumeroCelularAntigo, opt => opt.MapFrom(per => per.NumeroCelular));
            map5.ForMember(per => per.FlagCelularConfirmado, opt => opt.MapFrom(per => per.FlagCelularConfirmado));
            map5.ForMember(per => per.CodigoValidacao, opt => opt.MapFrom(per => per.NumeroCelular));


            var map6 = Mapper.CreateMap<BLL.DTO.Registro, Vaga>();
            map6.ForAllMembers(opt => opt.Ignore());
            map6.ForMember(vaga => vaga.NomeCidade, opt => opt.MapFrom(vagaapi => vagaapi.Cidade));
            map6.ForMember(vaga => vaga.SiglaEstado, opt => opt.MapFrom(vagaapi => vagaapi.SiglaEstado));
            map6.ForMember(vaga => vaga.Cidade, opt => opt.MapFrom(vagaapi => vagaapi.Cidade));// Helper.FormatarCidade(datarow["Nme_Cidade"].ToString(), datarow["Sig_Estado"].ToString())));
            map6.ForMember(vaga => vaga.Codigo, opt => opt.MapFrom(vagaapi => vagaapi.CodigoVaga));
            map6.ForMember(vaga => vaga.Funcao, opt => opt.MapFrom(vagaapi => vagaapi.Funcao));
            map6.ForMember(vaga => vaga.SalarioInicial, opt => opt.MapFrom(vagaapi => Convert.ToDecimal(vagaapi.SalarioMin) ));
            map6.ForMember(vaga => vaga.SalarioFinal, opt => opt.MapFrom(vagaapi => Convert.ToDecimal(vagaapi.SalarioMax)));
            map6.ForMember(vaga => vaga.QuantidadeVaga, opt => opt.MapFrom(vagaapi => vagaapi.Quantidade));
            map6.ForMember(vaga => vaga.Atribuicao, opt => opt.MapFrom(vagaapi => vagaapi.Atribuicoes != null ? vagaapi.Atribuicoes : string.Empty));
            map6.ForMember(vaga => vaga.Beneficio, opt => opt.MapFrom(vagaapi => vagaapi.Beneficios != null ? vagaapi.Beneficios : string.Empty));
            map6.ForMember(vaga => vaga.Requisito, opt => opt.MapFrom(vagaapi => vagaapi.Requisitos != null ? vagaapi.Requisitos : string.Empty));
            map6.ForMember(vaga => vaga.Escolaridade, opt => opt.MapFrom(vagaapi => vagaapi.Escolaridade));
            map6.ForMember(vaga => vaga.DataAnuncio, opt => opt.MapFrom(vagaapi => vagaapi.DataAnuncio));
            map6.ForMember(vaga => vaga.URL, opt => opt.MapFrom(vagaapi => vagaapi.Url));
           // map6.ForMember(vaga => vaga.IdentificadorDeficiencia, opt => opt.MapFrom(vagaapi => vagaapi.Deficiencia));
            map6.ForMember(vaga => vaga.DescricaoDeficiencia, opt => opt.MapFrom(vagaapi => vagaapi.Deficiencia));
            map6.ForMember(vaga => vaga.Disponibilidade, opt => opt.MapFrom(vagaapi => string.Join(" ", vagaapi.Disponibilidade)));
            map6.ForMember(vaga => vaga.TipoVinculo, opt => opt.MapFrom(vagaapi => vagaapi.TipoVinculo != null ? string.Join(" ", (vagaapi.TipoVinculo)) : "Efetivo"));//
            map6.ForMember(vaga => vaga.IdentificadorOrigem, opt => opt.MapFrom(vagaapi => vagaapi.TipoOrigem));
            map6.ForMember(vaga => vaga.TipoOrigem, opt => opt.MapFrom(vagaapi => vagaapi.TipoOrigem));
            map6.ForMember(vaga => vaga.BNERecomenda, opt => opt.MapFrom(vagaapi => vagaapi.BNERecomenda));
            //map6.ForMember(vaga => vaga.Candidatou, opt => opt.MapFrom(vagaapi => Convert.ToBoolean(datarow["Flg_Candidatou"])));
            map6.ForMember(vaga => vaga.IdentificadorVaga, opt => opt.MapFrom(vagaapi => vagaapi.Id));
            map6.ForMember(vaga => vaga.AreaBNE, opt => opt.MapFrom(vagaapi => vagaapi.Area));
            map6.ForMember(vaga => vaga.FlagVagaArquivada, opt => opt.MapFrom(vagaapi => vagaapi.Status.Equals("Arquivada") ? true : false));
            map6.ForMember(vaga => vaga.IdFilial, opt => opt.MapFrom(vagaapi => vagaapi.IdFilial));
            map6.ForMember(vaga => vaga.Bairro, opt => opt.MapFrom(vagaapi => !string.IsNullOrEmpty(vagaapi.Bairro) ? Helper.AjustarString(vagaapi.Bairro) : string.Empty));
            map6.ForMember(vaga => vaga.Curso, opt => opt.MapFrom(vagaapi => vagaapi.Cursos.Length > 0 ? vagaapi.Cursos[0] : null));
            map6.ForMember(vaga => vaga.Plano, opt => opt.MapFrom(vagaapi => vagaapi.Plano));
            map6.ForMember(vaga => vaga.Oferece_Cursos, opt => opt.MapFrom(vagaapi => vagaapi.Oferece_Cursos));


            Mapper.AssertConfigurationIsValid();

        }
    }
}