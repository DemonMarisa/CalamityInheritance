using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Placeables;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class TrueBiomeBlade : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.damage = 160;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 21;
            Item.useTime = 21;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7.5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 54;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<TrueBiomeOrb>();
            Item.shootSpeed = 12f;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Dirt);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BiomeBlade>().
                AddIngredient<LivingShard>(5).
                AddIngredient(ItemID.Ectoplasm, 5).
                AddIngredient<DepthCells>(10).
                AddIngredient<Lumenyl>(10).
                AddIngredient<Voidstone>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
