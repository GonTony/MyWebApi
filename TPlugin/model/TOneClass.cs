using System;
using System.Collections.Generic;
using System.Text;
using TPlugin.Interface;

namespace TPlugin.model
{
   public class TOneClass:ITClass
    {
        public string title = "1234567";
        public string tt() {
            return "this is test";
        }

        string ITClass.Name()
        {
            return this.GetType().Name;
        }
    }
}
