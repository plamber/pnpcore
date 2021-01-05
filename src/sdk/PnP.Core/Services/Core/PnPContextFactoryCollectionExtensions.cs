﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
#if !NETSTANDARD2_0
using System.Runtime.InteropServices;
#endif

namespace PnP.Core.Services
{
    /// <summary>
    /// Extension class for the IServiceCollection type to provide supporting methods for the PnPContextFactory service
    /// </summary>
    public static class PnPContextFactoryCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="PnPContextFactory"/> to the collection of services
        /// </summary>
        /// <param name="collection">Collection of loaded services</param>
        /// <returns>Collection of loaded services</returns>
        public static IServiceCollection AddPnPContextFactory(this IServiceCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            // Add a SharePoint Online Context Factory service instance
            return collection
                .AddHttpHandlers()
                .AddHttpClients()
                .AddPnPServices();
        }

        /// <summary>
        /// Adds the <see cref="PnPContextFactory"/> to the collection of services with options
        /// </summary>
        /// <param name="collection">Collection of loaded services</param>
        /// <param name="options"><see cref="PnPContextFactory"/> configuration options</param>
        /// <returns>Collection of loaded services</returns>
        public static IServiceCollection AddPnPContextFactory(this IServiceCollection collection, Action<PnPContextFactoryOptions> options)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            collection.Configure(options);

            // Add a PnP Context Factory service instance
            return collection
                .AddHttpHandlers()
                .AddHttpClients()
                .AddPnPServices();
        }

        private static IServiceCollection AddHttpHandlers(this IServiceCollection collection)
        {
            // Use transient for the DelegatingHandlers
            // https://stackoverflow.com/questions/53223411/httpclient-delegatinghandler-unexpected-life-cycle
            collection.AddTransient<SharePointRestRetryHandler, SharePointRestRetryHandler>();
            collection.AddTransient<MicrosoftGraphRetryHandler, MicrosoftGraphRetryHandler>();

            return collection;
        }

        private static IServiceCollection AddHttpClients(this IServiceCollection collection)
        {
#if !NETSTANDARD2_0
            if (RuntimeInformation.RuntimeIdentifier == "browser-wasm")
            {
                collection.AddHttpClient<SharePointRestClient>()
                    .AddHttpMessageHandler<SharePointRestRetryHandler>();
            }
            else
            {
                collection.AddHttpClient<SharePointRestClient>()
                    .AddHttpMessageHandler<SharePointRestRetryHandler>()
                    // We use cookies by adding them to the header which works great when used from Core framework,
                    // however when running the .NET Standard 2.0 version from .NET Framework we explicetely have to
                    // tell the http client to not use the default (empty) cookie container
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                    {
                        UseCookies = false
                    });
            }
#else
            collection.AddHttpClient<SharePointRestClient>()
                .AddHttpMessageHandler<SharePointRestRetryHandler>()
                // We use cookies by adding them to the header which works great when used from Core framework,
                // however when running the .NET Standard 2.0 version from .NET Framework we explicetely have to
                // tell the http client to not use the default (empty) cookie container
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                {
                    UseCookies = false
                });
#endif

            collection.AddHttpClient<MicrosoftGraphClient>()
                .AddHttpMessageHandler<MicrosoftGraphRetryHandler>();

            return collection;
        }

        private static IServiceCollection AddPnPServices(this IServiceCollection collection)
        {
            return collection
                   .AddTransient<IPnPContextFactory, PnPContextFactory>();
        }
    }
}
