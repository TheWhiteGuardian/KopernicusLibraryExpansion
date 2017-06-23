using System;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration;
using UnityEngine;
using Kopernicus;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using KLE.Noise;

namespace KLE
{
    public class PQSMod_VertexExoticNoise : PQSMod
    {
        public Double deformity = 100;
        public Double frequency = 10;
        public Double lacunarity = 2.5;
        public Double noiseMode = 0;
        public QualityMode qualityMode = QualityMode.Low;
        public Int32 octaves = 4;
        public Int32 seed = 100;
        public Double persistence = 0.5;
        public Boolean addRange = false;
        public Double minAltitude = 0;
        public Double maxAltitude = 1;
        private Double minAlt;
        private Double maxAlt;
        private ExoticNoise noise;
        public override void OnSetup()
        {
            minAlt = minAltitude + sphere.radius;
            maxAlt = maxAltitude + sphere.radius;
            noise = new ExoticNoise(frequency, lacunarity, noiseMode, qualityMode, octaves, seed, persistence);
        }
        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            if (addRange)
            {
                if (data.vertHeight < maxAlt && data.vertHeight > minAlt)
                {
                    data.vertHeight += noise.GetValue(data.directionFromCenter) * deformity;
                }
            }
            if (!addRange)
            {
                data.vertHeight += noise.GetValue(data.directionFromCenter) * deformity;
            }
        }
    }

    [RequireConfigType(ConfigType.Node)]
    public class VertexExoticNoise : ModLoader<PQSMod_VertexExoticNoise>
    {
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

        [ParserTarget("lacunarity", optional = true)]
        public NumericParser<double> lacunarity
        {
            get { return mod.lacunarity; }
            set { mod.lacunarity = value; }
        }

        [ParserTarget("noiseMode", optional = true)]
        public NumericParser<double> noiseMode
        {
            get { return mod.noiseMode; }
            set { mod.noiseMode = value; }
        }

        [ParserTarget("qualityMode", optional = true)]
        public NumericParser<QualityMode> qualityMode
        {
            get { return mod.qualityMode; }
            set { mod.qualityMode = value; }
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

        [ParserTarget("persistence", optional = true)]
        public NumericParser<double> persistence
        {
            get { return mod.persistence; }
            set { mod.persistence = value; }
        }

        [ParserTarget("addRange", optional = true)]
        public NumericParser<bool> addRange
        {
            get { return mod.addRange; }
            set { mod.addRange = value; }
        }

        [ParserTarget("minAltitude", optional = true)]
        public NumericParser<double> minAltitude
        {
            get { return mod.minAltitude; }
            set { mod.minAltitude = value; }
        }

        [ParserTarget("maxAltitude", optional = true)]
        public NumericParser<double> maxAltitude
        {
            get { return mod.maxAltitude; }
            set { mod.maxAltitude = value; }
        }
    }
}