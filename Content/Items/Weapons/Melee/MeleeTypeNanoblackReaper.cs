using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    [LegacyName("NanoblackReaperLegacyMelee")]
    public class MeleeTypeNanoblackReaper : ModItem, ILocalizedModType
    {
        
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public static float Knockback = 9f;
        public int NewDamage = CIServerConfig.Instance.ShadowspecBuff? 700 : 455;
        public static float Speed = 12f;

        public override void SetDefaults()
        {
            Item.width = 78;
            Item.height = 64;
            Item.damage = 455;
            Item.knockBack = Knockback;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item18;

            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.Calamity().devItem = true;

            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.ArmorPenetration = 500;
            Item.shoot = ModContent.ProjectileType<MeleeTypeNanoblackReaperProj>();
            Item.shootSpeed = Speed;
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
