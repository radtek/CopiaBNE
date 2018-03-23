using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BNE.BLL;

namespace Bne.Web.Services.API.Controllers
{
    public class BNEApiController : ApiController
    {
        internal HttpResponseMessage errorRequestPost(HttpStatusCode badRequest, string msgError)
        {
            return Request.CreateErrorResponse(badRequest, msgError);
        }

        internal UsuarioFilialPerfil Login(decimal CPF, DateTime dataNascimento)
        {
            PessoaFisica objPessoaFisica;
            if (PessoaFisica.CarregarPorCPF(CPF, out objPessoaFisica))
            {
                //Se a data informada for diferente do cadastro mostra mensagem para ir para o SOSRH.
                if (dataNascimento.ToString("yyyy-MM-dd") == objPessoaFisica.DataNascimento.ToString("yyyy-MM-dd"))
                {
                    //Busca qtos perfis esse usuário tem desbloquados													  
                    UsuarioFilialPerfil objUsuarioFilialPerfil;
                    if (UsuarioFilialPerfil.CarregarUsuarioEmpresaPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil))
                    {
                        int quantidadeEmpresa = UsuarioFilialPerfil.QuantidadeUsuarioEmpresa(objPessoaFisica.IdPessoaFisica);

                        if (quantidadeEmpresa != 0) //Se o usuário tiver empresa relacionada.
                        {
                            return objUsuarioFilialPerfil;
                        }
                    }
                }
            }

            return null;
        }
    }
}
