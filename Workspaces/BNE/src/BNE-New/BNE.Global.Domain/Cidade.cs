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

            #region Recuperando estado, se tiver
            const string pattern = @"([\w\s'-]+)[/]([a-zA-Z]{2})"; //Ex. Curitiba/Paraná
            var regex = new Regex(pattern);
            Match match = regex.Match(nomeCidadeUF.Trim());
            if (match.Success)
            {
                var nomeCidade = match.Groups[1].Value;
                var siglaEstado = match.Groups[2].Value;

                return _cidadeRepository.Get(n => n.Nome.ToLower() == nomeCidade.ToLower() && n.Estado.UF.ToLower() == siglaEstado.ToLower());
            }
            else
            {
                var nomeCidade = nomeCidadeUF;
                return _cidadeRepository.Get(n => n.Nome.ToLower() == nomeCidade.ToLower());
            }
            #endregion

        }

    }
}
