using CalamityInheritance.Content.Projectiles.Armor.Magic;
using CalamityInheritance.Content.Projectiles.Heals;
using CalamityInheritance.Core.GlobalInstance.Players;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class SilvaBonus
    {
        public static void HitNPCWithProj_All(CIPlayer player, Projectile proj, NPC.HitInfo hit, NPC target)
        {
            if (player.Player.whoAmI != Main.myPlayer)
                return;
            if (!player.HasCount("SilvaHealCD"))
            {
                player.Player.SpawnLifeStealProj(target, proj.GetSource_FromThis(), ProjectileType<SilvaOrbLegacy>(), proj.Center, Vector2.UnitX.RotatedByRandom(6.29f) * 6f);
                player.AddCount("SilvaHealCD", Main.rand.Next(10, 30));
            }
        }
        public static void HitNPC_Magic(CIPlayer player, NPC.HitInfo hit, NPC target)
        {
            SoundEngine.PlaySound(SoundID.Zombie103, target.Center);
            int silvaBurstDamage = (int)(800 + 0.6f * hit.SourceDamage);
            Projectile.NewProjectile(player.Player.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileType<SilvaBurst>(), silvaBurstDamage, 8f, player.Player.whoAmI);
        }
        public static void PostUpdate_Melee(Player player)
        {
            float multiplier = player.statLife / (float)player.statLifeMax2;
            player.GetDamage<MeleeDamageClass>() += (float)(multiplier * 0.2);
        }
        public static void ModifyHitNPC_Melee(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.NextBool(4))
                modifiers.SourceDamage *= 5;
        }
    }
}
