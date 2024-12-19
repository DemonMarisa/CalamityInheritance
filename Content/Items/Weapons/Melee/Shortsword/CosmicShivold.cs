using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items;
using CalamityMod.Projectiles.Melee.Shortswords;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class CosmicShivold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.width = 44;
            Item.height = 44;
            Item.damage = 218;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<CosmicShivProjold>();
            Item.shootSpeed = 2.4f;

            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.Calamity().donorItem = true; //Yatagarasu#0001
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalShiv>().
                AddIngredient<CosmiliteBar>(8).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

        }
    }
}
