//-- Data: 17/05/2013 10:15
//-- Autor: Francisco Ribas

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class HistoricoPublicacaoVaga // Tabela: BNE_Historico_Publicacao_Vaga
	{
        public static void RegistrarHistorico(int IdVaga, String Mensagem)
        {
            HistoricoPublicacaoVaga objHistoricoPublicacaoVaga = new HistoricoPublicacaoVaga();
            objHistoricoPublicacaoVaga.IdVaga = IdVaga;
            objHistoricoPublicacaoVaga.DescricaoHistorico = Mensagem;
            objHistoricoPublicacaoVaga.Save();
        }
	}
}