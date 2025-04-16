using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class DrataliornusLegacy: CIRanged, ILocalizedModType
    {
        
        public virtual string SteamsDetail => Language.GetTextValue("StreamsDetail");

        //应某龙弓激推人要求:
        //击败终灾与星流后：右键倍率0.35 -> 1.0, 基础面板 700 -> 1457, 右键使用速度48 -> 12;
        public double RightClickDamageRatio = CalamityConditions.DownedSupremeCalamitas.IsMet() && CalamityConditions.DownedExoMechs.IsMet()? 1.0: 0.35;

        public int GetWeaponDamage = CalamityConditions.DownedSupremeCalamitas.IsMet() && CalamityConditions.DownedExoMechs.IsMet()? 1457 : 700;
        public int GetRightClickSpeed =CalamityConditions.DownedSupremeCalamitas.IsMet() && CalamityConditions.DownedExoMechs.IsMet() ? 12 : 48; 
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        /************龙弓: 最后的爆改**************
        *龙弓与元素箭袋(旧)/金源射手套联动:
        *左键每次发射的弹幕+1 和 右键同时发射10个且两份复制的龙焰火球
        *龙弓·左键现在根据玩家破甲与防御值提供乘算增伤
        *算法为(1 + 玩家穿甲数/150 + (1 - 玩家护甲值/100)), 这一效果会直接加在武器上
        *爆改了龙弓的火球AI, 现在龙弓的火球能保证追踪
        *严格按照Tooltip的描述重新分配了伤害倍率
        *总之, 这一套下来能让龙弓突破千万dps大关
        *纯输出的情况下预估dps(左键)可能可以突破至亿, 视情况而定
        *之后不对龙弓做任何的修改
        *题外话:
        *我都不知道为什么要跟别的模组卷谁的数值高, 到底是图什么?
        *********************************************/
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 84;
            Item.damage = GetWeaponDamage;
            Item.knockBack = 1f;
            Item.shootSpeed = 18f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 12;
            Item.useAnimation = 24;
            Item.reuseDelay = 120;  //左键受击后120帧内玩家无法再次使用龙弓
            Item.useLimitPerAnimation = 2;
            Item.UseSound = SoundID.Item5;
            Item.shoot = ModContent.ProjectileType<DragonBow>();
            Item.value = CIShopValue.RarityPricePureRed;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.useTurn = false;
            Item.useAmmo = AmmoID.Arrow;
            Item.autoReuse = true;
            Item.rare = ModContent.RarityType<PureRed>();
        }
        /*为啥我不能在SetDefaults里设置武器的暴击率啊?*/
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 6;
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (CIServerConfig.Instance.ShadowspecBuff)
            {
                damage.Base *= 5;
                if (player.altFunctionUse == 1)
                    damage.Base *= 5;
            }
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noUseGraphic = false;  //右键采用海啸的方式
                Item.reuseDelay = GetRightClickSpeed;
            }
            else
            {
                Item.reuseDelay = 120;
                Item.noUseGraphic = true;
                if (player.ownedProjectileCounts[Item.shoot] > 0)
                {
                    return false;
                }
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //更强大的海啸
            if (player.altFunctionUse == 2)
            {
                var usPlayer = player.CIMod();
                int numFlames = 5;
                if(usPlayer.GodSlayerRangedSet && usPlayer.AuricSilvaSet) //佩戴金源射手时
                numFlames = 10;
                int flameID = ModContent.ProjectileType<DragonBowFlameRework>();
                int flameDamage = (int)(damage * RightClickDamageRatio);
                //直接增加伤害倍率, 即0.65f(右键倍率) + 经过穿甲计算后的倍率, 对于20穿甲的玩家, 这一倍率是0.99≈1f, 即取武器本身的伤害
                //对于30穿甲则取1.15f别率.
                
                if(Main.zenithWorld)
                    flameDamage += flameDamage; //处于天顶世界时这玩意弹幕右键基础面板会被双倍

                const float fifteenHundredthPi = 0.471238898f;
                Vector2 spinningpoint = velocity;
                spinningpoint.Normalize();
                spinningpoint *= 36f;
                for (int i = 0; i < numFlames; ++i)
                {
                    float piArrowOffset = i - (numFlames - 1) / 2;
                    Vector2 offsetSpawn = spinningpoint.RotatedBy(fifteenHundredthPi * piArrowOffset, new Vector2());
                    Projectile.NewProjectile(source, position.X + offsetSpawn.X, position.Y + offsetSpawn.Y, velocity.X * 0.7f, velocity.Y * 0.7f, flameID, flameDamage, knockback, player.whoAmI, 1f, 0f);
                    if(usPlayer.GodSlayerRangedSet && usPlayer.AuricSilvaSet) //佩戴金源射手时
                    Projectile.NewProjectile(source, position.X + offsetSpawn.X, position.Y + offsetSpawn.Y, velocity.X * 0.9f, velocity.Y * 0.9f, flameID, flameDamage, knockback, player.whoAmI, 1f, 0f);
                }
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<DragonBow>(), 0, 0f, player.whoAmI);
            }

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4f, 0f);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlanteraLegendary>().
                AddIngredient<DaemonsFlame>().
                AddIngredient<Deathwind>().
                AddIngredient<HeavenlyGaleold>().
                AddConsumeItemCallback(CIRecipesCallback.DontConsumeExoWeapons). //旧天风合成时不会被消耗掉
                AddIngredient<BurningSkyLegacy>(4).        //丛林龙掉落的全部武器
                AddIngredient<DragonSword>(4).        //丛林龙掉落的全部武器
                AddIngredient<AncientDragonsBreath>(4).     //只限定为旧版本，这里是故意的。
                AddIngredient<ChickenCannonLegacy>(4).
                AddIngredient<DragonStaff>(4).
                AddIngredient<YharimsCrystal>(4).
                AddIngredient<YharonSonStaff>(4).
                AddIngredient<DragonSpear>(4).
                AddIngredient<TheFinalDawn>(4).
                AddIngredient<YharimsGiftLegacy>(8).
                AddIngredient<EffulgentFeather>(160).
                AddIngredient<YharonSoulFragment>(160). //龙魂与化魂神晶用于合成这个物品时候不会被消耗
                AddConsumeItemCallback(CIRecipesCallback.DontConsumePostDOGMaterials).
                AddIngredient<AscendantSpiritEssence>(160). //调整为化魂神晶
                AddConsumeItemCallback(CIRecipesCallback.DontConsumePostDOGMaterials).
                AddIngredient<CalamityMod.Items.Placeables.Ores.AuricOre>(320). //调整为320个原灾的金源矿
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
