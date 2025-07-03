using System;
using System.Collections.Generic;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class HalibutCannonLegendary : CIRanged, ILocalizedModType
    {
        //待办事项：1.与炼狱模组的匹配，2.数值平衡
        //注：大比目鱼的进化设定上只有数值与弹丸数，我们不做其他的升级了
        public const int BaseDamage = 5;
        public static string TextRoute => $"{Generic.GetWeaponLocal}.Ranged.HalibutCannonLegendary";
        internal bool IsDownedWallOfFlesh = Main.hardMode;
        internal bool IsDownedMoonLord = Condition.DownedMoonLord.IsMet();
        internal bool IsDownedPostYharonBoss = (CIDownedBossSystem.DownedLegacyScal || DownedBossSystem.downedCalamitas) && DownedBossSystem.downedExoMechs;
        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaults()
        {
            Item.width = 112;
            Item.height = 52;
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 10;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit += 10 * (Convert.ToInt32(IsDownedWallOfFlesh) + Convert.ToInt32(IsDownedMoonLord) + Convert.ToInt32(IsDownedPostYharonBoss));
            base.ModifyWeaponCrit(player, ref crit);
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float buff = (float)((float)(BaseDamage + DamageScaling() + Generic.GenericLegendBuffInt()) / BaseDamage);
            damage *= buff;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //shoot分三个阶段，分别代表不同的情况。
            int everyBulletAddPerStage = 6;
            int basewMinBullet = 7;
            int baseMaxBullet = 9;
            bool stage1 = Condition.Hardmode.IsMet();
            bool stage2 = Condition.DownedMoonLord.IsMet();
            bool stage3 = (DownedBossSystem.downedCalamitas || CIDownedBossSystem.DownedLegacyScal) && DownedBossSystem.downedExoMechs;
            //boolenm转int
            int stageCounter = Convert.ToInt32(stage1) + Convert.ToInt32(stage2) + Convert.ToInt32(stage3);
            int addBulletCount = stageCounter * everyBulletAddPerStage;
            int bulletAmt = Main.rand.Next(basewMinBullet + addBulletCount, baseMaxBullet + addBulletCount);
            for (int index = 0; index < bulletAmt; ++index)
            {
                float SpeedX = velocity.X + Main.rand.Next(-10, 11) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-10, 11) * 0.05f;
                int shot = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
                Main.projectile[shot].timeLeft = 120;
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            int booster = DamageScaling() + Generic.GenericLegendBuffInt();
            string update = this.GetLocalization("LegendaryScaling").Format(booster.ToString());
            tooltips.FindAndReplace("[SCALING]", update);
            string myGodDude = !Condition.DownedSkeletron.IsMet() && !Main.hardMode ? Language.GetTextValue($"{TextRoute}.OnPreKnockDownedSkeletron") : null;
            if (myGodDude is not null)
                tooltips.Add(new TooltipLine(Mod, "myGod", myGodDude));
            if (!ModLoader.TryGetMod("InfernumMod", out Mod infernum))
                return;
            string getInfernum = Language.GetTextValue($"{TextRoute}.OnInfernumTooltip");
            if (getInfernum is not null)
                tooltips.Add(new TooltipLine(Mod, "infer", getInfernum));
        }
        public static int DamageScaling()
        {
            bool isDownedAnyEvilBoss = Condition.DownedBrainOfCthulhu.IsMet() || Condition.DownedEaterOfWorlds.IsMet();
            bool isDownedAnyEvilBossCalamity = DownedBossSystem.downedHiveMind || DownedBossSystem.downedPerforator;
            bool isDownedAnyCalClone = DownedBossSystem.downedCalamitasClone || CIDownedBossSystem.DownedCalClone;
            bool isDownedExoOrCalamitas = DownedBossSystem.downedExoMechs || DownedBossSystem.downedCalamitas;
            int downedMechsOrCalTripleBoss = 0;
            int downedPostGolemPreCutlistBoss = 0;
            int downedSentinelsBoss = 0;
            downedMechsOrCalTripleBoss += CheckDownedMech();
            downedPostGolemPreCutlistBoss += CheckDownedGolem();
            downedSentinelsBoss += CheckDownedSentinels();
            int mul = 0;
            //Calamity has over 40 bosses, should i have to do every stage?
            mul += CheckDownedAndBuff(Condition.DownedKingSlime.IsMet(), 1);
            mul += CheckDownedAndBuff(DownedBossSystem.downedDesertScourge, 2);
            mul += CheckDownedAndBuff(Condition.DownedEyeOfCthulhu.IsMet(), 3);
            mul += CheckDownedAndBuff(DownedBossSystem.downedCrabulon, 4);
            mul += CheckDownedAndBuff(isDownedAnyEvilBoss, 5);
            mul += CheckDownedAndBuff(isDownedAnyEvilBossCalamity, 6);
            mul += CheckDownedAndBuff(Condition.DownedQueenBee.IsMet(), 7);
            mul += CheckDownedAndBuff(Condition.DownedSkeletron.IsMet(), 8);
            mul += CheckDownedAndBuff(Condition.DownedDeerclops.IsMet(), 9);
            mul += CheckDownedAndBuff(DownedBossSystem.downedSlimeGod, 10);
            mul += CheckDownedAndBuff(Main.hardMode, 11);
            mul += CheckDownedAndBuff(Condition.DownedQueenSlime.IsMet(), 12);
            mul += CheckDownedAndBuff(DownedBossSystem.downedCryogen, 13);
            mul += downedMechsOrCalTripleBoss * 15;
            mul += CheckDownedAndBuff(isDownedAnyCalClone, 16);
            mul += CheckDownedAndBuff(Condition.DownedPlantera.IsMet(), 17);
            mul += CheckDownedAndBuff(DownedBossSystem.downedAstrumAureus, 18);
            mul += CheckDownedAndBuff(DownedBossSystem.downedLeviathan, 19);
            mul += CheckDownedAndBuff(Condition.DownedGolem.IsMet(), 20);
            mul += downedPostGolemPreCutlistBoss * 21;
            mul += CheckDownedAndBuff(Condition.DownedCultist.IsMet(), 22);
            mul += CheckDownedAndBuff(DownedBossSystem.downedAstrumDeus, 23);
            mul += CheckDownedAndBuff(Condition.DownedMoonLord.IsMet(), 24);
            mul += CheckDownedAndBuff(DownedBossSystem.downedGuardians, 25);
            mul += CheckDownedAndBuff(DownedBossSystem.downedDragonfolly, 26);
            mul += CheckDownedAndBuff(DownedBossSystem.downedProvidence, 27);
            mul += downedSentinelsBoss * 28;
            mul += CheckDownedAndBuff(DownedBossSystem.downedPolterghast, 29);
            mul += CheckDownedAndBuff(DownedBossSystem.downedBoomerDuke, 30);
            mul += CheckDownedAndBuff(DownedBossSystem.downedDoG, 31);
            mul += CheckDownedAndBuff(DownedBossSystem.downedYharon, 32);
            mul += CheckDownedAndBuff(isDownedExoOrCalamitas, 33);
            mul += CheckDownedAndBuff(CIDownedBossSystem.DownedLegacyScal, 34);
            mul += CheckDownedAndBuff(DownedBossSystem.downedPrimordialWyrm, 35);
            return mul;
        }

        private static int CheckDownedSentinels()
        {
            int downdeTime = 0;
            downdeTime += DownedBossSystem.downedSignus ? 1 : 0;
            downdeTime += DownedBossSystem.downedCeaselessVoid ? 1 : 0;
            downdeTime += DownedBossSystem.downedStormWeaver ? 1 : 0;
            return downdeTime;
        }

        private static int CheckDownedGolem()
        {
            int downedTime = 0;
            downedTime += Condition.DownedDukeFishron.IsMet() ? 1 : 0;
            downedTime += Condition.DownedEmpressOfLight.IsMet() ? 1 : 0;
            downedTime += DownedBossSystem.downedRavager ? 1 : 0;
            downedTime += DownedBossSystem.downedPlaguebringer ? 1 : 0;
            return downedTime;
        }

        private static int CheckDownedMech()
        {
            int downedTime = 0;
            downedTime += Condition.DownedTwins.IsMet() ? 1 : 0;
            downedTime += Condition.DownedDestroyer.IsMet() ? 1 : 0;
            downedTime += Condition.DownedSkeletronPrime.IsMet() ? 1 : 0;
            downedTime += DownedBossSystem.downedCryogen ? 1 : 0;
            downedTime += DownedBossSystem.downedAquaticScourge ? 1 : 0;
            downedTime += DownedBossSystem.downedBrimstoneElemental ? 1 : 0;
            return downedTime;
        }

        public static int CheckDownedAndBuff(bool isDownedOrNot, int stat) => stat * Convert.ToInt32(isDownedOrNot);
    }
}