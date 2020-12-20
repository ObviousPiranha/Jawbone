using System;
using System.Buffers;

namespace Piranha.Jawbone.Tools
{
    public class Buffer<T> : IDisposable
    {
        private readonly ArrayPool<T> _pool;
        private T[] _array = Array.Empty<T>();
        
        public int Capacity => _array.Length;
        public int Count { get; private set; }
        public ref T this[int index] => ref _array[index];

        private void AddWithResize(T value)
        {
            if (Capacity > 0)
            {
                var array = _pool.Rent(Capacity * 2);
                _array.AsSpan(0, Count).CopyTo(array);
                array[Count++] = value;
                _pool.Return(_array);
                _array = array;
            }
            else
            {
                _array = _pool.Rent(8);
                _array[0] = value;
                Count = 1;
            }
        }

        private void AddWithResize(ReadOnlySpan<T> values)
        {
            var minCapacity = Count + values.Length;

            if (Capacity > 0)
            {
                var nextCapacity = Capacity * 2;
                
                while (nextCapacity < minCapacity)
                    nextCapacity *= 2;
                
                var array = _pool.Rent(nextCapacity);
                _array.AsSpan(0, Count).CopyTo(array);
                values.CopyTo(array.AsSpan(Count));
                Count += values.Length;
                _pool.Return(_array);
                _array = array;
            }
            else
            {
                var nextCapacity = 8;

                while (nextCapacity < minCapacity)
                    nextCapacity *= 2;
                
                _array = _pool.Rent(nextCapacity);
                values.CopyTo(_array);
                Count = values.Length;
            }
        }

        public Buffer() : this(ArrayPool<T>.Shared)
        {
        }

        public Buffer(ArrayPool<T> pool)
        {
            _pool = pool;
        }

        public void Add(T value)
        {
            if (Count < Capacity)
            {
                _array[Count++] = value;
            }
            else
            {
                AddWithResize(value);
            }
        }

        public void Add(ReadOnlySpan<T> values)
        {
            var gap = Capacity - Count;

            if (values.Length <= gap)
            {
                values.CopyTo(_array.AsSpan(Count));
                Count += values.Length;
            }
            else
            {
                AddWithResize(values);
            }
        }

        public void Clear() => Count = 0;
        public Span<T> AsSpan() => _array.AsSpan(0, Count);

        public void Dispose()
        {
            Clear();
            
            if (Capacity > 0)
            {
                _pool.Return(_array);
                _array = Array.Empty<T>();
            }
        }
    }
}
