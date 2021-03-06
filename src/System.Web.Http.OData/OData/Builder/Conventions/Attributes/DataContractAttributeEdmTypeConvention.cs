﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace System.Web.Http.OData.Builder.Conventions.Attributes
{
    /// <summary>
    /// Configures classes that have the <see cref="DataContractAttribute"/> to follow DataContract serialization/deserialization rules.
    /// </summary>
    public class DataContractAttributeEdmTypeConvention : AttributeEdmTypeConvention<IStructuralTypeConfiguration, DataContractAttribute>
    {
        public DataContractAttributeEdmTypeConvention()
            : base(allowMultiple: false)
        {
        }

        /// <summary>
        /// Removes properties that do not have the <see cref="DataMemberAttribute"/> attribute from the edm type.
        /// </summary>
        /// <param name="edmTypeConfiguration">The edm type to configure.</param>
        /// <param name="model">The edm model that this type belongs to.</param>
        /// <param name="attribute">The <see cref="DataContractAttribute"/> found on this type.</param>
        public override void Apply(IStructuralTypeConfiguration edmTypeConfiguration, ODataModelBuilder model, DataContractAttribute attribute)
        {
            if (edmTypeConfiguration == null)
            {
                throw Error.ArgumentNull("edmTypeConfiguration");
            }

            IEnumerable<PropertyConfiguration> allProperties = edmTypeConfiguration.Properties.ToArray();
            foreach (PropertyConfiguration property in allProperties)
            {
                if (!property.PropertyInfo.GetCustomAttributes(typeof(DataMemberAttribute), inherit: true).Any())
                {
                    edmTypeConfiguration.RemoveProperty(property.PropertyInfo);
                }
            }
        }
    }
}
