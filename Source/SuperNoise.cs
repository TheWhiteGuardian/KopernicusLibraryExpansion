using System;
using Kopernicus;
using Kopernicus.Configuration.ModLoader;
using LibNoise;
using UnityEngine;

namespace KLE
{
    namespace Mods
    {
        public class KLE_SuperNoise : PQSMod
        {
            public Double deformity;
            public Int32 generator1;
            public Int32 generator2;
            public Int32 generator3;
            public Int32 generator4;
            public Int32 iterations;
            public Int32 generators;
            public Double lacunarity;
            public Int32 qualityLevel;
            NoiseQuality quality;
            public Double frequency;
            public Double persistence;

            Perlin p1, p2, p3, p4;
            Vector4 genMult = new Vector4(0, 0, 0, 0);
            public override void OnSetup()
            {
                qualityLevel = Mathf.Clamp(qualityLevel, 1, 3);
                if (qualityLevel == 1) { quality = NoiseQuality.Low; }
                if (qualityLevel == 2) { quality = NoiseQuality.Standard; }
                if (qualityLevel == 3) { quality = NoiseQuality.High; }
                generators = Mathf.Clamp(generators, 1, 4);
                if (generators == 1) { genMult.x = 1; }
                if (generators == 2) { genMult.y = 1; }
                if (generators == 3) { genMult.z = 1; }
                if (generators == 4) { genMult.w = 1; }
                p1 = new Perlin(frequency, lacunarity, persistence, iterations, generator1, quality);
                p2 = new Perlin(frequency, lacunarity, persistence, iterations, generator2, quality);
                p3 = new Perlin(frequency, lacunarity, persistence, iterations, generator3, quality);
                p4 = new Perlin(frequency, lacunarity, persistence, iterations, generator4, quality);
            }

            public override void OnVertexBuildHeight(PQS.VertexBuildData data)
            {
                data.vertHeight += GetDifference(data.directionFromCenter) * deformity;
            }

            public double GetDifference(Vector3 direction)
            {
                return System.Math.Abs(System.Math.Abs(System.Math.Abs((p1.GetValue(direction) * genMult.x) - (p2.GetValue(direction) * genMult.y)) - (p3.GetValue(direction) * genMult.z)) - (p4.GetValue(direction) * genMult.w));
            }
        }
    }
    namespace Loaders
    {
        [RequireConfigType(ConfigType.Node)]
        public class SuperNoise : ModLoader<Mods.KLE_SuperNoise>
        {
            [ParserTarget("deformity")]
            public NumericParser<double> deformity
            {
                get { return mod.deformity; }
                set { mod.deformity = value; }
            }
            [ParserTarget("generator1")]
            public NumericParser<int> generator1
            {
                get { return mod.generator1; }
                set { mod.generator1 = value; }
            }
            [ParserTarget("generator2")]
            public NumericParser<int> generator2
            {
                get { return mod.generator2; }
                set { mod.generator2 = value; }
            }
            [ParserTarget("generator3")]
            public NumericParser<int> generator3
            {
                get { return mod.generator3; }
                set { mod.generator3 = value; }
            }
            [ParserTarget("generator4")]
            public NumericParser<int> generator4
            {
                get { return mod.generator4; }
                set { mod.generator4 = value; }
            }
            [ParserTarget("iterations")]
            public NumericParser<int> iterations
            {
                get { return mod.iterations; }
                set { mod.iterations = value; }
            }
            [ParserTarget("generators")]
            public NumericParser<int> generators
            {
                get { return mod.generators; }
                set { mod.generators = value; }
            }
            [ParserTarget("lacunarity")]
            public NumericParser<double> lacunarity
            {
                get { return mod.lacunarity; }
                set { mod.lacunarity = value; }
            }
            [ParserTarget("qualityLevel")]
            public NumericParser<int> qualityLevel
            {
                get { return mod.qualityLevel; }
                set { mod.qualityLevel = value; }
            }
            [ParserTarget("frequency")]
            public NumericParser<double> frequency
            {
                get { return mod.frequency; }
                set { mod.frequency = value; }
            }
            [ParserTarget("persistence")]
            public NumericParser<double> persistence
            {
                get { return mod.persistence; }
                set { mod.persistence = value; }
            }
        }
    }
}
