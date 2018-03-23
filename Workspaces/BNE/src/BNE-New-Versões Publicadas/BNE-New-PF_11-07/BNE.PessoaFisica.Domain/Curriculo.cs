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
                result.SaldoCandidatura = (row[7].ToString() != "" ? Convert.ToInt32(row[7]) : 0);
                result.DisseQueNaoTemExperiencia = row[8].ToString() != "";
                result.DataNaoTemExperiencia = row[9] != DBNull.Value ? Convert.ToDateTime(row[9]) : (DateTime?)null;
            }
            //35988 - Salvar Visualização da vaga
            if (result.idCurriculo > 0) //Garantindo que o cpf exista no BNE velho
            {
                Mapper.ToOld.PessoaFisica.SalvarVisualizacaoVaga(idVaga, result.idCurriculo);
            }
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
            _curriculoRepository.Add(curriculo);

            return true;
        }

        public bool EstaNaRegiaoDeCampanhaBH(int idCidade)
        {
            return new Mapper.ToOld.PessoaFisica().EstaNaRegiaoDeCampanhaBH(idCidade);
        }
    }
}