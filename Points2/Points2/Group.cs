using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points
{
    [Table("Group")]
    public class Group
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [MaxLength(100), Unique]
        public string GroupName { get; set; }
        [MaxLength(100)]
        public int AdminId { get; set; }
        public byte[] ImageSource { get; set; }


    }
}
