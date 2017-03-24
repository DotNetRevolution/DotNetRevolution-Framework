using System;
using System.Reflection;

namespace DotNetRevolution.EventSourcing.Auditor.WebApi.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}