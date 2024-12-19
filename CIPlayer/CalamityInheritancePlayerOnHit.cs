using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs;
using Terraria.ID;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.Potions;
using CalamityMod.Cooldowns;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.CalPlayer;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region On Hit NPC With Proj
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (Player.whoAmI != Main.myPlayer)
                return;

            CalamityGlobalNPC cgn = target.Calamity();

            if (perforatorLore)
            {
                target.AddBuff(BuffID.Ichor, 90);
            }
            if (hiveMindLore)
            {
                target.AddBuff(BuffID.CursedInferno, 90);
            }
            if (providenceLore)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 180, false);
            }
            if (holyWrath)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 180, false);
            }

        }
        #endregion

        #region Debuffs
        public void NPCDebuffs(NPC target, bool melee, bool ranged, bool magic, bool summon, bool rogue, bool whip, bool proj = false, bool noFlask = false)
        {
            if ((melee || rogue || whip) && !noFlask)
            {
                if (armorShattering)
                {
                    CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<Crumbling>());
                }
            }
        }
        #endregion

        #region Item
        public void ItemOnHit(Item item, int damage, Vector2 position, bool crit, bool npcCheck)
        {
            var source = Player.GetSource_ItemUse(item);
            CalamityPlayer modPlayer = Player.Calamity();
            if (item.CountsAsClass<MeleeDamageClass>())
            {
                titanBoost = 600;
                if (npcCheck)
                {
                    if (modPlayer.ataxiaGeyser && Player.ownedProjectileCounts[ModContent.ProjectileType<ChaoticGeyser>()] < 3)
                    {
                        // Ataxia True Melee Geysers: 15%, softcap starts at 300 base damage
                        int geyserDamage = CalamityUtils.DamageSoftCap(damage * 0.15, 45);
                        Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<ChaoticGeyser>(), geyserDamage, 2f, Player.whoAmI, 0f, 0f);
                    }
                    if (modPlayer.soaring)
                    {
                        double useTimeMultiplier = 0.85 + (item.useTime * item.useAnimation / 3600D); //28 * 28 = 784 is average so that equals 784 / 3600 = 0.217777 + 1 = 21.7% boost
                        double wingTimeFraction = Player.wingTimeMax / 20D;

                        // TODO -- this scaling function is probably totally screwed. What is it supposed to do?
                        double meleeStatMultiplier = (double)(Player.GetTotalDamage<MeleeDamageClass>().Additive * (float)(Player.GetTotalCritChance<MeleeDamageClass>() / 10f));

                        if (Player.wingTime < Player.wingTimeMax)
                            Player.wingTime += (int)(useTimeMultiplier * (wingTimeFraction + meleeStatMultiplier));

                        if (Player.wingTime > Player.wingTimeMax)
                            Player.wingTime = Player.wingTimeMax;
                    }
                    if (modPlayer.bloodflareMelee && item.CountsAsClass<MeleeDamageClass>() && modPlayer.bloodflareMeleeHits < 15 && !modPlayer.bloodflareFrenzy && !Player.HasCooldown(BloodflareFrenzy.ID))
                        modPlayer.bloodflareMeleeHits++;
                }
            }
        }
        #endregion
        public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback)
        {
            if (yPower)
                knockback += item.knockBack * 0.25f;
        }
    }
}
