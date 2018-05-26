using Kopernicus;
using Kopernicus.Configuration.ModLoader;

namespace KLE
{
    namespace Mods
    {
        public class KLE_AltitudeLimit : PQSMod
        {
            public double maxAltitude;
            double maximum;
            public override void OnSetup()
            {
                //Account for the fact that data.vertHeight counts from sphere center
                maximum = sphere.radius + maxAltitude;
            }
            public override void OnVertexBuildHeight(PQS.VertexBuildData data)
            {
                if (data.vertHeight > maximum)
                {
                    data.vertHeight = maximum;
                }
                //Else do nothing
            }
        }
    }
    namespace Loaders
    {
        [RequireConfigType(ConfigType.Node)]
        public class AltitudeLimit : ModLoader<Mods.KLE_AltitudeLimit>
        {
            [ParserTarget("altitude")]
            public NumericParser<double> altitude
            {
                get { return mod.maxAltitude; }
                set { mod.maxAltitude = value; }
            }
        }
    }
}
