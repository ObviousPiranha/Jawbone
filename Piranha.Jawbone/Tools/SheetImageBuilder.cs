using System;
using System.Collections.Generic;
using System.IO;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools.CollectionExtensions;

namespace Piranha.Jawbone.Tools
{
    public sealed class SheetImageBuilder : IDisposable
    {
        private Rectangle32 Blit(
            IntPtr source,
            Rectangle32 sourceRectangle,
            IntPtr destination,
            Point32 destinationPosition)
        {
            Span<int> rects = stackalloc int[]
            {
                sourceRectangle.Position.X,
                sourceRectangle.Position.Y,
                sourceRectangle.Size.X,
                sourceRectangle.Size.Y,
                destinationPosition.X,
                destinationPosition.Y,
                0,
                0
            };

            var result = _sdl.BlitSurface(
                source,
                rects[0],
                destination,
                ref rects[4]);

            if (result < 0)
                throw new SdlException("Error on surface blit: " + _sdl.GetError());
            
            return new Rectangle32(
                new Point32(rects[4], rects[5]),
                new Point32(rects[6], rects[7]));
        }

        private readonly List<IntPtr> _sheetSurfaces = new();
        private readonly IStb _stb;
        private readonly ISdl2 _sdl;
        private readonly SheetBuilder _sheetBuilder;

        public SheetImageBuilder(IStb stb, ISdl2 sdl, Point32 size)
        {
            _stb = stb;
            _sdl = sdl;
            _sheetBuilder = new SheetBuilder(size);
        }

        public void Dispose()
        {
            foreach (var pointer in _sheetSurfaces)
                _sdl.FreeSurface(pointer);

            _sheetSurfaces.Clear();
        }

        public void SaveImages(string folder)
        {
            for (int i = 0; i < _sheetSurfaces.Count; ++i)
            {
                var view = new SurfaceView(_sheetSurfaces[i]);
                var path = Path.Combine(folder, $"sheet-{i}.png");
                _stb.StbiWritePng(
                    path,
                    _sheetBuilder.SheetSize.X,
                    _sheetBuilder.SheetSize.Y,
                    4,
                    view.Pixels,
                    _sheetBuilder.SheetSize.X * 4);
            }
        }

        public SheetPosition Add(IntPtr surface)
        {
            var view = new SurfaceView(surface);
            var width = view.Width;
            var height = view.Height;
            var slotSize = new Point32(width + 2, height + 2);
            var sheetPosition = _sheetBuilder.Allocate(slotSize);

            if (sheetPosition.SheetIndex == _sheetSurfaces.Count)
            {
                var blankSurface = _sdl.CreateRGBSurface(
                    0,
                    _sheetBuilder.SheetSize.X,
                    _sheetBuilder.SheetSize.Y,
                    32,
                    Platform.Rmask,
                    Platform.Gmask,
                    Platform.Bmask,
                    Platform.Amask);
                
                _sheetSurfaces.Add(blankSurface);
            }

            var sheetSurface = _sheetSurfaces[sheetPosition.SheetIndex];
            var pos = sheetPosition.Rectangle.Position;
            _ = Blit(
                surface,
                new Rectangle32(
                    default,
                    new Point32(width, height)),
                sheetSurface,
                pos.Moved(1, 1));

            // top left corner
            _ = Blit(
                surface,
                new Rectangle32(
                    default,
                    new Point32(1, 1)),
                sheetSurface,
                pos);

            // top edge
            _ = Blit(
                surface,
                new Rectangle32(
                    default,
                    new Point32(width, 1)),
                sheetSurface,
                pos.Moved(1, 0));
            
            // top right corner
            _ = Blit(
                surface,
                new Rectangle32(
                    new Point32(width - 1, 0),
                    new Point32(1, 1)),
                sheetSurface,
                pos.Moved(1 + width, 0));

            // left edge
            _ = Blit(
                surface,
                new Rectangle32(
                    default,
                    new Point32(1, height)),
                sheetSurface,
                pos.Moved(0, 1));
            
            // right edge
            _ = Blit(
                surface,
                new Rectangle32(
                    new Point32(width - 1, 0),
                    new Point32(1, height)),
                sheetSurface,
                pos.Moved(1 + width, 1));
            
            // bottom left corner
            _ = Blit(
                surface,
                new Rectangle32(
                    new Point32(0, height - 1),
                    new Point32(1, 1)),
                sheetSurface,
                pos.Moved(0, 1 + height));
            
            // bottom edge
            _ = Blit(
                surface,
                new Rectangle32(
                    new Point32(0, height - 1),
                    new Point32(width, 1)),
                sheetSurface,
                pos.Moved(1, 1 + height));
            
            // bottom right corner
            _ = Blit(
                surface,
                new Rectangle32(
                    new Point32(width - 1, height - 1),
                    new Point32(1, 1)),
                sheetSurface,
                pos.Moved(1 + width, 1 + height));

            return new SheetPosition(
                sheetPosition.SheetIndex,
                sheetPosition.Rectangle.Padded(1));
        }

        public SheetPosition Load(ReadOnlySpan<byte> imageData)
        {
            var pixelData = default(IntPtr);
            var surface = default(IntPtr);
            
            try
            {
                pixelData = _stb.StbiLoadFromMemory(
                    imageData[0],
                    imageData.Length,
                    out var width,
                    out var height,
                    out var comp,
                    4);
                
                if (pixelData.IsInvalid())
                    throw new Exception("Error reading image data.");

                surface = _sdl.CreateRGBSurfaceFrom(
                    pixelData,
                    width,
                    height,
                    32,
                    4 * width,
                    Platform.Rmask,
                    Platform.Gmask,
                    Platform.Bmask,
                    Platform.Amask);
                
                if (surface.IsInvalid())
                    throw new Exception("Error creating SDL surface from image data.");

                return Add(surface);
            }
            finally
            {
                if (surface.IsValid())
                    _sdl.FreeSurface(surface);
                
                if (pixelData.IsValid())
                    _stb.StbiImageFree(pixelData);
            }
        }
    }
}
