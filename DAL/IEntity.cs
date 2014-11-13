using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public  interface IEntity<T,U>
    {
       T getInfo(U identifiant);

    }
}
