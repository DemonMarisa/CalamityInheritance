﻿using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.ArmorProj;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Projectiles.Typeless.Heal;
using CalamityInheritance.Content.Projectiles.Magic;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region On Hit NPC With Item
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (Player.whoAmI != Main.myPlayer)
                return;
            Player player = Main.LocalPlayer;
            int weaponDamage = player.HeldItem.damage;
            if (GodSlayerMelee && fireCD <= 0 && (hit.DamageType == DamageClass.Melee || hit.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>()))
            {
                int finalDamage = 500 + weaponDamage / 2;
                Vector2 getSpwanPos = new(Player.Center.Y, Player.Center.X);
                Vector2 velocity = CIFunction.GiveVelocity(200f);
                Projectile.NewProjectile(Player.GetSource_FromThis(), getSpwanPos, velocity * 4f, ModContent.ProjectileType<GodslayerDartMount>(), finalDamage, 0f, Player.whoAmI);
                fireCD = 60;
            }
            //T2庇护: 物品击中敌人时使自己免疫防损
            if (item.type == ModContent.ItemType<DefenseBlade>() && hit.Damage > 5 && DefendTier2 && Player.whoAmI == Main.myPlayer)
            {
                Player.AddBuff(ModContent.BuffType<DefenderBuff>(), 60);
            }
            //熟练度

            bool isTrueMelee = hit.DamageType.CountsAsClass<TrueMeleeDamageClass>() || hit.DamageType.CountsAsClass<TrueMeleeNoSpeedDamageClass>();
            bool isMelee = hit.DamageType.CountsAsClass<MeleeNoSpeedDamageClass>() || hit.DamageType.CountsAsClass<MeleeDamageClass>() || isTrueMelee;

            if (isMelee && target.active)
                GiveExpMelee(target, isTrueMelee, isMelee, hit.Crit);
        }
        #endregion

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
                    CIFunction.SpawnHealProj(proj.GetSource_FromThis(), proj.Center, Player, heal, 8f, 1f , CD, ModContent.ProjectileType<SilvaOrbLegacy>());
                if (GodSlayerMagicSet)
                    CIFunction.SPSpawnHealProj(proj.GetSource_FromThis(), proj.Center, Player, gsheal, 6f, 3f, gsCD, ModContent.ProjectileType<GodSlayerHealOrbLegacy>());
            }
        }
        #endregion
    }
}
