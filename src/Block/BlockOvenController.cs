using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace StoneBakeOven
{
    public class BlockOvenController : BlockFirepit
    {
        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            var item = byPlayer.InventoryManager.ActiveHotbarSlot?.Itemstack?.Collectible;
            return item?.CombustibleProps != null
                && item.CombustibleProps.BurnTemperature > 0
                && base.OnBlockInteractStart(world, byPlayer, blockSel);
        }

        public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
        {
            return new WorldInteraction[]{
                new WorldInteraction(){
                    ActionLangCode="blockhelp-firepit-refuel",
                    MouseButton=EnumMouseButton.Right,
                    HotKeyCode = "sneak",
                    Itemstacks = new ItemStack[]{new ItemStack(world.GetItem(new AssetLocation("game:firewood")))}
                },
                new WorldInteraction(){
                    ActionLangCode="blockhelp-firepit-ignite",
                    MouseButton=EnumMouseButton.Right,
                    HotKeyCode = "sneak",
                    Itemstacks = new ItemStack[]{new ItemStack(world.GetBlock(new AssetLocation("game:torch-basic-lit-up")))}
                }
            };
        }
    }
}