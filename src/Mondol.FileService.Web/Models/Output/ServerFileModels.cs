using System;
using Mondol.FileService.Db.Entities;

namespace Mondol.FileService.Web.Models.Output.Server
{
    public class FileDetailData
    {
        public FileData File { get; set; }
        public FileOwnerTypeId Owner { get; set; }
    }

    public class FileData
    {
        public string Name { get; set; }
        public long Length { get; set; }
        public int ServerId { get; set; }
        public int MimeId { get; set; }
        public string Hash { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
