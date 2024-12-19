using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class TerraShiv : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 13;
            Item.width = 42;
            Item.height = 42;
            Item.damage = 240;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<TerraShivProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TrueNightsStabber>());
            recipe.AddIngredient(ModContent.ItemType<TrueExcaliburShortsword>());
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(),5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
