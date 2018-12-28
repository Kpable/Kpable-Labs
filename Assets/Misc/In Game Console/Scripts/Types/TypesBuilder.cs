using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Kpable.InGameConsole
{
    public class TypesBuilder
    {
        internal static BaseType Build(TypeCode type)
        {
            switch (type)
            {
                case TypeCode.Boolean:
                    return new BoolType();
                case TypeCode.Int32:
                    return new IntType();
                case TypeCode.Decimal:
                    return new FloatType();

                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.DateTime:
                case TypeCode.DBNull:
                case TypeCode.Double:
                case TypeCode.Empty:
                case TypeCode.Int16:
                case TypeCode.Int64:
                case TypeCode.Object:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.String:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                default:
                    return new AnyType();
            }
        }
    }
}