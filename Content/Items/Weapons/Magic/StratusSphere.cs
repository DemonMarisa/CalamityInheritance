using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Rarities;
using CalamityMod.Items.Placeables.Ores;
using CalamityInheritance.Content.Projectiles.Magic;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class StratusSphere : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 251;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.width = 22;
            Item.height = 24;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.shoot = ModContent.ProjectileType<StratusSphereProj>();
            Item.shootSpeed = 7f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.mana = 30;
            Item.knockBack = 2;
            Item.UseSound = SoundID.Item20;
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.holdStyle = 3;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.rare = ModContent.RarityType<PureGreen>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Lumenyl>(), 5);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 4);
            recipe.AddIngredient(ModContent.ItemType<ExodiumCluster> (), 12);
            recipe.AddIngredient(ItemID.NebulaArcanum);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
