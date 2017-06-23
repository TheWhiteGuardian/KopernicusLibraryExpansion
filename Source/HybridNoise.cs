using System;
using UnityEngine;
using Kopernicus.Configuration.ModLoader;
using Kopernicus;
using LibNoise.Unity.Generator;
using LibNoise.Unity;
using KLE.Noise;

namespace KLE
{
    public class PQSMod_HybridNoise : PQSMod
    {
        //Thank GregroxMun for this suggestion!
        public Double deformity = 100;
        public Int32 seed = 1000;
        public Double frequency = 7;
        public Double lacunarity = 1;
        public Int32 octaves = 4;
        public QualityMode quality = QualityMode.Medium;
        public Single offset = 1;
        private ModuleBase noiseMap;
        private Single lac;
        public override void OnSetup()
        {
            lac = Convert.ToSingle(lacunarity);
            noiseMap = new HybridMultifractal(frequency, lac, octaves, seed, quality, offset);
        }
        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            data.vertHeight += Convert.ToSingle(noiseMap.GetValue(data.directionFromCenter)) * deformity;
        }
    }
    [RequireConfigType(ConfigType.Node)]
    public class HybridNoise : ModLoader<PQSMod_HybridNoise>
    {
        //Thank GregroxMun for this suggestion!

        [ParserTarget("deformity", optional = true)]
        public NumericParser<double> deformity
        {
            get { return mod.deformity; }
            set { mod.deformity = value; }
        }
        [ParserTarget("seed", optional = true)]
        public NumericParser<int> seed
        {
            get { return mod.seed; }
            set { mod.seed = value; }
        }
        [ParserTarget("frequency", optional = true)]
        public NumericParser<double> frequency
        {
            get { return mod.frequency; }
            set { mod.frequency = value; }
        }
        [ParserTarget("lacunarity", optional = true)]
        public NumericParser<double> lacunarity
        {
            get { return mod.lacunarity; }
            set { mod.lacunarity = value; }
        }
        [ParserTarget("octaves", optional = true)]
        public NumericParser<int> octaves
        {
            get { return mod.octaves; }
            set { mod.octaves = value; }
        }
        [ParserTarget("offset", optional = true)]
        public NumericParser<float> offset
        {
            get { return mod.offset; }
            set { mod.offset = value; }
        }
        [ParserTarget("quality", optional = false)]
        public EnumParser<LibNoise.Unity.QualityMode> quality
        {
            get { return mod.quality; }
            set { mod.quality = value; }
        }
    }
}