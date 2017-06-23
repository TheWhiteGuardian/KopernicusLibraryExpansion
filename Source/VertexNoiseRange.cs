using UnityEngine;
using Kopernicus;
using System;
using Kopernicus.Configuration.ModLoader;
using LibNoise.Unity.Generator;
using LibNoise.Unity;
using KLE.Noise;

namespace KLE
{
    public class PQSMod_VertexNoiseRange : PQSMod
    {
        public enum NoiseType
        {
            RiggedMultifractal,
            Perlin,
            Billow,
            HybridMultifractal
        }
        public NoiseType noiseType = NoiseType.RiggedMultifractal;

        public Int32 seed;
        public Double deformity;
        public Double octaves;
        public Double frequency;
        public Double lacunarity;
        public Double persistence;
        public Single offset;
        public Double rangeStart;
        public Double rangeEnd;
        public QualityMode mode = QualityMode.High;

        private Billow billow;
        private RiggedMultifractal rigged;
        private Perlin perlin;
        private HybridMultifractal hybrid;
        private Int32 modMode = 0;
        private Double finalStart;
        private Double finalEnd;

        public override void OnSetup()
        {
            finalStart = rangeStart = sphere.radius;
            finalEnd = rangeEnd + sphere.radius;
            switch (noiseType)
            {
                case NoiseType.Billow:
                    billow = new Billow(frequency, lacunarity, persistence, Convert.ToInt32(octaves), seed, mode);
                    modMode = 1;
                    break;
                case NoiseType.HybridMultifractal:
                    hybrid = new HybridMultifractal(frequency, Convert.ToSingle(persistence), Convert.ToInt32(octaves), seed, mode, offset);
                    modMode = 2;
                    break;
                case NoiseType.Perlin:
                    perlin = new Perlin(frequency, lacunarity, persistence, Convert.ToInt32(octaves), seed, mode);
                    modMode = 3;
                    break;
                case NoiseType.RiggedMultifractal:
                    rigged = new RiggedMultifractal(frequency, lacunarity, Convert.ToInt32(octaves), seed, mode);
                    modMode = 4;
                    break;
                default:
                    throw new ArgumentNullException(nameof(noiseType));
            }
        }
        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            if (data.vertHeight > finalStart && data.vertHeight < finalEnd)
            {
                if (modMode == 1)
                {
                    data.vertHeight += billow.GetValue(data.directionFromCenter) * deformity;
                }
                if (modMode == 2)
                {
                    data.vertHeight += hybrid.GetValue(data.directionFromCenter) * deformity;
                }
                if (modMode == 3)
                {
                    data.vertHeight += perlin.GetValue(data.directionFromCenter) * deformity;
                }
                if (modMode == 4)
                {
                    data.vertHeight += rigged.GetValue(data.directionFromCenter) * deformity;
                }
            }
        }
    }
    [RequireConfigType(ConfigType.Node)]
    public class VertexNoiseRange : ModLoader<PQSMod_VertexNoiseRange>
    {
        [ParserTarget("noiseType", optional = true)]
        public EnumParser<PQSMod_VertexNoiseRange.NoiseType> noiseType
        {
            get { return mod.noiseType; }
            set { mod.noiseType = value; }
        }
        [ParserTarget("seed", optional = true)]
        public NumericParser<int> seed
        {
            get { return mod.seed; }
            set { mod.seed = value; }
        }
        [ParserTarget("deformity", optional = true)]
        public NumericParser<double> deformity
        {
            get { return mod.deformity; }
            set { mod.deformity = value; }
        }
        [ParserTarget("octaves", optional = true)]
        public NumericParser<double> octaves
        {
            get { return mod.octaves; }
            set { mod.octaves = value; }
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
        [ParserTarget("persistence", optional = true)]
        public NumericParser<double> persistence
        {
            get { return mod.persistence; }
            set { mod.persistence = value; }
        }
        [ParserTarget("offset", optional = true)]
        public NumericParser<float> offset
        {
            get { return mod.offset; }
            set { mod.offset = value; }
        }
        [ParserTarget("rangeStart", optional = true)]
        public NumericParser<double> rangeStart
        {
            get { return mod.rangeStart; }
            set { mod.rangeStart = value; }
        }
        [ParserTarget("rangeEnd", optional = true)]
        public NumericParser<double> rangeEnd
        {
            get { return mod.rangeEnd; }
            set { mod.rangeEnd = value; }
        }
        [ParserTarget("mode", optional = true)]
        public NumericParser<LibNoise.Unity.QualityMode> mode
        {
            get { return mod.mode; }
            set { mod.mode = value; }
        }
    }
}