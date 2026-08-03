using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Magic;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Magic
{
    public class PulsePistolLegacy : CIMagic
    {
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 22;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 27;
            Item.knockBack = 0f;
            Item.useTime = Item.useAnimation = 21;
            Item.autoReuse = true;
            Item.mana = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISounds.PulseRifleFire;
            Item.noMelee = true;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileType<PulsePistolProj>();
            Item.shootSpeed = 5.2f; // This may seem low but the shot has 10 extra updates.
        }
        public override void UseItemFrame(Player player)
        {
            player.ChangeDir(Math.Sign((player.LocalMouseWorld() - player.Center).X));

            float animProgress = 0.5f - player.itemTime / (float)player.itemTimeMax;
            // 向鼠标的旋转
            float rotation = (player.Center - player.LocalMouseWorld()).ToRotation() * player.gravDir + MathHelper.PiOver2;
            float offset = -0.03f * (float)Math.Pow((0.6f - animProgress) / 0.6f, 2);
            if (animProgress < 0.4f)
                rotation += offset * player.direction;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
            CIUtils.NoHeldProjUpdateAim(player, MathHelper.ToDegrees(offset), 1);
        }
        public override Vector2? HoldoutOffset() => new Vector2(-2,- 2f);
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 5).
                    AddIngredient(CalamityMaterials.DubiousPlating, 7).
                    AddIngredient(CalamityMaterials.AerialiteBar, 4).
                    AddTile(TileID.Anvils).
                    Register();
            }
            else
            {

            }
        }
    }
}
