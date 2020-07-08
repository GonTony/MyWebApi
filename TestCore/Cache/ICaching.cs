using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore
{
   public interface ICaching
    {
        object Get(string key);

        void Set(string key ,object value);
    }
}
