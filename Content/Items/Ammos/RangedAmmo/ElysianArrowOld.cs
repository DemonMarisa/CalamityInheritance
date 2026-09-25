using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Ammo.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Ammos.RangedAmmo
{
    public class ElysianArrowOld : CIAmmo
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 36;
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 24);
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ProjectileType<ElysianArrowProjOld>();
            Item.shootSpeed = 10f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {

            CreateRecipe(150).
                AddIngredient(ItemID.HolyArrow, 150).
                AddIngredient<UnholyEssenceLegacy>().
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
