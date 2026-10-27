using CalamityInheritance.Common.Blance;
using CalamityInheritance.Content.Buff.Armor.Melee;
using CalamityInheritance.Content.Buff.Armor.Rogue;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Projectiles.Armor.Ranged;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class TarragonBonus
    {
        public static void Main_PostUpdate(Player player)
        {
            player.calmed = true;
            player.lifeMagnet = true;
            if (player.HasBuff<TarragonCloak>())
            {
                int index = player.FindBuffIndex(BuffType<TarragonCloak>());
                if (player.buffTime[index] < 2 && player.buffType[index] == BuffType<TarragonCloak>())
                {
                    player.AddCD(LAPContent.CDType<TarragonCloakCD>(), 1500);
                    player.DelBuff(index);
                }
            }
            if (player.HasBuff<TarragonImmunity>())
            {
                int index = player.FindBuffIndex(BuffType<TarragonImmunity>());
                if (player.buffTime[index] < 2 && player.buffType[index] == BuffType<TarragonImmunity>())
                {
                    player.AddCD(LAPContent.CDType<TarragonImmunityCD>(), 1500);
                    player.DelBuff(index);
                }
            }
        }
        public static void Magic_OnHitNPC(CIPlayer player, NPC.HitInfo hit, NPC target)
        {
            if (hit.Crit)
                player.TarragonMagicHitCount++;
            if (!player.HasCount("TarragonMagicHealCD"))
            {
                if (Main.rand.NextBool(2))
                {
                    int healAmount = Main.rand.Next(6) + 3;
                    player.Player.NCHeal(healAmount);
                    player.AddCount("TarragonMagicHealCD", 90);
                }
            }
        }
        public static void MagicShoot_PostUpdateMisc(Player player)
        {
            if (Main.myPlayer != player.whoAmI || player.ItemTimeIsZero || !player.ItemAnimationActive || player.HeldItem is null || player.HeldItem.damage == 0 || !player.HeldItem.DamageType.CountsAsClass(DamageClass.Magic))
                return;
            if (!player.CI().HasCount("TarragonMagicFireEffectCD"))
            {
                if (player.CI().TarragonMagicHitCount >= 5 && player.whoAmI == Main.myPlayer)
                {
                    Vector2 velocity = player.GetPlayerToMouseVector2() * 3f;
                    player.CI().TarragonMagicHitCount = 0;
                    int leafAmt = 9 + Main.rand.Next(3); // 9, 10, or 11
                    int leafDamage = player.GetIntDamage<MagicDamageClass>(CIArmorBlance.TarragonMagicLeafBaseD(player));

                    for (int l = 0; l < leafAmt; l++)
                    {
                        float spreadMult = 0.001f * l;
                        float xDiff = velocity.X + Main.rand.Next(-6, 6) * spreadMult;
                        float yDiff = velocity.Y + Main.rand.Next(-6, 6) * spreadMult;
                        int projectile = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, new Vector2(xDiff, yDiff), ProjectileID.Leaf, leafDamage, 0f, player.whoAmI);
                        if (projectile.InBounds(Main.maxProjectiles))
                        {
                            Main.projectile[projectile].DamageType = DamageClass.Generic;
                            Main.projectile[projectile].timeLeft = 120;
                        }
                    }
                }
                player.CI().AddCount("TarragonMagicFireEffectCD", (int)player.ApplyWeaponAttackSpeed(player.ActiveItem(), player.ActiveItem().useTime, 2));
            }
        }
        public static void Melee_Hurt(Player player)
        {
            if (Main.rand.NextBool(4))
                player.AddBuff(BuffType<TarraLife>(), 240);
            if (player.CI().AuricSet)
                player.AddBuff(BuffType<TarraLife>(), 240);
        }
        public static void MeleeArmorSetBonus_KeyPress(CIPlayer player, Player player1)
        {
            if (player1.whoAmI != Main.myPlayer)
                return;
            if (!player1.HasCD<TarragonCloakCD>())
                player1.AddBuff(BuffType<TarragonCloak>(), 602);
        }
        public static void Ranged_OnHitNPCWithProj(CIPlayer player, Projectile proj, NPC.HitInfo hit, NPC target)
        {
            if (player.Player.whoAmI != Main.myPlayer)
                return;
            if (!player.HasCount("TarragonRangedProj"))
            {
                int finalDamage = player.Player.GetIntDamage<RangedDamageClass>(CIArmorBlance.TarragonRangedHit(player.Player));
                int projectileTypes = ProjectileType<TarraEnergy>();
                float randomAngleOffset = (float)(Main.rand.NextFloat(MathHelper.TwoPi));
                Vector2 direction = new((float)Math.Cos(randomAngleOffset), (float)Math.Sin(randomAngleOffset));
                float randomSpeed = Main.rand.NextFloat(6f, 8f);
                Projectile.NewProjectile(proj.GetSource_FromThis(), proj.Center, direction * randomSpeed, projectileTypes, finalDamage, proj.knockBack);
                player.AddCount("TarragonRangedProj", 4);
            }
        }
        public static void Rogue_PostUpdateMisc(Player player)
        {
            for (int l = 0; l < Player.MaxBuffs; l++)
            {
                bool shouldAffect = Main.debuff[player.buffType[l]];
                if (shouldAffect)
                    player.GetDamage<ThrowingDamageClass>() += 0.1f;
            }
        }
        public static void Rogue_KeyPress(CIPlayer player, Player player1)
        {
            if (player1.whoAmI != Main.myPlayer)
                return;
            if (!player1.HasCD<TarragonImmunityCD>())
            {
                player1.AddBuff(BuffType<TarragonImmunity>(), 180);
            }
        }
        public static void Summon_PostUpdateMisc(Player player)
        {
            if (player.miscCounter % 30 == 0)
            {
                int damage = player.CalcIntDamage<GenericDamageClass>(200);
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage && Vector2.Distance(player.Center, npc.Center) <= 300f)
                    {
                        if (player.whoAmI == Main.myPlayer)
                        {
                            int dir = npc.Center.X > player.Center.X ? 1 : -1;
                            player.ApplyDamageToNPC(npc, damage, 1f, dir, false, DamageClass.Generic, true);
                        }
                    }
                }
            }
        }
    }
}
