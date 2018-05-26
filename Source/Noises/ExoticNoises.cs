using UnityEngine;
using LibNoise;

namespace KLE.Noises
{
    public enum NoiseType
    {
        HeteroTerrain = 0,
        MultiFractal = 1,
        Turbulence = 2,
        HybridMultifractal = 3,
        DistortedHeteroTerrain = 4,
        StrataHeteroTerrain = 5,
        PlanetNoise = 6,
        DoubleMultifractal = 7,
        Brownian = 8,
        NoiseRocks = 9,
        SlickRocks = 10
    }
    public enum KSPNoiseType
    {
        Perlin = 0,
        RiggedMultifractal = 1,
        Billow = 2,
        Voronoi = 3,
        Simplex = 4,
        NormalizedSimplex = 5
    }
    public class Utils
    {
        public static double Abs(double input)
        {
            if (input < 0) return -input;
            else return input;
        }
        public static double gnoise(float noisesize, double x, double y, double z, bool hard, Perlin p)
        {
            if (noisesize != 0.0f)
            {
                noisesize = 1.0f / noisesize;
                x *= noisesize;
                y *= noisesize;
                z *= noisesize;
            }

            if (hard) return Abs(2.0f * p.GetValue(x, y, z) - 1.0f);
            return p.GetValue(x, y, z);
        }
        /* Turbulence
        public static float Turbulence(Vector3d direction, Perlin p, bool hard, int octaves, float amplitude, float frequency)
        {
            double x = direction.x, y = direction.y, z = direction.z;
            float amp = 1f, outp, time;
            outp = (float)(2.0f * Utils.gnoise(1, x, y, z, false, p));
            if (hard)
            {
                outp = Mathf.Abs(outp);
            }
            for (int i = 0; i < octaves; i++)
            {
                amp *= amplitude;
                x *= frequency;
                y *= frequency;
                z *= frequency;
                time = (float)(amp * (2.0f * Utils.gnoise(1, x, y, z, false, p)));
                if (hard)
                {
                    time = Mathf.Abs(time);
                }
                outp += time;
            }
            return outp;
        }
        */
        /* HeteroTerrain
        public static float heteroTerrain(double x, double y, double z, float H, float lacunarity, float octaves, float offset, Perlin p)
        {
            float value, increment, rmd;
            int i;
            float pwHL = Mathf.Pow(lacunarity, -H);
            float pwr = pwHL;
            value = offset + (float)p.GetValue(x, y, z);
            x *= lacunarity;
            y *= lacunarity;
            z *= lacunarity;

            for (i = 1; i < (int)octaves; i++)
            {
                increment = ((float)p.GetValue(x, y, z) + offset) * pwr * value;
                value += increment;
                pwr *= pwHL;
                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
            }
            rmd = octaves - Mathf.Floor(octaves);
            if (rmd != 0.0f)
            {
                increment = ((float)p.GetValue(x, y, z) + offset) * pwr * value;
                value += rmd * increment;
            }
            return value;
        }
        */
        /* MultiFractal
        public static float multiFractal(Vector3d direction, float H, float lacunarity, float octaves, Perlin p)
        {
            double x = direction.x, y = direction.y, z = direction.z;
            float rmd, value = 1.0f, pwr = 1.0f, pwHL = Mathf.Pow(lacunarity, -H);
            int i;

            for (i = 0; i < (int)Mathf.Round(octaves); i++)
            {
                value *= (pwr * (float)p.GetValue(x, y, z) + 1.0f);
                pwr *= pwHL;
                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
            }
            rmd = octaves - Mathf.Floor(octaves);
            if (rmd != 0.0f) value *= (rmd * (float)p.GetValue(x, y, z) * pwr + 1.0f);
            return value;

        }
        */
        public static float ridgedMultiFractal(Vector3d direction, float H, float lacunarity, float octaves, float offset, float gain, Perlin p)
        {
            double x = direction.x, y = direction.y, z = direction.z;
            float result, signal, weight;
            int i;
            float pwHL = Mathf.Pow(lacunarity, -H);
            float pwr = pwHL;   /* starts with i=1 instead of 0 */

            signal = offset - Mathf.Abs((float)p.GetValue(direction));
            signal *= signal;
            result = signal;


            for (i = 1; i < (int)Mathf.Floor(octaves); i++)
            {
                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
                weight = signal * gain;
                if (weight > 1.0f) weight = 1.0f;
                else if (weight < 0.0f) weight = 0.0f;
                signal = offset - Mathf.Abs((float)p.GetValue(x, y, z));
                signal *= signal;
                signal *= weight;
                result += signal * pwr;
                pwr *= pwHL;
            }

            return result;
        }
        public static GradientNoiseBasis GetNoiseType(NoiseType inputType, float H, float lacunarity, float octaves, float offset, Perlin p, float amplitude, float frequency, float distort, bool hard, float gain)
        {
            switch (inputType)
            {
                case NoiseType.HeteroTerrain:
                    return new HeteroNoise(H, lacunarity, octaves, offset, p);
                case NoiseType.MultiFractal:
                    return new MultiFractal(H, lacunarity, octaves, p);
                case NoiseType.Turbulence:
                    return new Turbulence(p, hard, (int)Mathf.Round(octaves), amplitude, frequency);
                case NoiseType.HybridMultifractal:
                    return new HybridMultifractal(H, gain, offset, lacunarity, (int)Mathf.Round(octaves), p);
                case NoiseType.DistortedHeteroTerrain:
                    return new DistortedHeteroTerrain(H, lacunarity, octaves, offset, p, distort);
                case NoiseType.StrataHeteroTerrain:
                    return new DistortedHeteroTerrain(H, lacunarity, octaves, offset, p, distort);
                case NoiseType.PlanetNoise:
                    return new PlanetNoise(p, (int)Mathf.Round(octaves), distort, amplitude, frequency, hard);
                case NoiseType.DoubleMultifractal:
                    return new DoubleMultifractal(H, lacunarity, octaves, p, offset, gain);
                case NoiseType.Brownian:
                    return new Brownian(lacunarity, H, octaves, p);
                case NoiseType.NoiseRocks:
                    return new NoiseRocks(p, distort, amplitude, frequency, hard, (int)Mathf.Round(octaves));
                case NoiseType.SlickRocks:
                    return new NoiseRocks(p, distort, amplitude, frequency, hard, (int)Mathf.Round(octaves));
                default:
                    return new PlanetNoise(p, 1);
            }
        }
        static bool DistanceConverter(double input)
        {
            if (input == 1) return true; else return false;
        }
        public static KSPNoise GetKSPNoise(KSPNoiseType noiseType, double frequency, double lacunarity, double persistance, int octaves, int seed, NoiseQuality mode)
        {
            switch (noiseType)
            {
                case KSPNoiseType.Billow:
                    return new TWGBillow(frequency, lacunarity, persistance, octaves, seed, mode);
                case KSPNoiseType.NormalizedSimplex:
                    return new TWGNormalizedSimplex(seed, octaves, persistance, frequency);
                case KSPNoiseType.Perlin:
                    return new TWGPerlin(frequency, lacunarity, persistance, octaves, seed, mode);
                case KSPNoiseType.RiggedMultifractal:
                    return new TWGRigged(frequency, lacunarity, octaves, seed, mode);
                case KSPNoiseType.Simplex:
                    return new TWGSimplex(seed, octaves, persistance, frequency);
                case KSPNoiseType.Voronoi:
                    return new TWGVoronoi(frequency, persistance, seed, DistanceConverter(lacunarity));
                default:
                    return new TWGFallback();
            }
        }
    }

