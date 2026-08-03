using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Magic;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Content.RecipeGroupAdd;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Magic
{
    public class GaussPistolLegacy : CIMagic
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 22;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.damage = 150;
            Item.knockBack = 11f;
            Item.useTime = Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISounds.GaussWeaponFire;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;

            Item.shoot = ProjectileType<GaussPistolShotLegacy>();
            Item.shootSpeed = 14f;
        }
        public override void UseItemFrame(Player player)
        {
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, LAPUtilities.GetVector2(player.Center, player.LocalMouseWorld()).ToRotation());
            CIUtils.NoHeldProjUpdateAim(player, 0, 1);
        }
        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 12).
                    AddIngredient(CalamityMaterials.DubiousPlating, 8).
                    AddRecipeGroup(LAPRecipeGroup.AnyMythrilBar, 10).
                    AddIngredient(ItemID.SoulofMight, 20).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddRecipeGroup(LAPRecipeGroup.AnyMythrilBar, 10).
                    AddIngredient(ItemID.SoulofSight, 20).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
