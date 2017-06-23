using System;
using UnityEngine;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration;
using Kopernicus;

namespace TWG
{
    #region PQSMods
    public class PQSMod_CoastlineSmoother : PQSMod
    {
        /*
         A gradient is created between startingAltitude and EndingAltitude.
         The mod is ONLY active between those two altitudes.
         */

        //The starting altitude (m)
        public Double startingAltitude = 0;
        //The ending altitude (m)
        public Double endingAltitude = 0;

        //If you want to use a map to specify very specific beach areas.
        public Boolean useMap = false;
        //Greyscale map, white = active.
        public MapSO map;

        //The strength. Increase this number for even stronger deformation.
        public Int32 strength = 0;

        //Add extra altitude if you want
        public Double offset = 0;

        //Internals
        private Double finalStart;
        private Double finalEnd;
        private Double delta;
        public Single cutoff = 1f;


        //Called when the mod is activated.
        public override void OnSetup()
        {
            //Simplicity
            finalStart = startingAltitude + sphere.radius;
            finalEnd = endingAltitude + sphere.radius;
            delta = endingAltitude - startingAltitude;

            //Safety precaution for smart_ASS Kerbals... lookin' at you, Jeb!
            if (finalStart > finalEnd)
                throw new ArgumentException("startingAltitude should be smaller than endingAltitude", nameof(startingAltitude));

            if (useMap)
            {
                if (map == null)
                    throw new ArgumentNullException(nameof(map));
            }
        }


        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            if (useMap)
            {
                if (map.GetPixelFloat(data.u, data.v) == cutoff)
                {
                    //Get active range
                    if (data.vertHeight > finalStart && data.vertHeight < finalEnd)
                    {
                        //Current altitude to altitude ASL
                        var altitude = data.vertHeight - sphere.radius;

                        //Altitude ASL -> position in range
                        var position = altitude / endingAltitude;

                        //To inversely exponential curvepoint
                        var FloatCurve = (Math.Pow(position, 2)) + (-2 * position) + 1;

                        //Adapt strength to position on curve
                        var strengthConv = FloatCurve * (1 / strength);

                        //Get altitude fraction
                        var NewAlt = Math.Pow(altitude, strengthConv);

                        //Finalize
                        data.vertHeight = (altitude / NewAlt) + sphere.radius + offset;

                        //Ninja'd
                    }
                }
            }
            else
            {
                //Get active range
                if (data.vertHeight > finalStart && data.vertHeight < finalEnd)
                {
                    //Current altitude to altitude ASL
                    var altitude = data.vertHeight - sphere.radius;

                    //Altitude ASL -> position in range
                    var position = altitude / endingAltitude;

                    //To inversely exponential curvepoint
                    var FloatCurve = (Math.Pow(position, 2)) + (-2 * position) + 1;

                    //Adapt strength to position on curve
                    var strengthConv = FloatCurve * (1 / strength);

                    //Get altitude fraction
                    var NewAlt = Math.Pow(altitude, strengthConv);

                    //Finalize
                    data.vertHeight = (altitude / NewAlt) + sphere.radius + offset;

                    //Ninja'd again
                }
            }
        }
    }
    #endregion

    #region ModLoader
    //Load it up, Scotty!

    //#LameJokes
    [RequireConfigType(ConfigType.Node)]
    public class CoastlineSmoother : ModLoader<PQSMod_CoastlineSmoother>
    {
        [ParserTarget("startingAltitude", optional = true)]
        public NumericParser<double> startingAltitude
        {
            get { return mod.startingAltitude; }
            set { mod.startingAltitude = value; }
        }
        [ParserTarget("endingAltitude", optional = true)]
        public NumericParser<double> endingAltitude
        {
            get { return mod.endingAltitude; }
            set { mod.endingAltitude = value; }
        }
        [ParserTarget("strength", optional = true)]
        public NumericParser<int> strength
        {
            get { return mod.strength; }
            set { mod.strength = value; }
        }
        [ParserTarget("offset", optional = true)]
        public NumericParser<double> offset
        {
            get { return mod.offset; }
            set { mod.offset = value; }
        }
        [ParserTarget("useMap", optional = true)]
        public NumericParser<bool> useMap
        {
            get { return mod.useMap; }
            set { mod.useMap = value; }
        }
        [ParserTarget("map")]
        public MapSOParser_GreyScale<MapSO> map
        {
            get { return mod.map; }
            set { mod.map = value; }
        }
        [ParserTarget("cutoff", optional = true)]
        public NumericParser<float> cutoff
        {
            get { return mod.cutoff; }
            set { mod.cutoff = value; }
        }
    }
    #endregion
}