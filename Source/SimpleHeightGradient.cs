using UnityEngine;
using Kopernicus;
using Kopernicus.Configuration.ModLoader;

namespace KLE
{
    namespace Mods
    {
        public class KLE_SimpleHeightGradient : PQSMod
        {
            public Color colorLow, colorHigh;
            public float blend;
            public override void OnVertexBuild(PQS.VertexBuildData data)
            {
                data.vertColor = Color.Lerp(data.vertColor, Color.Lerp(colorLow, colorHigh, Mathf.Clamp01(((float)((data.vertHeight - sphere.radiusMin) / sphere.radiusMax)))), blend);
            }
        }
    }
    namespace Loaders
    {
        [RequireConfigType(ConfigType.Node)]
        public class SimpleHeightGradient : ModLoader<Mods.KLE_SimpleHeightGradient>
        {
            [ParserTarget("colorStart")]
            public ColorParser colorStart
            {
                get { return mod.colorLow; }
                set { mod.colorLow = value; }
            }
            [ParserTarget("colorEnd")]
            public ColorParser colorEnd
            {
                get { return mod.colorHigh; }
                set { mod.colorHigh = value; }
            }
            [ParserTarget("blend")]
            public NumericParser<float> blend
            {
                get { return mod.blend; }
                set { mod.blend = value; }
            }
        }
    }
}
