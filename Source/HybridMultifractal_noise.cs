using System;
using UnityEngine;
using LibNoise.Unity;
using LibNoise.Unity.Generator;

namespace KLE.Noise
{

    public class HybridMultifractal : ModuleBase
    {
        #region Fields

        private Double _frequency = 1.0;
        private Double _lacunarity = 2.0f;
        private QualityMode _quality = QualityMode.Medium;
        private Int32 _octaveCount = 6;
        private Double _persistence = 0.5;
        private Single _gain = 1.0f;
        private Single _offset = 0.7f;
        

        private Perlin _noise = new Perlin(PerlFreq, PerlLac, PerlPers, PerlOct, PerlSeed, PerlQuality);
        //_PerlinFreq, _PerlinLac, _PerlinPers, int octaves, int seed, QualityMode quality
        private static Double PerlFreq = 15;
        private static Double PerlLac = 2.5;
        private static Double PerlPers = 0.4;
        private static Int32 PerlOct = 5;
        private static Int32 PerlSeed = 2000;
        private static QualityMode PerlQuality = QualityMode.High;

        

        private readonly Double[] _weights = new Double[Utils.OctavesMaximum];
        #endregion






        #region Constructors

        public HybridMultifractal()
            : base(0)
        {
        }

        public HybridMultifractal(Double frequency, Single lacunarity, Int32 octaves, Int32 seed, QualityMode quality, Single offset)
            : base(0)
        {
            Frequency = frequency;
            Lacunarity = lacunarity;
            OctaveCount = octaves;
            Seed = seed;
            Quality = quality;
            Offset = offset;
        }

        




        #endregion






        #region Properties

        /// <summary>
        /// Gets or sets the frequency of the first octave.
        /// </summary>
        public Double Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        public Single Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        /// <summary>
        /// Gets or sets the lacunarity of the perlin noise.
        /// </summary>
        public Double Lacunarity
        {
            get { return _lacunarity; }
            set { _lacunarity = value; }
        }

        /// <summary>
        /// Gets or sets the quality of the perlin noise.
        /// </summary>
        public QualityMode Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        /// <summary>
        /// Gets or sets the number of octaves of the perlin noise.
        /// </summary>
        public Int32 OctaveCount
        {
            get { return _octaveCount; }
            set { _octaveCount = Mathf.Clamp(value, 1, Utils.OctavesMaximum); }
        }

        /// <summary>
        /// Gets or sets the seed of the perlin noise.
        /// </summary>
        public Int32 Seed
        {
            get { return PerlSeed; }
            set { PerlSeed = value; }
        }

        #endregion






        private void UpdateWeights()
        {
            var f = 1.0;
            for (var i = 0; i < Utils.OctavesMaximum; i++)
            {
                _weights[i] = Math.Pow(f, -1.0);
                f *= _lacunarity;
            }
        }
        
        public override Double GetValue(double x, double y, double z)
        {
            float signal;
            int curOctave;

            x *= _frequency;
            y *= _frequency;
            z *= _frequency;

            // Initialize value : get first octave of function; later octaves are weighted
            float value = Convert.ToSingle(_noise.GetValue(x, y, z)) + _offset;
            float weight = _gain * value;

            x *= _lacunarity;
            y *= _lacunarity;
            z *= _lacunarity;

            // inner loop of spectral construction, where the fractal is built
            for (curOctave = 1; weight > 0.001 && curOctave < _octaveCount; curOctave++)
            {
                // prevent divergence
                if (weight > 1.0)
                    weight = 1.0f;
                signal = (_offset + Convert.ToSingle(_noise.GetValue(x, y, z)));
                signal *= weight;
                value += signal;
                weight *= _gain * signal;

                // Go to the next octave.
                x *= _lacunarity;
                y *= _lacunarity;
                z *= _lacunarity;
            }

            //take care of remainder in _octaveCount
            float remainder = _octaveCount - (int)_octaveCount;

            if (remainder > 0.0f)
            {
                signal = Convert.ToSingle(_noise.GetValue(x, y, z));
                signal *= remainder;
                value += signal;
            }

            return value;
        }
    }
}




