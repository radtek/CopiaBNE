using System;
using System.Text.RegularExpressions;
using BNE.Global.Data.Repositories;

namespace BNE.Global.Domain
{
    public class Sexo
    {

        private readonly ISexoRepository _sexoRepository;

        public Sexo(ISexoRepository sexoRepository)
        {
            _sexoRepository = sexoRepository;
        }

        public Model.Sexo GetByChar(string sexo)
        {
            return _sexoRepository.Get(n => n.Sigla == sexo);
        }

    }
}
