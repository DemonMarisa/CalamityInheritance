using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    // Deny me no longer!
    public class NanoblackReaperLegacyMelee : ModItem, ILocalizedModType
    {
        
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public static float Knockback = 9f;
        public static float Speed = 13f;

        public override void SetDefaults()
        {
            Item.width = 78;
            Item.height = 64;
            Item.damage = 450;
            Item.knockBack = Knockback;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item18;

            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.Calamity().devItem = true;

            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.shoot = ModContent.ProjectileType<NanoblackMainLegacyMelee>();
            Item.shootSpeed = Speed;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GhoulishGouger>().
                AddIngredient<SoulHarvester>().
                AddIngredient<EssenceFlayer>().
                AddIngredient<ShadowspecBar>(5).
                AddIngredient<EndothermicEnergy>(40).
                AddIngredient<DarkPlasma>(10).
                AddIngredient(ItemID.Nanites, 400).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
