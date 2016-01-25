using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;

namespace DotNetRevolution.MVC.MetadataProvider
{
    public class InterfaceMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(
            IEnumerable<Attribute> attributes,
            Type containerType,
            Func<object> modelAccessor,
            Type modelType,
            string propertyName)
        {
            Contract.Assume(((containerType == null || !(containerType.IsInterface)) || modelAccessor?.Target != null));

            // if containerType is an interface, get the actual type and the attributes of the current property on that type.
            if (containerType != null && containerType.IsInterface)
            {
                // get the target of the lambda expression (model => XXX)
                var target = modelAccessor.Target;

                // get container field
                var containerFieldInfo = target.GetType().GetField("container");
                Contract.Assume(containerFieldInfo != null);

                // get the model object (@model XXX)
                var container = containerFieldInfo.GetValue(target);

                // make sure the container is not null, if target.Container is null, container will be null
                if (container != null)
                {
                    // get the type of model object
                    containerType = container.GetType();

                    // get the type descriptor
                    var typeDescriptor = GetTypeDescriptor(containerType);
                    Contract.Assume(typeDescriptor != null);

                    // get the properties of the container type
                    var properties = typeDescriptor.GetProperties();
                    Contract.Assume(properties != null);

                    // get the property of the container
                    var propertyDescriptor = properties[propertyName];
                    Contract.Assume(propertyDescriptor?.Attributes != null);

                    // get attributes from property
                    attributes = FilterAttributes(containerType, propertyDescriptor, propertyDescriptor.Attributes.Cast<Attribute>());
                }
            }

            // call the base method to create metadata with the attributes
            return base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
        }
    }
}
