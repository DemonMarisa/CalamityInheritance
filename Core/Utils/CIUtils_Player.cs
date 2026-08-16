using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static bool GiveIFrames(this Player player, int cooldownSlot, int frames, bool blink = false)
        {
            if (!((cooldownSlot < 0) ? (player.immuneTime < frames) : (player.hurtCooldowns[cooldownSlot] < frames)))
            {
                return false;
            }

            player.immune = true;
            player.immuneNoBlink = !blink;
            if (cooldownSlot < 0)
            {
                if (player.immuneTime < frames)
                {
                    player.immuneTime = frames;
                }
            }
            else if (player.hurtCooldowns[cooldownSlot] < frames)
            {
                player.hurtCooldowns[cooldownSlot] = frames;
            }

            return true;
        }

        public static bool CantUseHoldout(this Player player, bool needsToHold = true)
        {
            if (player != null && player.active && !player.dead && !(!player.channel && needsToHold) && !player.CCed)
            {
                return player.noItems;
            }
            return true;
        }
        public static float CalcDamage<T>(this Player player, float baseDamage) where T : DamageClass => player.GetTotalDamage<T>().ApplyTo(baseDamage);
        public static int CalcIntDamage<T>(this Player player, float baseDamage) where T : DamageClass => (int)player.CalcDamage<T>(baseDamage);
        public static void AddDR(this Player player, float dr)
        {
            player.endurance += (1f - player.endurance) * dr;
        }
        public static bool IsUnderwater(this Player player) => Collision.DrownCollision(player.position, player.width, player.height, player.gravDir);
    }
}
