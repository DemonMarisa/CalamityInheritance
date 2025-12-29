using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class Exobladeold : CIMelee, ILocalizedModType
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
            CalamityInheritancePlayer usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<ExobeamoldExoLore>(), damage, knockback, player.whoAmI, 0f);
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<Exobeamold>(), damage, knockback, player.whoAmI, 0f);
            }
            return false;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float lifeAmount = player.statLife / player.statLifeMax2;
            lifeAmount = 1 - lifeAmount;
            damage *= 1 + (lifeAmount  * 0.1f);
            CalamityInheritancePlayer usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                damage *= 0.5f;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            CIFunction.BetterSwing(player);
            if (Main.rand.NextBool(4))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.TerraBlade, 0f, 0f, 100, new Color(0, 255, 255));
        }

        private int hitCount = 0;
        private int hitCount2 = 0;
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();

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

            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
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

            target.ExoDebuffs();

            if (!target.canGhostHeal || player.moonLeech)
                return;

            int healAmount = Main.rand.Next(4) + 5;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, Request<Texture2D>($"{Generic.WeaponPath}/Melee/ExobladeoldGlow").Value);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Melee.Exobladeold.ExoLoreOn");
                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TerratomereOld>().
                AddIngredient<AnarchyBlade>().
                AddIngredient<FlarefrostBlade>().
                AddIngredient<EntropicClaymore>().
                AddIngredient<PhoenixBlade>().
                AddIngredient<StellarStriker>().
                AddIngredient<AuricBarold>(10).
                DisableDecraft().
                AddTile<DraedonsForgeold>().
                Register();
                

            CreateRecipe().
                AddIngredient<TerratomereOld>().
                AddIngredient<AnarchyBlade>().
                AddIngredient<FlarefrostBlade>().
                AddIngredient<EntropicClaymore>().
                AddIngredient<PhoenixBlade>().
                AddIngredient<StellarStriker>().
                AddIngredient<MiracleMatter>().
                AddDecraftCondition(CalamityConditions.DownedExoMechs).
                AddTile<DraedonsForge>().
                Register();
            
            CreateRecipe().
                AddIngredient<TerratomereOld>().
                AddIngredient<AnarchyBlade>().
                AddIngredient<FlarefrostBlade>().
                AddIngredient<EntropicClaymore>().
                AddIngredient<PhoenixBlade>().
                AddIngredient<StellarStriker>().
                DisableDecraft().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                DisableDecraft().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
