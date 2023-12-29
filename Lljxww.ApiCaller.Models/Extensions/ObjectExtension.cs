﻿using System.Collections.Generic;
using System.Linq;

namespace Lljxww.ApiCaller.Models.Extensions;

public static class ObjectExtension
{
    public static Dictionary<string, string>? AsDictionary(this object source)
    {
        return source switch
        {
            null => default,
            Dictionary<string, string> dictionary => dictionary,
            _ => source.GetType()
                .GetProperties()
                .ToDictionary(propInfo => propInfo.Name,
                    propInfo => propInfo.GetValue(source) == null ? "" : propInfo.GetValue(source)!.ToString()!)
        };
    }
}