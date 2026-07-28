using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.MagicGun.Rifle;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicGun.Rifle
{
    public class Thunderstorm : CIMagic
    {
        public override void ExSD()
        {
            Item.width = 48;
            Item.height = 22;
            Item.damage = 132;
            Item.mana = 50;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
            Item.UseSound = CISounds.PlasmaBlast;
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ProjectileType<ThunderstormShot>();
        }
        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.ArmoredShell, 2).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}
