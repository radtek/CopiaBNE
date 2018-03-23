using BNE.PessoaFisica.Data.Repositories;
using System;
using System.Data;
using System.Linq;

namespace BNE.PessoaFisica.Domain
{
    public class Curriculo
    {
        private readonly ICurriculoRepository _curriculoRepository;

        public Curriculo(ICurriculoRepository curriculoRepository)
        {
            _curriculoRepository = curriculoRepository;
        }

        public Model.Curriculo GetById(int id)
        {
            return _curriculoRepository.GetById(id);
        }

        public Model.Curriculo GetByIdPessoaFisica(int idPessoaFisica)
        {
            return _curriculoRepository.GetMany(p => p.PessoaFisica.Id == idPessoaFisica).SingleOrDefault();
        }


        public Command.InformacaoCurriculo CarregarInformacoesCurriculo(int idVaga, decimal cpf)
        {
            Command.InformacaoCurriculo result = new Command.InformacaoCurriculo();

            DataTable dt = new Mapper.ToOld.PessoaFisica().CarregarInformacoesCurriculo(idVaga, cpf);

            foreach (DataRow row in dt.Rows)
            {
                result.idCurriculo = Convert.ToInt32(row[0]);
                result.EhVip = Convert.ToBoolean(row[1]);
                result.JaEnvioCvParaVaga = row[2].ToString() != "";
                result.EmpresaBloqueada = row[3].ToString() != "";
                result.EstaNaRegiaoBH = row[4].ToString() != "";
                result.TemExperienciaProfissional = row[5].ToString() != "";
                result.TemFormacao = row[6].ToString() != "";
            }
            //35988 - Salvar Visualização da vaga
            Mapper.ToOld.PessoaFisica.SalvarVisualizacaoVaga(idVaga, result.idCurriculo);
            return result;
        }

        /// <summary>
        /// Validar se o Curriculo está ativo
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <returns></returns>
        public bool CurriculoAtivo(int idPessoaFisica)
        {
            var curriculo = _curriculoRepository.GetMany(p => p.PessoaFisica.Id == idPessoaFisica).SingleOrDefault();

            if (curriculo != null)
            {
                return curriculo.SituacaoCurriculo.Id == (int)(Enumeradores.SituacaoCurriculo.Bloqueado) ? false : 
                    curriculo.SituacaoCurriculo.Id == (int)(Enumeradores.SituacaoCurriculo.Cancelado) ? false : true;
            }
            return false;
        }

        public bool SalvarMiniCurriculo(Model.Curriculo curriculo)
        {
            try
            {
                _curriculoRepository.Add(curriculo);

                return true;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public bool EstaNaRegiaoDeCampanhaBH(int idCidade)
        {
            return new Mapper.ToOld.PessoaFisica().EstaNaRegiaoDeCampanhaBH(idCidade);
        }
    }
}