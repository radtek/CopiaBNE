using System.Data.Entity.Spatial;

namespace BNE.Global.Model
{
    public class Bairro
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public DbGeography Localizacao { get; set; }

        public virtual Cidade Cidade { get; set; }

    }
}
