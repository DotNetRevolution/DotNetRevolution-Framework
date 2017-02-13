using System;
using System.Threading.Tasks;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionWaiterContract))]
    public interface IProjectionWaiter
    {
        void Wait(ICommandHandlingResult result);
        void Wait(ICommandHandlingResult result, TimeSpan timeout);
        Task WaitAsync(ICommandHandlingResult result);
        Task WaitAsync(ICommandHandlingResult result, TimeSpan timeout);

        void Wait(TransactionIdentity transactionIdentity);
        void Wait(TransactionIdentity transactionIdentity, TimeSpan timeout);
        Task WaitAsync(TransactionIdentity transactionIdentity);
        Task WaitAsync(TransactionIdentity transactionIdentity, TimeSpan timeout);
    }
}