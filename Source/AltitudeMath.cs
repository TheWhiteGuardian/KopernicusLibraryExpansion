using System;
using UnityEngine;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration;
using Kopernicus;
using LibNoise.Unity;
using LibNoise.Unity.Generator;

namespace KLE
{
    #region PQSMods
    public class PQSMod_AltitudeMath : PQSMod
    {
        //Enter the command entries.
        public enum Command
        {
            Add,
            Subtract,
            Multiply,
            Divide
        }
        //The amount added or subtracted, or the number multiplied or divided by
        public Double effect = 0;
        private Double ModMode = 0;
        public Boolean effectIsPercentage = false;
        private Double effectAsPercent;
        public Command command;

        public override void OnSetup()
        {
            switch(command)
            {
                case Command.Add:
                    ModMode = 1;
                    break;
                case Command.Subtract:
                    ModMode = 2;
                    break;
                case Command.Multiply:
                    ModMode = 3;
                    break;
                case Command.Divide:
                    ModMode = 4;
                    break;
                default:
                    throw new ArgumentException("Command is something undefinable. Valid entries are Add, Subtract, Multiply and Divide.", nameof(command));
            }
            effectAsPercent = effect / 100;

        }
        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            if (ModMode == 4 && effect == 0)
                throw new ArgumentException("That's not funny, man.", nameof(effect));
            if (!effectIsPercentage)
            {
                if (ModMode == 1)
                    data.vertHeight = data.vertHeight + effect;
                if (ModMode == 2)
                    data.vertHeight = data.vertHeight - effect;
                if (ModMode == 3)
                    data.vertHeight = data.vertHeight * effect;
                if (ModMode == 4)
                    data.vertHeight = data.vertHeight / effect;
            }
            if (effectIsPercentage)
            {
                if (ModMode == 1)
                    data.vertHeight = data.vertHeight + (data.vertHeight * effectAsPercent);
                if (ModMode == 2)
                    data.vertHeight = data.vertHeight - (data.vertHeight * effectAsPercent);
                if (ModMode == 3)
                    data.vertHeight = data.vertHeight * (data.vertHeight * effectAsPercent);
                if (ModMode == 4)
                    data.vertHeight = data.vertHeight / (data.vertHeight * effectAsPercent);
            }
        }
    }
    #endregion

    #region ModLoader
    [RequireConfigType(ConfigType.Node)]
    public class AltitudeMath : ModLoader<PQSMod_AltitudeMath>
    {
        [ParserTarget("command", optional = true)]
        public EnumParser<PQSMod_AltitudeMath.Command> command
        {
            get { return mod.command; }
            set { mod.command = value; }
        }
        [ParserTarget("effect", optional = true)]
        public NumericParser<double> effect
        {
            get { return mod.effect; }
            set { mod.effect = value; }
        }
        [ParserTarget("effectIsPercentage", optional = true)]
        public NumericParser<bool> effectIsPercentage
        {
            get { return mod.effectIsPercentage; }
            set { mod.effectIsPercentage = value; }
        }
    }
    #endregion
}
