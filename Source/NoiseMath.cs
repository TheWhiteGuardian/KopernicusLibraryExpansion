using System;
using UnityEngine;
using Kopernicus.Configuration.ModLoader;
using Kopernicus;
using System.Linq;
using Kopernicus.Configuration;
using Kopernicus.Components;
using LibNoise.Unity.Generator;
using LibNoise.Unity;
using LibNoise.Unity.Operator;
using KLE.Noise;

namespace KLE
{

    #region PQSMods
    public class PQSMod_NoiseMath : PQSMod
    {
        public Double deformity;
        public enum Mod1
        {
            Billow,
            Perlin,
            Rigged,
            Hybrid,
            Simplex,
            Voronoi
        }
        public Mod1 noiseMod1;
        public enum Mod2
        {
            Billow,
            Perlin,
            Rigged,
            Hybrid,
            Simplex,
            Voronoi
        }
        public enum Command
        {
            Addition,
            Subtract,
            Multiply,
            Divide
        }
        public Command command;
        public Mod2 noiseMod2;

        public Int32 seed1 = 1000;
        public Int32 seed2 = 1000;

        public Double octaves1 = 8;
        public Double octaves2 = 8;

        public Double persistence1 = 0.5;
        public Double persistence2 = 0.5;

        public Double lacunarity1 = 2.5;
        public Double lacunarity2 = 2.5;

        public Double frequency1 = 5;
        public Double frequency2 = 5;

        public Boolean voronoiEnableDistance1 = true;
        public Boolean voronoiEnableDistance2 = true;

        public Double displacement1 = 0;
        public Double displacement2 = 0;

        public QualityMode Mode1 = QualityMode.Low;
        public QualityMode Mode2 = QualityMode.Low;

        private Simplex simplex1;
        private Simplex simplex2;

        private Perlin perlin1;
        private Perlin perlin2;

        private Billow bill1;
        private Billow bill2;

        private RiggedMultifractal rig1;
        private RiggedMultifractal rig2;

        private HybridMultifractal hyb1;
        private HybridMultifractal hyb2;

        private Voronoi vor1;
        private Voronoi vor2;

        private Double modMode1 = 0;
        private Double modMode2 = 0;
        private Double modMode3 = 0;

