using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding.CodeContract;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandHandlerContract))]
    public interface ICommandHandler
    {
        bool Reusable { [Pure] get; }
        
        ICommandHandlingResult Handle(ICommandHandlerContext context);
                
        Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext context);
    }
}
