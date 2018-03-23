using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BNE.BLL
{
    public class TipoUsuarioParceiro
    {
        private const string QRY_ALL =  @"SELECT Idf_Tipo_Usuario_Parceiro, Des_Tipo_Usuario_Parceiro FROM BNE.BNE_Tipo_Usuario_Parceiro;";
        private const string QRY_FIND = @"SELECT Des_Tipo_Usuario_Parceiro FROM BNE.BNE_Tipo_Usuario_Parceiro WHERE Idf_Tipo_Usuario_Parceiro = @Idf_Tipo_Usuario_Parceiro;";



        public int Id { get; set; }
        public string Descricao { get; set; }

        public static List<TipoUsuarioParceiro> loadObjects() 
        {
            List<TipoUsuarioParceiro> list = new List<TipoUsuarioParceiro>();
            var dr = DataAccessLayer.ExecuteReader(CommandType.Text, QRY_ALL, new List<System.Data.SqlClient.SqlParameter>());
            while (dr.Read())
            {
                TipoUsuarioParceiro tipo = new TipoUsuarioParceiro()
                {
                    Id = dr.GetByte(dr.GetOrdinal("Idf_Tipo_Usuario_Parceiro")),
                    Descricao = dr.GetString(dr.GetOrdinal("Des_Tipo_Usuario_Parceiro"))
                };
                list.Add(tipo);
            }
            return list;
        }

        public static TipoUsuarioParceiro loadObject(int ObjectID) 
        {
            TipoUsuarioParceiro obj = null;
            var parms = new List<SqlParameter> { new SqlParameter { ParameterName = "Idf_Tipo_Usuario_Parceiro", SqlDbType = SqlDbType.TinyInt, Value = ObjectID }  };
            var dr = DataAccessLayer.ExecuteReader(CommandType.Text, QRY_FIND, parms);
            if (dr.Read())
            {
                obj = new TipoUsuarioParceiro()
                {
                    Id = ObjectID,
                    Descricao = dr.GetString(dr.GetOrdinal("Des_Tipo_Usuario_Parceiro"))
                };
            }
            return obj;
        }


    }
}