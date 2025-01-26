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
    public class SubductionSlicerMelee : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 48;
            Item.damage = 95;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.Item1;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<SubductionSlicerProjMelee>();
            Item.shootSpeed = 16f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ScoriaBar>(9).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
