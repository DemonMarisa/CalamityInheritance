using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class MangroveChakramLegacyMelee : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.damage = 84;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<MangroveChakramProjectileLegacyMelee>();
            Item.shootSpeed = 16f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PerennialBar>(7).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
