using CalamityInheritance.Content.Projectiles.Magic.Books;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Books
{
    public class BurningSeaLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Magic;
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 75;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6.5f;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<BurningSeaProj>();
            Item.shootSpeed = 15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpellTome).
                AddIngredient<UnholyCore>(5).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
