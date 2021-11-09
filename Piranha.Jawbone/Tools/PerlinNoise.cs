using System;
using System.Collections.Immutable;
using System.Linq;

namespace Piranha.Jawbone.Tools
{
    // https://lotsacode.wordpress.com/2010/02/24/perlin-noise-in-c/
    public class PerlinNoise
    {
        public static ImmutableArray<byte> CreatePermutation(Random random)
        {
            var builder = ImmutableArray.CreateBuilder<byte>(256);
            
            for (int i = 0; i < 256; ++i)
                builder.Add(unchecked((byte)i));
            
            for (int i = 0; i < 16; ++i)
            {
                for (int j = 0; j < builder.Count; ++j)
                {
                    var swapIndex = random.Next(j, builder.Count);
                    var swapValue = builder[j];
                    builder[j] = builder[swapIndex];
                    builder[swapIndex] = swapValue;
                }
            }

            return builder.MoveToImmutable();
        }

        public static ImmutableArray<double> CreateGradients(Random random)
        {
            var builder = ImmutableArray.CreateBuilder<double>(256 * 3);

            for (int i = 0; i < 256; ++i)
            {
                var z = 1d - 2d * random.NextDouble();
                var r = Math.Sqrt(1d - z * z);
                var theta = 2d * Math.PI * random.NextDouble();
                builder.Add(r * Math.Cos(theta));
                builder.Add(r * Math.Sin(theta));
                builder.Add(z);
            }

            return builder.MoveToImmutable();
        }

        private static double Smooth(double n) => n * n * (3d - 2d * n);
        private static double Lerp(double t, double a, double b) => a + t * (b - a);

        private readonly ImmutableArray<byte> _permutation;
        private readonly ImmutableArray<double> _gradients;

        public PerlinNoise(Random random)
        {
            _permutation = CreatePermutation(random);
            _gradients = CreateGradients(random);
        }

        private int Permutate(int n) => _permutation[n & 255];
        private int Index(int x, int y, int z) => Permutate(x + Permutate(y + Permutate(z)));

        private double Lattice(int ix, int iy, int iz, double fx, double fy, double fz)
        {
            var index = Index(ix, iy, iz);
            var gi = index * 3;
            var result =
                _gradients[gi] * fx +
                _gradients[gi + 1] * fy +
                _gradients[gi + 2] * fz;
            
            return result;
        }

        public double Noise(double x, double y, double z)
        {
            var ix = (int)Math.Floor(x);
            var fx0 = x - ix;
            var fx1 = fx0 - 1d;
            var wx = Smooth(fx0);

            var iy = (int)Math.Floor(y);
            var fy0 = y - iy;
            var fy1 = fy0 - 1d;
            var wy = Smooth(fy0);

            var iz = (int)Math.Floor(z);
            var fz0 = z - iz;
            var fz1 = fz0 - 1d;
            var wz = Smooth(fz0);

            var vx0 = Lattice(ix, iy, iz, fx0, fy0, fz0);
            var vx1 = Lattice(ix + 1, iy, iz, fx1, fy0, fz0);
            var vy0 = Lerp(wx, vx0, vx1);

            vx0 = Lattice(ix, iy + 1, iz, fx0, fy1, fz0);
            vx1 = Lattice(ix + 1, iy + 1, iz, fx1, fy1, fz0);
            var vy1 = Lerp(wx, vx0, vx1);

            var vz0 = Lerp(wy, vy0, vy1);

            vx0 = Lattice(ix, iy, iz + 1, fx0, fy0, fz1);
            vx1 = Lattice(ix + 1, iy, iz + 1, fx1, fy0, fz1);
            vy0 = Lerp(wx, vx0, vx1);

            vx0 = Lattice(ix, iy + 1, iz + 1, fx0, fy1, fz1);
            vx1 = Lattice(ix + 1, iy + 1, iz + 1, fx1, fy1, fz1);
            vy1 = Lerp(wx, vx0, vx1);

            var vz1 = Lerp(wy, vy0, vy1);
            var result = Lerp(wz, vz0, vz1);
            return result;
        }
    }
}
