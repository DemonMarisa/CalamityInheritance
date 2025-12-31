using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class StarofDestructionLegacy : CIRogueClass
    {
        public override void ExSD()
        {
            Item.width = Item.height = 94;
            Item.damage = 150;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 10f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ProjectileType<StarofDestructionLegacyProj>();
            Item.shootSpeed = 5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MeldConstruct>(10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
