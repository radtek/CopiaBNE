using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public class Indicado
    {
        private int _id;
        public int Id { get { return this._id;  } }
        public Indicacao Indicacao { get; set; }
        public string Nome { get; set; }
        public string NumDDD { get; set; }
        public string NumeroCelular { get; set; }
        public string Email { get; set; }

        public Indicado(){}

        public override int GetHashCode()
        {
            int hash = 47;
            hash = hash * 23 + this.Nome.GetHashCode();
            hash = hash * 23 + this.NumDDD.GetHashCode();
            hash = hash * 23 + this.NumeroCelular.GetHashCode();
            return hash;
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (!(other is Indicacao))
                return false;

            if (this.GetHashCode() == other.GetHashCode())
                return true;
            else
                return false;
        }


    }
}
