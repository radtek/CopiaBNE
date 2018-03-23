using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BNE.BLL;

namespace BNE.Console.Mapper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Do();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        #region Enviar
        public static void Do()
        {
            var objMapeamentoPessoaJuridica = new BNE.BLL.Custom.Mapper.PessoaJuridica();

            var filiais = Filial.ListarFiliais();

            //Parallel.ForEach(filiais, new ParallelOptions { MaxDegreeOfParallelism = 1 }, objFilial =>
            //Parallel.ForEach(filiais, objFilial =>
            foreach (var objFilial in filiais)
            {
                var watcher = new System.Diagnostics.Stopwatch();
                watcher.Start();
                objFilial.Endereco.CompleteObject();
                objFilial.Endereco.Cidade.CompleteObject();

                if (objFilial.CNAEPrincipal != null)
                    objFilial.CNAEPrincipal.CompleteObject();

                if (objFilial.NaturezaJuridica != null)
                    objFilial.NaturezaJuridica.CompleteObject();

                var lista = UsuarioFilialPerfil.ListarUsuariosFilial(objFilial);

                using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            objMapeamentoPessoaJuridica.Salvar(objFilial, lista, trans);

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            var targetInvocation = ex as System.Reflection.TargetInvocationException;
                            if (targetInvocation != null && targetInvocation.InnerException != null && targetInvocation.InnerException.Message.Contains("UsuarioPessoaJuridica_FuncaoSinonimo"))
                                EL.GerenciadorException.GravarExcecao(ex, "Falha ao importar a filial, problema no mapemento das funções dos usuários " + objFilial.IdFilial);
                            else
                                EL.GerenciadorException.GravarExcecao(ex, "Falha ao importar a filial " + objFilial.IdFilial);

                            trans.Rollback();
                        }
                    }
                }
                watcher.Stop();
                EL.GerenciadorException.GravarExcecao(new Exception("Filial " + objFilial.IdFilial + " importada em " + watcher.Elapsed));
            }
            //);

        }
        #endregion
    }
}
