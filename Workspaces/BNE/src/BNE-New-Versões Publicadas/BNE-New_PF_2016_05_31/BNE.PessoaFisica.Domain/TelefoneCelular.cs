using BNE.PessoaFisica.Data.Repositories;
using System;

namespace BNE.PessoaFisica.Domain
{
    public class TelefoneCelular
    {
        private readonly ITelefoneCelularRepository _telefoneCelularRepository;

		public TelefoneCelular(ITelefoneCelularRepository telefoneCelularRepository)
		{
			_telefoneCelularRepository = telefoneCelularRepository;
		}

        public Model.TelefoneCelular GetByPessoaFisica(Model.PessoaFisica objPessoaFisica)
        {
            return _telefoneCelularRepository.Get(n => n.PessoaFisica.Id == objPessoaFisica.Id);
        }

        public void Atualizar(Model.TelefoneCelular objTelefoneCelular)
        {
            _telefoneCelularRepository.Update(objTelefoneCelular);
        }

        public void Adicionar(Model.TelefoneCelular objTelefoneCelular)
        {
            _telefoneCelularRepository.Add(objTelefoneCelular);
        }

        public void Excluir(Model.TelefoneCelular objTelefoneCelular)
        {
            _telefoneCelularRepository.Delete(objTelefoneCelular);
        }

        public void SalvarTelefone(Model.PessoaFisica objPessoaFisica, byte? DDD, decimal? numero)
        {
            Salvar(objPessoaFisica, DDD, numero);
        }

        private void Salvar(Model.PessoaFisica objPessoaFisica, byte? DDD, decimal? numero)
        {
            if (DDD.HasValue && DDD == 0)
                DDD = null;

            if (numero.HasValue && numero == Decimal.Zero)
                numero = null;

            Model.TelefoneCelular objTelefoneCelular = null;

            if (objPessoaFisica != null)
                objTelefoneCelular = GetByPessoaFisica(objPessoaFisica);

            if (objTelefoneCelular == null) //Se não existe telefone
            {
                if (DDD.HasValue && numero.HasValue) //O telefone foi informado
                {
                    objTelefoneCelular = new Model.TelefoneCelular
                    {
                        DDD = DDD.Value,
                        Numero = numero.Value,
                        PessoaFisica = objPessoaFisica                        
                    };
                    Adicionar(objTelefoneCelular);
                }
            }
            else
            {
                if (!DDD.HasValue && !numero.HasValue) //telefone não informado então está deletando a informação
                {
                    Excluir(objTelefoneCelular);
                }
                else if ((DDD.HasValue && objTelefoneCelular.DDD != DDD) && (numero.HasValue && objTelefoneCelular.Numero != numero))//Atualizando
                {
                    objTelefoneCelular.DDD = DDD.Value;
                    objTelefoneCelular.Numero = numero.Value;
                    Atualizar(objTelefoneCelular);
                }
            }
        }
		
    }
}
