using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class MeleeTypeHammerPwnageLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory =>"Content.Items.Weapons.Melee";
        public static readonly float Speed = 12f;
        public override void SetStaticDefaults()
        {
            if(CalamityInheritanceConfig.Instance.CustomShimmer == true)
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Pwnagehammer>()] = ModContent.ItemType<MeleeTypeHammerPwnageLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()] = ModContent.ItemType<Pwnagehammer>();
            }
        }

        public override void SetDefaults()
        {
            Item.width = 68;
            Item.height = 68;
            Item.damage = 90;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.knockBack = 10f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>();
            Item.shootSpeed = 12f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Pwnhammer).
                AddIngredient(ItemID.HallowedBar, 7).
                AddIngredient(ItemID.SoulofFright, 3).
                AddIngredient(ItemID.SoulofMight, 3).
                AddIngredient(ItemID.SoulofSight, 3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}