using NovaArquitetura.Business.EL;
using NovaArquitetura.Data.Infrastructure;
using NovaArquitetura.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaArquitetura.Business
{
    public class Disciplina
    {
        private DatabaseFactory dbFactory = new DatabaseFactory();

        public IEnumerable<Entities.Disciplina> GetAll()
        {
            var repositoryDisciplina = new DisciplinaRepository(dbFactory);

            return repositoryDisciplina.GetAll();
        }

        public Entities.Disciplina GetById(int id)
        {
            var repositoryDisciplina = new DisciplinaRepository(dbFactory);

            Entities.Disciplina objDisciplina = repositoryDisciplina.GetById(id);
            if (objDisciplina == null)
            {
                throw new RecordNotFoundException(typeof(Entities.Aluno));
            }

            return repositoryDisciplina.GetById(id);
        }

        public List<Entities.Disciplina> GetByAluno(int id)
        {
            var repositoryAluno = new AlunoRepository(dbFactory);
            Entities.Aluno objAluno = repositoryAluno.GetById(id);
            if (objAluno == null)
                throw new EL.RecordNotFoundException(typeof(Entities.Aluno));

            return objAluno.Disciplinas.ToList();
        }

        public Entities.Disciplina Create(Entities.Disciplina objDisciplina)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            var repositoryDisciplina = new DisciplinaRepository(dbFactory);

            objDisciplina.DataCadastro = DateTime.Now;
            objDisciplina.DataAtualizacao = DateTime.Now;

            repositoryDisciplina.Add(objDisciplina);

            unitOfWork.SaveChanges();

            return objDisciplina;
        }

        public Entities.Disciplina Update(Entities.Disciplina objDisciplina)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            var repositoryDisciplina = new DisciplinaRepository(dbFactory);

            Entities.Disciplina objDisciplinaBD = repositoryDisciplina.GetById(objDisciplina.Id);
            if (objDisciplinaBD == null)
                throw new EL.RecordNotFoundException(objDisciplina.GetType());

            objDisciplinaBD.Nome = objDisciplina.Nome;
            objDisciplinaBD.DataAtualizacao = DateTime.Now;

            unitOfWork.SaveChanges();

            return objDisciplinaBD;
        }

        public bool Delete(Entities.Disciplina objDisciplina)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            var repositoryDisciplina = new DisciplinaRepository(dbFactory);

            repositoryDisciplina.Delete(objDisciplina);

            unitOfWork.SaveChanges();

            return true;
        }

        public bool AddAluno(int id, int idAluno)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            var repositoryDisciplina = new DisciplinaRepository(dbFactory);
            var repositoryAluno = new AlunoRepository(dbFactory);

            Entities.Disciplina objDisciplina = repositoryDisciplina.GetById(id);
            if (objDisciplina == null)
                throw new RecordNotFoundException(typeof(Entities.Disciplina));

            Entities.Aluno objAluno = repositoryAluno.GetById(idAluno);
            if (objAluno == null)
                throw new RecordNotFoundException(typeof(Entities.Aluno));

            objDisciplina.Alunos.Add(objAluno);

            repositoryDisciplina.Update(objDisciplina);

            unitOfWork.SaveChanges();

            return true;
        }

        public bool RemoveAluno(int id, int idAluno)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            var repositoryDisciplina = new DisciplinaRepository(dbFactory);
            var repositoryAluno = new AlunoRepository(dbFactory);

            Entities.Disciplina objDisciplina = repositoryDisciplina.GetById(id);
            if (objDisciplina == null)
                throw new RecordNotFoundException(typeof(Entities.Disciplina));

            Entities.Aluno objAluno = objDisciplina.Alunos.FirstOrDefault(a => a.Id == idAluno);
            if (objAluno == null)
                throw new RecordNotFoundException(typeof(Entities.Aluno));

            objDisciplina.Alunos.Remove(objAluno);

            repositoryDisciplina.Update(objDisciplina);

            unitOfWork.SaveChanges();

            return true;
        }
    }
}