//Thank you everbytes!
/*
namespace LibNoise.Filter
{
    /// <summary>
    /// Noise module that outputs 3-dimensional hybrid-multifractal noise.
    ///
    /// Hybrid-multifractal noise the perturbations are combined additively, 
    /// but the single perturbation is computed by multiplying two quantities 
    /// called weight and signal. The signal quantity is the standard multifractal 
    /// perturbation, and the weight quantity is the multiplicative combination 
    /// of all the previous signal quantities.
    ///
    /// Hybrid-multifractal attempts to control the amount of details according
    /// to the slope of the underlying overlays. Hybrid Multifractal  is 
    /// conventionally used to generate terrains with smooth valley areas and 
    /// rough peaked mountains. With high Lacunarity values, it tends to produce 
    /// embedded plateaus. 
    /// 
    /// Some good parameter values to start with:
    ///		gain = 1.0;
    ///		offset = 0.7;
    ///		spectralExponent = 0.25;
    /// 
    /// </summary>
    public class HybridMultiFractal : FilterModule, IModule3D, IModule2D
    {
        #region Ctor/Dtor

        /// <summary>
        /// 0-args constructor
        /// </summary>
        public HybridMultiFractal()
        {
            _gain = 1.0f;
            _offset = 0.7f;
            _spectralExponent = 0.25f;

            ComputeSpectralWeights();
        }

        #endregion

        #region IModule2D Members

        /// <summary>
        /// Generates an output value given the coordinates of the specified input value.
        /// </summary>
        /// <param name="x">The input coordinate on the x-axis.</param>
        /// <param name="y">The input coordinate on the y-axis.</param>
        /// <returns>The resulting output value.</returns>
        public float GetValue(float x, float y)
        {
            float signal;
            int curOctave;

            x *= _frequency;
            y *= _frequency;

            // Initialize value : get first octave of function; later octaves are weighted
            float value = _source2D.GetValue(x, y) + _offset;
            float weight = _gain * value;

            x *= _lacunarity;
            y *= _lacunarity;

            // inner loop of spectral construction, where the fractal is built
            for (curOctave = 1; weight > 0.001 && curOctave < _octaveCount; curOctave++)
            {
                // prevent divergence
                if (weight > 1.0)
                    weight = 1.0f;

                // get next higher frequency
                signal = (_offset + _source2D.GetValue(x, y)) * _spectralWeights[curOctave];

                // The weighting from the previous octave is applied to the signal.
                signal *= weight;

                // Add the signal to the output value.
                value += signal;

                // update the (monotonically decreasing) weighting value
                weight *= _gain * signal;

                // Go to the next octave.
                x *= _lacunarity;
                y *= _lacunarity;
            }

            //take care of remainder in _octaveCount
            float remainder = _octaveCount - (int)_octaveCount;

            if (remainder > 0.0f)
            {
                signal = _source2D.GetValue(x, y);
                signal *= _spectralWeights[curOctave];
                signal *= remainder;
                value += signal;
            }

            return value;
        }

        #endregion

        #region IModule3D Members

        /// <summary>
        /// Generates an output value given the coordinates of the specified input value.
        /// </summary>
        /// <param name="x">The input coordinate on the x-axis.</param>
        /// <param name="y">The input coordinate on the y-axis.</param>
        /// <param name="z">The input coordinate on the z-axis.</param>
        /// <returns>The resulting output value.</returns>
        public float GetValue(float x, float y, float z)
        {
            float signal;
            int curOctave;

            x *= _frequency;
            y *= _frequency;
            z *= _frequency;

            // Initialize value : get first octave of function; later octaves are weighted
            float value = _source3D.GetValue(x, y, z) + _offset;
            float weight = _gain * value;

            x *= _lacunarity;
            y *= _lacunarity;
            z *= _lacunarity;

            // inner loop of spectral construction, where the fractal is built
            for (curOctave = 1; weight > 0.001 && curOctave < _octaveCount; curOctave++)
            {
                // prevent divergence
                if (weight > 1.0)
                    weight = 1.0f;

                // get next higher frequency
                signal = (_offset + _source3D.GetValue(x, y, z)) * _spectralWeights[curOctave];

                // The weighting from the previous octave is applied to the signal.
                signal *= weight;

                // Add the signal to the output value.
                value += signal;

                // update the (monotonically decreasing) weighting value
                weight *= _gain * signal;

                // Go to the next octave.
                x *= _lacunarity;
                y *= _lacunarity;
                z *= _lacunarity;
            }

            //take care of remainder in _octaveCount
            float remainder = _octaveCount - (int)_octaveCount;

            if (remainder > 0.0f)
            {
                signal = _source3D.GetValue(x, y, z);
                signal *= _spectralWeights[curOctave];
                signal *= remainder;
                value += signal;
            }

            return value;
        }

        #endregion
    }
} */
