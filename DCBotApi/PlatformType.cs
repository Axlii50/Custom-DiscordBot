﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi
{
   public enum PlatformType
    {
        All = 1,
        PC = 2,
        STEAM = 4,
        EPIC = 8,
        XBOXONE = 16,
    }

    internal static class PlatformTypeExstension
    {
        /// <summary>
        /// custom GetEnumarator for PlatformType enum 
        /// this allows to simply use foreach without writing some function to return list of all types that are contained inside of type
        /// and just use variable as list for iterating
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerator<PlatformType> GetEnumerator(this PlatformType type)
        {
            bool allplatforms = true;
            if (type.HasFlag(PlatformType.PC))
            {
                yield return PlatformType.PC;
                allplatforms = false;
            }

            if (type.HasFlag(PlatformType.STEAM))
            {
                yield return PlatformType.STEAM;
                allplatforms = false;
            }

            if (type.HasFlag(PlatformType.XBOXONE))
            {
                yield return PlatformType.XBOXONE;
                allplatforms = false;
            }

            if (type.HasFlag(PlatformType.EPIC))
            {
                yield return PlatformType.EPIC;
                allplatforms = false;
            }

            if (allplatforms) yield return PlatformType.All;
        }
    }
}
