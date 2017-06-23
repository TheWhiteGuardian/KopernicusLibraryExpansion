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
    public class PQSMod_MapArtist : PQSMod
    {
        public MapSO heightMap;
        public MapSO colorMap;
        public Double heightmapDeformity = 0;
        public Boolean scaleDeformityByRadius = false;
        public Boolean addSimplexNoise = false;
        public Boolean smoothHeightMap = false;
        public Double offset = 0;
        private Simplex simplex1;
        private Simplex simplex2;
        private Double heightDeformity;

        #region simplexNoise
        public Int32 seed = 1000;
        public Double deformity = 100;
        public Int32 octaves = 2;
        public Double persistence = 0.5;
        public Double frequency = 15;
        #endregion

        #region Smoothvalues
        private Int32 smoothSeed = 2217;
        public Double smoothDeformity = 0;
        private Int32 smoothOctaves;
        private Double smoothPersistence = 0.4;
        private Double smoothFrequency;
        #endregion

        public override void OnSetup()
        {
            //AddSimplexNoise
            simplex1 = new Simplex(seed, octaves, persistence, frequency);
            //SmoothHeightMap
            simplex2 = new Simplex(smoothSeed, smoothOctaves, smoothPersistence, smoothFrequency);

            #region setSmoothDeformity
            if (smoothDeformity == 0)
            {
                if (deformity < 500)
                    smoothDeformity = 20;
                if (deformity > 500 && deformity < 2000)
                    smoothDeformity = 200;
                if (deformity > 2000 && deformity < 4000)
                    smoothDeformity = 400;
                if (deformity > 4000 && deformity < 8000)
                    smoothDeformity = deformity / 10;
                if (deformity > 8000)
                    smoothDeformity = deformity;

            }
            #endregion

            #region setSmoothValues
            if (sphere.radius < 500000)
            {
                smoothOctaves = 2;
                smoothFrequency = 50;
            }
            if (sphere.radius > 50000 && sphere.radius < 100000)
            {
                smoothOctaves = 6;
                smoothFrequency = 70;
            }
            if (sphere.radius > 100000 && sphere.radius < 200000)
            {
                smoothOctaves = 12;
                smoothFrequency = 200;
            }
            if (sphere.radius > 200000)
            {
                smoothOctaves = 24;
                smoothFrequency = 400;
            }
            #endregion

            if (heightMap == null)
                throw new ArgumentNullException(nameof(heightMap));
            if (colorMap == null)
                throw new ArgumentNullException(nameof(colorMap));
            if (scaleDeformityByRadius)
                heightDeformity = sphere.radius * heightmapDeformity;
            if (!scaleDeformityByRadius)
                heightDeformity = heightmapDeformity;
        }
        public override void OnVertexBuild(PQS.VertexBuildData data)
        {
            data.vertColor = colorMap.GetPixelColor(data.u, data.v);
        }

        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            if (addSimplexNoise == true && smoothHeightMap == true)
            {
                data.vertHeight = data.vertHeight + (simplex1.noise(data.directionFromCenter) * deformity) + (simplex2.noise(data.directionFromCenter) * smoothDeformity) + offset + (heightDeformity * heightMap.GetPixelFloat(data.u, data.v));
            }
            if (addSimplexNoise == true && smoothHeightMap == false)
            {
                data.vertHeight = data.vertHeight + (simplex1.noise(data.directionFromCenter) * deformity) + offset + (heightDeformity * heightMap.GetPixelFloat(data.u, data.v));
            }
            if (addSimplexNoise == false && smoothHeightMap == true)
            {
                data.vertHeight = data.vertHeight + (simplex2.noise(data.directionFromCenter) * smoothDeformity) + offset + (heightDeformity * heightMap.GetPixelFloat(data.u, data.v));
            }
            if (addSimplexNoise == false && smoothHeightMap == false)
            {
                data.vertHeight = data.vertHeight + offset + (heightDeformity * heightMap.GetPixelFloat(data.u, data.v));
            }
        }
        public override Double GetVertexMaxHeight()
        {
            if (addSimplexNoise == true && smoothHeightMap == true)
            {
                return heightDeformity + smoothDeformity + deformity + offset;
            }
            if (addSimplexNoise == true && smoothHeightMap == false)
            {
                return heightDeformity + deformity + offset;
            }
            if (addSimplexNoise == false && smoothHeightMap == true)
            {
                return heightDeformity + smoothDeformity + offset;
            }
            if (addSimplexNoise == false && smoothHeightMap == false)
            {
                return heightDeformity + offset;
            }
            else
                return offset;
        }
    }
    #endregion

    #region ModLoader
    [RequireConfigType(ConfigType.Node)]
    public class MapArtist : ModLoader<PQSMod_MapArtist>
    {
        [ParserTarget("heightMap", optional = false)]
        public MapSOParser_GreyScale<MapSO> heightMap
        {
            get { return mod.heightMap; }
            set { mod.heightMap = value; }
        }
        [ParserTarget("colorMap", optional = true)]
        public MapSOParser_GreyScale<MapSO> colorMap
        {
            get { return mod.colorMap; }
            set { mod.colorMap = value; }
        }
        [ParserTarget("heightMapDeformity", optional = true)]
        public NumericParser<double> heightMapDeformity
        {
            get { return mod.heightmapDeformity; }
            set { mod.heightmapDeformity = value; }
        }
        [ParserTarget("scaleDeformityByRadius", optional = true)]
        public NumericParser<bool> scaleDeformityByRadius
        {
            get { return mod.scaleDeformityByRadius; }
            set { mod.scaleDeformityByRadius = value; }
        }
        [ParserTarget("addSimplexNoise", optional = true)]
        public NumericParser<bool> addSimplexNoise
        {
            get { return mod.addSimplexNoise; }
            set { mod.addSimplexNoise = value; }
        }
        [ParserTarget("smoothHeightMap", optional = true)]
        public NumericParser<bool> smoothHeightMap
        {
            get { return mod.smoothHeightMap; }
            set { mod.smoothHeightMap = value; }
        }
        [ParserTarget("offset", optional = true)]
        public NumericParser<double> offset
        {
            get { return mod.offset; }
            set { mod.offset = value; }
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
        [ParserTarget("persistence", optional = true)]
        public NumericParser<double> persistence
        {
            get { return mod.persistence; }
            set { mod.persistence = value; }
        }
        [ParserTarget("frequency", optional = true)]
        public NumericParser<double> frequency
        {
            get { return mod.frequency; }
            set { mod.frequency = value; }
        }
        [ParserTarget("smoothDeformity", optional = true)]
        public NumericParser<double> smoothDeformity
        {
            get { return mod.smoothDeformity; }
            set { mod.smoothDeformity = value; }
        }
    }
    #endregion
}