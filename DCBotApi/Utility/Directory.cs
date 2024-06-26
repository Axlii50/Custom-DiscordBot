﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Utility
{
    internal class Directory
    {
        /// <summary>
        /// Return given path with full as full path not as part
        /// </summary>
        /// <param name="PathAdd"></param>
        /// <returns></returns>
        public static string GetPath(string PathAdd)
        {
            string AppFullname = Assembly.GetEntryAssembly().FullName;
            string Appname = AppFullname.Split(',')[0] + ".dll";
            string Path = Assembly.GetEntryAssembly().Location.Replace(Appname, "");
            return Path + PathAdd;
        }
    }
}
