using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs;
using Terraria.ID;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Healing;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.ArmorProj;

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
                Projectile.NewProjectile(Player.GetSource_FromThis(), getSpwanPos, velocity * 4f, ModContent.ProjectileType<GodSlayerDart>(), finalDamage, 0f, Player.whoAmI);
                fireCD = 60;
            }
        }
        #endregion

        public override void OnHitNPCWithProj(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            CalamityPlayer modPlayer = Player.Calamity();
            CalamityGlobalNPC cgn = target.Calamity();
            Player player = Main.player[projectile.owner];
            var usPlayer = player.CIMod();
            var heldingItem = player.ActiveItem();
            if (Player.whoAmI != Main.myPlayer)
                return;
            //近战射弹
            MeleeOnHit(projectile, target, hit, damageDone);
            //远程射弹
            RangedOnHit(projectile, target, hit, damageDone);
            //魔法射弹
            MagicOnHit(projectile, target, hit, damageDone);
            //召唤射弹
            SummonOnHit(projectile, target, hit, damageDone);
            //盗贼射弹
            RogueOnHit(projectile, target, hit, damageDone);
            //全局射弹
            GenericOnhit(projectile, target, hit, damageDone);
            //debuff
            DebuffOnHit(projectile, target, hit, damageDone);
            //debuff
            //吸血射弹
            if (!projectile.npcProj && !projectile.trap && projectile.friendly)
                ProjLifesteal(target, projectile, damageDone, hit.Crit);
        }

        public void DebuffOnHit(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            bool ifMelee = projectile.CountsAsClass<MeleeDamageClass>() || projectile.CountsAsClass<MeleeNoSpeedDamageClass>();
            bool ifTrueMelee = projectile.CountsAsClass<TrueMeleeDamageClass>() || projectile.CountsAsClass<TrueMeleeNoSpeedDamageClass>();
            bool ifRogue = projectile.CountsAsClass<RogueDamageClass>();
            bool ifSummon = projectile.CountsAsClass<SummonDamageClass>();
            if (ifMelee || ifTrueMelee || ifRogue || projectile.CountsAsClass<SummonMeleeSpeedDamageClass>())
            {
                if (BuffStatsArmorShatter)
                    CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<Crumbling>());
            }
            if (ifMelee || ifTrueMelee)
            {
                if (ElemGauntlet)
                {
                    target.AddBuff(ModContent.BuffType<ElementalMix>(), 300, false);
                    target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300, false);
                    target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300, false);
                    target.AddBuff(BuffID.Frostburn2, 300);
                    target.AddBuff(BuffID.CursedInferno, 300);
                    target.AddBuff(BuffID.Inferno, 300);
                    target.AddBuff(BuffID.Venom, 300);
                }
            }
            if (ifSummon)
            {
                if (NucleogenesisLegacy)
                {
                    target.AddBuff(BuffID.Electrified, 120);
                    target.AddBuff(ModContent.BuffType<HolyFlames>(), 300, false);
                    target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300, false);
                    target.AddBuff(ModContent.BuffType<Irradiated>(), 300, false);
                    target.AddBuff(ModContent.BuffType<Shadowflame>(), 300, false);
                }
            }
            //北辰鹦哥鱼的射弹计数器
            if (projectile.type == ModContent.ProjectileType<PolarStarLegacy>())
                PolarisBoostCounter += 1;

            #region Lore
            if (LorePerforator)
                target.AddBuff(BuffID.Ichor, 90);
            if (LoreHive)
                target.AddBuff(BuffID.CursedInferno, 90);
                
            if (LoreProvidence || PanelsLoreProvidence)
                target.AddBuff(ModContent.BuffType<HolyInferno>(), 180, false);

            if (BuffStatsHolyWrath)
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 180, false);

            if (YharimsInsignia)
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 120, false);

            if (BuffStatsDraconicSurge && Main.zenithWorld)
                target.AddBuff(ModContent.BuffType<Dragonfire>(), 360, false);
            #endregion
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
            CalamityGlobalProjectile modProj = proj.Calamity();

            if (Main.player[Main.myPlayer].lifeSteal > 0f && !Player.moonLeech && target.lifeMax > 5)
            {
                if (AuricSilvaSet)
                {
                    double healMult = 0.1;
                    int heal = Main.rand.Next(5, 11);

                    if (CalamityGlobalProjectile.CanSpawnLifeStealProjectile(healMult, heal))
                        CalamityGlobalProjectile.SpawnLifeStealProjectile(proj, Player, heal, ModContent.ProjectileType<SilvaOrb>(), 3000f, 2f);
                }
                if (GodSlayerMagicSet)
                {
                    double healMult = 0.1;
                    int heal = Main.rand.Next(5, 11);

                    if (CalamityGlobalProjectile.CanSpawnLifeStealProjectile(healMult, heal))
                        CalamityGlobalProjectile.SpawnLifeStealProjectile(proj, Player, heal, ModContent.ProjectileType<GodSlayerHealOrb>(), 3000f, 2f);
                }
            }
        }
        #endregion
    }
}
