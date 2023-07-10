// Copyright (c) Mondol. All rights reserved.
// 
// Author:  frank
// Email:   frank@mondol.info
// Created: 2016-12-12
// 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using System.Text;

namespace Mondol.WebPlatform.Swagger
{
    public static class SwaggerServiceCollectionExtensions
    {
        private static readonly DefaultBehaviorSetup DefaultBehaviorSetup = new DefaultBehaviorSetup();
        private static readonly SchemeIdGenerator SchemeIdGen = new SchemeIdGenerator();

        public static void AddSwaggerService(this IServiceCollection services, string appBasePath)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();

                var clientDocDesc = ReadAllText("Swagger/DocDesc.txt");
                var serverDocDesc = clientDocDesc + "\r\n\r\n6. 本文档中所有接口均采用IP白名单授权";
                options.SwaggerDoc("client", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "文件服务客户端API接口文档",
                    Description = clientDocDesc,
                    Contact = new OpenApiContact()
                    {
                        Name = "Frank",
                        Email = "frank@mondol.info"
                    },
                    Extensions =
                    {
                        ["docName"] = new OpenApiString("client")
                    }
                });
                options.SwaggerDoc("server", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "文件服务服务端API接口文档",
                    Description = serverDocDesc,
                    Contact = new OpenApiContact()
                    {
                        Name = "Frank",
                        Email = "frank@mondol.info"
                    },
                    Extensions =
                    {
                        ["docName"] = new OpenApiString("server")
                    }
                });

                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (apiDesc.GroupName == null || apiDesc.GroupName == docName)
                    {
                        DefaultBehaviorSetup.Apply(apiDesc);
                        return true;
                    }
                    return false;
                });
                options.DescribeAllParametersInCamelCase();
                options.CustomSchemaIds(SchemeIdGen.SchemaIdSelector);

                options.OperationFilter<FixTagsDocOperationFilter>();
                options.DocumentFilter<FixRequiredOperationFilter>();
                options.OperationFilter<FixRequiredOperationFilter>();
                options.OperationFilter<AppendApiNameOperationFilter>();

                //此段代码用于查找Swagger.XmlCommentsOperationFilter报NullReferenceException的产生源
                //产生原因为在Action里声明了URL变量，但Action参数里未声明
                //options.TagActionsBy(ad =>
                //{
                //    var mmIsNull = ad.ParameterDescriptions.Any(p => p.ModelMetadata == null);
                //    if (mmIsNull)
                //        Debug.WriteLine("Bad Action: " + ad.ActionDescriptor.DisplayName);
                //    return "BadActionTest";
                //});

                options.IncludeXmlComments(Path.Combine(appBasePath, "Mondol.FileService.Web.xml"));
            });

            services.AddSingleton<XmlCommentManager>();
        }

        private static string ReadAllText(string path)
        {
            var asm = typeof(SwaggerServiceCollectionExtensions).GetTypeInfo().Assembly;
            var fileProvider = new EmbeddedFileProvider(asm);
            using (var stream = new StreamReader(fileProvider.GetFileInfo(path).CreateReadStream(), Encoding.UTF8))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
