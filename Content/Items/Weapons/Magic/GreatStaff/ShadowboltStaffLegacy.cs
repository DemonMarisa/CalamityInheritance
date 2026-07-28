using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.GreatStaff;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.GreatStaff
{
    public class ShadowboltStaffLegacy : CIMagic
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 56;
            Item.damage = 280;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<ShadowBeam>();
            Item.shootSpeed = 6f;
            Item.rare = RarityType<AbsoluteGreen>();
        }
        public override void AddRecipes()
        {
            //尝试使用带防御性的编码
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.ShadowbeamStaff).
                    AddIngredient(CalamityMaterials.RuinousSoul, 2).
                    AddTile(TileID.LunarCraftingStation).
                    Register();

            }
            else
            {

            }
        }
    }
}