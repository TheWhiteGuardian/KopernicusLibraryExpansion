using System;
using LibNoise.Unity.Generator;
using UnityEngine;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration;
using Kopernicus;
using LibNoise.Unity;

namespace TWG
{
    #region PQSMods
    public class PQSParadiseSpike : PQSMod
    {
        public Double frequency = 5;
        public Double octaves = 4;
        public Double deformity = 500;
        public Int32 seed = 1000;
        public Double lacunarity = 2.5;
        public Double cutoff = 0.5;
        public Double persistence = 0.5;
        public Boolean onlyOnOcean = false;
        public Double oceanRadius;
        private Double internalRadius;
        public QualityMode mode = QualityMode.Low;
        public Boolean addNoise = false;
        public Double noiseDeformity = 5;
        public Double offset;
        public enum Indexer
        {
            Billow,
            Perlin,
            RiggedMultifractal,
            Simplex
        }
        public Indexer indexer;
        private Billow bill1;
        private Perlin perl1;
        private RiggedMultifractal rig1;
        private Simplex simp1;

        public override void OnSetup()
        {
            internalRadius = oceanRadius + sphere.radius;
        }

        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            switch(indexer)
            {
                case Indexer.Billow:
                    bill1 = new Billow(frequency, lacunarity, persistence, Convert.ToInt32(octaves), seed, mode);
                    if (onlyOnOcean)
                    {
                        if (data.vertHeight < internalRadius)
                        {
                            if (bill1.GetValue(data.directionFromCenter) > cutoff)
                            {
                                data.vertHeight += deformity + offset;
                            }
                        }
                    }
                    if (!onlyOnOcean)
                    {
                        if (bill1.GetValue(data.directionFromCenter) > cutoff)
                        {
                            data.vertHeight += deformity + offset;
                        }
                    }
                    if (addNoise)
                    {
                        var Simp2 = new Simplex(seed, octaves, persistence, frequency);
                        if (bill1.GetValue(data.directionFromCenter) > cutoff)
                            data.vertHeight = Simp2.noise(data.directionFromCenter) * noiseDeformity;
                    }
                    break;
                case Indexer.Perlin:
                    perl1 = new Perlin(frequency, lacunarity, persistence, Convert.ToInt32(octaves), seed, mode);
                    if (onlyOnOcean)
                    {
                        if (data.vertHeight < internalRadius)
                        {
                            if (perl1.GetValue(data.directionFromCenter) > cutoff)
                            {
                                data.vertHeight += deformity + offset;
                            }
                        }
                    }
                    if (!onlyOnOcean)
                    {
                        if (perl1.GetValue(data.directionFromCenter) > cutoff)
                        {
                            data.vertHeight += deformity + offset;
                        }
                    }
                    if (addNoise)
                    {
                        var Simp2 = new Simplex(seed, octaves, persistence, frequency);
                        if (perl1.GetValue(data.directionFromCenter) > cutoff)
                            data.vertHeight = Simp2.noise(data.directionFromCenter) * noiseDeformity;
                    }
                    break;
                case Indexer.RiggedMultifractal:
                    rig1 = new RiggedMultifractal(frequency, lacunarity, Convert.ToInt32(octaves), seed, mode);
                    if (onlyOnOcean)
                    {
                        if (data.vertHeight < internalRadius)
                        {
                            if (rig1.GetValue(data.directionFromCenter) > cutoff)
                            {
                                data.vertHeight += deformity + offset;
                            }
                        }
                    }
                    if (!onlyOnOcean)
                    {
                        if (rig1.GetValue(data.directionFromCenter) > cutoff)
                        {
                            data.vertHeight += deformity + offset;
                        }
                    }
                    
                    if (addNoise)
                    {
                        var Simp2 = new Simplex(seed, octaves, persistence, frequency);
                        if (rig1.GetValue(data.directionFromCenter) > cutoff)
                            data.vertHeight = Simp2.noise(data.directionFromCenter) * noiseDeformity;
                    }
                    break;
                case Indexer.Simplex:
                    simp1 = new Simplex(seed, octaves, persistence, frequency);
                    if (onlyOnOcean)
                    {
                        if (data.vertHeight < internalRadius)
                        {
                            if (simp1.noise(data.directionFromCenter) > cutoff)
                            {
                                data.vertHeight += deformity + offset;
                            }
                        }
                    }
                    if (!onlyOnOcean)
                    {
                        if (simp1.noise(data.directionFromCenter) > cutoff)
                        {
                            data.vertHeight += deformity + offset;
                        }
                    }
                    if (addNoise)
                    {
                        var Simp2 = new Simplex(seed, octaves, persistence, frequency);
                        if (simp1.noise(data.directionFromCenter) > cutoff)
                            data.vertHeight = Simp2.noise(data.directionFromCenter) * noiseDeformity;
                    }
                    break;
            }
        }
    }
    #endregion

    #region ModLoader
    [RequireConfigType(ConfigType.Node)]
    public class TWGParadiseSpike : ModLoader<PQSParadiseSpike>
    {
        [ParserTarget("frequency", optional = true)]
        public NumericParser<double> frequency
        {
            get { return mod.frequency; }
            set { mod.frequency = value; }
        }
        [ParserTarget("octaves", optional = true)]
        public NumericParser<double> octaves
        {
            get { return mod.frequency; }
            set { mod.frequency = value; }
        }
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
        [ParserTarget("lacunarity", optional = true)]
        public NumericParser<double> lacunarity
        {
            get { return mod.lacunarity; }
            set { mod.lacunarity = value; }
        }
        [ParserTarget("cutoff", optional = true)]
        public NumericParser<double> cutoff
        {
            get { return mod.cutoff; }
            set { mod.cutoff = value; }
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
        [ParserTarget("addNoise", optional = true)]
        public NumericParser<bool> addNoise
        {
            get { return mod.addNoise; }
            set { mod.addNoise = value; }
        }
        [ParserTarget("noiseDeformity", optional = true)]
        public NumericParser<double> noiseDeformity
        {
            get { return mod.noiseDeformity; }
            set { mod.noiseDeformity = value; }
        }
        [ParserTarget("offset", optional = true)]
        public NumericParser<double> offset
        {
            get { return mod.offset; }
            set { mod.offset = value; }
        }
        [ParserTarget("indexer", optional = true)]
        public EnumParser<PQSParadiseSpike.Indexer> indexer
        {
            get { return mod.indexer; }
            set { mod.indexer = value; }
        }
        [ParserTarget("onlyOnOcean", optional = true)]
        public NumericParser<bool> onlyOnOcean
        {
            get { return mod.onlyOnOcean; }
            set { mod.onlyOnOcean = value; }
        }
        [ParserTarget("oceanRadius", optional = true)]
        public NumericParser<double> oceanRadius
        {
            get { return mod.oceanRadius; }
            set { mod.oceanRadius = value; }
        }
    }
    #endregion
}