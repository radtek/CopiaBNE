namespace BNE.Mapper.ToOld
{
    public class PesquisaVaga
    {
        public BLL.PesquisaVaga RecuperarPesquisaVaga(int idPesquisa)
        {
            return BLL.PesquisaVaga.LoadObject(idPesquisa);
        }
    }
}
