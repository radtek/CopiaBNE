using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using BNE.BLL;

namespace BNE.Web.Services.Integracao
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Tanque" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Tanque.svc or Tanque.svc.cs at the Solution Explorer and start debugging.
    public class Tanque : ITanque
    {
        #region UsuarioTanque
        public DTO.OutUsuarioTanque UsuarioTanque(DTO.InUsuarioTanque curriculo)
        {
            DTO.OutUsuarioTanque retorno = new DTO.OutUsuarioTanque();
            DataTable dt = PessoaFisica.UsuarioTanque(curriculo.cpf);
            foreach (DataRow row in dt.Rows)
            {
                DTO.UsuarioDTO usuario = new DTO.UsuarioDTO();

                usuario.idfUsuarioFilialPerfil = Convert.ToInt32(row["Idf_Usuario_Filial_Perfil"]);
                usuario.nome = row["Nme_Pessoa"].ToString();
                usuario.nomeEmpresa = row["Raz_Social"].ToString();
                usuario.cnpj = row["Num_CNPJ"].ToString();
                usuario.telefone = row["Telefone"].ToString();

                retorno.listaUsuarioFilialPerfil.Add(usuario);

            }
            return retorno;

        }
        #endregion

        #region LiberaUsuarioTanque
        public DTO.OutLiberaUsuarioTanque liberaUsuarioTanque(DTO.InLiberaUsuarioTanque liberaUsuario, bool novoUsuario = true)
        {
            try
            {
                if (novoUsuario)
                {
                    //Verificar se para este usuário, já não existe um celular selecionador ativo
                    CelularSelecionador celsel = CelularSelecionador.RecuperarCelularSelecionadorByCodigo(liberaUsuario.usuarioFilialPerfil);

                    if (celsel != null)
                    {
                        //Caso exista, apenas reconfigura as datas
                        celsel.DataInicioUtilizacao = liberaUsuario.dataInicio;
                        celsel.DataFimUtilizacao = liberaUsuario.dtaFim;
                        celsel.Update();

                        return new DTO.OutLiberaUsuarioTanque()
                        {
                            status = true
                        };
                    }

                    CelularSelecionador.Criar(UsuarioFilialPerfil.LoadObject(liberaUsuario.usuarioFilialPerfil), liberaUsuario.dataInicio, liberaUsuario.dtaFim);
                }
                else
                {
                    //Carrega o objeto do usuário já existente (top 1 desc da data de cadastro - ultimo cadastro)
                    CelularSelecionador celSel = new CelularSelecionador();
                    celSel = CelularSelecionador.RecuperarCelularSelecionadorByCodigo(liberaUsuario.usuarioFilialPerfil);

                    celSel.UsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(liberaUsuario.usuarioFilialPerfil);
                    celSel.Celular = Celular.LoadObject(1);
                    celSel.DataInicioUtilizacao = liberaUsuario.dataInicio;
                    celSel.DataFimUtilizacao = liberaUsuario.dtaFim;
                    celSel.FlagUtilizaServicoTanque = true;
                    
                    celSel.Update();
                }

                return new DTO.OutLiberaUsuarioTanque()
                {
                    status = true
                };
            }
            catch (Exception e)
            {
                EL.GerenciadorException.GravarExcecao(e);
                return new DTO.OutLiberaUsuarioTanque()
                {
                    status = false
                };
            }

        }
        #endregion

        #region RetornaCelularSelecionador
        public DTO.CelularUsuarioDTO RetornaCelularSelecionador(int IdUsuarioFilialPerfil)
        {
            try
            {
                CelularSelecionador celSel = new CelularSelecionador();
                celSel = CelularSelecionador.RecuperarCelularSelecionadorTanque(IdUsuarioFilialPerfil);

                if (celSel != null)
                {
                    return new DTO.CelularUsuarioDTO()
                    {
                        IdUsuarioFilialPerfil = celSel.UsuarioFilialPerfil.IdUsuarioFilialPerfil,
                        IdCelularSelecionador = celSel.IdCelularSelecionador,
                        IdCelular = celSel.Celular.IdCelular,
                        DataInicio = celSel.DataInicioUtilizacao,
                        DataFim = celSel.DataFimUtilizacao
                    };
                }
                return null;
            }
            catch (Exception e)
            {
                EL.GerenciadorException.GravarExcecao(e);
                return null;
            }

        }
        #endregion
    }
}
