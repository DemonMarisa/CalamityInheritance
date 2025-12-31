using CalamityInheritance.Content.Projectiles.Rogue.Spears;
using CalamityMod;
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

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Spears
{
    public class ShardofAntumbraLegacy : CIRogueClass
    {
        public override void ExSD()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 240;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ProjectileType<ShardofAntumbraLegacyProj>();
            Item.shootSpeed = 24f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MeldConstruct>(15).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
