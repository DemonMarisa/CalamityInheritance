using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Ranged.HandGun;
using CalamityInheritance.Content.Items.Weapons.Ranged.Misc;
using CalamityInheritance.Content.Projectiles.ExoWeapons;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Rarity.Special;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class CelestialObliterator : CIExoWeapon
    {
        private int shot;

        private int burst;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 400;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 120;
            Item.height = 38;
            Item.useTime = 4;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.rare = RarityType<SeraphPurple>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.9f;
        }
        public override void UseItemFrame(Player player)
        {
            CIUtils.NoHeldProjUpdateAim(player, 0, 1);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + Main.rand.Next(-15, 16) * 0.05f;
            float SpeedY = velocity.Y + Main.rand.Next(-15, 16) * 0.05f;
            int pCounts = 10;
            Projectile.NewProjectile(source, position.X, position.Y - 6, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
            if (shot >= 8)
            {
                shot = 0;
                burst++;
                for (int i = 0; i < pCounts; i++)
                {
                    Vector2 vector = new Vector2(SpeedX, SpeedY).RotatedByRandom((double)MathHelper.ToRadians(2f));
                    vector *= 0.4f + Main.rand.NextFloat(10f) / 25f;
                    Projectile.NewProjectile(source, position.X, position.Y - 6, vector.X, vector.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
                }
            }
            else
            {
                shot++;
                SoundEngine.PlaySound(SoundID.Item40);
            }
            if (burst >= 5)
            {
                if (UseAlt)
                {
                    burst = 0;
                    SoundEngine.PlaySound(SoundID.Item38);
                    Vector2 vector2 = Vector2.Normalize(new Vector2(SpeedX, SpeedY)).RotatedBy(60, default) * 9f;
                    Vector2 vector3 = Vector2.Normalize(new Vector2(SpeedX, SpeedY)).RotatedBy(-60, default) * 9f;
                    Projectile.NewProjectile(source, position.X, position.Y - 6, vector2.X, vector2.Y, ProjectileType<ExoGunBlast>(), damage, knockback, player.whoAmI, 1f, 0f);
                    Projectile.NewProjectile(source, position.X, position.Y - 6, vector3.X, vector3.Y, ProjectileType<ExoGunBlast>(), damage, knockback, player.whoAmI, 1f, 0f);
                }
                else
                {
                    burst = 0;
                    SoundEngine.PlaySound(SoundID.Item38);
                    Vector2 vector4 = Vector2.Normalize(new Vector2(SpeedX, SpeedY)).RotatedBy(45, default) * 9f;
                    Vector2 vector5 = Vector2.Normalize(new Vector2(SpeedX, SpeedY)).RotatedBy(-45, default) * 9f;
                    Projectile.NewProjectile(source, position.X, position.Y - 6, vector4.X, vector4.Y, ProjectileType<ExoGunBlastsplit>(), damage, knockback, player.whoAmI, 1f, 0f);
                    Projectile.NewProjectile(source, position.X, position.Y - 6, vector5.X, vector5.Y, ProjectileType<ExoGunBlastsplit>(), damage, knockback, player.whoAmI, 1f, 0f);
                }
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-35f, -6f);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalBlaster>().
                AddIngredient<PearlGodLegacy>().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeoldTile>().
                Register();

            CreateRecipe().
                AddIngredient<ElementalBlaster>().
                AddIngredient<PearlGodLegacy>().
                AddIngredient<AncientMiracleMatter>().
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
