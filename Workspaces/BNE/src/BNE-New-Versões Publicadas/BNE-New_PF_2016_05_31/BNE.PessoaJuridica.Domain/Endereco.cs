using BNE.PessoaJuridica.Data.Repositories;

namespace BNE.PessoaJuridica.Domain
{
    public class Endereco
    {

        private readonly IEnderecoRepository _enderecoRepository;

        public Endereco(IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        public void Atualizar(Model.Endereco objEndereco)
        {
            _enderecoRepository.Update(objEndereco);
        }

        public void Adicionar(Model.Endereco objEndereco)
        {
            _enderecoRepository.Add(objEndereco);
        }

    }
}
