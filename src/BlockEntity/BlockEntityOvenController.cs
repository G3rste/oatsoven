using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace OatsOven
{
    public class BlockEntityOvenController : BlockEntityFirepit, StoneOvenHeatSource
    {
        public virtual int heat() => (int)furnaceTemperature;
        public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
        {
            if (!fuelSlot.Empty)
            {
                base.OnTesselation(mesher, tesselator);
            }
            return false;
        }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            dsc.Append(Lang.Get("Temperature: {0}°C", (int)furnaceTemperature));
        }
    }
}