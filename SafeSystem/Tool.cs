using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeSystem
{
    public static class Tool
    {
        public static String connstr= ConfigurationManager.AppSettings["connectionString"].ToString();

    }
}
