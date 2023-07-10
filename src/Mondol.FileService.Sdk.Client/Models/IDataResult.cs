namespace Mondol.FileService.Models
{
    /// <summary>
    /// 带附加Data的操作结果接口定义
    /// </summary>
    public interface IDataResult<TData> : IResult where TData : class
    {
        TData Data { get; set; }
    }
}