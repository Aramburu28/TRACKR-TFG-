using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points
{
    [Table("UserGroup")]
    public class UserGroup
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [MaxLength(100), Column("userid")]
        public int UserId { get; set; }
        [MaxLength(100), Column("groupid")]
        public int GroupId { get; set; }
        [MaxLength(100), DefaultValue(0), Column("points")]
        public int Points {  get; set; }


    }
}