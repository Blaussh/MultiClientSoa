using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using CarRental.Common;
using Core.Common.Contracts;
using Core.Common.Core;

namespace CarRental.Business.Managers
{
    public class ManagerBase
    {
        protected string _loginName;

        public ManagerBase()
        {
            OperationContext context = OperationContext.Current;
            if (context != null)
            {
                _loginName = context.IncomingMessageHeaders.GetHeader<string>("String", "System");
                if (_loginName.IndexOf(@"\") > 1) _loginName = string.Empty;
                if (!string.IsNullOrEmpty(_loginName))
                    _authorizationAccount = LoadAuthorizationValidationAccount(_loginName);
            }

            if (ObjectBase.Container != null)
                ObjectBase.Container.SatisfyImportsOnce(this);
        }

        protected Account _authorizationAccount = null;
        protected virtual Account LoadAuthorizationValidationAccount(string loginName)
        {
            return null;
        }

        protected T ExecuteFaultHandledOperation<T>(Func<T> codeToExecute)
        {
            try
            {
                return codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        protected void ExecuteFaultHandledOperation(Action codeToExecute)
        {
            try
            {
                codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        protected void ValidateAuthorization(IAccountOwnedEntity entity)
        {
            if (!Thread.CurrentPrincipal.IsInRole(Security.CarRentalAdminRole))
            {
                if (_loginName != string.Empty && entity.OwnerAccountId != _authorizationAccount.AccountId)
                {
                    AuthorizationValidationException ex = new AuthorizationValidationException("");
                    throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
                }
            }
        }
    }
}
