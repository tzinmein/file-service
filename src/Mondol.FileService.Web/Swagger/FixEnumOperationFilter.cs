using Microsoft.OpenApi.Models;
using Mondol.WebPlatform.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApplication1
{
    /// <summary>
    /// 截止Swashbuckle 1.0.0-rc3
    /// 修复 如果参数是枚举类型并且未写参数注释则使用枚举的注释
    /// 修复 枚举取值加上键/值说明
    /// </summary>
    public class FixEnumOperationFilter : IDocumentFilter, IOperationFilter
    {
        private readonly XmlCommentManager _xmlCommentMgr;

        public FixEnumOperationFilter(XmlCommentManager xmlCommentMgr)
        {
            _xmlCommentMgr = xmlCommentMgr;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var dict = new Dictionary<OpenApiSchema, object>();
            FixEnums(swaggerDoc.Components.Schemas, dict);
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var pd in context.ApiDescription.ParameterDescriptions)
            {
                OpenApiParameter op;
                Type enumType = null;
                if ((enumType = GetRealEnumType(pd.Type)) != null &&
                    (op = operation.Parameters.FirstOrDefault(p => p.Name.Equals(pd.Name, StringComparison.OrdinalIgnoreCase))) != null)
                {
                    if (string.IsNullOrWhiteSpace(op.Description))
                        op.Description = _xmlCommentMgr.GetTypeSummary(enumType.FullName);

                    var eVals = _xmlCommentMgr.GetEnumValuesSummary(enumType);
                    op.Description += "\r\n" + string.Join(" | ", eVals.Select(p => $"{p.Value} - {p.Key}"));
                    //var ops = (PartialSchema)op;
                    ////ops.Enum = eVals.Select(p => $"{p.Value} - {p.Key}" as object).ToList();
                }
            }
        }

        private Type GetRealEnumType(Type type)
        {
            if (type == null)
                return null;

            var tInfo = type.GetTypeInfo();
            if (tInfo.IsArray)
            {
                type = tInfo.GetElementType();
            }
            else if (tInfo.IsGenericType)
            {
                //Nullable<>, IEnumerable<>
                type = tInfo.GetGenericArguments().First();
            }

            tInfo = type.GetTypeInfo();
            return tInfo.IsEnum ? type : null;
        }

        private void FixEnums(IEnumerable<OpenApiSchema> schemas, Dictionary<OpenApiSchema, object> dictExclude)
        {
            foreach (var schema in schemas)
            {
                if (!dictExclude.ContainsKey(schema))
                {
                    dictExclude[schema] = null;

                    if (schema.Enum?.Any() ?? false)
                    {
                        var enumType = schema.Enum.First().GetType();
                        var eVals = _xmlCommentMgr.GetEnumValuesSummary(enumType);
                        schema.Description += "\r\n" + string.Join(" | ", eVals.Select(p => $"{p.Value} - {p.Key}"));
                    }

                    if (schema.Properties?.Values?.Any() ?? false)
                        FixEnums(schema.Properties.Values, dictExclude);
                }
            }
        }

        private void FixEnums(IDictionary<string, OpenApiSchema> schemaDict, Dictionary<OpenApiSchema, object> dictExclude)
        {
            foreach (var kv in schemaDict)
            {
                var typeName = kv.Key;
                var schema = kv.Value;

                if (!dictExclude.ContainsKey(schema))
                {
                    dictExclude[schema] = null;

                    if (schema.Enum?.Any() ?? false)
                    {
                        var type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == typeName && _xmlCommentMgr.GetTypeSummary(t.FullName) == schema.Description);
                        if (type != null)
                        {
                            var eVals = _xmlCommentMgr.GetEnumValuesSummary(type);
                            schema.Description += "\r\n" + string.Join(" | ", eVals.Select(p => $"{p.Value} - {p.Key}"));
                        }
                    }
                    else if (schema.Properties?.Values?.Any() ?? false)
                    {
                        FixEnums(schema.Properties.Values, dictExclude);
                    }
                }
            }
        }
    }
}
