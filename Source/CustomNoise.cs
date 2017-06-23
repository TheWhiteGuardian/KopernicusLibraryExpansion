using System;
using UnityEngine;
using Kopernicus;
using Kopernicus.Configuration.ModLoader;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using Kopernicus.Configuration;

namespace KLE
{
    public class PQSMod_CustomNoise : PQSMod
    {
        #region configLayout
        public Double frequency;
        public Int32 octaves;
        public Int32 seed;
        public QualityMode mode;
        public Double lacunarity;
        public Double deformity;
        public Double persistence;

        public Vector2 pass1;
        public Vector2 pass2;
        public Vector2 pass3;
        public Vector2 pass4;
        public Vector2 pass5;
        public Vector2 pass6;
        public Vector2 pass7;
        public Vector2 pass8;
        public Vector2 pass9;
        public Vector2 pass10;

        /* legend
        0 = Add
        1 = Subtract
        2 = Multiply
        3 = Divide
        4 = Power
        5 = Absolute value
        6 = Sine
        7 = Cosine

         */
        public Boolean addColor;
        public Boolean addHeight;
        public Color colorStart;
        public Color colorEnd;
        public Single blend;
        private ModuleBase Noise;
        #endregion

        public override void OnSetup()
        {
            Noise = new CustomNoise(frequency, lacunarity, seed, octaves, mode, persistence, pass1, pass2, pass3, pass4, pass5, pass6, pass7, pass8, pass9, pass10);
        }

        public override void OnVertexBuild(PQS.VertexBuildData data)
        {
            if (addColor)
            {
                var colorFloat = Convert.ToSingle(Noise = new CustomNoise(frequency, lacunarity, seed, octaves, mode, persistence, pass1, pass2, pass3, pass4, pass5, pass6, pass7, pass8, pass9, pass10));
                var finalColor = Color.Lerp(colorStart, colorEnd, colorFloat);
                data.vertColor = Color.Lerp(finalColor, data.vertColor, blend);
            }
        }

        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {

            if (addHeight)
            {
                data.vertHeight += Noise.GetValue(data.directionFromCenter) * deformity;
            }
        }

        public class CustomNoise : ModuleBase
        {
            #region Fields
            private Double _frequency = 1.0;
            private Double _lacunarity = 2.0;
            private Int32 _seed;
            private Int32 _octaveValue = 6;
            private QualityMode _noiseQuality = QualityMode.Medium;
            private Double _persistence;
            private Int32 _passCount;


            private Vector2 _pass1;
            private Vector2 _pass2;
            private Vector2 _pass3;
            private Vector2 _pass4;
            private Vector2 _pass5;
            private Vector2 _pass6;
            private Vector2 _pass7;
            private Vector2 _pass8;
            private Vector2 _pass9;
            private Vector2 _pass10;
            #endregion

            #region Constructors
            public CustomNoise()
                : base(0)
            {
            }
            public CustomNoise(Double frequency, Double lacunarity, Int32 seed, Int32 octaves, QualityMode mode, Double persistence,
                Vector2 pass1, Vector2 pass2, Vector2 pass3, Vector2 pass4, Vector2 pass5, Vector2 pass6, Vector2 pass7, Vector2 pass8,
                Vector2 pass9, Vector2 pass10)
                : base(0)
            {
                Frequency = frequency;
                Lacunarity = lacunarity;
                Seed = seed;
                OctaveCount = octaves;
                NoiseMode = mode;
                Persistence = persistence;
                Edit1 = pass1;
                Edit2 = pass2;
                Edit3 = pass3;
                Edit4 = pass4;
                Edit5 = pass5;
                Edit6 = pass6;
                Edit7 = pass7;
                Edit8 = pass8;
                Edit9 = pass9;
                Edit10 = pass10;
            }
            #endregion

