using System.ComponentModel.DataAnnotations.Schema;

namespace EleganceAdmin.Core.Entity
{
    public class BaseEntity : Furion.DatabaseAccessor.Entity
    {
        /// <summary>
        /// 创建人唯一标识
        /// </summary>
        [Column("created_uid")]
        public string CreatedUId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [Column("created_uname")]
        public string CreatedUName { get; set; }

        /// <summary>
        /// 最后修改人唯一标识
        /// </summary>
        [Column("updated_uid")]
        public string UpdatedUId { get; set; }

        /// <summary>
        /// 最后修改人名称
        /// </summary>
        [Column("updated_uname")]
        public string UpdatedUName { get; set; }
    }
}