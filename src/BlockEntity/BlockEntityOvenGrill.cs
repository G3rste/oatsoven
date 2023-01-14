using System;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace OatsOven
{
    public class BlockEntityOvenGrill : BlockEntity, StoneOvenHeatSource
    {
        protected int grillTemperature = 20;
        public virtual int heat() => grillTemperature;

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
            if (api.Side == EnumAppSide.Server)
            {
                RegisterGameTickListener(UpdateHeatSource, 5000);
            }
        }

        private void UpdateHeatSource(float dt)
        {
            int maxSurroundingTemperature = EnvironmentTemperature();
            foreach (var neighbour in new BlockPos[] { Pos.EastCopy(), Pos.WestCopy(), Pos.NorthCopy(), Pos.SouthCopy() })
            {
                if (Api.World.BlockAccessor.GetBlockEntity(neighbour) is StoneOvenHeatSource heatSource)
                {
                    maxSurroundingTemperature = Math.Max(maxSurroundingTemperature, (int)(heatSource.heat() * 0.85f));
                }
            }
            grillTemperature = maxSurroundingTemperature;
            CheckSmoke();
            MarkDirty(true);
        }

        private void CheckSmoke()
        {
            if (Block.Variant["temperature"] == "cold" && grillTemperature >= 100)
            {
                Block = Api.World.GetBlock(Block.CodeWithVariant("temperature", "hot"));
                Api.World.BlockAccessor.SetBlock(Block.Id, Pos);
            }
            if (Block.Variant["temperature"] == "hot" && grillTemperature < 100)
            {
                Block = Api.World.GetBlock(Block.CodeWithVariant("temperature", "cold"));
                Api.World.BlockAccessor.SetBlock(Block.Id, Pos);
            }
        }

        protected virtual int EnvironmentTemperature()
        {
            var conds = Api.World.BlockAccessor.GetClimateAt(Pos, EnumGetClimateMode.NowValues);   //TODO: Is there a performance issue here?
            return conds == null ? 20 : (int)conds.Temperature;
        }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            if (grillTemperature <= 25)
            {
                dsc.Append(Lang.Get("Temperature: {0}°C", Lang.Get("Cold")));
            }
            else
            {
                dsc.Append(Lang.Get("Temperature: {0}°C", (int)grillTemperature));
            }
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            tree.SetInt("grillTemperature", grillTemperature);
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
        {
            base.FromTreeAttributes(tree, worldAccessForResolve);
            grillTemperature = tree.GetInt("grillTemperature", 20);
        }
    }
}