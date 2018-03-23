using BNE.PessoaFisica.Domain.Repositories;

namespace BNE.PessoaFisica.Domain.Model
{
    public class TelefoneCelular : Telefone
    {
        private readonly ITelefoneCelularRepository _telefoneCelularRepository;

        public TelefoneCelular(ITelefoneCelularRepository telefoneCelularRepository)
        {
            _telefoneCelularRepository = telefoneCelularRepository;
        }

        private TelefoneCelular()
        {
        }

        public void Salvar(PessoaFisica objPessoaFisica, byte? DDD, decimal? numero)
        {
            if (DDD.HasValue && DDD == 0)
                DDD = null;

            if (numero.HasValue && numero == decimal.Zero)
                numero = null;

            TelefoneCelular objTelefoneCelular = null;

            //if (objPessoaFisica != null)
            //    objTelefoneCelular = GetByPessoaFisica(objPessoaFisica);

            if (objTelefoneCelular == null) //Se não existe telefone
            {
                if (DDD.HasValue && numero.HasValue) //O telefone foi informado
                {
                    objTelefoneCelular = new TelefoneCelular
                    {
                        DDD = DDD.Value,
                        Numero = numero.Value,
                        PessoaFisica = objPessoaFisica
                    };
                    _telefoneCelularRepository.Add(objTelefoneCelular);
                }
            }
            else
            {
                if (!DDD.HasValue && !numero.HasValue) //telefone não informado então está deletando a informação
                {
                    _telefoneCelularRepository.Delete(objTelefoneCelular);
                }
                else if ((DDD.HasValue && objTelefoneCelular.DDD != DDD) && (numero.HasValue && objTelefoneCelular.Numero != numero)) //Atualizando
                {
                    objTelefoneCelular.DDD = DDD.Value;
                    objTelefoneCelular.Numero = numero.Value;
                    _telefoneCelularRepository.Update(objTelefoneCelular);
                }
            }
        }

        public TelefoneCelular GetByPessoaFisica(PessoaFisica objPessoaFisica)
        {
            //TODO Está certo acessar o repositorio aqui?
            return _telefoneCelularRepository.Get(n => n.PessoaFisica.Id == objPessoaFisica.Id);
        }
    }
}