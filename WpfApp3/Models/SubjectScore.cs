using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Models
{
    public class SubjectScore
    {
        public string Subject { get; set; }
        public int ScoresNum { get; set; }
        public int ScoresSum { get; set; }
        public double AvgScore { get; set; }
    }
}
