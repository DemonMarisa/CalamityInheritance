using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Projectiles.CalProjChange;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class Minigun : CIRanged, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Ranged";
        public override void SetDefaults()
        {
            Item.damage = 275;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 92;
            Item.height = 44;
            //速射机枪使用时间3->2
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shoot = ModContent.ProjectileType<KingsbaneHoldoutReal>();
            Item.shootSpeed = 22f;
            Item.useAmmo = AmmoID.Bullet;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + Main.rand.Next(-15, 16) * 0.05f;
            float SpeedY = velocity.Y + Main.rand.Next(-15, 16) * 0.05f;
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() > 0.8f;

        public override void AddRecipes()
        {
            //取消金源锭需求
            CreateRecipe().
                AddIngredient(ItemID.ChainGun).
                AddIngredient<ClockGatlignum>().
                AddIngredient<CosmiliteBar>(12).
                AddIngredient<AscendantSpiritEssence>(5).
                AddTile<CosmicAnvil>().
                Register();


            // CreateRecipe().
            //     AddIngredient(ItemID.ChainGun).
            //     AddIngredient<ClockGatlignum>().
            //     AddIngredient<AuricBarold>().
            //     AddTile<CosmicAnvil>().
            //     Register();

            // CreateRecipe().
            //     AddIngredient(ItemID.ChainGun).
            //     AddIngredient<ClockGatlignum>().
            //     AddIngredient<AuricBar>(5).
            //     AddTile<CosmicAnvil>().
            //     Register();
        }
    }
}
