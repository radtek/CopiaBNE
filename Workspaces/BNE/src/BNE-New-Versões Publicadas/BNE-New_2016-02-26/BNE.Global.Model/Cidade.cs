using System.Data.Entity.Spatial;

namespace BNE.Global.Model
{
    public class Cidade
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public DbGeography Localizacao { get; set; }

        public virtual Estado Estado { get; set; }

    }
}
