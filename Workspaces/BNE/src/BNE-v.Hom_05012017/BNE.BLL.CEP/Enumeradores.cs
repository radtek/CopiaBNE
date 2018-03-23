using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace BNE.CEP
{
    [DataContract]
    public enum Colunas
    {
        [EnumMember, Column(Name = "Num_Cep")]
        Cep,
        [EnumMember, Column(Name = "Tip_Logradouro")]
        TipoLogradouro,
        [EnumMember, Column(Name = "Des_Endereco")]
        Logradouro,
        [EnumMember, Column(Name = "Des_Bairro")]
        Bairro,
        [EnumMember, Column(Name = "Des_Cidade")]
        Cidade,
        [EnumMember, Column(Name = "Des_UF")]
        Estado,
        [EnumMember, Column(Name = "Des_Complemento")]
        Complemento
    }

    [DataContract]
    public enum DirecaoOrdenacao
    {
        [EnumMember]
        ASC,
        [EnumMember]
        DESC
    }
}
