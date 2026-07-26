using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.Flails;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Flails
{
    public class TumbleweedLegacy : CIMelee
    {
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 36;
            Item.damage = 125;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 8f;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.channel = true;
            Item.shoot = ProjectileType<TumbleweedProj>();
            Item.shootSpeed = 12f;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.Sunfury).
                    AddIngredient(CalamityMaterials.GrandScale, 5).
                    AddIngredient(ItemID.SoulofMight, 5).
                    AddTile(TileID.MythrilAnvil).
                    Register();

            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.Sunfury).
                    AddIngredient(ItemID.SoulofMight, 5).
                    AddTile(TileID.MythrilAnvil).
                    Register();

            }
        }
    }
}
