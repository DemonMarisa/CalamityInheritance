using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Ranged;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Ranged
{
    public class Taser : CIRanged
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 26;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 20;
            Item.knockBack = 0f;
            Item.useTime = Item.useAnimation = 28;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISounds.PlasmaBolt;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileType<TaserProj>();
            Item.shootSpeed = 25f;
        }

        public override bool CanUseItem(Player player) => !player.HasProj(Item.shoot);

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 7).
                    AddIngredient(CalamityMaterials.DubiousPlating, 7).
                    AddIngredient(CalamityMaterials.AerialiteBar, 4).
                    AddTile(TileID.Anvils).
                    Register();
            }
        }
    }
}
