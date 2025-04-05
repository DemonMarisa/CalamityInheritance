using System.Data;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Sounds;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Rarity.Special;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class DestroyerLegendary: CIMagic, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Magic";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public static readonly int BaseDamage = 22;
        public override void SetDefaults()
        {
            Item.width = 96;
            Item.height = 34;
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DestroyerLegendaryBomb>();
            Item.shootSpeed = 20f;
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = CIConfig.Instance.LegendaryRarity ? ModContent.RarityType<SHPCAqua>() : ModContent.RarityType<MaliceChallengeDrop>();
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= (BaseDamage + LegendaryBuff()) / BaseDamage;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            //升级的Tooltip:
            if (mp.DestroyerTier1)
            {
                string t1 = Language.GetTextValue($"{Generic.GetWeaponLocal}.Magic.DestroyerLegendary.TierOne");
                tooltips.Add(new TooltipLine(Mod, "TIERONE", t1));
            }
            if (mp.DestroyerTier2)
            {
                string t2 = Language.GetTextValue($"{Generic.GetWeaponLocal}.Magic.DestroyerLegendary.TierTwo");
                tooltips.Add(new TooltipLine(Mod, "TIERTWO", t2));
            }
            if (mp.DestroyerTier1)
            {
                string t3 = Language.GetTextValue($"{Generic.GetWeaponLocal}.Magic.DestroyerLegendary.TierThree");
                tooltips.Add(new TooltipLine(Mod, "TIERTHREE", t3));
            }
            //以下，用于比较复杂的计算
            int boostPercent = LegendaryBuff();
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.UseSound = CommonCalamitySounds.LaserCannonSound;
            }
            else
            {
                Item.UseSound = SoundID.Item92;
            }
            return base.CanUseItem(player);
        }
        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2)
                mult *= player.CIMod().DestroyerTier3 ? 0.01f : 0.3f;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            if (player.altFunctionUse == 2)
                return 1f;

            return 1 / 7.14f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var p = player.CIMod();
            int bCounts = p.DestroyerTier2 ? 5 : 3;
            int lCounts = p.DestroyerTier2 ? 5 : 3;
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < lCounts; i++)
                {
                    float velX = velocity.X + Main.rand.Next(-20, 21) * 0.05f;
                    float velY = velocity.Y + Main.rand.Next(-20, 21) * 0.05f;
                    
                    Projectile.NewProjectile(source, position, new(velX,velY), ModContent.ProjectileType<DestroyerLegendaryLaser>(), damage, knockback * 0.5f, player.whoAmI, 0f, 0f);
                }
                if(p.fireCD == 0)
                {
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<DestroyerLegendaryBomb>(), damage, knockback, player.whoAmI, 1f);
                    p.fireCD = 60;
                }
                return false;
            }
            else
            {
                for (int j = 0; j < bCounts; j++)
                {
                    float velX = velocity.X + Main.rand.Next(-40, 41) * 0.05f;
                    float velY = velocity.Y + Main.rand.Next(-40, 41) * 0.05f;
                    Projectile.NewProjectile(source, position.X, position.Y, velX, velY, ModContent.ProjectileType<DestroyerLegendaryBomb>(), (int)(damage * 1.1), knockback, player.whoAmI, 0f, 0f);
                }
                
                return false;
            }
        }
        public static int LegendaryBuff()
        {
            //SHPC时期较早，因此具备时期较多的伤害增长. 同时，此处也采用与叶流类似的机制——即使用加算，而非乘算的增伤
            int dmgBuff = 0;
            dmgBuff += DownedBossSystem.downedCalamitasClone ? 5 : 0;   //27
            dmgBuff += Condition.DownedPlantera.IsMet() ? 5: 0;         //32
            dmgBuff += DownedBossSystem.downedLeviathan ? 5 : 0;        //37
            dmgBuff += DownedBossSystem.downedAstrumAureus? 5 : 0;      //42
            dmgBuff += Condition.DownedGolem.IsMet() ? 8 : 0;           //50
            dmgBuff += Condition.DownedEmpressOfLight.IsMet() ? 10 : 0; //60
            dmgBuff += Condition.DownedDukeFishron.IsMet() ? 10 : 0;    //70
            dmgBuff += DownedBossSystem.downedRavager ? 10 : 0;         //80
            dmgBuff += DownedBossSystem.downedPlaguebringer ? 10 : 0;   //90
            dmgBuff += Condition.DownedCultist.IsMet() ? 10 : 0;        //100
            //没有星神游龙是故意的，我不希望有人说在冲线阶段浪费时间打这个玩意
            dmgBuff += Condition.DownedMoonLord.IsMet() ? 20: 0;        //120
            dmgBuff += DownedBossSystem.downedGuardians ? 30: 0;        //150
            dmgBuff += DownedBossSystem.downedProvidence ? 30 : 0;      //180
            dmgBuff += DownedBossSystem.downedSignus ? 15 : 0;          //195
            dmgBuff += DownedBossSystem.downedCeaselessVoid ? 15 : 0;   //210
            dmgBuff += DownedBossSystem.downedStormWeaver ? 15 : 0;     //225
            dmgBuff += DownedBossSystem.downedPolterghast ? 30 : 0;     //255
            dmgBuff += DownedBossSystem.downedBoomerDuke ? 30 : 0;      //275
            //我tm又忘记金龙了，不管了，fuckyou
            dmgBuff += DownedBossSystem.downedDragonfolly? 5: 0;        //280
            dmgBuff += DownedBossSystem.downedDoG ? 50 : 0;             //330
            dmgBuff += DownedBossSystem.downedYharon ? 50 : 0;          //380
            dmgBuff += DownedBossSystem.downedCalamitas ? 70 : 0;       //450
            dmgBuff += DownedBossSystem.downedExoMechs ? 70 : 0;        //520
            dmgBuff += (DownedBossSystem.downedExoMechs && DownedBossSystem.downedCalamitas && DownedBossSystem.downedPrimordialWyrm) ? 480 : 0; //1000
            return dmgBuff;
        }
    }
}