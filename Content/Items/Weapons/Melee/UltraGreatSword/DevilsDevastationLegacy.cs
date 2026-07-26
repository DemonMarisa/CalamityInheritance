using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Weapons.Melee.GreatSwords;
using CalamityInheritance.Content.Items.Weapons.Melee.LightGreadtSword;
using CalamityInheritance.Content.Projectiles.Melee.UltraGreatSword;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Utils;
using LAP.Common.CalamityModCross;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Melee.UltraGreatSword
{
    public class DevilsDevastationLegacy : CIMelee
    {
        public override void SetDefaults()
        {
            Item.width = 118;
            Item.height = 118;
            Item.damage = 166;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useTurn = false;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.shoot = ProjectileType<Oathblade>();
            Item.shootSpeed = 28f;
            Item.rare = RarityType<DeepBlue>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source2, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Shoot 3 oathblades in a spread
            int index = 8;
            for (int j = -index; j <= index; j += index)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(j));
                Projectile.NewProjectile(source2, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }

            //Not actually sure what this middle code does
            float speed = Item.shootSpeed;
            Vector2 source = player.RotatedRelativePoint(player.MountedCenter, true);
            float directionX = Main.mouseX + Main.screenPosition.X - source.X;
            float directionY = Main.mouseY + Main.screenPosition.Y - source.Y;
            if (player.gravDir == -1f)
            {
                directionY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - source.Y;
            }
            Vector2 direction = new Vector2(directionX, directionY);
            float aimDist = direction.Length();
            if (float.IsNaN(direction.X) && float.IsNaN(direction.Y) || direction.X == 0f && direction.Y == 0f)
            {
                direction.X = player.direction;
                direction.Y = 0f;
                aimDist = speed;
            }
            else
            {
                aimDist = speed / aimDist;
            }

            //Shoot 9 tridents from below
            int projAmt = 3; //Since the method spawns 3 projectiles, it spawns 9 total
            for (int projIndex = 0; projIndex < projAmt; projIndex++)
            {
                source = new Vector2(player.Center.X + (float)(Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y + 600f);
                source.X = (source.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                source.Y += 100 * projIndex;
                direction.X = Main.mouseX + Main.screenPosition.X - source.X;
                direction.Y = Main.mouseY + Main.screenPosition.Y - source.Y;
                if (direction.Y < 0f)
                {
                    direction.Y *= -1f;
                }
                if (direction.Y < 20f)
                {
                    direction.Y = 20f;
                }
                aimDist = direction.Length();
                aimDist = speed / aimDist;
                direction.X *= aimDist;
                direction.Y *= aimDist;
                direction.X += Main.rand.NextFloat(-40f, 40f) * 0.02f;
                direction.Y += Main.rand.NextFloat(-40f, 40f) * 0.02f;
                direction.Y *= -1;
                Projectile.NewProjectile(source2, source, direction, ProjectileType<DemonFork>(), damage, knockback, player.whoAmI, 0f, Main.rand.Next(5));
                Projectile.NewProjectile(source2, source, direction, ProjectileType<DemonFork>(), damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));
                Projectile.NewProjectile(source2, source, direction, ProjectileType<DemonFork>(), damage, knockback, player.whoAmI, 0f, 1f);
            }
            return false;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.ShadowbeamStaff);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 150);
            target.AddBuff(BuffID.OnFire, 300);
            if (hit.Crit)
            {
                target.AddBuff(BuffID.ShadowFlame, 450);
                target.AddBuff(BuffID.OnFire, 900);
                player.ApplyDamageToNPC(target, Item.damage * 4, 0f, 0, false);
                float firstDustScale = 1.7f;
                float secondDustScale = 0.8f;
                float thirdDustScale = 2f;
                Vector2 dustRotation = (target.rotation - MathHelper.PiOver2).ToRotationVector2();
                Vector2 dustVelocity = dustRotation * target.velocity.Length();
                SoundEngine.PlaySound(SoundID.Item14, target.Center);
                for (int i = 0; i < 40; i++)
                {
                    int dustInt = Dust.NewDust(target.position, target.width, target.height, DustID.ShadowbeamStaff, 0f, 0f, 200, default, firstDustScale);
                    Dust dust = Main.dust[dustInt];
                    dust.position = target.Center + Vector2.UnitY.RotatedByRandom(Math.PI) * Main.rand.NextFloat() * target.width / 2f;
                    dust.noGravity = true;
                    dust.velocity.Y -= 4.5f;
                    dust.velocity *= 3f;
                    dust.velocity += dustVelocity * Main.rand.NextFloat();
                    dustInt = Dust.NewDust(target.position, target.width, target.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, secondDustScale);
                    dust.position = target.Center + Vector2.UnitY.RotatedByRandom(Math.PI) * Main.rand.NextFloat() * target.width / 2f;
                    dust.velocity.Y -= 3f;
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    dust.fadeIn = 1f;
                    dust.color = Color.Crimson * 0.5f;
                    dust.velocity += dustVelocity * Main.rand.NextFloat();
                }
                for (int j = 0; j < 20; j++)
                {
                    int dustInt = Dust.NewDust(target.position, target.width, target.height, DustID.ShadowbeamStaff, 0f, 0f, 0, default, thirdDustScale);
                    Dust dust = Main.dust[dustInt];
                    dust.position = target.Center + Vector2.UnitX.RotatedByRandom(Math.PI).RotatedBy((double)target.velocity.ToRotation(), default) * target.width / 3f;
                    dust.noGravity = true;
                    dust.velocity.Y -= 1.5f;
                    dust.velocity *= 0.5f;
                    dust.velocity += dustVelocity * (0.6f + 0.6f * Main.rand.NextFloat());
                }
            }
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<Devastation>().
                    AddIngredient<ExaltedOathbladeLegacy>().
                    AddIngredient(CalMaterialsID.CosmiliteBarID, 8).
                    AddIngredient(CalamityMaterials.NightmareFuel, 20).
                    AddTile(CalTileID.CosmicAnvilID).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient<Devastation>().
                    AddIngredient<ExaltedOathbladeLegacy>().
                    AddTile<DraedonsForgeoldTile>().
                    Register();
            }
        }
    }
}
