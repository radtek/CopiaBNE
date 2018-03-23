using System;
using BNE.PessoaJuridica.Data.Repositories;

namespace BNE.PessoaJuridica.Domain
{
    public class TelefoneCelular
    {

        private readonly ITelefoneCelularRepository _telefoneCelularRepository;

        public TelefoneCelular(ITelefoneCelularRepository telefoneCelularRepository)
        {
            _telefoneCelularRepository = telefoneCelularRepository;
        }

        public Model.TelefoneCelular GetByPessoaJuridica(Model.PessoaJuridica objPessoaJuridica)
        {
            return _telefoneCelularRepository.Get(n => n.PessoaJuridica.Id == objPessoaJuridica.Id);
        }

        public Model.TelefoneCelular GetByUsuario(Model.Usuario objUsuario)
        {
            return _telefoneCelularRepository.Get(n => n.Usuario.Id == objUsuario.Id);
        }

        public Model.TelefoneCelular GetByUsuarioPessoaJuridica(Model.UsuarioPessoaJuridica objUsuarioPessoaJuridica)
        {
            return _telefoneCelularRepository.Get(n => n.UsuarioPessoaJuridica.Usuario.Id == objUsuarioPessoaJuridica.Usuario.Id && n.UsuarioPessoaJuridica.PessoaJuridica.Id == objUsuarioPessoaJuridica.PessoaJuridica.Id);
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

        public void SalvarTelefone(Model.PessoaJuridica objPessoaJuridica, byte? DDD, decimal? numero)
        {
            Salvar(objPessoaJuridica, DDD, numero);
        }

        public void SalvarTelefone(Model.Usuario objUsuario, byte? DDD, decimal? numero)
        {
            Salvar(objUsuario, DDD, numero);
        }

        public void SalvarTelefone(Model.UsuarioPessoaJuridica objUsuarioPessoaJuridica, byte? DDD, decimal? numero)
        {
            Salvar(objUsuarioPessoaJuridica, DDD, numero);
        }

        private void Salvar(object objGeneric, byte? DDD, decimal? numero)
        {
            if (DDD.HasValue && DDD == 0)
                DDD = null;

            if (numero.HasValue && numero == Decimal.Zero)
                numero = null;

            Model.TelefoneCelular objTelefoneCelular = null;

            var objPessoaJuridica = objGeneric as Model.PessoaJuridica;
            if (objPessoaJuridica != null)
                objTelefoneCelular = GetByPessoaJuridica(objPessoaJuridica);

            var objUsuario = objGeneric as Model.Usuario;
            if (objUsuario != null)
                objTelefoneCelular = GetByUsuario(objUsuario);

            var objUsuarioPessoaJuridica = objGeneric as Model.UsuarioPessoaJuridica;
            if (objUsuarioPessoaJuridica != null)
                objTelefoneCelular = GetByUsuarioPessoaJuridica(objUsuarioPessoaJuridica);

            if (objTelefoneCelular == null) //Se não existe telefone
            {
                if (DDD.HasValue && numero.HasValue) //O telefone foi informado
                {
                    objTelefoneCelular = new Model.TelefoneCelular
                    {
                        DDD = DDD.Value,
                        Numero = numero.Value,
                        PessoaJuridica = objPessoaJuridica,
                        Usuario = objUsuario,
                        UsuarioPessoaJuridica = objUsuarioPessoaJuridica
                    };
                    Adicionar(objTelefoneCelular);
                }
            }
            else
            {
                if (!DDD.HasValue && !numero.HasValue) //Email não informado então está deletando a informação
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