            #region Properties
            public Double Frequency
            {
                get { return _frequency; }
                set { _frequency = value; }
            }
            public Double Lacunarity
            {
                get { return _lacunarity; }
                set { _lacunarity = value; }
            }
            public Int32 PassCount
            {
                get { return _passCount; }
                set { _passCount = value; }
            }
            public Int32 Seed
            {
                get { return _seed; }
                set { _seed = value; }
            }
            public Int32 OctaveCount
            {
                get { return _octaveValue; }
                set { _octaveValue = Mathf.Clamp(value, 1, Utils.OctavesMaximum); }
            }
            public QualityMode NoiseMode
            {
                get { return _noiseQuality; }
                set { _noiseQuality = value; }
            }
            public Double Persistence
            {
                get { return _persistence; }
                set { _persistence = value; }
            }

            public Vector2 Edit1
            {
                get { return _pass1; }
                set { _pass1 = value; }
            }
            public Vector2 Edit2
            {
                get { return _pass2; }
                set { _pass2 = value; }
            }
            public Vector2 Edit3
            {
                get { return _pass3; }
                set { _pass3 = value; }
            }
            public Vector2 Edit4
            {
                get { return _pass4; }
                set { _pass4 = value; }
            }
            public Vector2 Edit5
            {
                get { return _pass5; }
                set { _pass5 = value; }
            }
            public Vector2 Edit6
            {
                get { return _pass6; }
                set { _pass6 = value; }
            }
            public Vector2 Edit7
            {
                get { return _pass7; }
                set { _pass7 = value; }
            }
            public Vector2 Edit8
            {
                get { return _pass8; }
                set { _pass8 = value; }
            }
            public Vector2 Edit9
            {
                get { return _pass9; }
                set { _pass9 = value; }
            }
            public Vector2 Edit10
            {
                get { return _pass10; }
                set { _pass10 = value; }
            }
            #endregion

