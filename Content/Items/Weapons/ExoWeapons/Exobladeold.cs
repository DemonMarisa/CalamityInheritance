using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Melee.GreatSwords;
using CalamityInheritance.Content.Items.Weapons.Melee.UltraGreatSword;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.ExoWeapons;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class Exobladeold : CIExoWeapon
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.damage = 900;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 9f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 114;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = RarityType<CatalystViolet>();
            Item.shoot = ProjectileType<Exobeamold>();
            Item.shootSpeed = 19f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (UseAlt)
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<ExobeamoldExoLore>(), damage, knockback, player.whoAmI, 0f);
            else
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<Exobeamold>(), damage, knockback, player.whoAmI, 0f);
            return false;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float lifeAmount = player.statLife / player.statLifeMax2;
            lifeAmount = 1 - lifeAmount;
            damage *= 1 + lifeAmount * 0.1f;
            if (UseAlt)
                damage *= 0.5f;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            CIUtils.BetterSwing(player);
            if (Main.rand.NextBool(4))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.TerraBlade, 0f, 0f, 100, new Color(0, 255, 255));
        }

        private int hitCount = 0;
        private int hitCount2 = 0;
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item88, player.Center);
            float xPos = player.position.X + 800 * Main.rand.NextBool(2).ToDirectionInt();
            float yPos = player.position.Y + Main.rand.Next(-800, 801);
            Vector2 startPos = new Vector2(xPos, yPos);
            Vector2 velocity = target.position - startPos;
            float dir = 10 / startPos.X;
            velocity.X *= dir * 150;
            velocity.Y *= dir * 150;
            velocity.X = MathHelper.Clamp(velocity.X, -15f, 15f);
            velocity.Y = MathHelper.Clamp(velocity.Y, -15f, 15f);

            if (UseAlt)
            {
                hitCount++;
                hitCount2++;

                if (hitCount >= 5 || target.life <= target.lifeMax * 0.15f)
                {
                    Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, Vector2.Zero, ProjectileType<Exoboomold>(), damageDone / 4, (int)Item.knockBack, Main.myPlayer);
                    hitCount = 0;
                }
                if (hitCount2 >= 2 || target.life <= target.lifeMax * 0.15f)
                {
                    for (int comet = 0; comet < 2; comet++)
                    {
                        float ai1 = Main.rand.NextFloat() + 0.5f;
                        Projectile.NewProjectile(player.GetSource_OnHit(target), startPos, velocity, ProjectileType<CIExocomet>(), damageDone, (int)Item.knockBack, player.whoAmI, 0f, ai1);
                    }
                    hitCount2 = 0;
                }
            }
            else
            {
                if (player.ownedProjectileCounts[ProjectileType<CIExocomet>()] < 8)
                {
                    for (int comet = 0; comet < 2; comet++)
                    {
                        float ai1 = Main.rand.NextFloat() + 0.5f;
                        Projectile.NewProjectile(player.GetSource_OnHit(target), startPos, velocity, ProjectileType<CIExocomet>(), damageDone, (int)Item.knockBack, player.whoAmI, 0f, ai1);
                    }
                }

                if (target.life <= target.lifeMax * 0.05f)
                {
                    Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, Vector2.Zero, ProjectileType<Exoboomold>(), damageDone / 4, (int)Item.knockBack, Main.myPlayer);
                    hitCount = 0;
                }
            }
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
            int healAmount = Main.rand.Next(4) + 5;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TerratomereOld>().
                AddIngredient<EntropicClaymoreLegacy>().
                AddIngredient<PhoenixBlade>().
                AddIngredient<StellarStrikerLegacy>().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeoldTile>().
                Register();

            CreateRecipe().
                AddIngredient<TerratomereOld>().
                AddIngredient<EntropicClaymoreLegacy>().
                AddIngredient<PhoenixBlade>().
                AddIngredient<StellarStrikerLegacy>().
                AddIngredient<AncientMiracleMatter>().
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
