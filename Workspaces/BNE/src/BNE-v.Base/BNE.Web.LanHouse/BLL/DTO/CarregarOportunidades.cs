using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL.DTO
{
    public class CarregarOportunidades
    {
        public LAN_Oportunidade_Cidade LAN_Oportunidade_Cidade { get; set; }
        public bool JaCandidatado { get; set; }
    }
}