            public override double GetValue(double x, double y, double z)
            {
                Vector2 defaultValue = new Vector2(0, 0);
                var value = 0.0;
                var persVal = 1.0;
                x *= _frequency;
                y *= _frequency;
                z *= _lacunarity;

                for (int i = 0; i < OctaveCount; i++)
                {
                    var rx = Utils.MakeInt32Range(x);
                    var ry = Utils.MakeInt32Range(y);
                    var rz = Utils.MakeInt32Range(z);
                    var seed2 = (_seed + i) & 0xffffffff;
                    var signal = Utils.GradientCoherentNoise3D(rx, ry, rz, seed2, _noiseQuality);

                    #region user-entered stuff

                    if(!(Edit1 == defaultValue))
                    {
                        if(Edit1.x == 0)
                        {
                            signal += Edit1.y;
                        }
                        if(Edit1.x == 1)
                        {
                            signal -= Edit1.y;
                        }
                        if(Edit1.x == 2)
                        {
                            signal *= Edit1.y;
                        }
                        if(Edit1.x == 3)
                        {
                            signal /= Edit1.y;
                        }
                        if(Edit1.x == 4)
                        {
                            if(Edit1.y > 0 && Edit1.y < 1 && signal < 0)
                            {
                                signal = Math.Pow(Math.Abs(signal), Edit1.y);
                            }
                            else
                            {
                                signal = Math.Pow(signal, Edit1.y);
                            }
                        }
                        if(Edit1.x == 5)
                        {
                            signal = Math.Abs(signal);
                        }
                        if(Edit1.x == 6)
                        {
                            signal = Math.Sin(signal);
                        }
                        if(Edit1.x >= 7)
                        {
                            signal = Math.Cos(signal);
                        }
                    }



                    if (!(Edit2 == defaultValue))
                    {
                        if (Edit2.x == 0)
                        {
                            signal += Edit2.y;
                        }
                        if (Edit2.x == 1)
                        {
                            signal -= Edit2.y;
                        }
                        if (Edit2.x == 2)
                        {
                            signal *= Edit2.y;
                        }
                        if (Edit2.x == 3)
                        {
                            signal /= Edit2.y;
                        }
                        if (Edit2.x == 4)
                        {
                            if (Edit2.y > 0 && Edit2.y < 1 && signal < 0)
                            {
                                signal = Math.Pow(Math.Abs(signal), Edit2.y);
                            }
                            else
                            {
                                signal = Math.Pow(signal, Edit2.y);
                            }
                        }
                        if (Edit2.x == 5)
                        {
                            signal = Math.Abs(signal);
                        }
                        if (Edit2.x == 6)
                        {
                            signal = Math.Sin(signal);
                        }
                        if (Edit2.x >= 7)
                        {
                            signal = Math.Cos(signal);
                        }
                    }



                    if (!(Edit3 == defaultValue))
                    {
                        if (Edit3.x == 0)
                        {
                            signal += Edit3.y;
                        }
                        if (Edit3.x == 1)
                        {
                            signal -= Edit3.y;
                        }
                        if (Edit3.x == 2)
                        {
                            signal *= Edit3.y;
                        }
                        if (Edit3.x == 3)
                        {
                            signal /= Edit3.y;
                        }
                        if (Edit3.x == 4)
                        {
                            if (Edit3.y > 0 && Edit3.y < 1 && signal < 0)
                            {
                                signal = Math.Pow(Math.Abs(signal), Edit3.y);
                            }
                            else
                            {
                                signal = Math.Pow(signal, Edit3.y);
                            }
                        }
                        if (Edit3.x == 5)
                        {
                            signal = Math.Abs(signal);
                        }
                        if (Edit3.x == 6)
                        {
                            signal = Math.Sin(signal);
                        }
                        if (Edit3.x >= 7)
                        {
                            signal = Math.Cos(signal);
                        }
                    }



                    if (!(Edit4 == defaultValue))
                    {
                        if (Edit4.x == 0)
                        {
                            signal += Edit4.y;
                        }
                        if (Edit4.x == 1)
                        {
                            signal -= Edit4.y;
                        }
                        if (Edit4.x == 2)
                        {
                            signal *= Edit4.y;
                        }
                        if (Edit4.x == 3)
                        {
                            signal /= Edit4.y;
                        }
                        if (Edit4.x == 4)
                        {
                            if (Edit4.y > 0 && Edit4.y < 1 && signal < 0)
                            {
                                signal = Math.Pow(Math.Abs(signal), Edit4.y);
                            }
                            else
                            {
                                signal = Math.Pow(signal, Edit4.y);
                            }
                        }
                        if (Edit4.x == 5)
                        {
                            signal = Math.Abs(signal);
                        }
                        if (Edit4.x == 6)
                        {
                            signal = Math.Sin(signal);
                        }
                        if (Edit4.x >= 7)
                        {
                            signal = Math.Cos(signal);
                        }
                    }



                    if (!(Edit5 == defaultValue))
                    {
                        if (Edit5.x == 0)
                        {
                            signal += Edit5.y;
                        }
                        if (Edit5.x == 1)
                        {
                            signal -= Edit5.y;
                        }
                        if (Edit5.x == 2)
                        {
                            signal *= Edit5.y;
                        }
                        if (Edit5.x == 3)
                        {
                            signal /= Edit5.y;
                        }
                        if (Edit5.x == 4)
                        {
                            if (Edit5.y > 0 && Edit5.y < 1 && signal < 0)
                            {
                                signal = Math.Pow(Math.Abs(signal), Edit5.y);
                            }
                            else
                            {
                                signal = Math.Pow(signal, Edit5.y);
                            }
                        }
                        if (Edit5.x == 5)
                        {
                            signal = Math.Abs(signal);
                        }
                        if (Edit5.x == 6)
                        {
                            signal = Math.Sin(signal);
                        }
                        if (Edit5.x >= 7)
                        {
                            signal = Math.Cos(signal);
                        }
                    }



                    if (!(Edit6 == defaultValue))
                    {
                        if (Edit6.x == 0)
                        {
                            signal += Edit6.y;
                        }
                        if (Edit6.x == 1)
                        {
                            signal -= Edit6.y;
                        }
                        if (Edit6.x == 2)
                        {
                            signal *= Edit6.y;
                        }
                        if (Edit6.x == 3)
                        {
                            signal /= Edit6.y;
                        }
                        if (Edit6.x == 4)
                        {
                            if (Edit6.y > 0 && Edit6.y < 1 && signal < 0)
                            {
                                signal = Math.Pow(Math.Abs(signal), Edit6.y);
                            }
                            else
                            {
                                signal = Math.Pow(signal, Edit6.y);
                            }
                        }
                        if (Edit6.x == 5)
                        {
                            signal = Math.Abs(signal);
                        }
                        if (Edit6.x == 6)
                        {
                            signal = Math.Sin(signal);
                        }
                        if (Edit6.x >= 7)
                        {
                            signal = Math.Cos(signal);
                        }
                    }



                    if (!(Edit7 == defaultValue))
                    {
                        if (Edit7.x == 0)
                        {
                            signal += Edit7.y;
                        }
                        if (Edit7.x == 1)
                        {
                            signal -= Edit7.y;
                        }
                        if (Edit7.x == 2)
                        {
                            signal *= Edit7.y;
                        }
                        if (Edit7.x == 3)
                        {
                            signal /= Edit7.y;
                        }
                        if (Edit7.x == 4)
                        {
                            if (Edit7.y > 0 && Edit7.y < 1 && signal < 0)
                            {
                                signal = Math.Pow(Math.Abs(signal), Edit7.y);
                            }
                            else
                            {
                                signal = Math.Pow(signal, Edit7.y);
                            }
                        }
                        if (Edit7.x == 5)
                        {
                            signal = Math.Abs(signal);
                        }
                        if (Edit7.x == 6)
                        {
                            signal = Math.Sin(signal);
                        }
                        if (Edit7.x >= 7)
                        {
                            signal = Math.Cos(signal);
                        }
                    }



                    if (!(Edit8 == defaultValue))
                    {
                        if (Edit8.x == 0)
                        {
                            signal += Edit8.y;
                        }
                        if (Edit8.x == 1)
                        {
                            signal -= Edit8.y;
                        }
                        if (Edit8.x == 2)
                        {
                            signal *= Edit8.y;
                        }
                        if (Edit8.x == 3)
                        {
                            signal /= Edit8.y;
                        }
                        if (Edit8.x == 4)
                        {
                            if (Edit8.y > 0 && Edit8.y < 1 && signal < 0)
                            {
                                signal = Math.Pow(Math.Abs(signal), Edit8.y);
                            }
                            else
                            {
                                signal = Math.Pow(signal, Edit8.y);
                            }
                        }
                        if (Edit8.x == 5)
                        {
                            signal = Math.Abs(signal);
                        }
                        if (Edit8.x == 6)
                        {
                            signal = Math.Sin(signal);
                        }
                        if (Edit8.x >= 7)
                        {
                            signal = Math.Cos(signal);
                        }
                    }



                    if (!(Edit9 == defaultValue))
                    {
                        if (Edit9.x == 0)
                        {
                            signal += Edit9.y;
                        }
                        if (Edit9.x == 1)
                        {
                            signal -= Edit9.y;
                        }
                        if (Edit9.x == 2)
                        {
                            signal *= Edit9.y;
                        }
                        if (Edit9.x == 3)
                        {
                            signal /= Edit9.y;
                        }
                        if (Edit9.x == 4)
                        {
                            if (Edit9.y > 0 && Edit9.y < 1 && signal < 0)
                            {
                                signal = Math.Pow(Math.Abs(signal), Edit9.y);
                            }
                            else
                            {
                                signal = Math.Pow(signal, Edit9.y);
                            }
                        }
                        if (Edit9.x == 5)
                        {
                            signal = Math.Abs(signal);
                        }
                        if (Edit9.x == 6)
                        {
                            signal = Math.Sin(signal);
                        }
                        if (Edit9.x >= 7)
                        {
                            signal = Math.Cos(signal);
                        }
                    }



                    if (!(Edit10 == defaultValue))
                    {
                        if (Edit10.x == 0)
                        {
                            signal += Edit10.y;
                        }
                        if (Edit10.x == 1)
                        {
                            signal -= Edit10.y;
                        }
                        if (Edit10.x == 2)
                        {
                            signal *= Edit10.y;
                        }
                        if (Edit10.x == 3)
                        {
                            signal /= Edit10.y;
                        }
                        if (Edit10.x == 4)
                        {
                            if (Edit10.y > 0 && Edit10.y < 1 && signal < 0)
                            {
                                signal = Math.Pow(Math.Abs(signal), Edit10.y);
                            }
                            else
                            {
                                signal = Math.Pow(signal, Edit10.y);
                            }
                        }
                        if (Edit10.x == 5)
                        {
                            signal = Math.Abs(signal);
                        }
                        if (Edit10.x == 6)
                        {
                            signal = Math.Sin(signal);
                        }
                        if (Edit10.x >= 7)
                        {
                            signal = Math.Cos(signal);
                        }
                    }
                    #endregion

                    value += signal * persVal;
                    x *= _lacunarity;
                    y *= _lacunarity;
                    z *= _lacunarity;
                    persVal *= _persistence;
                }
                return value;
            }
        }
    }

