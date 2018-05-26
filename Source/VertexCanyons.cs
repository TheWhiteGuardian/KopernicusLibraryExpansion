using Kopernicus;
using Kopernicus.Configuration.ModLoader;
using LibNoise;
using UnityEngine;

namespace KLE
{
    namespace Mods
    {
        public class KLE_VertexCanyons : PQSMod
        {
            public int generatorSeed1, generatorSeed2, noiseOctaves, noiseSeed;
            public double deformity, generatorPersistence, generatorFrequency, noiseDeformity, noiseFrequency, noiseLacunarity;
            public float canyonSize, roundnessFactor;
            //Canyon curve determines canyon shape

            public FloatCurve canyonCurve;
            public FloatCurve noiseDistribution;
            Simplex generator, generator2;
            RidgedMultifractal rid;

            void InitializeCurve(FloatCurve curve)
            {
                curve.Add(0, 0);
                curve.Add(0.3f, 0.5f);
                curve.Add(0.7f, 0.8f);
                curve.Add(1, 1);
            }

            public override void OnSetup()
            {
                generator = new Simplex(generatorSeed1, 1, generatorPersistence, generatorFrequency);
                generator2 = new Simplex(generatorSeed2, 1, generatorPersistence, generatorFrequency);
                canyonSize = Mathf.Clamp01(canyonSize);
                rid = new RidgedMultifractal(noiseFrequency, noiseLacunarity, noiseOctaves, noiseSeed, NoiseQuality.Low);

                //If the curve is empty
                if (canyonCurve == null)
                {
                    InitializeCurve(canyonCurve);
                }
                if (noiseDistribution == null)
                {
                    InitializeCurve(noiseDistribution);
                }
            }

            public float Generator(Vector3d direction)
            {
                //This gives a more potato-like shape
                return (float)(generator.noiseNormalized(direction) - (generator2.noiseNormalized(direction) * roundnessFactor));
            }

            public override void OnVertexBuildHeight(PQS.VertexBuildData data)
            {
                if (Generator(data.directionFromCenter) >= canyonSize)
                {
                    //Data.vertHeight += (canyon deformity at this location, depth dependent on generator) + (noise deformity at this location)
                    float maxRemapped = KLEMath.Remap(Generator(data.directionFromCenter), canyonSize, 0, 1, 1); //Maximize range of canyon placement generator for the distribution curves
                    data.vertHeight += (-deformity * canyonCurve.Evaluate(maxRemapped)) + (noiseDeformity * noiseDistribution.Evaluate(maxRemapped) * rid.GetValue(data.directionFromCenter));
                }
                //Else do nothing. :)

                //Because floatcurves, planet makers can shape the rim and shape of the canyons.
            }
        }
    }
    namespace Loaders
    {
        [RequireConfigType(ConfigType.Node)]
        public class VertexCanyons : ModLoader<Mods.KLE_VertexCanyons>
        {
            [ParserTarget("generatorSeed1")]
            public NumericParser<int> generatorSeed1
            {
                get { return mod.generatorSeed1; }
                set { mod.generatorSeed1 = value; }
            }
            [ParserTarget("generatorSeed2")]
            public NumericParser<int> generatorSeed2
            {
                get { return mod.generatorSeed2; }
                set { mod.generatorSeed2 = value; }
            }
            [ParserTarget("noiseOctaves")]
            public NumericParser<int> noiseOctaves
            {
                get { return mod.noiseOctaves; }
                set { mod.noiseOctaves = value; }
            }
            [ParserTarget("noiseSeed")]
            public NumericParser<int> noiseSeed
            {
                get { return mod.noiseSeed; }
                set { mod.noiseSeed = value; }
            }
            [ParserTarget("deformity")]
            public NumericParser<double> deformity
            {
                get { return mod.deformity; }
                set { mod.deformity = value; }
            }
            [ParserTarget("generatorPersistence")]
            public NumericParser<double> generatorPersistence
            {
                get { return mod.generatorPersistence; }
                set { mod.generatorPersistence = value; }
            }
            [ParserTarget("generatorFrequency")]
            public NumericParser<double> generatorFrequency
            {
                get { return mod.generatorFrequency; }
                set { mod.generatorFrequency = value; }
            }
            [ParserTarget("noiseDeformity")]
            public NumericParser<double> noiseDeformity
            {
                get { return mod.noiseDeformity; }
                set { mod.noiseDeformity = value; }
            }
            [ParserTarget("noiseFrequency")]
            public NumericParser<double> noiseFrequency
            {
                get { return mod.noiseFrequency; }
                set { mod.noiseFrequency = value; }
            }
            [ParserTarget("noiseLacunarity")]
            public NumericParser<double> noiseLacunarity
            {
                get { return mod.noiseLacunarity; }
                set { mod.noiseLacunarity = value; }
            }
            [ParserTarget("canyonSize")]
            public NumericParser<float> canyonSize
            {
                get { return mod.canyonSize; }
                set { mod.canyonSize = value; }
            }
            [ParserTarget("roundnessFactor")]
            public NumericParser<float> roundnessFactor
            {
                get { return mod.roundnessFactor; }
                set { mod.roundnessFactor = value; }
            }
            [ParserTarget("canyonCurve")]
            public FloatCurveParser canyonCurve
            {
                get { return mod.canyonCurve; }
                set { mod.canyonCurve = value.curve; }
            }
            [ParserTarget("noiseCurve")]
            public FloatCurveParser noiseCurve
            {
                get { return mod.noiseDistribution; }
                set { mod.noiseDistribution = value.curve; }
            }
        }
    }
}
