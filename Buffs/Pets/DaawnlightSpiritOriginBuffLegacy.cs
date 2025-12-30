using CalamityInheritance.Content.Projectiles.Pets;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Pets
{
    public class DaawnlightSpiritOriginBuffLegacy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 2;
            if (!player.HasProj<DaawnlightSpiritOriginMinionLegacy>())
            {
                player.ClearBuff(Type);
            }
        }
    }
}
