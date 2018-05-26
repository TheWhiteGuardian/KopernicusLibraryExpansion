using System;
using System.Collections.Generic;
using UnityEngine;
using Kopernicus;
using Kopernicus.Configuration.ModLoader;
using LibNoise;

namespace KLE
{
    namespace Mods
    {
        public class KLE_SludgeOcean : PQSMod
        {
            public Vector4 red, green, blue;
            Simplex simR, simG, simB;
            public override void OnSetup()
            {
                simR = new Simplex((int)(System.Math.Truncate(red.x)), red.y, red.z, red.w);
                simG = new Simplex((int)(System.Math.Truncate(green.x)), green.y, green.z, green.w);
                simB = new Simplex((int)(System.Math.Truncate(blue.x)), blue.y, blue.z, blue.w);
            }
            public override void OnVertexBuild(PQS.VertexBuildData data)
            {
                data.vertColor = new Color(Convert.ToSingle(simR.noiseNormalized(data.directionFromCenter)), Convert.ToSingle(simG.noiseNormalized(data.directionFromCenter)), Convert.ToSingle(simB.noiseNormalized(data.directionFromCenter)));
            }
        }
    }
    namespace Loaders
    {

    }
}
