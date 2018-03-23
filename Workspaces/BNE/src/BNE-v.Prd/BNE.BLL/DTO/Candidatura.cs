using BNE.BLL.Custom;

namespace BNE.BLL.DTO
{
    public class Candidatura
    {
        public string NomeFuncaoPretendida { get; private set; }
        public string NomeCidade { get; private set; }
        public string SiglaEstado { get; private set; }
        public string PrimeiroNome { get; private set; }
        public int Idade { get; private set; }
        public int IdCurriculo { get; private set; }
        public int IdVagaCandidato { get; private set; }

        public Candidatura(string funcaoPretendida, string cidade, string siglaEstado, string nome, int idade, int idCurriculo, int idVagaCandidato)
        {
            NomeFuncaoPretendida = funcaoPretendida;
            NomeCidade = cidade;
            SiglaEstado = siglaEstado;
            PrimeiroNome = Helper.RetornarPrimeiroNome(nome);
            Idade = idade;
            IdCurriculo = idCurriculo;
            IdVagaCandidato = idVagaCandidato;
        }
    }
}