    //Based off https://github.com/martijnberger/blender/blob/c359343f8dae6689c955dc1fa700cb26f6cd2e95/source/blender/blenlib/intern/noise.c
    //And https://github.com/martijnberger/blender/blob/c359343f8dae6689c955dc1fa700cb26f6cd2e95/source/blender/blenlib/BLI_noise.h
    //And https://github.com/martijnberger/blender/blob/master/source/blender/python/mathutils/mathutils_noise.c
    //And https://github.com/martijnberger/blender/blob/c359343f8dae6689c955dc1fa700cb26f6cd2e95/source/blender/blenlib/intern/noise.c
    //And the Blender 'landscape generator' addon

    //I only take credit for the c# conversion and the way that the above code is represented in this document - set up as a series of inheriting classes.

    /// <summary>
    /// Base class for all 'exotic' noises so that they can be merged into a single PQSMod handling all noises
    /// </summary>
    public abstract class GradientNoiseBasis
    {
        public virtual double GetValue(Vector3d direction)
        {
            return 0;
        }
    }
    /// <summary>
    /// HeteroTerrain noise constructor
    /// </summary>
    public sealed class HeteroNoise : GradientNoiseBasis
    {
        float H, lacunarity, octaves, offset; Perlin p;
        public HeteroNoise(float H, float lacunarity, float octaves, float offset, Perlin p)
        {
            this.H = H;
            this.lacunarity = lacunarity;
            this.octaves = octaves;
            this.offset = offset;
            this.p = p;
        }
        public override double GetValue(Vector3d direction)
        {
            double x = direction.x, y = direction.y, z = direction.z;
            float value, increment, rmd;
            int i;
            float pwHL = Mathf.Pow(lacunarity, -H);
            float pwr = pwHL;
            value = offset + (float)p.GetValue(x, y, z);
            x *= lacunarity;
            y *= lacunarity;
            z *= lacunarity;

            for (i = 1; i < (int)octaves; i++)
            {
                increment = ((float)p.GetValue(x, y, z) + offset) * pwr * value;
                value += increment;
                pwr *= pwHL;
                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
            }
            rmd = octaves - Mathf.Floor(octaves);
            if (rmd != 0.0f)
            {
                increment = ((float)p.GetValue(x, y, z) + offset) * pwr * value;
                value += rmd * increment;
            }
            return value;
        }
    }
    /// <summary>
    /// MultiFractal noise constructor
    /// </summary>
    public sealed class MultiFractal : GradientNoiseBasis
    {
        float H, lacunarity, octaves;
        Perlin p;
        public MultiFractal(float H, float lacunarity, float octaves, Perlin p)
        {
            this.H = H;
            this.lacunarity = lacunarity;
            this.octaves = octaves;
            this.p = p;
        }
        public override double GetValue(Vector3d direction)
        {
            double x = direction.x, y = direction.y, z = direction.z;
            float rmd, value = 1.0f, pwr = 1.0f, pwHL = Mathf.Pow(lacunarity, -H);
            int i;

            for (i = 0; i < (int)Mathf.Round(octaves); i++)
            {
                value *= (pwr * (float)p.GetValue(x, y, z) + 1.0f);
                pwr *= pwHL;
                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
            }
            rmd = octaves - Mathf.Floor(octaves);
            if (rmd != 0.0f) value *= (rmd * (float)p.GetValue(x, y, z) * pwr + 1.0f);
            return value;
        }
    }
    /// <summary>
    /// Turbulence noise constructor
    /// </summary>
    public sealed class Turbulence : GradientNoiseBasis
    {
        Perlin p; bool hard; int octaves; float amplitude, frequency;
        public Turbulence(Perlin p, bool hard, int octaves, float amplitude, float frequency)
        {
            this.p = p;
            this.hard = hard;
            this.octaves = octaves;
            this.amplitude = amplitude;
            this.frequency = frequency;
        }
        public override double GetValue(Vector3d direction)
        {
            double x = direction.x, y = direction.y, z = direction.z;
            float amp = 1f, outp, time;
            outp = (float)(2.0f * Utils.gnoise(1, x, y, z, false, p));
            if (hard)
            {
                outp = Mathf.Abs(outp);
            }
            for (int i = 0; i < octaves; i++)
            {
                amp *= amplitude;
                x *= frequency;
                y *= frequency;
                z *= frequency;
                time = (float)(amp * (2.0f * Utils.gnoise(1, x, y, z, false, p)));
                if (hard)
                {
                    time = Mathf.Abs(time);
                }
                outp += time;
            }
            return outp;
        }
    }
    /// <summary>
    /// HybridMultifractal constructor
    /// </summary>
    public sealed class HybridMultifractal : GradientNoiseBasis
    {
        float lac, gain, offset, H;
        int octaves;
        Perlin p;
        public HybridMultifractal(float H, float gain, float offset, float lacunarity, int octaves, Perlin p)
        {
            this.H = H;
            this.lac = lacunarity;
            this.gain = gain;
            this.offset = offset;
            this.octaves = octaves;
            this.p = p;
        }
        public override double GetValue(Vector3d direction)
        {
            double basisOutput = p.GetValue(direction);
            double x = direction.x, y = direction.y, z = direction.z;
            double process, weight, rmd = 0, signal;
            float pwHL = Mathf.Pow(lac, -H);
            float pwr = pwHL;

            process = basisOutput + offset;
            weight = gain * process;
            x *= lac;
            y *= lac;
            z *= lac;

            for (int i = 1; (weight > 0.001f) && (i < octaves); i++)
            {
                if (weight > 1.0f) weight = 1.0f;
                signal = (basisOutput + offset) * pwr;
                pwr *= pwHL;
                process += weight * signal;
                weight *= gain * signal;
                x *= lac;
                y *= lac;
                z *= lac;
            }
            rmd = octaves - Mathf.Floor(octaves);
            if (rmd != 0f) { process += rmd * ((basisOutput + offset) * pwr); }
            return process;
        }
    }
    /// <summary>
    /// Distorted hTerrain
    /// </summary>
    public sealed class DistortedHeteroTerrain : GradientNoiseBasis
    {
        HeteroNoise basis; float distort;
        public DistortedHeteroTerrain(float H, float lacunarity, float octaves, float offset, Perlin p, float distort)
        {
            this.distort = distort;
            basis = new HeteroNoise(H, lacunarity, octaves, offset, p);
        }
        public override double GetValue(Vector3d direction)
        {
            double x = direction.x, y = direction.y, z = direction.z;
            var h1 = (basis.GetValue(new Vector3d(x, y, z)) * 0.5f);
            var d = h1 * distort;
            var h2 = (basis.GetValue(new Vector3d(x, y, z)) * 0.25f);
            return (h1 * h1 + h2 * h2) * 0.5f;
        }
    }
    /// <summary>
    /// Strata hTerrain
    /// </summary>
    public sealed class StrataHeteroTerrain : GradientNoiseBasis
    {
        HeteroNoise n; float distort;
        public StrataHeteroTerrain(float H, float lacunarity, float octaves, float offset, Perlin p, float distort)
        {
            this.distort = distort;
            n = new HeteroNoise(H, lacunarity, octaves, offset, p);
        }
        public override double GetValue(Vector3d direction)
        {
            double x = direction.x, y = direction.y, z = direction.z;
            float val = (float)n.GetValue(new Vector3d(x, y, z)) * 0.5f;
            float steps = (Mathf.Sin(val * (distort * 5) * Mathf.PI) * (0.1f / (distort * 5) * Mathf.PI));
            return (val * (1.0f - 0.5f) + steps * 0.5f);
        }
    }
    /// <summary>
    /// Constructor implementation of the Planet Noise by Farsthary
    /// </summary>
    public sealed class PlanetNoise : GradientNoiseBasis
    {
        float nabla;
        Turbulence Turb;
        public PlanetNoise(Perlin p, int octaves, float nabla = 0.001f, float amplitude = 1, float frequency = 1, bool hard = false)
        {
            this.nabla = nabla;
            Turb = new Turbulence(p, hard, octaves, amplitude, frequency);
        }
        public override double GetValue(Vector3d direction)
        {
            double x = direction.x, y = direction.y, z = direction.z;
            var d = 0.001;
            var offset = nabla * 1000;
            x = Turb.GetValue(new Vector3d(direction.x, direction.y, direction.z));
            y = Turb.GetValue(new Vector3d(x + offset, y, z));
            z = Turb.GetValue(new Vector3d(x, y + offset, z));
            var xdy = x - Turb.GetValue(new Vector3d(x, y + d, z));
            var xdz = x - Turb.GetValue(new Vector3d(x, y, z + d));
            var ydx = y - Turb.GetValue(new Vector3d(x + d, y, z));
            var ydz = y - Turb.GetValue(new Vector3d(x, y, z + d));
            var zdx = z - Turb.GetValue(new Vector3d(x + d, y, z));
            var zdy = z - Turb.GetValue(new Vector3d(x, y + d, z));
            return (zdy - ydz) * (zdx - xdz) * (ydx - xdy);
        }
    }
    public sealed class DoubleMultifractal : GradientNoiseBasis
    {
        MultiFractal mf1, mf2;
        float offset, gain;
        public DoubleMultifractal(float H, float lacunarity, float octaves, Perlin p, float offset, float gain)
        {
            this.offset = offset; this.gain = gain;
            mf1 = new MultiFractal(1.0f, 1.0f, 1.0f, p);
            mf2 = new MultiFractal(H, lacunarity, octaves, p);
        }
        public override double GetValue(Vector3d direction)
        {
            double n1 = mf1.GetValue(new Vector3d(direction.x * 1.5f + 1, direction.y * 1.5f + 1, direction.z * 1.5f + 1)) * (offset * 0.5f);
            double n2 = mf2.GetValue(new Vector3d(direction.x - 1, direction.y - 1, direction.z - 1)) * (gain * 0.5f);
            return (n1 * n1 + n2 * n2) * 0.5f;
        }
    }
    public sealed class Brownian : GradientNoiseBasis
    {
        float lacunarity, octaves, H; Perlin p;
        public Brownian(float lacunarity, float H, float octaves, Perlin p)
        {
            this.lacunarity = lacunarity;
            this.octaves = octaves;
            this.p = p;
            this.H = H;
        }
        /*
         The following code is based on Ken Musgrave's explanations and sample
         source code in the book "Texturing and Modeling: A procedural approach"
         */
        public override double GetValue(Vector3d direction)
        {
            double x = direction.x, y = direction.y, z = direction.z;
            float rmd, value = 0.0f, pwr = 1.0f, pwHL = Mathf.Pow(lacunarity, -H);


            for (int i = 0; i < (int)Mathf.Round(octaves); i++)
            {
                value += (float)p.GetValue(x, y, z) * pwr;
                pwr *= pwHL;
                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
            }

            rmd = octaves - Mathf.Floor(octaves);
            if (rmd != 0f) value += rmd * (float)p.GetValue(x, y, z) * pwr;

            return value;
        }
    }
    public sealed class NoiseRocks : GradientNoiseBasis
    {
        Turbulence t1, t2, t3; float distortion;
        public NoiseRocks(Perlin p, float distortion, float amplitude, float frequency, bool hard, int depth)
        {
            this.distortion = distortion;
            t1 = new Turbulence(p, false, 4, 0, 0);
            t2 = new Turbulence(p, hard, 2, 0, 7);
            t3 = new Turbulence(p, hard, depth, amplitude, frequency);
        }
        public override double GetValue(Vector3d direction)
        {
            float p1 = (float)(t1.GetValue(direction)) * 0.125f * distortion;
            p1 = (float)(t2.GetValue(new Vector3d(direction.x + p1, direction.y + p1, direction.z)));
            float pa = p1 * 0.1875f * distortion;
            float b = (float)(t3.GetValue(new Vector3d(direction.x, direction.y, direction.z + pa)));
            return ((p1 + 0.5f * (b - p1)) * 0.5f + 0.5f);
        }
    }
    public sealed class SlickRocks : GradientNoiseBasis
    {
        MultiFractal mf; float distort; Perlin p; float H, lacunarity, octaves, offset, gain;
        public SlickRocks(Perlin p, float distort, float H, float lacunarity, float octaves, float offset, float gain)
        {
            this.p = p;
            this.distort = distort;
            this.H = H;
            this.lacunarity = lacunarity;
            this.octaves = octaves;
            this.gain = gain;
            this.offset = offset;
            mf = new MultiFractal(1.0f, 2.0f, 2.0f, p);
        }
        public override double GetValue(Vector3d direction)
        {
            var n = mf.GetValue(direction) * distort * 0.25f;
            var r = Utils.ridgedMultiFractal(new Vector3d(direction.x + n, direction.y + n, direction.z + n), H, lacunarity, octaves, offset + 0.1f, gain * 2, p);
            return (n + (n * r)) * 0.5f;
        }
    }