        public override void OnSetup()
        {
            simplex1 = new Simplex(seed1, octaves1, persistence1, frequency1);
            simplex2 = new Simplex(seed2, octaves2, persistence2, frequency2);

            perlin1 = new Perlin(frequency1, lacunarity1, persistence1, Convert.ToInt32(octaves1), seed1, Mode1);
            perlin2 = new Perlin(frequency2, lacunarity2, persistence2, Convert.ToInt32(octaves2), seed2, Mode2);

            rig1 = new RiggedMultifractal(frequency1, lacunarity1, Convert.ToInt32(octaves1), seed1, Mode1);
            rig2 = new RiggedMultifractal(frequency2, lacunarity2, Convert.ToInt32(octaves2), seed2, Mode2);

            vor1 = new Voronoi(frequency1, displacement1, seed1, voronoiEnableDistance1);
            vor2 = new Voronoi(frequency2, displacement2, seed2, voronoiEnableDistance2);

            bill1 = new Billow(frequency1, lacunarity1, persistence1, Convert.ToInt32(octaves1), seed1, Mode1);
            bill2 = new Billow(frequency2, lacunarity2, persistence2, Convert.ToInt32(octaves2), seed2, Mode2);

            switch (noiseMod1)
            {
                case Mod1.Billow:
                    modMode1 = 1.0;
                    break;

                case Mod1.Hybrid:
                    modMode1 = 2.0;
                    break;

                case Mod1.Perlin:
                    modMode1 = 3.0;
                    break;

                case Mod1.Rigged:
                    modMode1 = 4.0;
                    break;

                case Mod1.Simplex:
                    modMode1 = 5.0;
                    break;

                case Mod1.Voronoi:
                    modMode1 = 6.0;
                    break;
                default:
                    throw new ArgumentNullException(nameof(Mod1));
            }
            switch (noiseMod2)
            {
                case Mod2.Billow:
                    modMode2 = 0.1;
                    break;

                case Mod2.Hybrid:
                    modMode2 = 0.2;
                    break;

                case Mod2.Perlin:
                    modMode2 = 0.3;
                    break;

                case Mod2.Rigged:
                    modMode2 = 0.4;
                    break;

                case Mod2.Simplex:
                    modMode2 = 0.5;
                    break;

                case Mod2.Voronoi:
                    modMode2 = 0.6;
                    break;
                default:
                    throw new ArgumentNullException(nameof(Mod2));
            }
            modMode3 = modMode1 + modMode2;

        }
        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            //Activate OCD mode.
            switch (command)
            {
                case Command.Addition:
                    #region Addition
                    if (modMode3 == 1.1)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) + (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.2)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) + (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.3)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) + (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.4)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) + (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.5)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) + (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.6)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) + (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 2.1)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) + (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.2)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) + (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.3)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) + (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.4)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) + (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.5)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) + (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.6)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) + (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 3.1)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) + (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.2)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) + (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.3)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) + (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.4)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) + (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.5)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) + (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.6)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) + (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 4.1)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) + (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.2)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) + (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.3)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) + (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.4)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) + (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.5)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) + (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.6)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) + (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 5.1)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) + (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.2)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) + (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.3)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) + (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.4)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) + (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.5)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) + (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.6)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) + (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 6.1)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) + (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.2)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) + (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.3)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) + (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.4)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) + (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.5)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) + (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.6)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) + (vor2.GetValue(data.directionFromCenter) * deformity);
                    }
                    #endregion
                    break;
                case Command.Subtract:
                    #region Subtract
                    if (modMode3 == 1.1)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) - (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.2)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) - (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.3)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) - (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.4)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) - (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.5)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) - (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.6)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) - (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 2.1)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) - (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.2)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) - (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.3)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) - (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.4)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) - (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.5)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) - (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.6)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) - (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 3.1)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) - (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.2)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) - (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.3)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) - (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.4)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) - (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.5)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) - (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.6)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) - (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 4.1)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) - (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.2)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) - (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.3)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) - (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.4)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) - (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.5)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) - (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.6)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) - (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 5.1)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) - (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.2)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) - (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.3)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) - (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.4)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) - (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.5)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) - (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.6)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) - (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 6.1)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) - (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.2)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) - (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.3)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) - (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.4)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) - (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.5)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) - (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.6)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) - (vor2.GetValue(data.directionFromCenter) * deformity);
                    }
                    #endregion
                    break;
                case Command.Multiply:
                    #region Multiply
                    if (modMode3 == 1.1)
                    {
                        data.vertHeight += bill1.GetValue(data.directionFromCenter) * deformity * bill2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 1.2)
                    {
                        data.vertHeight += bill1.GetValue(data.directionFromCenter) * deformity * hyb2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 1.3)
                    {
                        data.vertHeight += bill1.GetValue(data.directionFromCenter) * deformity * perlin2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 1.4)
                    {
                        data.vertHeight += bill1.GetValue(data.directionFromCenter) * deformity * rig2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 1.5)
                    {
                        data.vertHeight += bill1.GetValue(data.directionFromCenter) * deformity * simplex2.noise(data.directionFromCenter);
                    }
                    if (modMode3 == 1.6)
                    {
                        data.vertHeight += bill1.GetValue(data.directionFromCenter) * deformity * vor2.GetValue(data.directionFromCenter);
                    }



                    if (modMode3 == 2.1)
                    {
                        data.vertHeight += hyb1.GetValue(data.directionFromCenter) * deformity * bill2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 2.2)
                    {
                        data.vertHeight += hyb1.GetValue(data.directionFromCenter) * deformity * hyb2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 2.3)
                    {
                        data.vertHeight += hyb1.GetValue(data.directionFromCenter) * deformity * perlin2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 2.4)
                    {
                        data.vertHeight += hyb1.GetValue(data.directionFromCenter) * deformity * rig2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 2.5)
                    {
                        data.vertHeight += hyb1.GetValue(data.directionFromCenter) * deformity * simplex2.noise(data.directionFromCenter);
                    }
                    if (modMode3 == 2.6)
                    {
                        data.vertHeight += hyb1.GetValue(data.directionFromCenter) * deformity * vor2.GetValue(data.directionFromCenter);
                    }



                    if (modMode3 == 3.1)
                    {
                        data.vertHeight += perlin1.GetValue(data.directionFromCenter) * deformity * bill2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 3.2)
                    {
                        data.vertHeight += perlin1.GetValue(data.directionFromCenter) * deformity * hyb2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 3.3)
                    {
                        data.vertHeight += perlin1.GetValue(data.directionFromCenter) * deformity * perlin2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 3.4)
                    {
                        data.vertHeight += perlin1.GetValue(data.directionFromCenter) * deformity * rig2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 3.5)
                    {
                        data.vertHeight += perlin1.GetValue(data.directionFromCenter) * deformity * simplex2.noise(data.directionFromCenter);
                    }
                    if (modMode3 == 3.6)
                    {
                        data.vertHeight += perlin1.GetValue(data.directionFromCenter) * deformity * vor2.GetValue(data.directionFromCenter);
                    }



                    if (modMode3 == 4.1)
                    {
                        data.vertHeight += rig1.GetValue(data.directionFromCenter) * deformity * bill2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 4.2)
                    {
                        data.vertHeight += rig1.GetValue(data.directionFromCenter) * deformity * hyb2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 4.3)
                    {
                        data.vertHeight += rig1.GetValue(data.directionFromCenter) * deformity * perlin2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 4.4)
                    {
                        data.vertHeight += rig1.GetValue(data.directionFromCenter) * deformity * rig2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 4.5)
                    {
                        data.vertHeight += rig1.GetValue(data.directionFromCenter) * deformity * simplex2.noise(data.directionFromCenter);
                    }
                    if (modMode3 == 4.6)
                    {
                        data.vertHeight += rig1.GetValue(data.directionFromCenter) * deformity * vor2.GetValue(data.directionFromCenter);
                    }



                    if (modMode3 == 5.1)
                    {
                        data.vertHeight += simplex1.noise(data.directionFromCenter) * deformity * bill2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 5.2)
                    {
                        data.vertHeight += simplex1.noise(data.directionFromCenter) * deformity * hyb2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 5.3)
                    {
                        data.vertHeight += simplex1.noise(data.directionFromCenter) * deformity * perlin2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 5.4)
                    {
                        data.vertHeight += simplex1.noise(data.directionFromCenter) * deformity * rig2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 5.5)
                    {
                        data.vertHeight += simplex1.noise(data.directionFromCenter) * deformity * simplex2.noise(data.directionFromCenter);
                    }
                    if (modMode3 == 5.6)
                    {
                        data.vertHeight += simplex1.noise(data.directionFromCenter) * deformity * vor2.GetValue(data.directionFromCenter);
                    }



                    if (modMode3 == 6.1)
                    {
                        data.vertHeight += vor1.GetValue(data.directionFromCenter) * deformity * bill2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 6.2)
                    {
                        data.vertHeight += vor1.GetValue(data.directionFromCenter) * deformity * hyb2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 6.3)
                    {
                        data.vertHeight += vor1.GetValue(data.directionFromCenter) * deformity * perlin2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 6.4)
                    {
                        data.vertHeight += vor1.GetValue(data.directionFromCenter) * deformity * rig2.GetValue(data.directionFromCenter);
                    }
                    if (modMode3 == 6.5)
                    {
                        data.vertHeight += vor1.GetValue(data.directionFromCenter) * deformity * simplex2.noise(data.directionFromCenter);
                    }
                    if (modMode3 == 6.6)
                    {
                        data.vertHeight += vor1.GetValue(data.directionFromCenter) * deformity * vor2.GetValue(data.directionFromCenter);
                    }
                    #endregion
                    break;
                case Command.Divide:
                    #region Divide
                    if (modMode3 == 1.1)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) / (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.2)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) / (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.3)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) / (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.4)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) / (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.5)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) / (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 1.6)
                    {
                        data.vertHeight += (bill1.GetValue(data.directionFromCenter) * deformity) / (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 2.1)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) / (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.2)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) / (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.3)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) / (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.4)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) / (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.5)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) / (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 2.6)
                    {
                        data.vertHeight += (hyb1.GetValue(data.directionFromCenter) * deformity) / (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 3.1)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) / (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.2)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) / (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.3)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) / (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.4)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) / (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.5)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) / (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 3.6)
                    {
                        data.vertHeight += (perlin1.GetValue(data.directionFromCenter) * deformity) / (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 4.1)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) / (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.2)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) / (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.3)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) / (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.4)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) / (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.5)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) / (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 4.6)
                    {
                        data.vertHeight += (rig1.GetValue(data.directionFromCenter) * deformity) / (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 5.1)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) / (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.2)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) / (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.3)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) / (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.4)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) / (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.5)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) / (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 5.6)
                    {
                        data.vertHeight += (simplex1.noise(data.directionFromCenter) * deformity) / (vor2.GetValue(data.directionFromCenter) * deformity);
                    }



                    if (modMode3 == 6.1)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) / (bill2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.2)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) / (hyb2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.3)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) / (perlin2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.4)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) / (rig2.GetValue(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.5)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) / (simplex2.noise(data.directionFromCenter) * deformity);
                    }
                    if (modMode3 == 6.6)
                    {
                        data.vertHeight += (vor1.GetValue(data.directionFromCenter) * deformity) / (vor2.GetValue(data.directionFromCenter) * deformity);
                    }
                    #endregion
                    break;
            }
            //Ded.
        }
    }
    #endregion

    #region ModLoader
    [RequireConfigType(ConfigType.Node)]
    public class NoiseDifference : ModLoader<PQSMod_NoiseMath>
    {
        [ParserTarget("noiseMod1", optional = false)]
        public EnumParser<PQSMod_NoiseMath.Mod1> noiseMod1
        {
            get { return mod.noiseMod1; }
            set { mod.noiseMod1 = value; }
        }
        [ParserTarget("noiseMod2", optional = false)]
        public EnumParser<PQSMod_NoiseMath.Mod2> noiseMod2
        {
            get { return mod.noiseMod2; }
            set { mod.noiseMod2 = value; }
        }
        [ParserTarget("seed1", optional = true)]
        public NumericParser<int> seed1
        {
            get { return mod.seed1; }
            set { mod.seed1 = value; }
        }
        [ParserTarget("seed2", optional = true)]
        public NumericParser<int> seed2
        {
            get { return mod.seed2; }
            set { mod.seed2 = value; }
        }
        [ParserTarget("octaves1", optional = true)]
        public NumericParser<double> octaves1
        {
            get { return mod.octaves1; }
            set { mod.octaves1 = value; }
        }
        [ParserTarget("octaves2", optional = true)]
        public NumericParser<double> octaves2
        {
            get { return mod.octaves2; }
            set { mod.octaves2 = value; }
        }
        [ParserTarget("persistence1", optional = true)]
        public NumericParser<double> persistence1
        {
            get { return mod.persistence1; }
            set { mod.persistence1 = value; }
        }
        [ParserTarget("persistence2", optional = true)]
        public NumericParser<double> persistence2
        {
            get { return mod.persistence2; }
            set { mod.persistence2 = value; }
        }
        [ParserTarget("lacunarity1", optional = true)]
        public NumericParser<double> lacunarity1
        {
            get { return mod.lacunarity1; }
            set { mod.lacunarity1 = value; }
        }
        [ParserTarget("lacunarity2", optional = true)]
        public NumericParser<double> lacunarity2
        {
            get { return mod.lacunarity2; }
            set { mod.lacunarity2 = value; }
        }
        [ParserTarget("frequency1", optional = true)]
        public NumericParser<double> frequency1
        {
            get { return mod.frequency1; }
            set { mod.frequency1 = value; }
        }
        [ParserTarget("frequency2", optional = true)]
        public NumericParser<double> frequency2
        {
            get { return mod.frequency2; }
            set { mod.frequency2 = value; }
        }
        [ParserTarget("voronoiEnableDistance1", optional = true)]
        public NumericParser<bool> voronoiEnableDistance1
        {
            get { return mod.voronoiEnableDistance1; }
            set { mod.voronoiEnableDistance1 = value; }
        }
        [ParserTarget("voronoiEnableDistance2", optional = true)]
        public NumericParser<bool> voronoiEnableDistance2
        {
            get { return mod.voronoiEnableDistance2; }
            set { mod.voronoiEnableDistance2 = value; }
        }
        [ParserTarget("displacement1", optional = true)]
        public NumericParser<double> displacement1
        {
            get { return mod.displacement1; }
            set { mod.displacement1 = value; }
        }
        [ParserTarget("displacement2", optional = true)]
        public NumericParser<double> displacement2
        {
            get { return mod.displacement2; }
            set { mod.displacement2 = value; }
        }
        [ParserTarget("Mode1", optional = true)]
        public NumericParser<LibNoise.Unity.QualityMode> mode1
        {
            get { return mod.Mode1; }
            set { mod.Mode1 = value; }
        }
        [ParserTarget("Mode2", optional = true)]
        public NumericParser<LibNoise.Unity.QualityMode> mode2
        {
            get { return mod.Mode2; }
            set { mod.Mode2 = value; }
        }
    }
    #endregion
}