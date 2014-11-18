using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicoSoft.Calendar
{
    
   public  class MedicoEvent
    {
       public static int cpt = 0;
       public string title { get; set; }
       public DateTime? start { get; set; }
       public DateTime? end { get; set; }
       public string allday;

       private int _id = 0;
       public int id { get { return cpt++; } }
    

    }
}
