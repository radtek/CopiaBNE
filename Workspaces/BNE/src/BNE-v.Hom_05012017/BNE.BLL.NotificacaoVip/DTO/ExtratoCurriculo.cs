using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL.NotificacoesVip.DTO
{
    public class ExtratoCurriculo
    {
        public string NomeCurriculo { get; set; }
        public int QtdVagasNoPerfil { get; set; }
        public string UrlVagasNoPerfil { get; set; }
        public List<ExtratoTemplate> ValoresExtrato { get; set; }

        #region RetortarExtratoCurriculo
        public static ExtratoCurriculo RetortarExtratoCurriculo(DataTable dtVip, DTO.Curriculo curriculo)
        {
            try
            {
                ExtratoCurriculo extrato = new ExtratoCurriculo();
                DataRow vip = dtVip.Rows[0];
                extrato.NomeCurriculo = curriculo.nome;
                extrato.QtdVagasNoPerfil = Convert.ToInt32(vip["QtdVagasPerfil"]);
                extrato.UrlVagasNoPerfil = "";

                List<ExtratoTemplate> templates = ExtratoTemplate.CriarLista(); //Cria lista padrão

                foreach(var template in templates)
                {
                    templates.Where(x => x.nome.Equals(template.nome)).FirstOrDefault().valor = Convert.ToInt32(vip[template.nome]);
                }

                extrato.ValoresExtrato = templates.Where(x =>x.valor > 0).ToList();
                return extrato;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha na criação do objeto do extrato do curriculo.");
                return null;
            }
        }
        #endregion

        #region templateLinha

        public string templateLinha { 
            get { return @"
                            <tr>
                                <td valign='top' align='center'>
                                    <table width='100%' cellspacing='0' cellpadding='0' border='0'>
                                        <tbody>
                                            <tr>
                                                <td valign='top' align='center'>
                                                    <table style='border-collapse:collapse;max-width:600px!important;background-color:#ffffff;' width='100%' cellspacing='0' cellpadding='0' border='0'>
                                                        <tbody>
                                                            <tr>
                                                                <td valign='top' align='center'>
                                                                    {ColunaEsquerda}
                                                                    {ColunaDireita}
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
            ";
            }
        }
        #endregion

        #region templateColuna
        public string templateColuna
        {
            get
            {
                return @"
                    <table style='background-color:#ffffff;border-collapse:collapse;' width='{tamanho}' align='left' cellspacing='0' cellpadding='0' border='0'>
                        <tbody>
                            <tr>
                                <td valign='top'>
                                    <table style='border-collapse:collapse;' width='100%' align='right' cellspacing='0' cellpadding='0' border='0'>
                                        <tbody>
                                            <tr>
                                                <td valign='top' align='left' >
                                                    <table cellpadding='24' cellspacing='0' border='0' width='100%' bgcolor='{corFundo}'>   
                                                        <tr>
                                                            <td align='center' valign='middle' >
                                                                <img src='http://mailing.bne.com.br/extratoVip/{imagem}' style='margin-bottom:8px;'>
                                                                <p style='color:{corTexto};font-family:Arial,sans-serif;font-size:0.9em;font-weight:normal;margin-top:0;text-align:center;margin:0;padding:0;line-height:120%;'>
                                                                    {titulo}
                                                                </p>
                                                                <p style='color:{corTexto};font-family:Arial,sans-serif;font-size:1.8em;font-weight:lighter;margin-top:0;text-align:center;margin:0;padding:0;line-height:120%;'>
                                                                    {valor}
                                                                </p>
                                                            </td>
                                                        </tr>
                                                    </table>                                                       
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>   
                ";
            }
        }
        #endregion

        #region templateVerVagas
        public string templateVerVagas
        {
            get
            {
                return "<tr > 	<td> 		<table cellspacing='0' cellpadding='0' border='0' bgcolor='#005BA1' width='100%' > 			<tr> 				<td align='center'> 				   <table > 						<tr> 							<td  align='center' style='padding-top:24px !important;'> 								<img src='http://mailing.bne.com.br/extratoVip/ic-vagas.png' style='margin-bottom:8px;'> 								<p style='color:#ffffff;font-family:Arial,sans-serif;font-size:1.8em;font-weight:lighter;margin-top:0;text-align:center;margin:0;padding:0;line-height:120%;'> 									{QtdVagasNoPerfil} VAGAS 								</p> 								<p style='color:#ffffff;font-family:Arial,sans-serif;font-size:0.9em;font-weight:normal;margin-top:0;text-align:center;margin:0;padding:0;line-height:120%;'> 									no seu perfil 								</p> 								 							</td> 						</tr> 					</table>                                           				</td>                                   			</tr> 		</table> 	</td> </tr> <!-- call to action --> <tr > 	<td> 		<a href='{UrlVagasNoPerfil}' title='Veja as vagas no seu perfil'> 		<table cellspacing='0' cellpadding='24' border='0' bgcolor='#005BA1' width='100%' > 			<tr> 				<td align='center'> 					<table> 						<tr> 							<td style='padding:12px 36px;background-color:#4caf50;border-radius:4px;' align='center'> 								<p style='font-size:1em;color:#ffffff;line-height:100%;'><strong>VER VAGAS</strong></p> 							</td> 						</tr> 					</table>                                            				</td>                                   			</tr> 		</table> 		</a> 	</td> </tr> ";
            }
        }
        #endregion

    }

    public class ExtratoTemplate
    {
        public string nome { get; set; }
        public string corFundo { get; set; }
        public string corTexto { get; set; }
        public string titulo { get; set; }
        public string imagem { get; set; }
        public int valor { get; set; }
        public int tamanho { get; set; }

        public ExtratoTemplate(string nome, string corFundo, string corTexto, string titulo, string imagem, int valor, int tamanho)
        {
            this.nome = nome;
            this.corFundo = corFundo;
            this.corTexto = corTexto;
            this.titulo = titulo;
            this.imagem = imagem;
            this.valor = valor;
            this.tamanho = tamanho;
        }

        public static List<ExtratoTemplate> CriarLista()
        {
            return new List<ExtratoTemplate>(){
                new ExtratoTemplate("QtdQuemMeviu", "#005BA1", "#ffffff", "Visualizações no seu currículo", "ic-seu-cv-foi-visualizado.png", 0, 300),
                new ExtratoTemplate("QtdVagasRecebidasJornal", "#006BAB", "#ffffff", "Vagas recebidas no Jornal de Vagas", "ic-jornal-de-vagas.png", 0, 300),
                new ExtratoTemplate("QtdCandidaturas", "#006BAB", "#ffffff", "Vagas candidatadas", "ic-vagas-candidatadas.png", 0, 300),
                new ExtratoTemplate("QtdVagasVisualizadas", "#0080B5", "#ffffff", "Vagas visualizadas", "ic-vagas-visualizadas.png", 0, 300),
                new ExtratoTemplate("QtdBuscasRealizadas", "#0080B5", "#ffffff", "Buscas realizadas", "ic-buscas-realizadas.png", 0, 300),
                new ExtratoTemplate("QtdLoginsRealizados", "#1693C3", "#ffffff", "Logins efetuados", "ic-logins-efetuados.png", 0, 300),
                new ExtratoTemplate("QtdVezesApareciNabusca", "#1693C3", "#ffffff", "Apareceu nas buscas", "ic-apareceu-nas-buscas.png", 0, 300),
                new ExtratoTemplate("QtdEmpresasPesquisaramnoPerfil", "#4EA7CF", "#ffffff", "Empresas que buscaram seu perfil", "ic-empresas-buscaram-perfil.png", 0, 300),

                new ExtratoTemplate("QtdVagasNaoVisualizada", "#C8D6DA", "#585d5f", "Vagas não visualizadas", "ic-vagas-no-seu-perfil-exato-nv.png", 0, 300),
                new ExtratoTemplate("QtdBuscaPerfil", "#C8D6DA", "#585d5f", "Buscas pelo seu perfil", "ic-buscas-pelo-seu-perfil.png", 0, 300),
                new ExtratoTemplate("QtdSmsRecebidos", "#DEE7EA", "#585d5f", "SMS recebidos", "ic-sms-recebidos.png", 0, 300),
                new ExtratoTemplate("QtdVagasNaCidadeERegiao", "#DEE7EA", "#585d5f", "Vagas na sua cidade e região", "ic-vagas-cidade-regiao.png", 0, 300),
                new ExtratoTemplate("QtdVagasNaArea", "#E7EEF0", "#585d5f", "Vagas na sua área", "ic-vagas-area.png", 0, 300)
            };
        }

    }

}

