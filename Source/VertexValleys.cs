using System;
using UnityEngine;
using Kopernicus;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration;
using LibNoise.Unity;
using LibNoise.Unity.Generator;

namespace KLE
{
    public class PQSMod_VertexValleys : PQSMod
    {
        public enum NoiseType
        {
            Billow,
            RiggedMultifractal,
            Perlin
        }
        public NoiseType noiseType;
        public Double deformity = 500;
        public Double frequency = 5;
        public Double lacunarity = 2.5;
        public Double persistence = 0.3;
        public QualityMode mode = QualityMode.High;
        public Int32 octaves = 12;
        public Int32 seed = 8532;
        public Double offset = 0;
        private ModuleBase Noise;

        public override void OnSetup()
        {
            switch (noiseType)
            {
                case NoiseType.Billow:
                    Noise = new Billow(frequency, lacunarity, persistence, octaves, seed, mode);
                    break;
                case NoiseType.Perlin:
                    Noise = new Perlin(frequency, lacunarity, persistence, octaves, seed, mode);
                    break;
                case NoiseType.RiggedMultifractal:
                    Noise = new RiggedMultifractal(frequency, lacunarity, octaves, seed, mode);
                    break;
                default:
                    throw new ArgumentNullException("Noise type seems to be something undefinable. Valid entries are Billow, Perlin and RiggedMultifractal.", nameof(noiseType));
            }
        }
        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            var absoluteNoise = 1 + Noise.GetValue(data.directionFromCenter); //From -1 - 1 to 0 - 2.
            var deformation = deformity * 0.5; //Scale by 0.5 for the new maxValue of 2.
            data.vertHeight = (data.vertHeight - deformation) + offset; //Negative by default as valleys are the targeted feature.
        }
    }
    [RequireConfigType(ConfigType.Node)]
    public class VertexValleys : ModLoader<PQSMod_VertexValleys>
    {
        [ParserTarget("noiseType", optional = false)]
        public EnumParser<PQSMod_VertexValleys.NoiseType> noiseType
        {
            get { return mod.noiseType; }
            set { mod.noiseType = value; }
        }
        [ParserTarget("deformity", optional = true)]
        public NumericParser<double> deformity
        {
            get { return mod.deformity; }
            set { mod.deformity = value; }
        }
        [ParserTarget("frequency", optional = true)]
        public NumericParser<double> frequency
        {
            get { return mod.frequency; }
            set { mod.frequency = value; }
        }
        [ParserTarget("persistence", optional = true)]
        public NumericParser<double> persistence
        {
            get { return mod.persistence; }
            set { mod.persistence = value; }
        }
        [ParserTarget("mode", optional = true)]
        public NumericParser<QualityMode> mode
        {
            get { return mod.mode; }
            set { mod.mode = value; }
        }
        [ParserTarget("octaves", optional = true)]
        public NumericParser<int> octaves
        {
            get { return mod.octaves; }
            set { mod.octaves = value; }
        }
        [ParserTarget("seed", optional = true)]
        public NumericParser<int> seed
        {
            get { return mod.seed; }
            set { mod.seed = value; }
        }
        [ParserTarget("offset", optional = true)]
        public NumericParser<double> offset
        {
            get { return mod.offset; }
            set { mod.offset = value; }
        }
    }
}
