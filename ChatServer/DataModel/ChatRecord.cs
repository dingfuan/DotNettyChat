using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.DataModel
{
    public class ChatRecord
    {
        [Key, Autoincrement]
        public long Id { get; set; }
        public string Action { get; set; }
        public string Content { get; set; }
        public DateTime Datetime { get; set; }
    }
}
