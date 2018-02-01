using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using CarRental.Data.Contracts.Repository_Interfaces;
using Core.Common.Data;

namespace CarRental.Data.Data_Repositories
{
    [Export(typeof(IAccountRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountRepository : DataRepositoryBase<Account>,IAccountRepository
    {
        protected override Account AddEntity(CarRentalContext entityContext, Account entity)
        {
            return entityContext.AccountSet.Add(entity);
        }

        protected override Account UpdateEntity(CarRentalContext entityContext, Account entity)
        {
            return (entityContext.AccountSet.Where(e => e.AccountId == entity.AccountId)).FirstOrDefault();
        }

        protected override IEnumerable<Account> GetEntities(CarRentalContext entityContext)
        {
            return entityContext.AccountSet.Select(e => e);
        }

        protected override Account GetEntity(CarRentalContext entityContext, int id)
        {
            return entityContext.AccountSet.FirstOrDefault(e => e.AccountId == id);
        }

        public Account GetByLogin(string login)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                return entityContext.AccountSet.FirstOrDefault(e => e.LoginEmail == login);
            }
        }
    }
}
