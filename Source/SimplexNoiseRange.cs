using UnityEngine;
using Kopernicus;
using System;
using Kopernicus.Configuration.ModLoader;
namespace KLE
{
    public class PQSMod_SimplexNoiseRange : PQSMod
    {
        public Int32 seed;

        public Double deformity;

        public Double persistence;

        public Double octaves;

        public Double rangeStart;

        public Double frequency;

        public Double rangeEnd;

        private Simplex simplex;

        public override void OnSetup()
        {
            simplex = new Simplex(seed, octaves, persistence, frequency);
        }
        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            if (data.vertHeight < sphere.radius + rangeEnd && data.vertHeight > sphere.radius + rangeStart)
                data.vertHeight += simplex.noise(data.directionFromCenter) * deformity;
        }
        public override Double GetVertexMaxHeight()
        {
            return deformity + rangeEnd;
        }
        public override Double GetVertexMinHeight()
        {
            return rangeStart - deformity;
        }
    }
    [RequireConfigType(ConfigType.Node)]
    public class SimplexNoiseRange : ModLoader<PQSMod_SimplexNoiseRange>
    {
        [ParserTarget("seed", optional = false)]
        public NumericParser<Int32> seed
        {
            get { return mod.seed; }
            set { mod.seed = value; }
        }

        [ParserTarget("deformity", optional = false)]
        public NumericParser<double> deformity
        {
            get { return mod.deformity; }
            set { mod.deformity = value; }
        }

        [ParserTarget("octaves", optional = false)]
        public NumericParser<double> octaves
        {
            get { return mod.octaves; }
            set { mod.octaves = value; }
        }

        [ParserTarget("persistence", optional = false)]
        public NumericParser<double> persistence
        {
            get { return mod.persistence; }
            set { mod.persistence = value; }
        }

        [ParserTarget("rangeStart", optional = false)]
        public NumericParser<double> rangeStart
        {
            get { return mod.rangeStart; }
            set { mod.rangeStart = value; }
        }

        [ParserTarget("rangeEnd", optional = false)]
        public NumericParser<double> rangeEnd
        {
            get { return mod.rangeEnd; }
            set { mod.rangeEnd = value; }
        }
    }
}