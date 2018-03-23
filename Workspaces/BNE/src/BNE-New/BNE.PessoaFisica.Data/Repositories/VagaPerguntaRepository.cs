using System;
using System.Collections.Generic;
using System.Linq;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class VagaPerguntaRepository : IVagaPerguntaRepository
    {
        public IEnumerable<VagaPergunta> CarregarPergunta(int idVaga)
        {
            var lista = new Mapper.ToOld.PessoaFisica().GetPergunta(Convert.ToInt32(idVaga));
            return lista.Select(pergunta => new VagaPergunta
            {
                DescricaoVagaPergunta = pergunta.descricaoVagaPergunta,
                IdVagaPergunta = pergunta.idVagaPergunta,
                TipoResposta = pergunta.tipoResposta
            });
        }
    }
}