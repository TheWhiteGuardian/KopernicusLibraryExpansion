using System;
using UnityEngine;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration;
using Kopernicus;

namespace KLE
{
    #region PQSMods
    public class PQSMod_FlattenRange : PQSMod
    {
        //Range starting altitude
        public Double cutoffStart = 0;

        //Range ending altitude
        public Double cutoffEnd = 0;

        //The altitude that must be flattened to
        public Double flattenTo = 0;

        //for if you want the range to be infinite.
        public Boolean cutoffEndToInfinity = false;


        public Boolean cutoffStartToInfinity = false;

        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            if (cutoffStartToInfinity && cutoffEndToInfinity)
                data.vertHeight = flattenTo + sphere.radius;

            if (cutoffStartToInfinity && !cutoffEndToInfinity)
                if (data.vertHeight <= cutoffEnd + sphere.radius)
                    data.vertHeight = flattenTo + sphere.radius;

            if (!cutoffStartToInfinity && cutoffEndToInfinity)
                if (data.vertHeight >= cutoffStart + sphere.radius)
                    data.vertHeight = flattenTo + sphere.radius;

            if (!cutoffStartToInfinity && !cutoffEndToInfinity)
                if (data.vertHeight >= cutoffStart + sphere.radius && data.vertHeight <= cutoffEnd + sphere.radius)
                    data.vertHeight = flattenTo + sphere.radius;
        }
    }
    #endregion

    #region ModLoader
    [RequireConfigType(ConfigType.Node)]
    public class FlattenRange : ModLoader<PQSMod_FlattenRange>
    {
        [ParserTarget("cutoffStart", optional = true)]
        public NumericParser<double> cutoffStart
        {
            get { return mod.cutoffStart; }
            set { mod.cutoffStart = value; }
        }

        [ParserTarget("cutoffEnd", optional = true)]
        public NumericParser<double> cutoffEnd
        {
            get { return mod.cutoffEnd; }
            set { mod.cutoffEnd = value; }
        }

        [ParserTarget("flattenTo", optional = true)]
        public NumericParser<double> flattenTo
        {
            get { return mod.flattenTo; }
            set { mod.flattenTo = value; }
        }

        [ParserTarget("cutoffEndToInfinity", optional = true)]
        public NumericParser<bool> cutoffEndToInfinity
        {
            get { return mod.cutoffEndToInfinity; }
            set { mod.cutoffEndToInfinity = value; }
        }

        [ParserTarget("cutoffStartToInfinity", optional = true)]
        public NumericParser<bool> cutoffStartToInfinity
        {
            get { return mod.cutoffStartToInfinity; }
            set { mod.cutoffStartToInfinity = value; }
        }
    }
    #endregion
}