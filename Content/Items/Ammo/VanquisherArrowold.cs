using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Ranged;

namespace CalamityInheritance.Content.Items.Ammo
{
    public class VanquisherArrowold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Ammo";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 46;
            Item.damage = 33;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3.5f;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.shoot = ModContent.ProjectileType<VanquisherArrowoldMain>();
            Item.shootSpeed = 10f;
            Item.ammo = AmmoID.Arrow;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Ammo/VanquisherArrowoldGlow").Value);
        }

        public override void AddRecipes()
        {
            CreateRecipe(999).
                AddIngredient<CosmiliteBar>().
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
