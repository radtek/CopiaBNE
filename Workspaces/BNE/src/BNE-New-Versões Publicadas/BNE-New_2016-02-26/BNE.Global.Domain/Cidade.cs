using System;
using System.Text.RegularExpressions;
using BNE.Global.Data.Repositories;

namespace BNE.Global.Domain
{
    public class Cidade
    {

        private readonly ICidadeRepository _cidadeRepository;

        public Cidade(ICidadeRepository cidadeRepository)
        {
            _cidadeRepository = cidadeRepository;
        }

        public Model.Cidade GetById(int id)
        {
            return _cidadeRepository.GetById(id);
        }

        public Model.Cidade GetByNomeUF(string nomeCidadeUF)
        {
            if (string.IsNullOrWhiteSpace(nomeCidadeUF))
                return null;

            string siglaEstado;
            var nomeCidade = siglaEstado = String.Empty;

            #region Recuperando estado, se tiver
            const string pattern = @"([\w\s'-]+)[/]([a-zA-Z]{2})"; //Ex. Curitiba/Paraná
            var regex = new Regex(pattern);
            Match match = regex.Match(nomeCidadeUF.Trim());
            if (match.Success)
            {
                nomeCidade = match.Groups[1].Value;
                siglaEstado = match.Groups[2].Value;
            }
            #endregion

            return _cidadeRepository.Get(n => n.Nome == nomeCidade && n.Estado.UF == siglaEstado);
        }

    }
}
