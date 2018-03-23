using System;
using System.Collections.Generic;
using System.Linq;
using LanHouse.Business.EL;
using LanHouse.Entities.BNE.Repositories;
using LanHouse.Entities.BNE.Infrastructure;

namespace LanHouse.Business
{
    public class FaseCadastro
    {
        private DatabaseFactory dbFactory = new DatabaseFactory();

        public IEnumerable<Entities.BNE.LAN_Fase_Cadastro> GetAll()
        {
            var repositoryFaseCadastro = new FaseCadastroRepository(dbFactory);
            
            return repositoryFaseCadastro.GetAll();
        }

        public Entities.BNE.LAN_Fase_Cadastro GetById(int id)
        {
            var repositoryFaseCadastro = new FaseCadastroRepository(dbFactory);

            Entities.BNE.LAN_Fase_Cadastro objFaseCadastro = repositoryFaseCadastro.GetById(id);
            if(objFaseCadastro == null)
            {
                throw new RecordNotFoundException(typeof(Entities.BNE.LAN_Fase_Cadastro));
            }

            return objFaseCadastro;
        }

        public Entities.BNE.LAN_Fase_Cadastro Create(Entities.BNE.LAN_Fase_Cadastro objFaseCadastro)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            var repositoryFaseCadastro = new FaseCadastroRepository(dbFactory);

            objFaseCadastro.Dta_Cadastro = DateTime.Now.ToString();

            repositoryFaseCadastro.Add(objFaseCadastro);

            unitOfWork.SaveChanges();

            return objFaseCadastro;
        }

        public Entities.BNE.LAN_Fase_Cadastro Update(Entities.BNE.LAN_Fase_Cadastro objFaseCadastro)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            var repositoryFaseCadastro = new FaseCadastroRepository(dbFactory);

            Entities.BNE.LAN_Fase_Cadastro objFaseCadastroBD = repositoryFaseCadastro.GetById(objFaseCadastro.Idf_Fase_Cadastro);
            if (objFaseCadastroBD == null)
                throw new RecordNotFoundException(objFaseCadastro.GetType());

            objFaseCadastroBD.Des_Fase_Cadastro = objFaseCadastro.Des_Fase_Cadastro;

            repositoryFaseCadastro.Update(objFaseCadastroBD);

            unitOfWork.SaveChanges();

            return objFaseCadastroBD;
        }

        public bool Delete(Entities.BNE.LAN_Fase_Cadastro objFaseCadastro)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            var repositoryFaseCadastro = new FaseCadastroRepository(dbFactory);

            repositoryFaseCadastro.Delete(objFaseCadastro);

            unitOfWork.SaveChanges();
            return true;
        }
    }
}
