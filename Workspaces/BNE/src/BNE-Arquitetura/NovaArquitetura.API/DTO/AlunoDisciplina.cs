using System.Collections.Generic;
using NovaArquitetura.Entities;

namespace NovaArquitetura.API.DTO
{
    public class AlunoDisciplina
    {
        public Aluno Aluno { get; set; }
        public List<int> Disciplinas { get; set; }
    }
}