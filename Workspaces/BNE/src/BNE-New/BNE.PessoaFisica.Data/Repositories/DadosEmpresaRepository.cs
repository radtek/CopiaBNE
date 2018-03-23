using System;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class DadosEmpresaRepository : IDadosEmpresaRepository
    {
        public DadosEmpresa RecuperarDados(int? idCurriculo, int idVaga)
        {

            var obj =  new Mapper.ToOld.PessoaFisica().DadosDaEmpresa(idCurriculo,idVaga);
            var retorno = new DadosEmpresa();
            retorno.Bairro = obj.Bairro;
            retorno.Cidade = obj.Cidade;
            retorno.CurriculoVIP = obj.CurriculoVIP;
            retorno.DataCadastro = obj.DataCadastro;
            retorno.DesAreaBne = obj.DesAreaBne;
            retorno.MensagemEmpresaConfidencial = obj.MensagemEmpresaConfidencial;
            retorno.NomeEmpresa = obj.NomeEmpresa;
            retorno.NumeroCNPJ = obj.NumeroCNPJ;
            retorno.NumeroTelefone = obj.NumeroTelefone;
            retorno.QuantidadeCurriculosVisualizados = obj.QuantidadeCurriculosVisualizados;
            retorno.QuantidadeFuncionarios = obj.QuantidadeFuncionarios;
            retorno.QuantidadeVagasDivulgadas = obj.QuantidadeVagasDivulgadas;
            retorno.VagaConfidencial = obj.VagaConfidencial;
            retorno.VagaSine = obj.VagaSine;
            retorno.ValorPlanoVIP = obj.ValorPlanoVIP;

            return retorno;
            throw new NotImplementedException();
        }
    }
}
