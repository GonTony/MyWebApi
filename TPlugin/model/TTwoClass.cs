using System;
using System.Collections.Generic;
using System.Text;
using TPlugin.Interface;

namespace TPlugin.model
{
   public class TTwoClass : ITClass
    {
        string ITClass.Name()
        {
            return this.GetType().Name + "7777777777777777";
        }
    }
}
