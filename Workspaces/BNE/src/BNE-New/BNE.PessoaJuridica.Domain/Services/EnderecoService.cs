using System;
using BNE.PessoaJuridica.Domain.Repositories;

namespace BNE.PessoaJuridica.Domain.Services
{
    public class EnderecoService
    {

        private readonly IEnderecoRepository _enderecoRepository;

        private readonly Global.Domain.Cidade _cidade;
        private readonly Global.Domain.Bairro _bairro;

        public EnderecoService(IEnderecoRepository enderecoRepository, Global.Domain.Cidade cidade, Global.Domain.Bairro bairro)
        {
            _enderecoRepository = enderecoRepository;
            _cidade = cidade;
            _bairro = bairro;
        }

        public void Atualizar(Model.Endereco objEndereco)
        {
            _enderecoRepository.Update(objEndereco);
        }

        public void Adicionar(Model.Endereco objEndereco)
        {
            _enderecoRepository.Add(objEndereco);
        }

        public void Remover(Model.Endereco objEndereco)
        {
            _enderecoRepository.Delete(objEndereco);
        }

        public void SalvarEndereco(Model.PessoaJuridica objPessoaJuridica, Command.CadastroPessoaJuridica command, DateTime baseData)
        {
            if (TemParametrosParaCriarEndereco(command))
            {
                //quando a receita retorna o nome da cidade com - ex:MOGI-GUACU  não encontra.
                command.Cidade = command.Cidade.Replace("-", " ");

                var objCidade = _cidade.GetByNomeUF(command.Cidade);

                objPessoaJuridica.Endereco.Cidade = objCidade;
                objPessoaJuridica.Endereco.CEP = command.CEP;

                var objBairro = _bairro.GetByNome(objCidade, command.Bairro);
                if (objBairro != null)
                {
                    objPessoaJuridica.Endereco.Bairro = objBairro;
                    objPessoaJuridica.Endereco.DescricaoBairro = null;
                }
                else
                {
                    objPessoaJuridica.Endereco.Bairro = null;
                    objPessoaJuridica.Endereco.DescricaoBairro = command.Bairro;
                }

                objPessoaJuridica.Endereco.Complemento = command.Complemento;
                objPessoaJuridica.Endereco.Logradouro = command.Logradouro;
                objPessoaJuridica.Endereco.Numero = command.Numero;
                objPessoaJuridica.Endereco.DataAlteracao = baseData;

                if (objPessoaJuridica.Endereco.Id != 0)
                {
                    Atualizar(objPessoaJuridica.Endereco);
                }
                else
                {
                    objPessoaJuridica.Endereco.DataCadastro = baseData;
                    Adicionar(objPessoaJuridica.Endereco);
                }
            }
            else
            {
                Remover(objPessoaJuridica.Endereco);
            }
        }

        public bool TemParametrosParaCriarEndereco(Command.CadastroPessoaJuridica command)
        {
            return !string.IsNullOrWhiteSpace(command.Cidade) && command.CEP > 0 && !string.IsNullOrWhiteSpace(command.Logradouro) && !string.IsNullOrWhiteSpace(command.Numero);
        }

    }
}
