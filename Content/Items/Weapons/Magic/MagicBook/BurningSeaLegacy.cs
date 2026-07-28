using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.MagicBook;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicBook
{
    public class BurningSeaLegacy : CIMagic
    {
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
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.SpellTome).
                    AddIngredient(CalamityMaterials.UnholyCore, 5).
                    AddTile(TileID.Bookcases).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.SpellTome).
                    AddIngredient(ItemID.SoulofFright, 5).
                    AddTile(TileID.Bookcases).
                    Register();
            }
        }
    }
}
