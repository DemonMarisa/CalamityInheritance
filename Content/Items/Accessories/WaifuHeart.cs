using System.Collections.Generic;
using CalamityInheritance.Content.Projectiles.Summon.SomeRandomGirls;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.Summon;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class WaifuHeart : CIAccessories, ILocalizedModType
    {
        public static bool FuckYouEHeart { get => fuckYouEHeart; set => fuckYouEHeart = value; }

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 8));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<HeartoftheElements>();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.defense = 10;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.brimstoneWaifu || modPlayer.sandWaifu || modPlayer.sandBoobWaifu || modPlayer.cloudWaifu || modPlayer.sirenWaifu)
            {
                return false;
            }
            return true;
        }

        private static bool fuckYouEHeart = true; //默认为true
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player != null && !player.dead)
                Lighting.AddLight((int)player.Center.X / 16, (int)player.Center.Y / 16, Main.DiscoR / 255f, Main.DiscoG / 255f, Main.DiscoB / 255f);

            CalamityPlayer modPlayer = player.Calamity();
            var usPlayer = player.CIMod();
            usPlayer.EHeartStats = true;
            modPlayer.allWaifus = true;
            
            modPlayer.elementalHeart = true;

            int brimmy = ProjectileType<BrimWaifu>();
            int siren = ProjectileType<WaterWaifuMinion>();
            int healer = ProjectileType<SandWaifuHealer>();
            int sandy = ProjectileType<SandWaifu>();
            int cloudy = ProjectileType<CloudWaifu>();
            if(hideVisual)
            {
                usPlayer.EHeartStatsBoost = true;
                usPlayer.FuckEHeart = true;
                modPlayer.allWaifus = false;
                fuckYouEHeart = false;
            }
            else
            {
                usPlayer.EHeartStatsBoost = false;
                usPlayer.FuckEHeart = false;
                modPlayer.allWaifus = true;
                fuckYouEHeart = true;
            }

            var source = player.GetSource_Accessory(Item);
            Vector2 velocity = new(0f, -1f);

            int baseDamage = player.ApplyArmorAccDamageBonusesTo(150);
            int elementalDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);

            float kBack = 2f + player.GetKnockback<SummonDamageClass>().Additive;
            if (player.ownedProjectileCounts[brimmy] > 1 || player.ownedProjectileCounts[siren] > 1 ||
                player.ownedProjectileCounts[healer] > 1 || player.ownedProjectileCounts[sandy] > 1 ||
                player.ownedProjectileCounts[cloudy] > 1)
            { 
                player.ClearBuff(BuffType<HotE>());
            }
            if(usPlayer.FuckEHeart == false)
            {
                if (player != null && player.whoAmI == Main.myPlayer && !player.dead)
                {
                    if (player.FindBuffIndex(BuffType<HotE>()) == -1)
                    {
                        player.AddBuff(BuffType<HotE>(), 3600, true);
                    }
                    if (player.ownedProjectileCounts[brimmy] < 1)
                    {
                        int p = Projectile.NewProjectile(source, player.Center, velocity, brimmy, elementalDmg, kBack, player.whoAmI);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = baseDamage;
                    }
                    if (player.ownedProjectileCounts[siren] < 1)
                    {
                        int p = Projectile.NewProjectile(source, player.Center, velocity, siren, elementalDmg, kBack, player.whoAmI);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = baseDamage;
                    }
                    if (player.ownedProjectileCounts[healer] < 1)
                    {
                        int p = Projectile.NewProjectile(source, player.Center, velocity, healer, elementalDmg, kBack, player.whoAmI);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = baseDamage;
                    }
                    if (player.ownedProjectileCounts[sandy] < 1)
                    {
                        int p = Projectile.NewProjectile(source, player.Center, velocity, sandy, elementalDmg, kBack, player.whoAmI);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = baseDamage;
                    }
                    if (player.ownedProjectileCounts[cloudy] < 1)
                    {
                        int p = Projectile.NewProjectile(source, player.Center, velocity, cloudy, elementalDmg, kBack, player.whoAmI);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = baseDamage;
                    }
                }
            }
        }
        // public static int IfVisualIsOff(bool hideVisual = true)
        // {
        //     return hideVisual? StatMutilpler: 0;
        // }
        public static readonly int StatMutilpler = 5;
        //这里的具体数值跟什么的没法参考，纯纯为了下面的tooltip凑参数用的
        //要看直接去砍misceffect里面的
        public static readonly int BasicLifeStatAndManaStatBuff = 15; 
        public static readonly int BasicLifeRegenSpeedStatBuff = 20;
        public static readonly int BasicMovementSpeedAndDRStatBuff = 5;
        public static readonly int BasicDamageAndCritsStatBuff = 5;
        public static readonly int BasicJumpSpeedStatBuff = 50;
        public static readonly int BasicManaCostReductionStatBuff = 5;

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int lifeAndManaStat         = fuckYouEHeart ? BasicLifeStatAndManaStatBuff      :   BasicLifeStatAndManaStatBuff+25;
            int lifeRegenStat           = fuckYouEHeart ? BasicLifeRegenSpeedStatBuff/20    : ((BasicLifeRegenSpeedStatBuff+80)/20);
            int movementSpeedAndDRStat  = fuckYouEHeart ? BasicMovementSpeedAndDRStatBuff   :   BasicMovementSpeedAndDRStatBuff+5;
            int damageAndCCStat         = fuckYouEHeart ? BasicDamageAndCritsStatBuff       :   BasicDamageAndCritsStatBuff+5;
            int jumSpeedStat            = fuckYouEHeart ? BasicJumpSpeedStatBuff            :   BasicJumpSpeedStatBuff+100;
            int manaCostReductionstat   = fuckYouEHeart ? BasicManaCostReductionStatBuff    :   BasicManaCostReductionStatBuff+5;
            string totalStatBuffs = this.GetLocalization("StatsBuff").Format(
                lifeAndManaStat.        ToString(),
                lifeRegenStat.          ToString(),
                movementSpeedAndDRStat. ToString(),
                damageAndCCStat.        ToString(),
                jumSpeedStat.           ToString(),
                manaCostReductionstat.  ToString()
            );
            tooltips.FindAndReplace("[STATSBUFF]", totalStatBuffs);
        }

        public override void UpdateVanity(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.allWaifusVanity = true;
            modPlayer.elementalHeart = true;

            int brimmy = ProjectileType<BrimWaifu>();
            int siren = ProjectileType<WaterWaifuMinion>();
            int healer = ProjectileType<SandWaifuHealer>();
            int sandy = ProjectileType<SandWaifu>();
            int cloudy = ProjectileType<CloudWaifu>();

            var source = player.GetSource_Accessory(Item);
            Vector2 velocity = new(0f, -1f);

            // 08DEC2023: Ozzatron: Elementals spawned with... Hold on a second. Why the fuck are we doing damage calculations when the accessory is in vanity?!
            int baseDamage = player.ApplyArmorAccDamageBonusesTo(200);
            int elementalDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);

            float kBack = 2f + player.GetKnockback<SummonDamageClass>().Additive;

            if (player.ownedProjectileCounts[brimmy] > 1 || player.ownedProjectileCounts[siren] > 1 ||
                player.ownedProjectileCounts[healer] > 1 || player.ownedProjectileCounts[sandy] > 1 ||
                player.ownedProjectileCounts[cloudy] > 1)
            {
                player.ClearBuff(BuffType<HotE>());
                
            }
            if (player != null && player.whoAmI == Main.myPlayer && !player.dead)
            {
                if (player.FindBuffIndex(BuffType<HotE>()) == -1)
                {
                    player.AddBuff(BuffType<HotE>(), 3600, true);
                }
                if (player.ownedProjectileCounts[brimmy] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, brimmy, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = baseDamage;
                }
                if (player.ownedProjectileCounts[siren] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, siren, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = baseDamage;
                }
                if (player.ownedProjectileCounts[healer] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, healer, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = baseDamage;
                }
                if (player.ownedProjectileCounts[sandy] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, sandy, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = baseDamage;
                }
                if (player.ownedProjectileCounts[cloudy] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, cloudy, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = baseDamage;
                }
            }
        }
    }
}
