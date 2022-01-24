using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace crm.Models
{

    [Index(nameof(symbol), IsUnique = true)]
    public class Currency
    {
        public int id { get; set; }

        [StringLength(450)]
        [DisplayName("Symbol waluty")]
        
        public string symbol { get; set; }
        [DisplayName("Nazwa waluty")]
        public string name { get; set; }
        public double rate { get; set; }
        [DefaultValue("false")]
        [DisplayName("Włączenie/wyłączenie synchronizacji waluty")]
        public bool is_sync { get; set; } = false;
        [DataType(DataType.Date)]
        public DateTime created_at { get; set; }
        [DataType(DataType.Date)]
        public DateTime updated_at { get; set; }
        [DefaultValue("false")]
        public bool ghosted { get; set; } = false;


    }
}
