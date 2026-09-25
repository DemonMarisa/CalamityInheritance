using CalamityInheritance.Content.Buff.Debuffs;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static bool HasIFrames(this Player player)
        {
            // Check old school iframes first (aka "cooldown timer -1". Regular hits, falling damage, etc.)
            if (player.immune || player.immuneTime > 0)
                return true;

            // Check more particular iframes. This primarily comes from traps, lava, and bosses.
            for (int i = 0; i < player.hurtCooldowns.Length; i++)
                if (player.hurtCooldowns[i] > 0)
                    return true;

            return false;
        }
        public static Vector2 RandomDebuffVisualSpot(this Player player) => player.Center + new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-20f, 20f));
        public static bool ReducedSpaceGravity(this Player player)
        {
            float num = (float)Main.maxTilesX / 4200f;
            num *= num;
            return (float)((double)(player.position.Y / 16f - (60f + 10f * num)) / (Main.worldSurface / (Main.remixWorld ? 1.0 : 6.0))) < 1f;
        }
        public static void ScalDebuffs(this Player target, int AbyssalFlamesduration, int VulnerabilityHexLegacyduration, int Horrorduration)
        {
            target.AddBuff(BuffType<AbyssalFlames>(), AbyssalFlamesduration, true);
            target.AddBuff(BuffType<VulnerabilityHexLegacy>(), VulnerabilityHexLegacyduration, true);
            if (Horrorduration > 1)
                target.AddBuff(BuffType<Horror>(), Horrorduration, true);
        }
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
