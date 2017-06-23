using UnityEngine;
using Kopernicus;
using System;
using Kopernicus.Configuration.ModLoader;
using LibNoise.Unity.Generator;
using LibNoise.Unity;
using KLE.Noise;

namespace KLE
{
    #region Mods
    /*
    public class PQSMod_RandomMapDecal : PQSMod
    {
        public Int32 maxDeformity;
        public Int32 minDeformity;
        public Double maxRadius;
        public Double minRadius;
        public Single cutoff;
        public MapSO map;

        public Double frequency;
        public Int32 seed;
        public Double octaves;
        public Double persistence;

        public Single radiusDeformityRelationship;

        private Simplex placementSimplex;
        private Int32 rInt;

        public override void OnSetup()
        {
            placementSimplex = new Simplex(seed, octaves, persistence, frequency);

            //Create random number in range
            System.Random r = new System.Random();
            rInt = r.Next(minDeformity, maxDeformity);
            

            
        }

        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            //Noise gen placer
            if (placementSimplex.noise(data.directionFromCenter) > cutoff)
            {
            }
        }

    }
    public class PQSMod_LandControlMap : PQSMod
    {
        public MapSO indexMap;
        public class LandClass
        {
            public string name;
        }
    }
    */
    public class PQSMod_Ellipsoid : PQSMod
    {
        public Boolean scaleDeformityByRadius;

        public Double AxisA;
        public Double AxisB;
        public Double AxisC;

        private Double axisA;
        private Double axisB;
        private Double axisC;
        
        public override void OnSetup()
        {
            if (scaleDeformityByRadius)
            {
                axisA = AxisA * sphere.radius;
                axisB = AxisB * sphere.radius;
                axisC = AxisC * sphere.radius;
            }
            if (!scaleDeformityByRadius)
            {
                axisA = AxisA;
                axisB = AxisB;
                axisC = AxisC;
            }
        }

        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            //data.vertHeight += (data.directionXZ.x * axisA) + (data.directionXZ.y * axisB) + (data.directionXZ.z * axisC);
            data.vertHeight += (data.directionFromCenter.x * axisA) + (data.directionFromCenter.y * axisB) + (data.directionFromCenter.z * axisC);
        }

    }
    public class PQSMod_OffsetTerrain : PQSMod
    {
        public Double latitudeOffset;
        public Double longitudeOffset;
        public Double altitudeOffset;

        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            data.longitude += longitudeOffset;
            data.latitude += latitudeOffset;
            data.vertHeight += altitudeOffset;
        }
    }
    /*
    public class PQSMod_Rivers : PQSMod
    {

    }
    */
    public class PQSMod_AltitudeLimiter : PQSMod
    {
        public Double maxAltitude = 2000;
        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            if (data.vertHeight >= maxAltitude + sphere.radius)
                data.vertHeight = maxAltitude + sphere.radius;
        }
    }
    #endregion





    #region Loader
    [RequireConfigType(ConfigType.Node)]
    public class AltitudeLimiter : ModLoader<PQSMod_AltitudeLimiter>
    {
        [ParserTarget("maxAltitude", optional = true)]
        public NumericParser<double> maxAltitude
        {
            get { return mod.maxAltitude; }
            set { mod.maxAltitude = value; }
        }
    }

    [RequireConfigType(ConfigType.Node)]
    public class OffsetTerrain : ModLoader<PQSMod_OffsetTerrain>
    {
        [ParserTarget("latitudeOffset", optional = true)]
        public NumericParser<double> latitudeOffset
        {
            get { return mod.latitudeOffset; }
            set { mod.latitudeOffset = value; }
        }
        [ParserTarget("longitudeOffset", optional = true)]
        public NumericParser<double> longitudeOffset
        {
            get { return mod.longitudeOffset; }
            set { mod.longitudeOffset = value; }
        }
        [ParserTarget("altitudeOffset", optional = true)]
        public NumericParser<double> altitudeOffset
        {
            get { return mod.altitudeOffset; }
            set { mod.altitudeOffset = value; }
        }
    }

    [RequireConfigType(ConfigType.Node)]
    public class Ellipsoid : ModLoader<PQSMod_Ellipsoid>
    {
        [ParserTarget("AxisA", optional = true)]
        public NumericParser<double> AxisA
        {
            get { return mod.AxisA; }
            set { mod.AxisA = value; }
        }
        [ParserTarget("AxisB", optional = true)]
        public NumericParser<double> AxisB
        {
            get { return mod.AxisB; }
            set { mod.AxisB = value; }
        }
        [ParserTarget("AxisC", optional = true)]
        public NumericParser<double> AxisC
        {
            get { return mod.AxisC; }
            set { mod.AxisC = value; }
        }
        [ParserTarget("scaleDeformityByRadius", optional = true)]
        public NumericParser<bool> scaleDeformityByRadius
        {
            get { return mod.scaleDeformityByRadius; }
            set { mod.scaleDeformityByRadius = value; }
        }
    }
    #endregion
}