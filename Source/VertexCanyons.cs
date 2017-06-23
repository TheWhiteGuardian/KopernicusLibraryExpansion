using System;
using UnityEngine;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration;
using Kopernicus;
using LibNoise.Unity;
using LibNoise.Unity.Generator;

namespace KLE
{
    #region PQSMods
    public class PQSMod_VertexCanyons : PQSMod
    {
        //Generates a highly deformed Simplex noise that isn't applied to the terrain, but the negative parts are taken as generator points for the canyons.
        #region NoiseValues
        public Int32 noiseSeed = 1000;
        public Double noiseDeformity = 100;
        public Double noiseFrequency = 4;
        public Double noisePersistence = 1;
        public Double noiseLacunarity = 2;
        public Int32 noiseOctaves = 4;
        public QualityMode noiseMode;
        private ModuleBase canyonNoiseMap;
        public Double noiseDisplacement = 0;
        public Boolean noiseEnableDistance = true;
        public enum NoiseType
        {
            Perlin,
            Billow,
            RiggedMultifractal,
            Voronoi
        }
        public NoiseType noiseType;
        #endregion

        #region SimplexValues
        public Int32 canyonSeed = 1000;
        public Double canyonThreshold = 2000;
        public Double canyonOctaves = 5;
        public Double canyonPersistence = 0.2;
        public Double canyonFrequency = 3;
        public Double canyonSize = 0;
        private Simplex canyonPlacementSimplex;
        #endregion

        public Double addCanyonDepth = 0;
        public Double multiplyCanyonBy = 1;

        public Boolean addColor = false;
        public Color canyonColor;
        public Single colorBlend = 1;

