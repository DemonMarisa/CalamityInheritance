using CalamityMod.Items;
using CalamityMod.NPCs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.LoreItems;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class GrandDad : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 124;
            Item.height = 124;
            Item.damage = 777;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useTurn = true;
            Item.knockBack = 77f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (CalamityGlobalNPC.ShouldAffectNPC(target))
            {
                target.knockBackResist = 7f;
                target.defense = 0;
            }
        }
        public override void AddRecipes()
        {
            if (CalamityInheritanceConfig.Instance.LegendaryitemsRecipes == true)
            {
                Recipe recipe1 = CreateRecipe();
                recipe1.AddIngredient<LoreRequiem>();
                recipe1.AddTile(TileID.AncientMythrilBrick);
                recipe1.Register();

                Recipe recipe2 = CreateRecipe();
                recipe2.AddIngredient<KnowledgeMoonLord>();
                recipe2.AddTile(TileID.AncientMythrilBrick);
                recipe2.Register();
            }
        }
    }
}
