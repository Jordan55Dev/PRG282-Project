using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG282Project
{
    public class Student
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Course { get; set; }

        public override string ToString()
        {
            return $"StudentID:{ID},Student Name:{Name},Student Age:{Age},Course:{Course}";
        }
    }
}
