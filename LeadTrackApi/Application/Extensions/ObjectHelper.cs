using Newtonsoft.Json;
using System;

namespace LeadTrackApi.Application.Extensions;

public static class ObjectHelper
{
    public static string Dump<T>(this T obj, bool indented = true) =>
        JsonConvert.SerializeObject(obj, indented ? Formatting.Indented : Formatting.None);
}
