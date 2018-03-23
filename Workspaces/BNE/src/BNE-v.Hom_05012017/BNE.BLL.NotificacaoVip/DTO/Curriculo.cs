using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL.NotificacoesVip.DTO
{
    public class Curriculo
    {
        public int idCurriculo { get; set; }
        public int idPessoaFisica { get; set; }
        public int idUsuarioFilialPerfil { get; set; }
        public decimal numeroCPF { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public decimal? numeroCelular { get; set; }
        public string funcao { get; set; }
        public string cidade { get; set; }
        public string siglaEstado { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataNascimento { get; set; }
        public DateTime dataInicioPlano { get; set; }

        public static ConcurrentBag<Curriculo> RetortarListaVipNotificacao(DataTable dtVip)
        {
            try
            {
                ConcurrentBag<Curriculo> lstCurriculoVip = new ConcurrentBag<Curriculo>();
                Curriculo curriculoVip = new Curriculo();
                foreach (DataRow vip in dtVip.Rows)
                {
                    try
                    {
                        curriculoVip = new Curriculo();

                        curriculoVip.idCurriculo = Convert.ToInt32(vip["Idf_Curriculo"]);
                        curriculoVip.idPessoaFisica = Convert.ToInt32(vip["Idf_Pessoa_Fisica"]);
                        curriculoVip.numeroCPF = decimal.Parse(vip["Num_CPF"].ToString());
                        curriculoVip.nome = BNE.BLL.Custom.Helper.RetornarPrimeiroNome(vip["Nme_Pessoa"].ToString());
                        curriculoVip.email = vip["Eml_Pessoa"].ToString();
                        curriculoVip.dataCadastro = Convert.ToDateTime(vip["Dta_Cadastro"]);
                        curriculoVip.dataInicioPlano = Convert.ToDateTime(vip["Dta_Inicio_Plano"]);
                        curriculoVip.dataNascimento = Convert.ToDateTime(vip["Dta_Nascimento"]);
                        curriculoVip.idUsuarioFilialPerfil = Convert.ToInt32(vip["Idf_Usuario_Filial_Perfil"]);

                        if (vip["Num_DDD_Celular"] != DBNull.Value && vip["Num_Celular"] != DBNull.Value)
                            curriculoVip.numeroCelular = decimal.Parse(vip["Num_DDD_Celular"].ToString() + vip["Num_Celular"].ToString());

                        curriculoVip.funcao = vip["Des_Funcao"].ToString();
                        curriculoVip.siglaEstado = vip["Sig_Estado"].ToString();
                        curriculoVip.cidade = vip["Nme_Cidade"].ToString();

                        lstCurriculoVip.Add(curriculoVip);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "Falha na criação do objeto do CV para notificação. IdCurriculo: " + vip["Idf_Curriculo"].ToString());
                    }
                }

                return lstCurriculoVip;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha na criação da lista de curriculos VIP para notificação.");
                return null;
            }

            
        }
    }

    
}
