using CalamityInheritance.Content.BaseClass.Buff;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Buff.Buffs
{
    public class CIGravityNormalizer : CIBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.VortexDebuff] = true;
            if (player.ReducedSpaceGravity())
            {
                player.gravity = Player.defaultGravity;
                if (player.wet)
                {
                    if (player.honeyWet)
                        player.gravity = 0.1f;
                    else if (player.merman)
                        player.gravity = 0.3f;
                    else if (player.trident && !player.lavaWet)
                        player.gravity = player.controlUp ? 0.1f : 0.25f;
                    else
                        player.gravity = 0.2f;
                }
            }
        }
    }
}
