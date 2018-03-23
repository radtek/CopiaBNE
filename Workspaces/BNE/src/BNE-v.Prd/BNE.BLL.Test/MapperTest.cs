using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using BNE.BLL.Custom.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.BLL.Test
{
    /// <summary>
    /// Summary description for MapperTest
    /// </summary>
    [TestClass]
    public class MapperTest
    {
        public MapperTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Salvar()
        {
            var objFilial = Filial.LoadObject(171388);

            objFilial.Endereco.CompleteObject();
            objFilial.Endereco.Cidade.CompleteObject();
            objFilial.CNAEPrincipal.CompleteObject();
            objFilial.NaturezaJuridica.CompleteObject();

            var lista = UsuarioFilialPerfil.ListarUsuariosFilial(objFilial);

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Assert.AreEqual(true, new PessoaJuridica().Salvar(objFilial, lista, trans));

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }


        }
    }
}
