
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApplication.Data.DbModels
{
    public class MonitorData
    {
        [Key]
        public int Id { get; set;}
        public int TalkTime { get; set; }
        public int AfterCallWorkTime { get; set; }
        public int Handled { get; set; }
        public int Offered { get; set; }
        public int HandledWithinSL { get; set; }
        public int QueueGroupID { get; set; }

        [ForeignKey("QueueGroupID")]
        public virtual QueueGroup QueueGroup { get; set; }

        public MonitorData()
        {
        }
    }
}
