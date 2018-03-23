
using System;
using System.Data;
namespace BNE.Sistmars.BLL
{
    public partial class Pessoa
    {
        private int _idPessoa;
        private decimal _cpf;
        private string _nome;
        private string _cidade;
        private string _estado;
        private int _CEP;
        private string _telefone;
        private string _email;
        private DateTime _dataNascimento;


        #region Construtor

        public Pessoa()
        {

        }

        #endregion

        #region Propriedades


        #region IdPessoa
        /// <summary>
        /// Retorna o CPF da Pessoa
        /// </summary>
        public int IdPessoa
        {
            get
            {
                return this._idPessoa;
            }
        }

        #endregion

        #region CPF
        /// <summary>
        /// Retorna o CPF da Pessoa
        /// </summary>
        public decimal CPF
        {
            get
            {
                return this._cpf;
            }
        }

        #endregion

        #region Nome

        /// <summary>
        /// Retorna o Nome da Pessoa
        /// </summary>
        public string Nome
        {
            get
            {
                return this._nome;
            }
        }

        #endregion

        #region Cidade

        /// <summary>
        /// Retorna o Nome da Pessoa
        /// </summary>
        public string Cidade
        {
            get
            {
                return this._cidade;
            }
        }

        #endregion

        #region Estado

        /// <summary>
        /// Retorna o Estado
        /// </summary>
        public string Estado
        {
            get
            {
                return this._estado;
            }
        }

        #endregion

        #region CEP

        /// <summary>
        /// Retorna o CEP
        /// </summary>
        public int CEP
        {
            get
            {
                return this._CEP;
            }
        }

        #endregion

        #region Telefone

        /// <summary>
        /// Retorna o Telefone
        /// </summary>
        public string Telefone
        {
            get
            {
                return this._telefone;
            }
        }

        #endregion

        #region Email

        /// <summary>
        /// Retorna o Email
        /// </summary>
        public string Email
        {
            get
            {
                return this._email;
            }
        }

        #endregion

        #region DataNascimento

        /// <summary>
        /// Retorna o DataNascimento
        /// </summary>
        public DateTime DataNascimento
        {
            get
            {
                return this._dataNascimento;
            }
        }

        #endregion

        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objPessoaFisica">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        private static bool SetInstance(IDataReader dr, Pessoa objPessoa)
        {
            try
            {
                if (dr.Read())
                {
                    objPessoa._idPessoa = Convert.ToInt32(dr["PES_CodigoPessoa"]);
                    objPessoa._cpf = Convert.ToDecimal(dr["PES_CPF"]);
                    objPessoa._nome = Convert.ToString(dr["PES_Nome"]);
                    objPessoa._cidade = Convert.ToString(dr["PES_Cidade"]);
                    objPessoa._estado = Convert.ToString(dr["PES_Estado"]);
                    objPessoa._CEP = Convert.ToInt32(dr["PES_CEP"]);
                    objPessoa._telefone = Convert.ToString(dr["PES_Telefone"]);
                    objPessoa._email = Convert.ToString(dr["PES_Email"]);
                    objPessoa._dataNascimento = Convert.ToDateTime(dr["PES_DataNascimento"]);

                    if (dr["PES_CodigoPessoa"] != DBNull.Value)
                        objPessoa._idPessoa = Convert.ToInt32(dr["PES_CodigoPessoa"]);
                    if (dr["PES_CPF"] != DBNull.Value)
                        objPessoa._cpf = Convert.ToDecimal(dr["PES_CPF"]);
                    if (dr["PES_Nome"] != DBNull.Value)
                        objPessoa._nome = Convert.ToString(dr["PES_Nome"]);
                    if (dr["PES_Cidade"] != DBNull.Value)
                        objPessoa._cidade = Convert.ToString(dr["PES_Cidade"]);
                    if (dr["PES_Estado"] != DBNull.Value)
                        objPessoa._estado = Convert.ToString(dr["PES_Estado"]);
                    if (dr["PES_CEP"] != DBNull.Value)
                        objPessoa._CEP = Convert.ToInt32(dr["PES_CEP"]);
                    if (dr["PES_Telefone"] != DBNull.Value)
                        objPessoa._telefone = Convert.ToString(dr["PES_Telefone"]);
                    if (dr["PES_DataNascimento"] != DBNull.Value)
                        objPessoa._dataNascimento = Convert.ToDateTime(dr["PES_DataNascimento"]);
                    if (dr["PES_Email"] != DBNull.Value)
                        objPessoa._email = Convert.ToString(dr["PES_Email"]);
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion
    }
}
