﻿using LiteDB;
using System;
using System.Diagnostics;
using System.Globalization;

namespace ImageBirb.Core.Common
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Setting
    {
        [BsonId]
        public string Key { get; set; }

        public string Value { get; set; }
        
        public bool AsBool()
        {
            if (!string.IsNullOrEmpty(Value) && bool.TryParse(Value, out var result))
            {
                return result;
            }

            return false;
        }

        public double AsDouble()
        {
            if (!string.IsNullOrEmpty(Value) && 
                double.TryParse(Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return 0;
        }

        public T AsEnum<T>() where T : struct
        {
            if (!string.IsNullOrEmpty(Value) && Enum.TryParse(Value, out T result))
            {
                return result;
            }

            return default(T);
        }

        private string DebuggerDisplay => Key + ": " + Value;
    }
}
