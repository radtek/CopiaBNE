using System;
using BNE.PessoaJuridica.Data.Repositories;

namespace BNE.PessoaJuridica.Domain
{
    public class TelefoneComercial
    {

        private readonly ITelefoneComercialRepository _telefoneComercialRepository;

        public TelefoneComercial(ITelefoneComercialRepository telefoneComercialRepository)
        {
            _telefoneComercialRepository = telefoneComercialRepository;
        }

        public Model.TelefoneComercial GetByPessoaJuridica(Model.PessoaJuridica objPessoaJuridica)
        {
            return _telefoneComercialRepository.Get(n => n.PessoaJuridica.Id == objPessoaJuridica.Id);
        }

        public Model.TelefoneComercial GetByUsuario(Model.Usuario objUsuario)
        {
            return _telefoneComercialRepository.Get(n => n.Usuario.Id == objUsuario.Id);
        }

        public Model.TelefoneComercial GetByUsuarioPessoaJuridica(Model.UsuarioPessoaJuridica objUsuarioPessoaJuridica)
        {
            return _telefoneComercialRepository.Get(n => n.UsuarioPessoaJuridica.Usuario.Id == objUsuarioPessoaJuridica.Usuario.Id && n.UsuarioPessoaJuridica.PessoaJuridica.Id == objUsuarioPessoaJuridica.PessoaJuridica.Id); // && n.UsuarioPessoaJuridica.Perfil.Id == objUsuarioPessoaJuridica.Perfil.Id);
        }

        public void Atualizar(Model.TelefoneComercial objTelefoneComercial)
        {
            _telefoneComercialRepository.Update(objTelefoneComercial);
        }

        public void Adicionar(Model.TelefoneComercial objTelefoneComercial)
        {
            _telefoneComercialRepository.Add(objTelefoneComercial);
        }

        public void Excluir(Model.TelefoneComercial objTelefoneComercial)
        {
            _telefoneComercialRepository.Delete(objTelefoneComercial);
        }

        public void SalvarTelefone(Model.PessoaJuridica objPessoaJuridica, byte? DDD, decimal? numero, decimal? ramal = null)
        {
            Salvar(objPessoaJuridica, DDD, numero, ramal);
        }

        public void SalvarTelefone(Model.Usuario objUsuario, byte? DDD, decimal? numero, decimal? ramal = null)
        {
            Salvar(objUsuario, DDD, numero, ramal);
        }

        public void SalvarTelefone(Model.UsuarioPessoaJuridica objUsuarioPessoaJuridica, byte? DDD, decimal? numero, decimal? ramal = null)
        {
            Salvar(objUsuarioPessoaJuridica, DDD, numero, ramal);
        }

        private void Salvar(object objGeneric, byte? DDD, decimal? numero, decimal? ramal)
        {
            if (DDD.HasValue && DDD == 0)
                DDD = null;

            if (numero.HasValue && numero == Decimal.Zero)
                numero = null;

            if (ramal.HasValue && ramal == Decimal.Zero)
                ramal = null;

            Model.TelefoneComercial objTelefoneComercial = null;

            var objPessoaJuridica = objGeneric as Model.PessoaJuridica;
            if (objPessoaJuridica != null)
                objTelefoneComercial = GetByPessoaJuridica(objPessoaJuridica);

            var objUsuario = objGeneric as Model.Usuario;
            if (objUsuario != null)
                objTelefoneComercial = GetByUsuario(objUsuario);

            var objUsuarioPessoaJuridica = objGeneric as Model.UsuarioPessoaJuridica;
            if (objUsuarioPessoaJuridica != null)
                objTelefoneComercial = GetByUsuarioPessoaJuridica(objUsuarioPessoaJuridica);

            if (objTelefoneComercial == null) //Se não existe telefone
            {
                if (DDD.HasValue && numero.HasValue) //O telefone foi informado
                {
                    objTelefoneComercial = new Model.TelefoneComercial
                    {
                        DDD = DDD.Value,
                        Numero = numero.Value,
                        Ramal = ramal,
                        PessoaJuridica = objPessoaJuridica,
                        Usuario = objUsuario,
                        UsuarioPessoaJuridica = objUsuarioPessoaJuridica
                    };
                    Adicionar(objTelefoneComercial);
                }
            }
            else
            {
                if (!DDD.HasValue && !numero.HasValue && !ramal.HasValue) //Telefone não informado então está deletando a informação
                {
                    Excluir(objTelefoneComercial);
                }
                else if (objTelefoneComercial.DDD != DDD || objTelefoneComercial.Numero != numero || objTelefoneComercial.Ramal != ramal)//Atualizando
                {
                    objTelefoneComercial.DDD = DDD.Value;
                    objTelefoneComercial.Numero = numero.Value;
                    objTelefoneComercial.Ramal = ramal;
                    Atualizar(objTelefoneComercial);
                }
            }
        }

    }
}
