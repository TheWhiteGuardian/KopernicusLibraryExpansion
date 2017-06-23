using System;
using Kopernicus.Configuration.ModLoader;
using UnityEngine;
using Kopernicus;
using LibNoise.Unity;
using LibNoise.Unity.Generator;

namespace KLE.Noise
{
    public class ExoticNoise : ModuleBase
    {
        #region fields
        private Double _frequency = 1.0;
        private Double _lacunarity = 2.0;
        private Double _modMode = 0;
        private QualityMode _quality = QualityMode.Medium;
        private Int32 _octaveCount = 6;
        private Int32 _seed;
        private Double _persistence;
        private readonly Double[] _weights = new Double[Utils.OctavesMaximum];
        #endregion

        #region Constructors
        public ExoticNoise()
            : base(0)
        {
            UpdateWeights();
        }

        public ExoticNoise(Double frequency, Double lacunarity, Double modMode, QualityMode quality, Int32 octaves, Int32 seed, Double persistence)
            : base(0)
        {
            Frequency = frequency;
            Lacunarity = lacunarity;
            OctaveCount = octaves;
            Seed = seed;
            Quality = quality;
            ModMode = modMode;
            Persistence = persistence;
        }
        #endregion

        #region properties
        public Double Persistence
        {
            get { return _persistence; }
            set { _persistence = value; }
        }
        public Double Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }
        public Double Lacunarity
        {
            get { return _lacunarity; }
            set
            {
                _lacunarity = value;
                UpdateWeights();
            }
        }
        public QualityMode Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }
        public Int32 OctaveCount
        {
            get { return _octaveCount; }
            set { _octaveCount = Mathf.Clamp(value, 1, Utils.OctavesMaximum); }
        }
        public Int32 Seed
        {
            get { return _seed; }
            set { _seed = value; }
        }
        public Double ModMode
        {
            get { return _modMode; }
            set { _modMode = value; }
        }
        #endregion

        #region Methods
        private void UpdateWeights()
        {
            var f = 1.0;
            for (var i = 0; i < Utils.OctavesMaximum; i++)
            {
                _weights[i] = Math.Pow(f, -1.0);
                f *= _lacunarity;
            }
        }
        #endregion

        public override double GetValue(double x, double y, double z)
        {
            //Add the frequency
            x *= _frequency;
            y *= _frequency;
            z *= _frequency;

            var calcResult = 0.0;

            var value = 0.0;
            var weight = 1.0;
            if (ModMode == 0)
            {
                for (var i = 0; i < _octaveCount; i++)
                {
                    var nx = Utils.MakeInt32Range(x);
                    var ny = Utils.MakeInt32Range(y);
                    var nz = Utils.MakeInt32Range(z);

                    Int64 seed = (_seed + i) & 0x7fffffff;
                    var signal = Utils.GradientCoherentNoise3D(nx, ny, nz, seed, _quality);
                    weight *= _persistence;

                    //go ham here on the signal
                    signal = Math.Abs(signal);
                    signal += signal - weight;


                    value += signal * weight;

                    x *= _lacunarity;
                    y *= _lacunarity;
                    z *= _lacunarity;


                    calcResult = value;
                }
            }
            if (ModMode == 1)
            {
                for (var i = 0; i < _octaveCount; i++)
                {
                    var nx = Utils.MakeInt32Range(x);
                    var ny = Utils.MakeInt32Range(y);
                    var nz = Utils.MakeInt32Range(z);

                    Int64 seed = (_seed + i) & 0x7fffffff;
                    var signal = Utils.GradientCoherentNoise3D(nx, ny, nz, seed, _quality);
                    weight *= _persistence;

                    //go ham here on the signal
                    signal = Math.Sqrt(signal);
                    signal = Math.Pow(signal, weight);
                    signal += Math.Abs(signal + weight);

                    value += signal * weight;

                    x *= _lacunarity;
                    y *= _lacunarity;
                    z *= _lacunarity;


                    calcResult = value * 0.5;
                }
            }
            if (ModMode == 2)
                for (var i = 0; i < _octaveCount; i++)
                {
                    var nx = Utils.MakeInt32Range(x);
                    var ny = Utils.MakeInt32Range(y);
                    var nz = Utils.MakeInt32Range(z);

                    Int64 seed = (_seed + i) & 0x7fffffff;
                    var signal = Utils.GradientCoherentNoise3D(nx, ny, nz, seed, _quality);
                    weight *= _persistence;

                    //go ham here on the signal
                    signal = Math.Pow(signal, (1 / weight));
                    signal += Math.Sin(signal);

                    value += signal * weight;

                    x *= _lacunarity;
                    y *= _lacunarity;
                    z *= _lacunarity;


                    calcResult = value;
                }
            return calcResult;
        }
    }
}