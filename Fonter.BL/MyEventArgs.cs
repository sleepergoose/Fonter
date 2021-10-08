using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fonter.BL
{
    public class MyEventArgs : EventArgs
    {
        public Dictionary<string, string> Data = new Dictionary<string, string>();

        public void Add(string Key, string Value)
        {
            Data.Add(Key, Value);
        }
    }
}
