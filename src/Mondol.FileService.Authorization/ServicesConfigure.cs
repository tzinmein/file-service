// Copyright (c) Mondol. All rights reserved.
// 
// Author:  frank
// Email:   frank@mondol.info
// Created: 2017-01-24
// ---------------------------------------------
// Refactored by alan.yu @ 2021-07-08
// 

using Microsoft.Extensions.DependencyInjection;
using Mondol.FileService.Authorization.Codecs;
using Mondol.FileService.Authorization.Codecs.Impls;
using Mondol.FileService.Authorization.Options;
using System;

namespace Mondol.FileService.Authorization
{
    /// <summary>
    /// FileService's IServiceCollection extension
    /// </summary>
    public static class ServiceConfigure
    {
        /// <summary>
        /// Add FileService.Sdk.Server related services
        /// </summary>
        public static IServiceCollection AddAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<IOwnerTokenCodec, OwnerTokenCodec>()
                    .AddSingleton<IUrlDataCodec, UrlDataCompatibilityCodec>()
                    .AddSingleton<AppSecretSigner>();

            return services;
        }

        /// <summary>
        /// Add FileService.Sdk.Server related services
        /// </summary>
        public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<AuthOption> configure)
        {
            return services.AddAuthorization().Configure(configure);
        }
    }
}
