﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Web.Http.OData.Builder.Conventions
{
    /// <summary>
    /// Convention to process properties of <see cref="IStructuralTypeConfiguration"/>.
    /// </summary>
    public interface IEdmPropertyConvention : IConvention
    {
        /// <summary>
        /// Applies the convention.
        /// </summary>
        /// <param name="edmProperty">The property the convention is applied on.</param>
        /// <param name="structuralTypeConfiguration">The <see cref="IStructuralTypeConfiguration"/> the edmProperty belongs to.</param>
        void Apply(PropertyConfiguration edmProperty, IStructuralTypeConfiguration structuralTypeConfiguration);
    }
}
