using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BNE.BLL.NotificacoesVip.DTO
{
    public class ExtratoCurriculo
    {
        public string NomeCurriculo { get; set; }
        public int QtdVagasNoPerfil { get; set; }
        public int QtdVagasNoPerfilMes { get; set; }
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
                extrato.QtdVagasNoPerfilMes = Convert.ToInt32(vip["QtdVagasPerfil_mes"]);
                extrato.UrlVagasNoPerfil = "";

                List<ExtratoTemplate> templates = ExtratoTemplate.CriarLista(); //Cria lista padrão

                foreach(var template in templates)
                {
                    templates.Where(x => x.nome.Equals(template.nome)).FirstOrDefault().valorTotal = Convert.ToInt32(vip[template.nome]);
                    templates.Where(x => x.nome.Equals(template.nome)).FirstOrDefault().valorMes = Convert.ToInt32(vip[template.nome + "_mes"]);
                }

                extrato.ValoresExtrato = templates.Where(x => x.valorMes > 0 && x.valorTotal > 0 && x.valorTotal >= x.valorMes).ToList();
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
                    <table style='background-color:#ffffff;border-collapse:collapse;' width='100%' align='left' cellspacing='0' cellpadding='0' border='0'>
                        <tbody>
                            <tr>
                                <td valign='top'>
                                    <table style='border-collapse:collapse;' width='100%' align='right' cellspacing='0' cellpadding='0' border='0'>
                                        <tbody>
                                            <tr>
                                                <td valign='top' align='left'>
                                                    <!-- Vagas na sua área -->
                                                    <table cellpadding='24' cellspacing='0' border='0' width='100%' bgcolor='{corFundo}'>
                                                        <tbody>
                                                            <tr>
                                                                <td align='center' valign='middle'>
                                                                    <img src='http://mailing.bne.com.br/extratoVip/{imagem}' style='margin-bottom:8px;'>
                                                                    <p style='color:{corTextoMes};font-family:Arial,sans-serif;font-size:0.9em;font-weight:normal;margin-top:0;text-align:center;margin:0;padding:0;line-height:120%;'>
                                                                        {titulo} (últimos 30 dias)
                                                                    </p>
                                                                    <p style='color:{corTextoMes};font-family:Arial,sans-serif;font-size:1.8em;font-weight:lighter;margin-top:0;text-align:center;margin:0;padding:0;line-height:120%;'>
                                                                        {valorMes}
                                                                    </p>
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
                            <!-- Total VIP -->
                            <tr>
                                <td valign='top'>
                                    <table style='border-collapse:collapse;' width='100%' align='right' cellspacing='0' cellpadding='0' border='0'>
                                        <tbody>
                                            <tr>
                                                <td valign='top' align='left'>
                                                    <!-- Vagas na sua área -->
                                                    <table cellpadding='8' cellspacing='0' border='0' width='100%' bgcolor='#f5f5f5'>
                                                        <tbody>
                                                            <tr>
                                                                <td align='center' valign='middle'>
                                                                    <p style='color:{corTextoTotal};font-family:Arial,sans-serif;font-size:0.8em;font-weight:normal;margin-top:0;text-align:center;margin:0;padding:0;line-height:120%;'>
                                                                        {prefixoQtdTotal}
                                                                        <strong>
                                                                            {valorTotal} {descricaoTotal}
                                                                        </strong>
                                                                    </p>
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
                            <!— IMG spacer —>
                            <tr>
	                            <td style='margin:0;padding:0;'>
		                            <table width='100%' cellspacing='0' cellpadding='0' border='0'>
			                            <tr>
				                            <td>
					                            <img src='http://mailing.bne.com.br/extratoVip/spacer-white.png' style='width:100%;max-width:600px;border:0;min-height:auto;outline:none;text-decoration:none;vertical-align:bottom;display:block;'>
				                            </td>
			                            </tr>
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
                return "<tr>     <td>         <table cellspacing='0' cellpadding='0' border='0' bgcolor='#005BA1' width='100%'>             <tr>                 <td align='center'>                     <table>                         <tr>                             <td align='center' style='padding-top:24px !important;'> <img src='http://mailing.bne.com.br/extratoVip/ic-vagas.png' style='margin-bottom:8px;'> <p style='color:#ffffff;font-family:Arial,sans-serif;font-size:1.8em;font-weight:lighter;margin-top:0;text-align:center;margin:0;padding:0;line-height:120%;'><small>No total,</small> {QtdVagasNoPerfil} VAGAS <small>no seu perfil</small></p>                                 <p style='color:#ffffff;font-family:Arial,sans-serif;font-size:0.9em;font-weight:normal;margin-top:0;text-align:center;margin:0;padding:0;line-height:120%;'>  </p>                                 <p style='color:#ffffff;font-family:Arial,sans-serif;font-size:0.8em;font-weight:normal;text-align:center;margin:0;padding-top:25;line-height:120%;'>* {QtdVagasNoPerfilMes} vagas cadastradas nos últimos 30 dias</p>                             </td>                         </tr>                     </table>                 </td>             </tr>         </table>     </td> </tr> <!-- call to action --> <tr>     <td>         <a href='{UrlVagasNoPerfil}' title='Veja as vagas no seu perfil'>             <table cellspacing='0' cellpadding='24' border='0' bgcolor='#005BA1' width='100%'>                 <tr>                     <td align='center'>                         <table>                             <tr>                                 <td style='padding:12px 36px;background-color:#4caf50;border-radius:4px;' align='center'>                                     <p style='font-size:1em;color:#ffffff;line-height:100%;'><strong>VER VAGAS</strong> </p>                                 </td>                             </tr>                         </table>                     </td>                 </tr>             </table>         </a>     </td> </tr>";
            }
        }
        #endregion

    }

    public enum Grupo
    {
        azul = 1,
        cinza = 2
    }

    public class ExtratoTemplate
    {
        public string nome { get; set; }
        public Grupo grupo { get; set; }
        public string corFundo { get; set; }
        public string corTextoMes { get; set; }
        public string corTextoTotal { get; set; }
        public string titulo { get; set; }
        public string imagem { get; set; }
        public int valorMes { get; set; }
        public int valorTotal { get; set; }
        public string prefixoQtdTotal { get; set; }
        public string sufixoQtdTotal { get; set; }

        public ExtratoTemplate(string nome, Grupo grupo, string corFundo, string corTextoMes, string corTextoTotal, string titulo, string imagem, int valorMes, int valorTotal, string prefixoQtdMes, string sufixoQtdTotal = "")
        {
            this.nome = nome;
            this.grupo = grupo;
            this.corFundo = corFundo;
            this.corTextoMes = corTextoMes;
            this.corTextoTotal = corTextoTotal;
            this.titulo = titulo;
            this.imagem = imagem;
            this.valorMes = valorMes;
            this.valorTotal = valorTotal;
            this.prefixoQtdTotal = prefixoQtdMes;
            this.sufixoQtdTotal = sufixoQtdTotal;
        }

        public static List<ExtratoTemplate> CriarLista()
        {
            return new List<ExtratoTemplate>(){
                new ExtratoTemplate("QtdQuemMeviu", Grupo.azul, "#005BA1", "#ffffff", "#757575", "Total de visualizações no seu currículo", "ic-seu-cv-foi-visualizado.png", 0, 0, "Desde que se cadastrou no BNE: "),
                new ExtratoTemplate("QtdCandidaturas", Grupo.azul, "#006BAB", "#ffffff", "#757575", "Vagas candidatadas", "ic-vagas-candidatadas.png", 0, 0, "Desde que se cadastrou no BNE: "),
                new ExtratoTemplate("QtdVagasVisualizadas", Grupo.azul, "#0080B5", "#ffffff", "#757575", "Vagas visualizadas no seu perfil", "ic-vagas-visualizadas.png", 0, 0, "Desde que se cadastrou no BNE: "),
                new ExtratoTemplate("QtdBuscasRealizadas", Grupo.azul, "#0080B5", "#ffffff", "#757575", "Buscas realizadas", "ic-buscas-realizadas.png", 0, 0, "Desde que se cadastrou no BNE: "),
                new ExtratoTemplate("QtdLoginsRealizados", Grupo.azul, "#1693C3", "#ffffff", "#757575", "Logins efetuados", "ic-logins-efetuados.png", 0, 0, "Desde que se cadastrou no BNE: "),
                new ExtratoTemplate("QtdVezesApareciNabusca", Grupo.azul, "#1693C3", "#ffffff", "#757575", "Aparições nas buscas", "ic-apareceu-nas-buscas.png", 0, 0, "Desde que se cadastrou no BNE: "),
                new ExtratoTemplate("QtdEmpresasPesquisaramnoPerfil", Grupo.azul, "#4EA7CF", "#ffffff", "#757575", "Empresas que buscaram seu perfil", "ic-empresas-buscaram-perfil.png", 0, 0,  "No total, ", "empresas buscaram seu perfil"),

                new ExtratoTemplate("QtdVagasNaoVisualizada", Grupo.cinza, "#C8D6DA", "#585d5f", "#999", "Vagas no seu perfil não visualizadas", "ic-vagas-no-seu-perfil-exato-nv.png", 0, 0, " Desde que se cadastrou no BNE: "),
                new ExtratoTemplate("QtdBuscaPerfil", Grupo.cinza, "#C8D6DA", "#585d5f", "#999", "Buscas pelo seu perfil", "ic-buscas-pelo-seu-perfil.png", 0, 0, "O BNE já realizou "),
                new ExtratoTemplate("QtdVagasNaArea", Grupo.cinza, "#E7EEF0", "#585d5f", "#999", "Vagas na sua área", "ic-vagas-area.png", 0, 0, "O BNE já divulgou ")
            };
        }

    }

}

