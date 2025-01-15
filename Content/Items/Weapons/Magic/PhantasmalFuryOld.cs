using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Projectiles.Magic;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class PhantasmalFuryOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 60;
            Item.damage = 260;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PhantasmalFuryProjOld>();
            Item.shootSpeed = 12f;
            Item.rare = ModContent.RarityType<AbsoluteGreen>();
        }


        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpectreStaff).
                AddIngredient<RuinousSoul>(2).
                AddIngredient<DarkPlasma>().
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
