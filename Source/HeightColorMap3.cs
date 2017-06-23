using System;
using UnityEngine;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration;
using Kopernicus;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using System.Linq;

namespace KLE
{
    //WIP
    namespace DevelopmentPQS
    {
        #region PQSMods

        public class PQSHeightColorMap3 : PQSMod
        {
            public Single blend;
            public LandClass[] landClasses;
            public static Double maximumAltitude;
            public class LandClass
            {
                public String name;
                public Double altStart;
                public Double altEnd;
                public Color color;
                public Double convertAltStart;
                public Double convertAltEnd;
                public Boolean lerpToNext;
                public Double fractalDelta
                {
                    get { return altEnd - altStart; }
                }

                //Initialize
                public LandClass(String name, Double fractalStart, Double fractalEnd, Color baseColor)
                {
                    this.name = name;
                    this.convertAltStart = fractalStart;
                    this.convertAltEnd = fractalEnd;
                    this.color = baseColor;
                }
            }
            public Int32 lcCount
            {
                get { return landClasses.Length; }
            }
            public LandClass SelectLandClassByHeight(Double height, out Int32 index)
            {
                for (Int32 itr = 0; itr < lcCount; itr++)
                {
                    index = itr;
                    if (height >= landClasses[itr].convertAltStart && height <= landClasses[itr].convertAltEnd)
                        return landClasses[itr];
                }
                index = lcCount - 1;
                return landClasses.Last();
            }
            public override void OnVertexBuild(PQS.VertexBuildData data)
            {
                maximumAltitude = sphere.radiusMax;
                Double vHeight = (data.vertHeight - sphere.radiusMin) / sphere.radiusDelta;
                Int32 index;
                LandClass lcSelected = SelectLandClassByHeight(vHeight, out index);
                if (lcSelected.lerpToNext)
                {
                    data.vertColor = Color.Lerp(data.vertColor,
                        Color.Lerp(lcSelected.color, landClasses[index + 1].color,
                        (Single)((vHeight - (lcSelected.convertAltStart / maximumAltitude)) / ((lcSelected.convertAltEnd / maximumAltitude) - (lcSelected.convertAltStart / maximumAltitude)))), blend);
                }
                else
                    data.vertColor = data.vertColor;
                //enter data for else
            }
        }

        #endregion

        #region ModLoader
        [RequireConfigType(ConfigType.Node)]
        public class HeightColorMap3 : ModLoader<PQSHeightColorMap3>
        {
        }
        #endregion
    }
}