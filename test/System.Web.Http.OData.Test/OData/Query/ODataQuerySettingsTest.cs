﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Builder.TestModels;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Query;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.TestCommon;

namespace System.Web.Http.OData.Query
{
    public class ODataQuerySettingsTest
    {
        [Fact]
        public void Ctor_Initializes_All_Properties()
        {
            // Arrange & Act
            ODataQuerySettings querySettings = new ODataQuerySettings();

            // Assert
            Assert.Equal(HandleNullPropagationOption.Default, querySettings.HandleNullPropagation);
            Assert.True(querySettings.EnsureStableOrdering);
        }

        [Fact]
        public void HandleNullPropagation_Property_RoundTrips()
        {
            Assert.Reflection.EnumProperty<ODataQuerySettings, HandleNullPropagationOption>(
                new ODataQuerySettings(),
                o => o.HandleNullPropagation,
                HandleNullPropagationOption.Default,
                HandleNullPropagationOption.Default - 1,
                HandleNullPropagationOption.True);
        }

        [Fact]
        public void EnsureStableOrdering_Property_RoundTrips()
        {
            Assert.Reflection.BooleanProperty<ODataQuerySettings>(
                new ODataQuerySettings(),
                o => o.EnsureStableOrdering,
                true);
        }
    }
}
