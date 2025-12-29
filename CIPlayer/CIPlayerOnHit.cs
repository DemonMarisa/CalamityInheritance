using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Content.Achievements;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Items.Weapons.Ranged.Scarlet;
using CalamityInheritance.Content.Projectiles.ArmorProj;
using CalamityInheritance.Content.Projectiles.Typeless.Heal;
using CalamityInheritance.Core;
using CalamityInheritance.NPCs.Boss.Yharon;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Projectiles.Summon;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (nanotechold && RaiderStacks < 150 && hit.DamageType == GetInstance<RogueDamageClass>())
            {
                RaiderStacks++;
            }
            if (LoreProvidence || PanelsLoreProvidence)
            {
                target.AddBuff(BuffType<HolyFlames>(), 420, false);
            }
            if (LoreJungleDragon)
            {
                target.AddBuff(BuffType<Dragonfire>(), 300, false);
            }
            if (PlagueHive)
            {
                target.AddBuff(BuffType<Plague>(), 360);
                if (Player.HasProjCount<PlagueBeeSmall>() < 6)
                {
                    int Type = ProjectileType<PlagueBeeSmall>();
                    for (int i = 0; i < 6; i++)
                    {
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Main.rand.NextVector2Circular(0.25f, 0.25f), Type, 45, 1, Player.whoAmI);
                    }
                }
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            
            if (Player.whoAmI != Main.myPlayer)
                return;
            Player player = Main.LocalPlayer;
            int weaponDamage = player.HeldItem.damage;
            if (GodSlayerMelee && fireCD <= 0 && (hit.DamageType == DamageClass.Melee || hit.DamageType == GetInstance<TrueMeleeDamageClass>()))
            {
                int finalDamage = 500 + weaponDamage / 2;
                Vector2 getSpwanPos = new(Player.Center.Y, Player.Center.X);
                Vector2 velocity = CIFunction.GiveVelocity(200f);
                Projectile.NewProjectile(Player.GetSource_FromThis(), getSpwanPos, velocity * 4f, ProjectileType<GodslayerDartMount>(), finalDamage, 0f, Player.whoAmI);
                fireCD = 60;
            }
            //T2庇护: 物品击中敌人时使自己免疫防损
            if (item.type == ItemType<DefenseBlade>() && hit.Damage > 5 && DefendTier2 && Player.whoAmI == Main.myPlayer)
            {
                Player.AddBuff(BuffType<DefenderBuff>(), 60);
            }
            //熟练度

            bool isTrueMelee = hit.DamageType.CountsAsClass<TrueMeleeDamageClass>() || hit.DamageType.CountsAsClass<TrueMeleeNoSpeedDamageClass>();
            bool isMelee = hit.DamageType.CountsAsClass<MeleeNoSpeedDamageClass>() || hit.DamageType.CountsAsClass<MeleeDamageClass>() || isTrueMelee;

            if (isMelee && target.active)
                GiveExpMelee(target, isTrueMelee, isMelee, hit.Crit);
        }
        public override void OnHitNPCWithProj(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (Player.whoAmI != Main.myPlayer)
                return;
            #region 熟练度
            if (target.active)
                GiveExp(target, hit, projectile);
            #endregion
            //近战射弹
            MeleeOnHit(projectile, target, hit, damageDone);
            //远程射弹
            RangedOnHit(projectile, target, hit, damageDone);
            //魔法射弹
            MagicOnHit(projectile, target, hit, damageDone);
            //召唤射弹
            SummonOnHit(projectile, target, hit, damageDone);
            //盗贼射弹
            RogueOnHit(projectile, target, hit, damageDone, projectile.Calamity().stealthStrike);
            //全局射弹
            GenericOnhit(projectile, target, hit, damageDone);
            // 熟练度升级
            // EarnPoints(hit, projectile);
            //debuff
            AddDebuff(projectile, target, ref hit);
            LegendaryDamageTask(projectile, target, hit);
            //吸血射弹
            if (!projectile.npcProj && !projectile.trap && projectile.friendly)
                ProjLifesteal(target, projectile, damageDone, hit.Crit);
        }
        public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback)
        {
            if (BuffStatsYharimsStin)
                knockback += item.knockBack * 0.25f;
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (DesertProwler && item.CountsAsClass<RangedDamageClass>())
                damage.Flat += 1f;
        }
        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            healValue = (int)(healValue * ManaHealMutipler);
        }

        #region Lifesteal
        private void ProjLifesteal(NPC target, Projectile proj, int damage, bool crit)
        {
            int heal = Main.rand.Next(5, 11);
            int CD = Main.rand.Next(1, 50);

            int gsheal = Main.rand.Next(7, 14);
            int gsCD = Main.rand.Next(10, 30);

            if (!Player.moonLeech && target.lifeMax > 5)
            {
                if (AuricSilvaFakeDeath)
                    CIFunction.SpawnHealProj(proj.GetSource_FromThis(), proj.Center, Player, heal, 8f, 1f , CD, ProjectileType<SilvaOrbLegacy>());
                if (GodSlayerMagicSet)
                    CIFunction.SPSpawnHealProj(proj.GetSource_FromThis(), proj.Center, Player, gsheal, 6f, 3f, gsCD, ProjectileType<GodSlayerHealOrbLegacy>());
            }
        }
        #endregion
    }
}
