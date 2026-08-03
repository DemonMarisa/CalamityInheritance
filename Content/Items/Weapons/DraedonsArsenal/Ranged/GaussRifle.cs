using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Ranged;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Ranged
{
    public class GaussRifle : CIRanged
    {
        public override void SetDefaults()
        {
            Item.width = 112;
            Item.height = 36;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 150;
            Item.knockBack = 30f;
            Item.useTime = Item.useAnimation = 32;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISounds.MechGaussRifle;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;

            Item.shoot = ProjectileType<GaussRifleBlast>();
            Item.shootSpeed = 27f;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 15).
                    AddIngredient(CalamityMaterials.DubiousPlating, 15).
                    AddIngredient(CalamityMaterials.InfectedArmorPlating, 10).
                    AddIngredient(CalamityMaterials.LifeAlloy, 5).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {

            }
        }
    }
}
