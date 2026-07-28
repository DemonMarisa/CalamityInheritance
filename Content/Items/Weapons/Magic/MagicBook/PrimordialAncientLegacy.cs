using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.MagicBook;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicBook
{
    public class PrimordialAncientLegacy : CIMagic
    {
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
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<PrimordialEarthLegacy>().
                    AddIngredient(ItemID.AncientBattleArmorMaterial, 5).
                    AddIngredient(CalamityMaterials.CosmiliteBar, 8).
                    AddIngredient(CalamityMaterials.EndothermicEnergy, 20).
                    AddTile(TileID.Bookcases).
                    Register();
            }
            else
            {

            }
        }
    }
}
