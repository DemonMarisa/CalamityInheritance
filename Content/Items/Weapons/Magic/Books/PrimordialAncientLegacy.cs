using CalamityInheritance.Content.Projectiles.Magic.Books;
using CalamityInheritance.Rarity;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Projectiles.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Books
{
    public class PrimordialAncientLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Magic;
        public override void ExSD()
        {
            Item.width = 40;
            Item.height = 56;
            Item.damage = 145;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 16;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<PrimordialAncientProj>();
            Item.shootSpeed = 8f;
            Item.rare = RarityType<DeepBlue>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PrimordialEarthLegacy>().
                AddIngredient(ItemID.AncientBattleArmorMaterial, 5).
                AddIngredient<CosmiliteBar>(8).
                AddIngredient<EndothermicEnergy>(20).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
