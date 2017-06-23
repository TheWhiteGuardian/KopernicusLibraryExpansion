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
    public class PQSMod_SmoothenAltitude : PQSMod
    {
        public Double maxAltitude = 10;
        public Double minAltitude = 0;
        public Double strength = 0;
        public Boolean limitEffectToRange = true;
        private Double average;

        public override void OnSetup()
        {
            if (maxAltitude < minAltitude)
            {
                throw new ArgumentException("The maximum should be larger than the minimum.", nameof(maxAltitude));
            }
            average = maxAltitude - minAltitude;
        }
        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            if (limitEffectToRange == true)
            {
                if (data.vertHeight > minAltitude + sphere.radius && data.vertHeight < maxAltitude + sphere.radius)
                {
                    if (data.vertHeight > average + sphere.radius)
                    {
                        data.vertHeight = data.vertHeight / average;
                    }
                    if (data.vertHeight < average + sphere.radius)
                    {
                        data.vertHeight = data.vertHeight * average;
                    }
                }
            }
            if (limitEffectToRange == false)
            {
                if (data.vertHeight > average + sphere.radius)
                {
                    data.vertHeight = data.vertHeight / average;
                }
                if (data.vertHeight < average + sphere.radius)
                {
                    data.vertHeight = data.vertHeight * average;
                }
            }
        }
    }
    #endregion

    #region ModLoader
    [RequireConfigType(ConfigType.Node)]
    public class SmoothenAltitude : ModLoader<PQSMod_SmoothenAltitude>
    {
        [ParserTarget("maxAltitude", optional = true)]
        public NumericParser<double> maxAltitude
        {
            get { return mod.maxAltitude; }
            set { mod.maxAltitude = value; }
        }
        [ParserTarget("minAltitude", optional = true)]
        public NumericParser<double> minAltitude
        {
            get { return mod.minAltitude; }
            set { mod.minAltitude = value; }
        }
        [ParserTarget("strength", optional = true)]
        public NumericParser<double> strength
        {
            get { return mod.strength; }
            set { mod.strength = value; }
        }
        [ParserTarget("limitEffectToRainge", optional = true)]
        public NumericParser<bool> limitEffectToRange
        {
            get { return mod.limitEffectToRange; }
            set { mod.limitEffectToRange = value; }
        }
    }
    #endregion
}