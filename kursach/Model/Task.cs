using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.Model
{
    public class Task
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly Term { get; set; }
        public string Assigne { get; set; }
        public string Status { get; set; }
    }
}
