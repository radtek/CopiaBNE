using System;
using System.Collections.Generic;
using NovaArquitetura.Data.Infrastructure;
using NovaArquitetura.Data.Repositories;
using System.Linq;
using NovaArquitetura.Business.EL;

namespace NovaArquitetura.Business
{
    public class Aluno
    {
        private DatabaseFactory dbFactory = new DatabaseFactory();

        public IEnumerable<Entities.Aluno> GetAll()
        {
            var repositoryAluno = new AlunoRepository(dbFactory);

            return repositoryAluno.GetAll();
        }

        public Entities.Aluno GetById(int id)
        {
            var repositoryAluno = new AlunoRepository(dbFactory);

            Entities.Aluno objAluno = repositoryAluno.GetById(id);
            if (objAluno == null)
            {
                throw new RecordNotFoundException(typeof(Entities.Aluno));
            }

            return objAluno;
        }

        public List<Entities.Aluno> GetByPartialName(string partialName)
        {
            var repositoryAluno = new AlunoRepository(dbFactory);

            if (string.IsNullOrWhiteSpace(partialName))
                return repositoryAluno.GetAll().ToList();

            return repositoryAluno.GetMany(a => a.Nome.StartsWith(partialName)).ToList();
        }

        public List<Entities.Aluno> GetByDisciplina(int id)
        {
            var repositoryAluno = new AlunoRepository(dbFactory);

            return repositoryAluno.GetMany(a => a.Disciplinas.Count(d => d.Id == id) > 0).ToList();
        }

        public Entities.Aluno Create(Entities.Aluno objAluno)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            var repositoryAluno = new AlunoRepository(dbFactory);

            objAluno.DataCadastro = DateTime.Now;
            objAluno.DataAtualizacao = DateTime.Now;

            repositoryAluno.Add(objAluno);

            unitOfWork.SaveChanges();

            return objAluno;
        }

        public Entities.Aluno Update(Entities.Aluno objAluno)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            var repositoryAluno = new AlunoRepository(dbFactory);

            Entities.Aluno objAlunoBD = repositoryAluno.GetById(objAluno.Id);
            if (objAlunoBD == null)
                throw new EL.RecordNotFoundException(objAluno.GetType());

            objAlunoBD.Nome = objAluno.Nome;
            objAlunoBD.DataAtualizacao = DateTime.Now;

            repositoryAluno.Update(objAlunoBD);

            unitOfWork.SaveChanges();

            return objAlunoBD;
        }

        public bool Delete(Entities.Aluno objAluno)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            var repositoryAluno = new AlunoRepository(dbFactory);

            repositoryAluno.Delete(objAluno);

            unitOfWork.SaveChanges();

            return true;
        }

    }
}
