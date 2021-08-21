using MongoDB.Bson;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev.Data.Mongo.IO
{
    [Table("dev_File")]
    public partial class File : BaseEntity, IPrimaryKey<ObjectId>
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        /// <summary>
        /// Enum FileType
        /// </summary>
        public int FileType { get; set; }
        public int Size { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Keywords (Hastag) json olarak tutarız
        /// </summary>
        public string Keywords { get; set; }
    }
}
