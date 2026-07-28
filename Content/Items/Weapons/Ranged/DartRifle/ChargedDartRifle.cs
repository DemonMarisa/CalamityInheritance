using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Ranged.DartRifle;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Content.RecipeGroupAdd;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.DartRifle
{
    public class ChargedDartRifle : CIRanged
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 325;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 74;
            Item.height = 40;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
            Item.shootSpeed = 22f;
            Item.shoot = ProjectileType<ChargedBlastLegacy>();
            Item.useAmmo = AmmoID.Dart;

        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool AltFunctionUse(Player player) => true;

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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(CISounds.LaserCannon);
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<ChargedBlastLegacy3>(), (int)((double)damage * 2), knockback, player.whoAmI, 0f, 0f);
                return false;
            }
            else
            {
                int num6 = Main.rand.Next(2, 5);
                for (int index = 0; index < num6; ++index)
                {
                    float SpeedX = velocity.X + Main.rand.Next(-40, 41) * 0.05f;
                    float SpeedY = velocity.Y + Main.rand.Next(-40, 41) * 0.05f;
                    int projectile = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage / 2, knockback, player.whoAmI, 0f, 0f);
                }
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<ChargedBlastLegacy>(), damage, knockback, player.whoAmI, 0f, 0f);
                return false;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddRecipeGroup(LAPRecipeGroup.AnyDartGun)
                .AddIngredient(ItemID.MartianConduitPlating, 25)
                .AddIngredient<CoreofEleum>(3)
                .AddIngredient(ItemID.FragmentVortex, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
