using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.Model
{
    public class Event
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateOnly Date { get; set; }
        public string Place { get; set; }
        public int Budget { get; set; }
        public string Status { get; set; }
    }
}