    [RequireConfigType(ConfigType.Node)]
    public class CustomNoise : ModLoader<PQSMod_CustomNoise>
    {
        [ParserTarget("frequency", optional = true)]
        public NumericParser<double> frequency
        {
            get { return mod.frequency; }
            set { mod.frequency = value; }
        }
        [ParserTarget("octaves", optional = true)]
        public NumericParser<int> octaves
        {
            get { return mod.octaves; }
            set { mod.octaves = value; }
        }
        [ParserTarget("seed", optional = true)]
        public NumericParser<int> seed
        {
            get { return mod.seed; }
            set { mod.seed = value; }
        }
        [ParserTarget("mode", optional = true)]
        public NumericParser<QualityMode> mode
        {
            get { return mod.mode; }
            set { mod.mode = value; }
        }
        [ParserTarget("lacunarity", optional = true)]
        public NumericParser<double> lacunarity
        {
            get { return mod.lacunarity; }
            set { mod.lacunarity = value; }
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
        [ParserTarget("addColor", optional = true)]
        public NumericParser<bool> addColor
        {
            get { return mod.addColor; }
            set { mod.addColor = value; }
        }
        [ParserTarget("addHeight", optional = true)]
        public NumericParser<bool> addHeight
        {
            get { return mod.addHeight; }
            set { mod.addHeight = value; }
        }
        [ParserTarget("colorStart", optional = true)]
        public NumericParser<Color> colorStart
        {
            get { return mod.colorStart; }
            set { mod.colorStart = value; }
        }
        [ParserTarget("colorEnd", optional = true)]
        public NumericParser<Color> colorEnd
        {
            get { return mod.colorEnd; }
            set { mod.colorEnd = value; }
        }
        [ParserTarget("blend", optional = true)]
        public NumericParser<float> blend
        {
            get { return mod.blend; }
            set { mod.blend = value; }
        }
        




        [ParserTarget("pass1", optional = true)]
        public NumericParser<Vector2> pass1
        {
            get { return mod.pass1; }
            set { mod.pass1 = value; }
        }
        [ParserTarget("pass2", optional = true)]
        public NumericParser<Vector2> pass2
        {
            get { return mod.pass2; }
            set { mod.pass2 = value; }
        }
        [ParserTarget("pass3", optional = true)]
        public NumericParser<Vector2> pass3
        {
            get { return mod.pass3; }
            set { mod.pass3 = value; }
        }
        [ParserTarget("pass4", optional = true)]
        public NumericParser<Vector2> pass4
        {
            get { return mod.pass4; }
            set { mod.pass4 = value; }
        }
        [ParserTarget("pass5", optional = true)]
        public NumericParser<Vector2> pass5
        {
            get { return mod.pass5; }
            set { mod.pass5 = value; }
        }
        [ParserTarget("pass6", optional = true)]
        public NumericParser<Vector2> pass6
        {
            get { return mod.pass6; }
            set { mod.pass6 = value; }
        }
        [ParserTarget("pass7", optional = true)]
        public NumericParser<Vector2> pass7
        {
            get { return mod.pass7; }
            set { mod.pass7 = value; }
        }
        [ParserTarget("pass8", optional = true)]
        public NumericParser<Vector2> pass8
        {
            get { return mod.pass8; }
            set { mod.pass8 = value; }
        }
        [ParserTarget("pass9", optional = true)]
        public NumericParser<Vector2> pass9
        {
            get { return mod.pass9; }
            set { mod.pass9 = value; }
        }
        [ParserTarget("pass10", optional = true)]
        public NumericParser<Vector2> pass10
        {
            get { return mod.pass10; }
            set { mod.pass10 = value; }
        }
    }
}
