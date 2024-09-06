using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Utils
{
    class ValidStr
    {
        char[] invalids = { '+', '-', '*', '%', '=', '\'', '\"', '\\', '/', ';' ,',',' '};
        public bool Valid(string str)
        {
            foreach (char c in invalids) 
            {
                if(str.Contains(c)) return false;
            }
            return true;
        }
    }
}
