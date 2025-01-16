using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Ranged;

namespace CalamityInheritance.Content.Items.Ammo
{
    public class ElysianArrowOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Ammo";
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
            Item.shoot = ModContent.ProjectileType<ElysianArrowProjOld>();
            Item.shootSpeed = 10f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            CreateRecipe(150).
                AddIngredient(ItemID.HolyArrow, 150).
                AddIngredient<UnholyEssence>().
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
