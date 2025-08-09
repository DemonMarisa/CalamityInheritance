using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Core;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.DownedBoss;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Ammo.RangedAmmo
{
    public class LightAmmo : CIAmmo, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Ammo";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 16;
            Item.damage = 99;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 1.25f;
            Item.value = CIShopValue.RarityPriceWhite;
            Item.shootSpeed = 1f;
            Item.rare = ModContent.RarityType<TrueScarlet>();
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<LightAmmoProj>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(999).
                AddIngredient(ItemID.HighVelocityBullet).
                AddIngredient<ExoPrism>(5).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<DraedonsForge>().
                Register();

            CreateRecipe(9999).
                AddIngredient(ItemID.HighVelocityBullet).
                AddIngredient<CalamitousEssence>(1).
                AddTile<DraedonsForge>().
                Register();

        }
    }
}