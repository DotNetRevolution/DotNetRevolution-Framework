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
    }
}