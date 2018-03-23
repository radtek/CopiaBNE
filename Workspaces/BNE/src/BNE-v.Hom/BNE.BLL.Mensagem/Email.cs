using System;
using System.Data;

namespace BNE.BLL.Mensagem
{
    public class Email
    {
        public string Anexo { get; set; }
        public string NomeAnexo { get; set; }
        public string Mensagem { get; set; }
        public string Assunto { get; set; }
        public string Key { get; set; }
        public string Para { get; set; }
        public string De { get; set; }
        public int Id { get; set; }

        internal void SetInstance(IDataReader dr)
        {
            Key = Convert.ToString(dr["key"]);
            Id = Convert.ToInt32(dr["id"]);
            De = Convert.ToString(dr["de"]);
            Para = Convert.ToString(dr["para"]);
            Assunto = Convert.ToString(dr["assunto"]);
            Mensagem = Convert.ToString(dr["mensagem"]);
            if ((dr["nomeanexo"] != DBNull.Value) && (dr["anexo"] != DBNull.Value))
            {
                NomeAnexo = Convert.ToString(dr["nomeanexo"]);
                Anexo = Convert.ToString(dr["anexo"]);
            }
        }

        public bool PossuiAnexo()
        {
            return !string.IsNullOrWhiteSpace(NomeAnexo) && !string.IsNullOrWhiteSpace(Anexo);
        }
    }
}