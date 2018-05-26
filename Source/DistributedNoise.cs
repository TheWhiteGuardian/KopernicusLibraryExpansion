using KLE.Noises;
using Kopernicus;
using Kopernicus.Configuration.ModLoader;
using LibNoise;

namespace KLE
{
    namespace Mods
    {
        public class KLE_DistributedNoise : PQSMod
        {
            KSPNoise noise, placementNoise;
            public KSPNoiseType noiseType, placementNoiseType;
            public double deformity;
            public float frequency, persistence, lacunarity, placementFrequency, placementPersistence, placementLacunarity, placementGain, placementOffset;
            public int octaves, seed, placementOctaves, placementSeed;
            public NoiseQuality mode, placementMode;
            public override void OnSetup()
            {
                noise = Utils.GetKSPNoise(noiseType, frequency, lacunarity, persistence, octaves, seed, mode);
                placementNoise = Utils.GetKSPNoise(placementNoiseType, placementFrequency, placementLacunarity, placementPersistence, placementOctaves, placementSeed, placementMode);
            }
            public override void OnVertexBuildHeight(PQS.VertexBuildData data)
            {
                data.vertHeight += deformity * noise.GetValue(data.directionFromCenter) * (placementOffset + (placementGain * (placementNoise.GetValue(data.directionFromCenter))));
            }
        }
    }
    namespace Loaders
    {
        [RequireConfigType(ConfigType.Node)]
        public class DistributedNoise : ModLoader<Mods.KLE_DistributedNoise>
        {
            [ParserTarget("noiseType")]
            public EnumParser<KSPNoiseType> noiseType
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
            [ParserTarget("frequency")]
            public NumericParser<float> frequency
            {
                get { return mod.frequency; }
                set { mod.frequency = value; }
            }
            [ParserTarget("persistence")]
            public NumericParser<float> persistence
            {
                get { return mod.persistence; }
                set { mod.persistence = value; }
            }
            [ParserTarget("lacunarity")]
            public NumericParser<float> lacunarity
            {
                get { return mod.lacunarity; }
                set { mod.lacunarity = value; }
            }
            [ParserTarget("placementFrequency")]
            public NumericParser<float> placFrequency
            {
                get { return mod.placementFrequency; }
                set { mod.placementFrequency = value; }
            }
            [ParserTarget("placementPersistence")]
            public NumericParser<float> placPersistence
            {
                get { return mod.placementPersistence; }
                set { mod.placementPersistence = value; }
            }
            [ParserTarget("placementLacunarity")]
            public NumericParser<float> placLac
            {
                get { return mod.placementLacunarity; }
                set { mod.placementLacunarity = value; }
            }
            [ParserTarget("placementGain")]
            public NumericParser<float> placGain
            {
                get { return mod.placementGain; }
                set { mod.placementGain = value; }
            }
            [ParserTarget("placementOffset")]
            public NumericParser<float> placOffset
            {
                get { return mod.placementOffset; }
                set { mod.placementOffset = value; }
            }
            [ParserTarget("octaves")]
            public NumericParser<int> octaves
            {
                get { return mod.octaves; }
                set { mod.octaves = value; }
            }
            [ParserTarget("seed")]
            public NumericParser<int> seed
            {
                get { return mod.seed; }
                set { mod.seed = value; }
            }
            [ParserTarget("placementOctaves")]
            public NumericParser<int> placOctaves
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
            [ParserTarget("mode")]
            public NumericParser<KopernicusNoiseQuality> mode
            {
                get { return (KopernicusNoiseQuality)(int)mod.mode; }
                set { mod.mode = (NoiseQuality)(int)value.value; }
            }
            [ParserTarget("placementMode")]
            public NumericParser<KopernicusNoiseQuality> placMode
            {
                get { return (KopernicusNoiseQuality)(int)mod.placementMode; }
                set { mod.placementMode = (NoiseQuality)(int)value.value; }
            }
        }
    }
}
