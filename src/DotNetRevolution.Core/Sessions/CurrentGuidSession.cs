using DotNetRevolution.Core.Extension;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Sessions
{
    public class CurrentGuidSession : GuidSession, ICurrentSession
    {
        public void AddVariable(string key, object variable)
        {
            InternalVariables.Add(key, variable);
            Contract.Assume(Variables.PureContainsKey(key));
        }

        public void RemoveVariable(string key)
        {
            InternalVariables.Remove(key);
            Contract.Assume(!Variables.PureContainsKey(key));
        }
    }
}
