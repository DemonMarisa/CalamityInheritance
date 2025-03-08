using CalamityInheritance.Content.Items;
using CalamityMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class HellBurstLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 14;
            Item.width = 52;
            Item.height = 52;
            Item.useTime = 31;
            Item.useAnimation = 31;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item34;
            Item.autoReuse = true;
            // item.shoot = ModContent.ProjectileType<FlameBeamTip>();
            Item.shootSpeed = 32f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Flamelash);
            recipe.AddIngredient(ItemID.CrystalVileShard);
            recipe.AddIngredient(ItemID.DarkShard, 2);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
