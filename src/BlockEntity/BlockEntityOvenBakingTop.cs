using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace OatsOven
{
    public class BlockEntityOvenBakingTop : BlockEntityOven
    {
        // We need to offset the height of the bread because it needs to be higher than in the actual vanilla oven
        // the exact numbers are derived from here https://github.com/anegostudios/vssurvivalmod/blob/f69a482d42b0a9544db1992cde147bb0d1609793/Systems/Cooking/Clayoven/BEClayOven.cs#L822
        const float offset = (15f - 1.01f) / 16f;

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
                if (Api.World.BlockAccessor.GetBlockEntity(neighbour) is BlockEntityOvenGrill heatSource)
                {
                    maxSurroundingTemperature = Math.Min(Math.Max(maxSurroundingTemperature, (int)(heatSource.heat() * 0.85f)), 300);
                }
            }
            ovenTemperature = maxSurroundingTemperature;
            HeatInput(dt);
            MarkDirty(true);
        }

        protected override bool TryAddFuel(ItemSlot slot)
        {
            return false;
        }

        protected override float[][] genTransformationMatrices()
        {
            var matrices = base.genTransformationMatrices();
            for (int i = 0; i < matrices.Length; i++)
            {
                matrices[i] = new Matrixf().Set(matrices[i]).Translate(0, offset, 0).Values;
            }
            return matrices;
        }

        protected override void OnBurnTick(float dt)
        {
            // do nothing, everything handled in UpdateHeatSource in a lower interval
        }
    }
}