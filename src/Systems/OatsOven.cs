using Vintagestory.API.Common;

namespace StoneBakeOven
{
    public class OatsOven : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);

            api.RegisterBlockEntityClass("OvenGrill", typeof(BlockEntityOvenGrill));

            api.RegisterBlockEntityClass("OvenBakingTop", typeof(BlockEntityOvenBakingTop));
            api.RegisterBlockClass("OvenBakingTop", typeof(BlockOvenBakingTop));

            api.RegisterBlockEntityClass("OvenController", typeof(BlockEntityOvenController));
            api.RegisterBlockClass("OvenController", typeof(BlockOvenController));
        }
    }
}