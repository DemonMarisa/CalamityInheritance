using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items;
using CalamityMod.Rarities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class NightsStabber : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 13;
            Item.width = 28;
            Item.height = 34;
            Item.damage = 52;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<NightsStabberProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SporeKnife>());
            recipe.AddIngredient(ModContent.ItemType<LeechingDagger>());
            recipe.AddIngredient(ModContent.ItemType<FlameburstShortsword>());
            recipe.AddIngredient(ModContent.ItemType<AncientShiv>());
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<SporeKnife>());
            recipe2.AddIngredient(ModContent.ItemType<BloodyRupture>());
            recipe2.AddIngredient(ModContent.ItemType<FlameburstShortsword>());
            recipe2.AddIngredient(ModContent.ItemType<AncientShiv>());
            recipe2.AddTile(TileID.DemonAltar);
            recipe2.Register();
        }
    }
}
