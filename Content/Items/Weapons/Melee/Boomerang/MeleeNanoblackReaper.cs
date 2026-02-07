using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using LAP.Core.Enums;
using LAP.Core.SystemsLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Boomerang
{
    public class MeleeNanoblackReaper : CIMelee, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<NanoblackReaper>(false);
        }
        public override void SetDefaults()
        {
            Item.width = 78;
            Item.height = 64;
            Item.damage = 180;
            Item.knockBack = 9f;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item18;

            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = RarityType<DonatorPink>();
            Item.Calamity().devItem = true;

            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.ArmorPenetration = 500;
            Item.shoot = ProjectileType<MeleeNanoblackReaperProj>();
            Item.shootSpeed = 12;

            Item.SetCalStatInflation(AllWeaponTier.DemonShadow);
        }
        public override bool MeleePrefix() => true;
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

            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                Register();

        }
    }
}
