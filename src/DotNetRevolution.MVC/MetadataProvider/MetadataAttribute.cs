using System;
using System.Web.Mvc;

namespace DotNetRevolution.MVC.MetadataProvider
{
    public abstract class MetadataAttribute : Attribute
    {
        /// <summary>
        /// Method for processing custom attribute data.
        /// </summary>
        /// <param name="modelMetadata">A ModelMetadata instance.</param>
        public abstract void Process(ModelMetadata modelMetadata);
    }
}