    //Semi-original code here
    public abstract class KSPNoise
    {
        public virtual double GetValue(Vector3d direction)
        {
            return 0; //Meant to be overridden
        }
    }
    public sealed class TWGRigged : KSPNoise
    {
        RidgedMultifractal rigged;
        public TWGRigged(double frequency, double lacunarity, int octaves, int seed, NoiseQuality mode)
        {
            rigged = new RidgedMultifractal(frequency, lacunarity, octaves, seed, mode);
        }
        public override double GetValue(Vector3d direction)
        {
            return rigged.GetValue(direction);
        }
    }
    public sealed class TWGPerlin : KSPNoise
    {
        Perlin perl;
        public TWGPerlin(double frequency, double lacunarity, double persistance, int octaves, int seed, NoiseQuality mode)
        {
            perl = new Perlin(frequency, lacunarity, persistance, octaves, seed, mode);
        }
        public override double GetValue(Vector3d direction)
        {
            return perl.GetValue(direction);
        }
    }
    public sealed class TWGBillow : KSPNoise
    {
        Billow b;
        public TWGBillow(double frequency, double lacunarity, double persistance, int octaves, int seed, NoiseQuality quality)
        {
            b = new Billow(frequency, lacunarity, persistance, octaves, seed, quality);
        }
        public override double GetValue(Vector3d direction)
        {
            return b.GetValue(direction);
        }
    }
    public sealed class TWGVoronoi : KSPNoise
    {
        Voronoi v;
        public TWGVoronoi(double voronoiFrequency, double voronoiDisplacement, int voronoiSeed, bool distanceEnabled)
        {
            v = new Voronoi(voronoiFrequency, voronoiDisplacement, voronoiSeed, distanceEnabled);
        }
        public override double GetValue(Vector3d direction)
        {
            return v.GetValue(direction);
        }
    }
    public sealed class TWGSimplex : KSPNoise
    {
        Simplex sim;
        public TWGSimplex(int seed, double octaves, double persistence, double frequency)
        {
            sim = new Simplex(seed, octaves, persistence, frequency);
        }
        public override double GetValue(Vector3d direction)
        {
            return sim.noise(direction);
        }
    }
    public sealed class TWGNormalizedSimplex : KSPNoise
    {
        Simplex sim;
        public TWGNormalizedSimplex(int seed, double octaves, double persistence, double frequency)
        {
            sim = new Simplex(seed, octaves, persistence, frequency);
        }
        public override double GetValue(Vector3d direction)
        {
            return sim.noiseNormalized(direction);
        }
    }
    /// <summary>
    /// The fallback class is not a noise, it is a backup function
    /// </summary>
    public sealed class TWGFallback : KSPNoise
    {
        public TWGFallback()
        {

        }
        public override double GetValue(Vector3d direction)
        {
            return 1;
        }
    }
}
