using System;
using System.Data.Entity.Spatial;

namespace BNE.Comum.Model
{
    public class EnderecoComum
    {

        public Int64 Id { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public int CEP { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DbGeography Geolocalizacao { get; set; }
        public DateTime? DataAtualizacaoGeolocalizacao { get; set; }

        public virtual Global.Model.Cidade Cidade { get; set; }
        public virtual Global.Model.Bairro Bairro { get; set; }

    }
}
