using System;
using UnityEngine;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration;
using Kopernicus;
using LibNoise.Unity;
using LibNoise.Unity.Generator;

namespace KLE
{
    #region Mod

    public class PQSMod_VertexFractures : PQSMod
    {
        public enum NoiseType
        {
            riggedMultifractal,
            billow,
            perlin,
            voronoi
        }
        public NoiseType noiseType;
        public Int32 ModMode;
        public Boolean useEnum;
        public Double deformity;
        public Single startLimiter;
        public Single endLimiter;
        public Double frequency;
        public Double lacunarity;
        public Double persistence;
        public Boolean enableDistance;
        public Double displacement;
        public Int32 octaves;
        public Int32 seed;
        public QualityMode mode;

        private Double convDeformity;
        private ModuleBase noise;
        public override void OnSetup()
        {
            convDeformity = deformity * -1;
        }
        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            if (useEnum)
            {
                switch (noiseType)
                {
                    case NoiseType.billow:
                        noise = new Billow(frequency, lacunarity, persistence, octaves, seed, mode);
                        break;
                    case NoiseType.riggedMultifractal:
                        noise = new RiggedMultifractal(frequency, lacunarity, octaves, seed, mode);
                        break;
                    case NoiseType.perlin:
                        noise = new Perlin(frequency, lacunarity, persistence, octaves, seed, mode);
                        break;
                    case NoiseType.voronoi:
                        noise = new Voronoi(frequency, displacement, seed, enableDistance);
                        break;
                    default:
                        throw new ArgumentNullException("noiseType is undefinable.", nameof(noiseType));
                }
            }
            if (!useEnum)
            {
                //0 = Billow
                //1 = Rigged
                //2 = Perlin
                //3 = Voronoi
                if (ModMode == 0)
                {
                    noise = new Billow(frequency, lacunarity, persistence, octaves, seed, mode);
                }
                if (ModMode == 1)
                {
                    noise = new RiggedMultifractal(frequency, lacunarity, octaves, seed, mode);
                }
                if (ModMode == 2)
                {
                    noise = new Perlin(frequency, lacunarity, persistence, octaves, seed, mode);
                }
                if (ModMode >= 3)
                {
                    noise = new Voronoi(frequency, displacement, seed, enableDistance);
                }
            }

            if (noise.GetValue(data.directionFromCenter) > startLimiter && noise.GetValue(data.directionFromCenter) < endLimiter)
            {
                data.vertHeight += convDeformity;
            }
        }
        public override double GetVertexMaxHeight()
        {
            if (deformity <= 0)
            {
                return deformity;
            }
            else
            {
                return 0;
            }
        }
        public override double GetVertexMinHeight()
        {
            if (deformity <= 0)
            {
                return 0;
            }
            else
            {
                return -deformity;
            }
        }
    }

    #endregion
    #region loader
    [RequireConfigType(ConfigType.Node)]
    public class VertexFractures : ModLoader<PQSMod_VertexFractures>
    {
        [ParserTarget("noiseType", optional = false)]
        public EnumParser<PQSMod_VertexFractures.NoiseType> noiseType
        {
            get { return mod.noiseType; }
            set { mod.noiseType = value; }
        }
        [ParserTarget("ModMode", optional = true)]
        public NumericParser<int> ModMode
        {
            get { return mod.ModMode; }
            set { mod.ModMode = value; }
        }
        [ParserTarget("useEnum", optional = true)]
        public NumericParser<bool> useEnum
        {
            get { return mod.useEnum; }
            set { mod.useEnum = value; }
        }
        [ParserTarget("deformity", optional = true)]
        public NumericParser<double> deformity
        {
            get { return mod.deformity; }
            set { mod.deformity = value; }
        }
        [ParserTarget("startLimiter", optional = true)]
        public NumericParser<float> startLimiter
        {
            get { return mod.startLimiter; }
            set { mod.startLimiter = value; }
        }
        [ParserTarget("endLimiter", optional = true)]
        public NumericParser<float> endLimiter
        {
            get { return mod.endLimiter; }
            set { mod.endLimiter = value; }
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
        [ParserTarget("enableDistance", optional = true)]
        public NumericParser<bool> enableDistance
        {
            get { return mod.enableDistance; }
            set { mod.enableDistance = value; }
        }
        [ParserTarget("displacement", optional = true)]
        public NumericParser<double> displacement
        {
            get { return mod.displacement; }
            set { mod.displacement = value; }
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
        [ParserTarget("mode", optional = true)]
        public NumericParser<QualityMode> mode
        {
            get { return mod.mode; }
            set { mod.mode = value; }
        }
    }
    #endregion
}