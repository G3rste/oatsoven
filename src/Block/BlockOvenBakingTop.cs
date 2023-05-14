using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace StoneBakeOven
{
    public class BlockOvenBakingTop : BlockClayOven
    {
        WorldInteraction interaction;
        public override void OnLoaded(ICoreAPI api)
        {
            //let the real clay oven do that shit
        }

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection bs)
        {
            if (world.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityOvenBakingTop beo) return beo.OnInteract(byPlayer, bs);

            return base.OnBlockInteractStart(world, byPlayer, bs);
        }

        public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
        {
            if (interaction == null)
            {
                var interactionArray = ObjectCacheUtil.TryGet<WorldInteraction[]>(api, "ovenInteractions");
                if (interactionArray != null && interactionArray.Length > 0)
                {
                    interaction = interactionArray[0];
                    interaction = new WorldInteraction(){
                        ActionLangCode = interaction.ActionLangCode,
                        HotKeyCode = interaction.HotKeyCode,
                        MouseButton = interaction.MouseButton,
                        Itemstacks = interaction.Itemstacks,
                        GetMatchingStacks = null
                    };
                }
            }

            if (interaction != null)
            {
                return new WorldInteraction[] { interaction };
            }
            return new WorldInteraction[0];
        }
    }
}