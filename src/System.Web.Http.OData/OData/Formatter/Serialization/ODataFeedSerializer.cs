﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Properties;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Atom;

namespace System.Web.Http.OData.Formatter.Serialization
{
    /// <summary>
    /// OData serializer for serializing a collection of <see cref="IEdmEntityType" />
    /// </summary>
    internal class ODataFeedSerializer : ODataEntrySerializer
    {
        private const string FeedNamespace = "http://schemas.datacontract.org/2004/07/";
        private const string SelfLinkRelation = "self";

        private IEdmCollectionTypeReference _edmCollectionType;

        public ODataFeedSerializer(IEdmCollectionTypeReference edmCollectionType, ODataSerializerProvider serializerProvider)
            : base(edmCollectionType, ODataPayloadKind.Feed, serializerProvider)
        {
            _edmCollectionType = edmCollectionType;
            if (!edmCollectionType.ElementType().IsEntity())
            {
                throw Error.NotSupported(SRResources.TypeMustBeEntityCollection, edmCollectionType.ElementType().FullName(), typeof(IEdmEntityType).Name);
            }
        }

        public override void WriteObject(object graph, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
        {
            if (messageWriter == null)
            {
                throw Error.ArgumentNull("messageWriter");
            }

            ODataWriter writer = messageWriter.CreateODataFeedWriter();
            WriteObjectInline(graph, writer, writeContext);
            writer.Flush();
        }

        public override void WriteObjectInline(object graph, ODataWriter writer, ODataSerializerContext writeContext)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull("writer");
            }

            if (writeContext == null)
            {
                throw Error.ArgumentNull("writeContext");
            }

            if (graph != null)
            {
                WriteFeed(graph, writer, writeContext);
            }
            else
            {
                throw new SerializationException(Error.Format(SRResources.CannotSerializerNull, ODataFormatterConstants.Feed));
            }
        }

        private void WriteFeed(object graph, ODataWriter writer, ODataSerializerContext writeContext)
        {
            ODataSerializer entrySerializer = SerializerProvider.GetEdmTypeSerializer(_edmCollectionType.ElementType());
            if (entrySerializer == null)
            {
                throw Error.NotSupported(SRResources.TypeCannotBeSerialized, _edmCollectionType.ElementType(), typeof(ODataMediaTypeFormatter).Name);
            }

            Contract.Assert(entrySerializer.ODataPayloadKind == ODataPayloadKind.Entry);

            IEnumerable enumerable = graph as IEnumerable; // Data to serialize
            if (enumerable != null)
            {
                ODataFeed feed = new ODataFeed();

                if (writeContext.EntitySet != null)
                {
                    IEntitySetLinkBuilder linkBuilder = SerializerProvider.EdmModel.GetEntitySetLinkBuilder(writeContext.EntitySet);
                    Uri feedSelfLink = linkBuilder.BuildFeedSelfLink(new FeedContext(writeContext.EntitySet, writeContext.UrlHelper, graph));
                    if (feedSelfLink != null)
                    {
                        feed.SetAnnotation(new AtomFeedMetadata() { SelfLink = new AtomLinkMetadata() { Relation = SelfLinkRelation, Href = feedSelfLink } });
                    }
                }

                // TODO: Bug 467590: remove the hardcoded feed id. Get support for it from the model builder ?
                feed.Id = FeedNamespace + _edmCollectionType.FullName();

                // If we have more OData format specific information apply it now.
                ODataResult odataFeedAnnotations = graph as ODataResult;
                if (odataFeedAnnotations != null)
                {
                    feed.Count = odataFeedAnnotations.Count;
                    feed.NextPageLink = odataFeedAnnotations.NextPageLink;
                }

                writer.WriteStart(feed);

                foreach (object entry in enumerable)
                {
                    entrySerializer.WriteObjectInline(entry, writer, writeContext);
                }

                writer.WriteEnd();
            }
        }
    }
}
