﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Web.Http.OData.Query
{
    /// <summary>
    /// This class describes the settings to use during query composition.
    /// </summary>
    public class ODataQuerySettings
    {
        private HandleNullPropagationOption _handleNullPropagationOption = HandleNullPropagationOption.Default;

        /// <summary>
        /// Instantiates a new instance of the <see cref="ODataQuerySettings"/> class
        /// and initializes the default settings.
        /// </summary>
        public ODataQuerySettings()
        {
            EnsureStableOrdering = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether query composition should
        /// alter the original query when necessary to ensure a stable sort order.
        /// </summary>
        /// <value>A <c>true</c> value indicates the original query should
        /// be modified when necessary to guarantee a stable sort order.
        /// A <c>false</c> value indicates the sort order can be considered
        /// stable without modifying the query.  Query providers that ensure
        /// a stable sort order should set this value to <c>false</c>.
        /// The default value is <c>true</c>.</value>
        public bool EnsureStableOrdering { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how null propagation should
        /// be handled during query composition. 
        /// </summary>
        /// <value>
        /// The default is <see cref="HandleNullPropagationOption.Default"/>.
        /// </value>
        public HandleNullPropagationOption HandleNullPropagation
        {
            get
            {
                return _handleNullPropagationOption;
            }
            set
            {
                HandleNullPropagationOptionHelper.Validate(value, "value");
                _handleNullPropagationOption = value;
            }
        }
    }
}