        public override void OnSetup()
        {
            canyonPlacementSimplex = new Simplex(canyonSeed, canyonOctaves, canyonPersistence, canyonFrequency);
            switch (noiseType)
            {
                case NoiseType.Perlin:
                    canyonNoiseMap = new Perlin(noiseFrequency, noiseLacunarity, noisePersistence, noiseOctaves, noiseSeed, noiseMode);
                    break;
                case NoiseType.Billow:
                    canyonNoiseMap = new Billow(noiseFrequency, noiseLacunarity, noisePersistence, noiseOctaves, noiseSeed, noiseMode);
                    break;
                case NoiseType.RiggedMultifractal:
                    canyonNoiseMap = new RiggedMultifractal(noiseFrequency, noiseLacunarity, noiseOctaves, noiseSeed, noiseMode);
                    break;
                case NoiseType.Voronoi:
                    canyonNoiseMap = new Voronoi(noiseFrequency, noiseDisplacement, noiseSeed, noiseEnableDistance);
                    break;
                default:
                    throw new ArgumentException("Noise Type seems to be something undefineable. Valid entries are Perlin, Billow, RiggedMultifractal and Voronoi.", nameof(NoiseType));
            }
        }

        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            if ((((canyonPlacementSimplex.noise(data.directionFromCenter) + 1) * 0.5 * canyonThreshold) - canyonSize) < 0)
            {
                data.vertHeight += ((canyonNoiseMap.GetValue(data.directionFromCenter) * noiseDeformity) * multiplyCanyonBy) + addCanyonDepth;
            }
        }
        public override void OnVertexBuild(PQS.VertexBuildData data)
        {
            if ((((canyonPlacementSimplex.noise(data.directionFromCenter) + 1) * 0.5 * canyonThreshold) - canyonSize) < 0)
            {
                if (addColor)
                    data.vertColor = Color.Lerp(data.vertColor, canyonColor, colorBlend);
            }
        }
    }
    #endregion

    #region ModLoader
    [RequireConfigType(ConfigType.Node)]
    public class VertexCanyons : ModLoader<PQSMod_VertexCanyons>
    {
        //NoiseValues
        [ParserTarget("noiseSeed", optional = true)]
        public NumericParser<int> noiseSeed
        {
            get { return mod.noiseSeed; }
            set { mod.noiseSeed = value; }
        }
        [ParserTarget("noiseDeformity", optional = true)]
        public NumericParser<double> noiseDeformity
        {
            get { return mod.noiseDeformity; }
            set { mod.noiseDeformity = value; }
        }
        [ParserTarget("noiseFrequency", optional = true)]
        public NumericParser<double> noiseFrequency
        {
            get { return mod.noiseFrequency; }
            set { mod.noiseFrequency = value; }
        }
        [ParserTarget("noisePersistence", optional = true)]
        public NumericParser<double> noisePersistence
        {
            get { return mod.noisePersistence; }
            set { mod.noisePersistence = value; }
        }
        [ParserTarget("noiseLacunarity", optional = true)]
        public NumericParser<double> noiseLacunarity
        {
            get { return mod.noiseLacunarity; }
            set { mod.noiseLacunarity = value; }
        }
        [ParserTarget("noiseOctaves", optional = true)]
        public NumericParser<int> noiseOctaves
        {
            get { return mod.noiseOctaves; }
            set { mod.noiseOctaves = value; }
        }
        [ParserTarget("noiseType", optional = false)]
        public EnumParser<PQSMod_VertexCanyons.NoiseType> noiseType
        {
            get { return mod.noiseType; }
            set { mod.noiseType = value; }
        }
        [ParserTarget("noiseMode", optional = false)]
        public EnumParser<LibNoise.Unity.QualityMode> noiseMode
        {
            get { return mod.noiseMode; }
            set { mod.noiseMode = value; }
        }
        [ParserTarget("noiseDisplacement", optional = true)]
        public NumericParser<double> noiseDisplacement
        {
            get { return mod.noiseDisplacement; }
            set { mod.noiseDisplacement = value; }
        }
        [ParserTarget("noiseEnableDistance", optional = true)]
        public NumericParser<bool> noiseEnableDistance
        {
            get { return mod.noiseEnableDistance; }
            set { mod.noiseEnableDistance = value; }
        }

        //SimplexValues
        [ParserTarget("canyonSeed", optional = true)]
        public NumericParser<int> canyonSeed
        {
            get { return mod.canyonSeed; }
            set { mod.canyonSeed = value; }
        }
        [ParserTarget("canyonThreshold", optional = true)]
        public NumericParser<double> canyonThreshold
        {
            get { return mod.canyonThreshold; }
            set { mod.canyonThreshold = value; }
        }
        [ParserTarget("canyonOctaves", optional = true)]
        public NumericParser<double> canyonOctaves
        {
            get { return mod.canyonOctaves; }
            set { mod.canyonOctaves = value; }
        }
        [ParserTarget("canyonPersistence", optional = true)]
        public NumericParser<double> canyonPersistence
        {
            get { return mod.canyonPersistence; }
            set { mod.canyonPersistence = value; }
        }
        [ParserTarget("canyonFrequency", optional = true)]
        public NumericParser<double> canyonFrequency
        {
            get { return mod.canyonFrequency; }
            set { mod.canyonFrequency = value; }
        }
        [ParserTarget("canyonSize", optional = true)]
        public NumericParser<double> canyonSize
        {
            get { return mod.canyonSize; }
            set { mod.canyonSize = value; }
        }

        //Other
        [ParserTarget("addCanyonDepth", optional = true)]
        public NumericParser<double> addCanyonDepth
        {
            get { return mod.addCanyonDepth; }
            set { mod.addCanyonDepth = value; }
        }
        [ParserTarget("multiplyCanyonBy", optional = true)]
        public NumericParser<double> multiplyCanyonBy
        {
            get { return mod.multiplyCanyonBy; }
            set { mod.multiplyCanyonBy = value; }
        }
        [ParserTarget("addColor", optional = true)]
        public NumericParser<bool> addColor
        {
            get { return mod.addColor; }
            set { mod.addColor = value; }
        }
        [ParserTarget("canyonColor", optional = false)]
        public ColorParser canyonColor
        {
            get { return mod.canyonColor; }
            set { mod.canyonColor = value; }
        }
        [ParserTarget("colorBlend", optional = true)]
        public NumericParser<float> colorBlend
        {
            get { return mod.colorBlend; }
            set { mod.colorBlend = value; }
        }
    }
    #endregion
}