//-- Data: 13/09/2017 12:21
//-- Autor: Gieyson Stelmak
namespace BNE.BLL
{
    public partial class VagaRapidaContato // Tabela: BNE_Vaga_Rapida_Contato
    {
        public VagaRapidaContato(Vaga objVaga, string nomeContato, string ddd, string fone)
        {
            Vaga = objVaga;
            NomeContato = nomeContato;
            NumeroDDDContato = ddd;
            NumeroContato = fone;
        }
    }
}