using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mondol.WebPlatform.Swagger
{
    /// <summary>
    /// 截止Swashbuckle 1.2.1版本控制器类的注释不会显示在类别栏里，此类可以解决
    /// </summary>
    public class FixTagsDocOperationFilter : IOperationFilter
    {
        private readonly XmlCommentManager _xmlCommentMgr;

        public FixTagsDocOperationFilter(XmlCommentManager xmlCommentMgr)
        {
            _xmlCommentMgr = xmlCommentMgr;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fullName = ((ControllerActionDescriptor)context.ApiDescription.ActionDescriptor).ControllerTypeInfo.FullName;
            var tagSummary = _xmlCommentMgr.GetTypeSummary(fullName);
            if (!string.IsNullOrEmpty(tagSummary))
            {
                operation.Tags[0].Name = $"{operation.Tags[0].Name} - {tagSummary}";
            }
        }
    }
}
