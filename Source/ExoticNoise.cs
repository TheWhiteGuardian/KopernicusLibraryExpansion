using KLE.Noises;
using LibNoise;
using UnityEngine;
using Kopernicus;
using Kopernicus.Configuration.ModLoader;

namespace KLE
{
    namespace Mods
    {
        public class KLE_ExoticNoise : PQSMod
        {
            KLE.Noises.GradientNoiseBasis noise;
            public NoiseType noiseType;
            Perlin p;
            public KSPNoiseType placementNoiseType;
            KSPNoise placement;
            int placementScale;
            public double deformity, toffset;
            public double perlFrequency, perlLacunarity, perlPersistence; public int perlOctaves, perlSeed; //Perlin values
            public float H, lacunarity, octaves, offset, amplitude, frequency, gain, distort;
            public bool hard, enablePlacement;
            public NoiseQuality perlMode, placementMode;
            public float placementFrequency, placementLacunarity, placementPersistence;
            public int placementOctaves, placementSeed;
            public float placementGain, placementOffset;

            public override void OnSetup()
            {
                p = new Perlin(perlFrequency, perlLacunarity, perlPersistence, perlOctaves, perlSeed, perlMode);
                noise = Utils.GetNoiseType(noiseType, H, lacunarity, octaves, offset, p, amplitude, frequency, distort, hard, gain);
                placement = Utils.GetKSPNoise(placementNoiseType, placementFrequency, placementLacunarity, placementPersistence, placementOctaves, placementSeed, placementMode);
                placementScale = System.Convert.ToInt32(enablePlacement);
            }
            public override void OnVertexBuildHeight(PQS.VertexBuildData data)
            {
                data.vertHeight += (toffset + (deformity * noise.GetValue(data.directionFromCenter))) * Mathf.Lerp(1, (float)((placement.GetValue(data.directionFromCenter) * placementGain) + placementOffset), placementScale);
            }
        }
    }
    namespace Loaders
    {
        [RequireConfigType(ConfigType.Node)]
        public class ExoticNoise : ModLoader<Mods.KLE_ExoticNoise>
        {
            [ParserTarget("noiseType")]
            public EnumParser<NoiseType> noiseType
            {
                get { return mod.noiseType; }
                set { mod.noiseType = value.value; }
            }
            [ParserTarget("deformity")]
            public NumericParser<double> deformity
            {
                get { return mod.deformity; }
                set { mod.deformity = value; }
            }
            [ParserTarget("terrainOffset")]
            public NumericParser<double> offset
            {
                get { return mod.toffset; }
                set { mod.toffset = value; }
            }
            #region PerlinFields
            [ParserTarget("perlinFrequency")]
            public NumericParser<double> pF
            {
                get { return mod.perlFrequency; }
                set { mod.perlFrequency = value; }
            }
            [ParserTarget("perlinLacunarity")]
            public NumericParser<double> pL
            {
                get { return mod.perlLacunarity; }
                set { mod.perlLacunarity = value; }
            }
            [ParserTarget("perlinPersistence")]
            public NumericParser<double> pP
            {
                get { return mod.perlPersistence; }
                set { mod.perlPersistence = value; }
            }
            [ParserTarget("perlinOctaves")]
            public NumericParser<int> pO
            {
                get { return mod.perlOctaves; }
                set { mod.perlOctaves = value; }
            }
            [ParserTarget("perlinSeed")]
            public NumericParser<int> pS
            {
                get { return mod.perlSeed; }
                set { mod.perlSeed = value; }
            }
            [ParserTarget("perlinMode")]
            public EnumParser<NoiseQuality> pM
            {
                get { return mod.perlMode; }
                set { mod.perlMode = value.value; }
            }
            #endregion

            #region NoiseFields
            [ParserTarget("factor")]
            public NumericParser<float> Fact
            {
                get { return mod.H; }
                set { mod.H = value; }
            }
            [ParserTarget("lacunarity")]
            public NumericParser<float> lac
            {
                get { return mod.lacunarity; }
                set { mod.lacunarity = value; }
            }
            [ParserTarget("octaves")]
            public NumericParser<float> octaves
            {
                get { return mod.lacunarity; }
                set { mod.lacunarity = value; }
            }
            [ParserTarget("offset")]
            public NumericParser<float> off
            {
                get { return mod.offset; }
                set { mod.offset = value; }
            }
            [ParserTarget("amplitude")]
            public NumericParser<float> amplitude
            {
                get { return mod.amplitude; }
                set { mod.amplitude = value; }
            }
            [ParserTarget("frequency")]
            public NumericParser<float> frequency
            {
                get { return mod.frequency; }
                set { mod.frequency = value; }
            }
            [ParserTarget("gain")]
            public NumericParser<float> gain
            {
                get { return mod.gain; }
                set { mod.gain = value; }
            }
            [ParserTarget("distort")]
            public NumericParser<float> distort
            {
                get { return mod.distort; }
                set { mod.distort = value; }
            }
            [ParserTarget("hard")]
            public NumericParser<bool> hard
            {
                get { return mod.hard; }
                set { mod.hard = value; }
            }
            #endregion

            //Enabling placement results in a semi-random distribution pattern instead of an even pattern across the globe.
            #region Placement
            [ParserTarget("placementNoiseType")]
            public EnumParser<KSPNoiseType> placNT
            {
                get { return mod.placementNoiseType; }
                set { mod.placementNoiseType = value; }
            }
            [ParserTarget("enablePlacement")]
            public NumericParser<bool> ePlacement
            {
                get { return mod.enablePlacement; }
                set { mod.enablePlacement = false; }
            }
            [ParserTarget("placementMode")]
            public EnumParser<NoiseQuality> placMode
            {
                get { return mod.placementMode; }
                set { mod.placementMode = value.value; }
            }
            [ParserTarget("placementFrequency")]
            public NumericParser<float> placFrequency
            {
                get { return mod.placementFrequency; }
                set { mod.placementFrequency = value; }
            }
            [ParserTarget("placementLacunarity")]
            public NumericParser<float> placLac
            {
                get { return mod.placementLacunarity; }
                set { mod.placementLacunarity = value; }
            }
            [ParserTarget("placementPersistence")]
            public NumericParser<float> placPers
            {
                get { return mod.placementPersistence; }
                set { mod.placementPersistence = value; }
            }
            [ParserTarget("placementOctaves")]
            public NumericParser<int> placOct
            {
                get { return mod.placementOctaves; }
                set { mod.placementOctaves = value; }
            }
            [ParserTarget("placementSeed")]
            public NumericParser<int> placSeed
            {
                get { return mod.placementSeed; }
                set { mod.placementSeed = value; }
            }
            [ParserTarget("placementGain")]
            public NumericParser<float> placementGain
            {
                get { return mod.placementGain; }
                set { mod.placementGain = value; }
            }
            [ParserTarget("placementOffset")]
            public NumericParser<float> placementOffset
            {
                get { return mod.placementOffset; }
                set { mod.placementOffset = value; }
            }
            #endregion
        }
    }
}
