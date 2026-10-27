using CalamityInheritance.Content.Projectiles.Legendary;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class DestroyerLegendary : LegendaryWeapon
    {
        public override Color DrawColor => new(65, 105, 225);
        public override int BaseDamage => 17;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 96;
            Item.height = 34;
            Item.DamageType = DamageClass.Magic;
            Item.damage = BaseDamage;
            Item.mana = 20;
            Item.useTime = Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<SHPCLegendHeldProj>();
            Item.shootSpeed = 20f;
            Item.noUseGraphic = true;
            Item.channel = true;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 必须手动转换，不然会按照int进行加成
            float Buff = (float)((float)(BaseDamage + LegendaryDamage() ) / (float)BaseDamage);
            damage *= Buff;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

              public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = Item.useAnimation = 7;
            }
            else
            {
                Item.useTime = Item.useAnimation = 60;
            }
            Item.mana = TierThreeMatcher? 0 : 20;
            return player.ownedProjectileCounts[ProjectileType<SHPCLegendHeldProj>()] < 1;
        }

        public override Vector2? HoldoutOffset()
        {
            // 在设置一点一点调的偏移量
            return new Vector2(-33, -8);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
                Projectile.NewProjectile(source, position, velocity * 0.1f, ProjectileType<SHPCLegendHeldProj>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
            else
                Projectile.NewProjectile(source, position, velocity * 0.1f, ProjectileType<SHPCLegendHeldProj>(), damage, knockback, player.whoAmI, 1f, 0f, 0f);
            return false;
        }
        public override int LegendaryDamage()
        {
            //SHPC时期较早，因此具备时期较多的伤害增长. 同时，此处也采用与叶流类似的机制——即使用加算，而非乘算的增伤
            // 砍了一刀增幅，后期太超模了
            int dmgBuff = 0;
            //bool DownCalclone = DownedBossSystem.downedCalamitasClone || CIDownedBossSystem.DownedCalClone;
            //dmgBuff += DownCalclone ? 2 : 0;   //27 - 19
            //dmgBuff += Condition.DownedPlantera.IsMet() ? 3 : 0;         //32 - 22
            //dmgBuff += DownedBossSystem.downedLeviathan ? 3 : 0;        //37 - 25
            //dmgBuff += DownedBossSystem.downedAstrumAureus ? 3 : 0;      //42 - 28
            //dmgBuff += Condition.DownedGolem.IsMet() ? 4 : 0;           //50 - 32
            //dmgBuff += Condition.DownedEmpressOfLight.IsMet() ? 4 : 0; //60 - 36
            //dmgBuff += Condition.DownedDukeFishron.IsMet() ? 4 : 0;    //70 - 40
            //dmgBuff += DownedBossSystem.downedRavager ? 4 : 0;         //80 - 44
            //dmgBuff += DownedBossSystem.downedPlaguebringer ? 4 : 0;   //90 - 48
            //dmgBuff += Condition.DownedCultist.IsMet() ? 5 : 0;        //100 - 53
            ////没有星神游龙是故意的，我不希望有人说在冲线阶段浪费时间打这个玩意
            //dmgBuff += Condition.DownedMoonLord.IsMet() ? 47 : 0;        //120 - 90
            //dmgBuff += DownedBossSystem.downedGuardians ? 20 : 0;       //170 - 110
            //dmgBuff += DownedBossSystem.downedProvidence ? 110 : 0;      //220 - 220
            //dmgBuff += DownedBossSystem.downedSignus ? 10 : 0;          //250 - 230
            //dmgBuff += DownedBossSystem.downedCeaselessVoid ? 10 : 0;   //280 - 240
            //dmgBuff += DownedBossSystem.downedStormWeaver ? 10 : 0;     //310 - 250
            //dmgBuff += DownedBossSystem.downedPolterghast ? 150 : 0;     //370 - 400
            //dmgBuff += DownedBossSystem.downedBoomerDuke ? 40 : 0;     //470 - 440
            ////我tm又忘记金龙了，不管了，fuckyou
            //dmgBuff += DownedBossSystem.downedDragonfolly ? 10 : 0;      //490 - 450
            //dmgBuff += DownedBossSystem.downedDoG ? 460 : 0;            //610 - 800
            //dmgBuff += DownedBossSystem.downedYharon ? 1460 : 0;         //760 - 2060
            //dmgBuff += DownedBossSystem.downedCalamitas ? 100 : 0;      //960 - 2160
            //dmgBuff += DownedBossSystem.downedExoMechs ? 100 : 0;       //1060 - 2260
            //dmgBuff += DownedBossSystem.downedExoMechs && DownedBossSystem.downedCalamitas && DownedBossSystem.downedPrimordialWyrm && CIDownedBossSystem.DownedLegacyScal ? 500 : 0; //1560 - 2760
            return dmgBuff;
        }
    }
}