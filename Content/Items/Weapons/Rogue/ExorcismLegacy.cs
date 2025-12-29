using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class ExorcismLegacy : CIRogueClass
    {
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<Exorcism>();
        }
        public override void ExSD()
        {
            Item.width = 34;
            Item.height = 42;
            Item.damage = 64;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ProjectileType<ExorcismProjLegacy>();
            Item.shootSpeed = 10f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HolyWater, 10).
                AddIngredient(ItemID.HallowedBar, 12).
                AddIngredient(ItemID.SoulofFright, 6).
                AddIngredient(ItemID.SoulofMight, 6).
                AddIngredient(ItemID.SoulofSight, 6).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
