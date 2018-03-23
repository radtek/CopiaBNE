using System;
using BNE.Mensagem.Data.Repositories;

namespace BNE.Mensagem.Domain
{
    public class Usuario
    {

        private readonly IUsuarioRepository _usuarioRepository;

        public Usuario(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public Model.Usuario RecuperarUsuario(Model.Sistema objSistema, Guid guidUsuario)
        {
            var objusuario = ExisteUsuario(objSistema, guidUsuario);

            if (objusuario != null)
                return objusuario;

            return CriarUsuario(objSistema, guidUsuario);
        }

        private Model.Usuario ExisteUsuario(Model.Sistema objSistema, Guid guidUsuario)
        {
            return _usuarioRepository.Get(n => n.Sistema.Id == objSistema.Id && n.Guid == guidUsuario);
        }

        private Model.Usuario CriarUsuario(Model.Sistema objSistema, Guid guidUsuario)
        {
            var objUsuario = new Model.Usuario { Sistema = objSistema, Guid = guidUsuario };
            _usuarioRepository.Add(objUsuario);
            return objUsuario;
        }


    }
}
