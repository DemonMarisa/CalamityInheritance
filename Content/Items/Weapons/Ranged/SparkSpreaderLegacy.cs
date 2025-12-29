using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class SparkSpreaderLegacy : FlamethrowerSpecial, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<SparkSpreader>();
        }
        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.knockBack = 1f;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.useTime = 10;
            Item.useAnimation = 30;
            Item.useAmmo = AmmoID.Gel;
            Item.shootSpeed = 5f;
            Item.shoot = ProjectileType<SparkSpreaderFireLegacy>();

            Item.width = 52;
            Item.height = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item34;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-4, 0);

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.Next(100) >= 70;

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnyGoldBar", 10).
                AddIngredient(ItemID.Ruby).
                AddIngredient(ItemID.Gel, 12).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
