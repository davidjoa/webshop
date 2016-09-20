using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson6.Interfaces
{
    public class SystemDateTime : IDateTime
    {

        private DateTime now;

        public DateTime Now
        {
            get
            {   return DateTime.Now;
                //return now; for singleton
            }
        }

        //public SystemDateTime()           for use of singleton
        //{

        //    now = DateTime.Now;
        //}

    }
}
