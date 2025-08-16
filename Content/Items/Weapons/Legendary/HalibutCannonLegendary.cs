using System;
using System.Collections.Generic;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Rarity;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
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
        public const int BaseDamage = 1;
        public static string TextRoute => $"{Generic.GetWeaponLocal}.Ranged.HalibutCannonLegendary";
        internal bool IsDownedWallOfFlesh = Main.hardMode;
        internal bool IsDownedMoonLord = Condition.DownedMoonLord.IsMet();
        internal bool IsDownedPostYharonBoss = (CIDownedBossSystem.DownedLegacyScal || DownedBossSystem.downedCalamitas) && DownedBossSystem.downedExoMechs;
        public override void AddRecipes()
        {
            //微光转化……
            Type.ShimmerEach<HalibutCannon>();
            base.AddRecipes();
        }
        public override void SetDefaults()
        {
            Item.width = 112;
            Item.height = 52;
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
            Item.UseSound = CISoundMenu.HalibutCannonFire;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-30, 0);
        }
        public override void UseItemFrame(Player player)
        {
            CIFunction.NoHeldProjUpdateAim(player, 0, 1);
            float rotation = (player.Center - player.Calamity().mouseWorld).ToRotation() * player.gravDir + MathHelper.PiOver2;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
        }
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit += 10 * (Convert.ToInt32(IsDownedWallOfFlesh) + Convert.ToInt32(IsDownedMoonLord) + Convert.ToInt32(IsDownedPostYharonBoss));
            base.ModifyWeaponCrit(player, ref crit);
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float buff = (float)((float)(BaseDamage + DamageScaling() + Generic.GenericLegendBuffInt(0)) / BaseDamage);
            damage *= buff;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // 重写了加成
            int basewMinBullet = 1;
            int baseMaxBullet = 4;
            int addBulletCount = 0;
            addBulletCount = AddBullet();
            int bulletAmt = Main.rand.Next(basewMinBullet + addBulletCount, baseMaxBullet + addBulletCount);
            for (int index = 0; index < bulletAmt; ++index)
            {
                int shot = Projectile.NewProjectile(source, position + new Vector2(0, 8), velocity.RotatedByRandom(MathHelper.ToRadians(3)) * Main.rand.NextFloat(0.65f, 1.1f), type, damage, knockback, player.whoAmI);
                Main.projectile[shot].timeLeft = 120;
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            int booster = DamageScaling() + Generic.GenericLegendBuffInt();
            string update = this.GetLocalization("LegendaryScaling").Format(booster.ToString(), AddBullet().ToString());
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
        public static int AddBullet()
        {
            int totaladd = 0;
            totaladd += CheckDownedAndBuff(NPC.downedBoss3, 1);                                                          // 1
            totaladd += CheckDownedAndBuff(Main.hardMode, 1);                                                            // 2
            totaladd += CheckDownedAndBuff(Condition.DownedMoonLord.IsMet(), 2);                                         // 4
            totaladd += CheckDownedAndBuff(DownedBossSystem.downedProvidence, 4);                                        // 8
            totaladd += CheckDownedAndBuff(DownedBossSystem.downedPolterghast, 4);                                       // 12
            totaladd += CheckDownedAndBuff(DownedBossSystem.downedDoG, 4);                                               // 16
            totaladd += CheckDownedAndBuff(DownedBossSystem.downedYharon || CIDownedBossSystem.DownedLegacyYharonP2, 8); // 24
            totaladd += CheckDownedAndBuff(CIDownedBossSystem.DownedLegacyScal, 6);                                      // 30
            return totaladd;
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
            mul += CheckDownedAndBuff(Condition.DownedKingSlime.IsMet(), 1);     // 2
            mul += CheckDownedAndBuff(DownedBossSystem.downedDesertScourge, 1);  // 3
            mul += CheckDownedAndBuff(Condition.DownedEyeOfCthulhu.IsMet(), 1);  // 4
            mul += CheckDownedAndBuff(DownedBossSystem.downedCrabulon, 1);       // 5
            mul += CheckDownedAndBuff(isDownedAnyEvilBoss, 1);                   // 6
            mul += CheckDownedAndBuff(isDownedAnyEvilBossCalamity, 1);           // 7
            mul += CheckDownedAndBuff(Condition.DownedQueenBee.IsMet(), 1);      // 8
            mul += CheckDownedAndBuff(Condition.DownedSkeletron.IsMet(), 1);     // 9
            mul += CheckDownedAndBuff(Condition.DownedDeerclops.IsMet(), 1);     // 10
            mul += CheckDownedAndBuff(DownedBossSystem.downedSlimeGod, 1);       // 11
            mul += CheckDownedAndBuff(Main.hardMode, 4);                         // 15
            mul += CheckDownedAndBuff(Condition.DownedQueenSlime.IsMet(), 1);    // 16
            mul += downedMechsOrCalTripleBoss * 2;                               // 18 20 22 24 26 28
            mul += CheckDownedAndBuff(isDownedAnyCalClone, 3);                   // 31
            mul += CheckDownedAndBuff(Condition.DownedPlantera.IsMet(), 1);      // 32
            mul += CheckDownedAndBuff(DownedBossSystem.downedAstrumAureus, 1);   // 33
            mul += CheckDownedAndBuff(DownedBossSystem.downedLeviathan, 1);      // 34
            mul += CheckDownedAndBuff(Condition.DownedGolem.IsMet(), 1);         // 35
            mul += downedPostGolemPreCutlistBoss * 2;                            // 37 39 41 43
            mul += CheckDownedAndBuff(Condition.DownedCultist.IsMet(), 2);       // 45
            mul += CheckDownedAndBuff(DownedBossSystem.downedAstrumDeus, 2);     // 47
            mul += CheckDownedAndBuff(Condition.DownedMoonLord.IsMet(), 10);     // 57
            mul += CheckDownedAndBuff(DownedBossSystem.downedGuardians, 3);      // 60
            mul += CheckDownedAndBuff(DownedBossSystem.downedDragonfolly, 3);    // 63
            mul += CheckDownedAndBuff(DownedBossSystem.downedProvidence, 62);    // 125
            mul += downedSentinelsBoss * 1;                                      // 126 127 128
            mul += CheckDownedAndBuff(DownedBossSystem.downedPolterghast, 2);    // 130
            mul += CheckDownedAndBuff(DownedBossSystem.downedBoomerDuke, 5);     // 135
            mul += CheckDownedAndBuff(DownedBossSystem.downedDoG, 5);            // 140
            mul += CheckDownedAndBuff(DownedBossSystem.downedYharon, 85);        // 225
            mul += CheckDownedAndBuff(isDownedExoOrCalamitas, 10);               // 235 245
            mul += CheckDownedAndBuff(CIDownedBossSystem.DownedLegacyScal, 255);  // 500
            mul += CheckDownedAndBuff(DownedBossSystem.downedPrimordialWyrm, 250);// 750
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