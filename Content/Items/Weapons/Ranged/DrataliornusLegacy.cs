using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Tiles.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class DrataliornusLegacy: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        private const double RightClickDamageRatio = 0.65;   //右键龙弓，倍率0.65，采用海啸的攻击模板
        private const int WeaponDamage = 174;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 84;
            Item.damage = WeaponDamage;
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

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                //右键采用海啸的方式
                Item.noUseGraphic = false;
                Item.reuseDelay = 48;
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
                int flameID = ModContent.ProjectileType<DragonBowFlame>();
                const int numFlames = 7;
                int flameDamage = (int)(damage * RightClickDamageRatio);

                const float fifteenHundredthPi = 0.471238898f;
                Vector2 spinningpoint = velocity;
                spinningpoint.Normalize();
                spinningpoint *= 36f;
                for (int i = 0; i < numFlames; ++i)
                {
                    float piArrowOffset = i - (numFlames - 1) / 2;
                    Vector2 offsetSpawn = spinningpoint.RotatedBy(fifteenHundredthPi * piArrowOffset, new Vector2());
                    Projectile.NewProjectile(source, position.X + offsetSpawn.X, position.Y + offsetSpawn.Y, velocity.X * 0.7f, velocity.Y * 0.7f, flameID, flameDamage, knockback, player.whoAmI, 1f, 0f);
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
                AddIngredient<BlossomFlux>().
                AddIngredient<DaemonsFlame>().
                AddIngredient<Deathwind>().
                AddIngredient<HeavenlyGaleold>().
                AddConsumeItemCallback(CIRecipesCallback.DontConsumeExoWeapons). //旧天风合成时不会被消耗掉
                AddIngredient<TheBurningSky>(4).        //丛林龙掉落的全部武器
                AddIngredient<DragonsBreathold>(4).     //只限定为旧版本，这里是故意的。
                AddIngredient<ChickenCannon>(4).
                AddIngredient<PhoenixFlameBarrage>(4).
                AddIngredient<YharimsCrystal>(4).
                AddIngredient<YharonsKindleStaff>(4).
                AddIngredient<Wrathwing>(4).
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
