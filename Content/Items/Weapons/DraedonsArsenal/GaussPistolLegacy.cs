using CalamityInheritance.Content.Projectiles.DraedonsArsenal;
using CalamityInheritance.Sounds.Cals;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Materials;
using LAP.Content.RecipeGroupAdd;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal
{
    internal class GaussPistolLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.DraedonsArsenal";
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
            Item.UseSound = CICalSounds.GaussWeaponFire;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;

            Item.shoot = ProjectileType<GaussPistolShotLegacy>();
            Item.shootSpeed = 14f;
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
            CIFunction.NoHeldProjUpdateAim(player, MathHelper.ToDegrees(offset), 1);
        }
        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(12).
                AddIngredient<DubiousPlating>(8).
                AddRecipeGroup(LAPRecipeGroup.AnyMythrilBar, 10).
                AddIngredient(ItemID.SoulofMight, 20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
