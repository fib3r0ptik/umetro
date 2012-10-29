﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EZTVMetro {
    public class EnumHelper<T> {
        public static IEnumerable<string> GetNames() {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException("Type '" + type.Name + "' is not an enum");

            return (
              from field in type.GetFields(BindingFlags.Public | BindingFlags.Static)
              where field.IsLiteral
              select field.Name).ToList<string>();

        }

    }
}
