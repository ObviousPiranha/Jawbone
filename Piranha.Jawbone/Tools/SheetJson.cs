using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Piranha.Jawbone.Tools
{
    public static class SheetJson
    {
        public static Dictionary<string, SheetPosition> Load(JsonElement array)
        {
            var result = new Dictionary<string, SheetPosition>();
            foreach (var element in array.EnumerateArray())
            {
                var sheetPosition = new SheetPosition(
                    element.GetProperty("sheetIndex").GetInt32(),
                    new Rectangle32(
                        new Point32(
                            element.GetProperty("x").GetInt32(),
                            element.GetProperty("y").GetInt32()),
                        new Point32(
                            element.GetProperty("width").GetInt32(),
                            element.GetProperty("height").GetInt32())));
                
                var name = element.GetProperty("name").GetString() ?? throw new NullReferenceException();
                result.Add(name, sheetPosition);
            }
            return result;
        }
    }
}
