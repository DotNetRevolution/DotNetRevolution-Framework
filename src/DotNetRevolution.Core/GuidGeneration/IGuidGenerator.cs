using DotNetRevolution.Core.GuidGeneration.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.GuidGeneration
{
    [ContractClass(typeof(GuidGeneratorContract))]
    public interface IGuidGenerator
    {
        Guid Create();
    }
}
