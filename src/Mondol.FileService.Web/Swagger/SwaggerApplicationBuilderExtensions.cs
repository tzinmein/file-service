// Copyright (c) Mondol. All rights reserved.
// 
// Author:  frank
// Email:   frank@mondol.info
// Created: 2016-12-12
// 
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;

namespace Mondol.WebPlatform.Swagger
{
    public static class SwaggerApplicationBuilderExtensions
    {
        public static void UseSwaggerService(this IApplicationBuilder app)
        {
            app.UseMiddleware<SwaggerBeforeHookMiddleware>();

            app.UseSwagger(opts =>
            {
                opts.RouteTemplate = "docs/apis/{documentName}/schema.json";
                opts.PreSerializeFilters.Add((sDoc, httpReq) =>
                {
                    var docName = ((OpenApiString)sDoc.Info.Extensions["docName"]).Value;
                    string prefix;
                    switch (docName)
                    {
                        case "client":
                            prefix = "/files"; //结尾不加/，因为文件上传接口结尾无/
                            break;
                        case "server":
                            prefix = "/sapi/";
                            break;
                        default:
                            throw new NotSupportedException("不支持的DocName：" + docName);
                    }

                    var dict = sDoc.Paths.Where(p => p.Key.StartsWith(prefix));
                    var newPaths = new OpenApiPaths();
                    foreach (var kv in dict)
                    {
                        newPaths.Add(kv.Key, kv.Value);
                    }
                    sDoc.Paths = newPaths;
                });
            });
            app.UseSwaggerUI(opts =>
            {
                opts.RoutePrefix = "docs/apis";
                opts.DocExpansion(DocExpansion.None);
                opts.DocumentTitle = "API文档";
                opts.EnableFilter();

                opts.SwaggerEndpoint("/docs/apis/client/schema.json", "客户端");
                opts.SwaggerEndpoint("/docs/apis/server/schema.json", "服务端");
            });
        }
    }
}
