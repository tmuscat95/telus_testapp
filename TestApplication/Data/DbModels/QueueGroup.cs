using System;
using System.ComponentModel.DataAnnotations;

namespace TestApplication.Data.DbModels
{
 public class QueueGroup
 {
    [Key]
    public int ID { get; set; }
    public string Name { get; set; }
    public int SLA_Percent { get; set; }
    public int SLA_Time { get; set; }

    public QueueGroup()
    {
        

    }
 }
}
