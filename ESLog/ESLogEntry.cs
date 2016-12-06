using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESLog {
    public class ESLogEntry {

        public string Title { get; set; }

        public string StackTrace { get; set; }

        public DateTime CreatedOn { get; set; }

        public int EventID { get; set; }

        public string EventName { get; set; }
    }
}
