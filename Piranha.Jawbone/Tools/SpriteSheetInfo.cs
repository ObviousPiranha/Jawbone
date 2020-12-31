using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;

namespace Piranha.Jawbone.Tools
{
    public record SpriteSheetInfo(
        Point32 SheetSize,
        ImmutableDictionary<string, SheetPosition> SheetPositions)
    {
        public static SpriteSheetInfo Load(JsonElement json)
        {
            var sheetSize = new Point32(
                json.GetProperty("width").GetInt32(),
                json.GetProperty("height").GetInt32());
            var builder = ImmutableDictionary.CreateBuilder<string, SheetPosition>();
            foreach (var element in json.GetProperty("sprites").EnumerateArray())
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
                builder.Add(name, sheetPosition);
            }
            
            return new SpriteSheetInfo(sheetSize, builder.ToImmutable());
        }
    }
}
