using System;
using LibNoise;
using Kopernicus;
using Kopernicus.Configuration.ModLoader;

namespace KLE
{
    public enum NoiseRenderMode
    {
        Normal = 0,
        Inverted = 1,
        Normalized = 2,
        InvertedNormalized = 3
    }
    namespace Mods
    {
        public class KLE_ImprovedVoronoi : PQSMod
        {
            public NoiseRenderMode mode;
            Voronoi v;
            public double deformity;
            public bool enableDistance;
            public int seed;
            public double offset;
            public double displacement;
            public double frequency;
            //Depending on NoiseRenderMode, calculate differently.

            float Remap(float noiseValue)
            {
                switch (mode)
                {
                    case NoiseRenderMode.Normal:
                        return noiseValue;
                    case NoiseRenderMode.Inverted:
                        return KLEMath.OneMinus(noiseValue);
                    case NoiseRenderMode.Normalized:
                        return (noiseValue + 1) / 2;
                    case NoiseRenderMode.InvertedNormalized:
                        return KLEMath.OneMinus((noiseValue + 1) / 2);
                    default:
                        throw new ArgumentException("[KLE_ImprovedVoronoi]: Noise value not readable!");
                }
            }

            public override void OnSetup()
            {
                v = new Voronoi(frequency, displacement, seed, enableDistance);
            }

            public override void OnVertexBuildHeight(PQS.VertexBuildData data)
            {
                data.vertHeight += (Remap((float)v.GetValue(data.directionFromCenter)) * deformity) + offset;
            }
        }
    }
    namespace Loaders
    {
        [RequireConfigType(ConfigType.Node)]
        public class ImprovedVoronoi : ModLoader<Mods.KLE_ImprovedVoronoi>
        {

            [ParserTarget("noiseRenderMode")]
            public EnumParser<NoiseRenderMode> noiseRenderMode
            {
                get { return mod.mode; }
                set { mod.mode = value; }
            }

            [ParserTarget("deformity")]
            public NumericParser<double> deformity
            {
                get { return mod.deformity; }
                set { mod.deformity = value; }
            }

            [ParserTarget("enableDistance")]
            public NumericParser<bool> enableDistance
            {
                get { return mod.enableDistance; }
                set { mod.enableDistance = value; }
            }

            [ParserTarget("seed")]
            public NumericParser<int> seed
            {
                get { return mod.seed; }
                set { mod.seed = value; }
            }

            [ParserTarget("offset")]
            public NumericParser<double> offset
            {
                get { return mod.offset; }
                set { mod.offset = value; }
            }

            [ParserTarget("displacement")]
            public NumericParser<double> displacement
            {
                get { return mod.displacement; }
                set { mod.displacement = value; }
            }

            [ParserTarget("frequency")]
            public NumericParser<double> frequency
            {
                get { return mod.frequency; }
                set { mod.frequency = value; }
            }
        }
    }
}
