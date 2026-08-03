using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Melee;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Melee
{
    public class GaussDagger : CIMelee
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.knockBack = 7f;

            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.CI().GaussFluxTimer == 0)
            {
                target.CI().GaussFluxTimer = 50;
                if (player.whoAmI == Main.myPlayer)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ProjectileType<GaussFlux>(), Item.damage, 0f, player.whoAmI, 0f, target.whoAmI);
                }
            }
        }

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
