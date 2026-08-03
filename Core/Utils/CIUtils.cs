using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Core.GlobalInstance.Items;
using CalamityInheritance.Core.GlobalInstance.NPCs;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.GlobalInstance.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static bool HasCalamity()
        {
            return CalamityInheritance.Calamity is not null;
        }
        public static CIPlayer CI(this Player player)
        {
            return player.GetModPlayer<CIPlayer>();
        }
        public static CIGlobalItems CI(this Item item)
        {
            return item.GetGlobalItem<CIGlobalItems>();
        }
        public static CIGProj CI(this Projectile proj)
        {
            return proj.GetGlobalProjectile<CIGProj>();
        }
        public static CalPlayerInfo CalPlayerInfo(this Player player)
        {
            return player.GetModPlayer<CalPlayerInfo>();
        }
        public static CIGNPC CI(this NPC npc)
        {
            return npc.GetGlobalNPC<CIGNPC>();
        }
        public static Vector2 RandomVelocity(float directionMult, float speedLowerLimit, float speedCap, float speedMult = 0.1f)
        {
            Vector2 vector = new Vector2(Main.rand.NextFloat(0f - directionMult, directionMult), Main.rand.NextFloat(0f - directionMult, directionMult));
            while (vector.X == 0f && vector.Y == 0f)
            {
                vector = new Vector2(Main.rand.NextFloat(0f - directionMult, directionMult), Main.rand.NextFloat(0f - directionMult, directionMult));
            }

            vector.SafeNormalize(Vector2.UnitX);
            return vector * (Main.rand.NextFloat(speedLowerLimit, speedCap) * speedMult);
        }
    }
}
