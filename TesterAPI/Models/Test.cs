using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TesterAPI.Models
{
    public class Test
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public static DateTime Start_Date {get; set;}
        public static DateTime End_Date { get; set; }

        public TimeSpan Duration = Start_Date - End_Date;
    }
}
