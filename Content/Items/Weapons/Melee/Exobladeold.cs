using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class Exobladeold : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.damage = 900;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 114;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.shoot = ModContent.ProjectileType<Exobeamold>();
            Item.shootSpeed = 19f;
            Item.rare = ModContent.RarityType<Violet>();
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            int lifeAmount = player.statLifeMax2 - player.statLife;
            damage.Flat += lifeAmount;
            base.ModifyWeaponDamage(player, ref damage);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.TerraBlade, 0f, 0f, 100, new Color(0, 255, 255));
        }

        private int hitCount = 0;
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitCount++;

            if (hitCount >= 5 || target.life <= target.lifeMax * 0.1f)
            {
                Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<Exoboompersistentold>(), damageDone, (int)Item.knockBack, Main.myPlayer);
                hitCount = 0;
            }

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
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Exocomet>()] < 16)
            {
                for (int comet = 0; comet < 2; comet++)
                {
                    float ai1 = Main.rand.NextFloat() + 0.5f;
                    Projectile.NewProjectile(player.GetSource_OnHit(target), startPos, velocity, ModContent.ProjectileType<Exocomet>(), damageDone, (int)Item.knockBack, player.whoAmI, 0f, ai1);
                }
            }

            if (!target.canGhostHeal || player.moonLeech)
                return;

            int healAmount = Main.rand.Next(3) + 5;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Terratomere>().
                AddIngredient<AnarchyBlade>().
                AddIngredient<FlarefrostBlade>().
                AddIngredient<EntropicClaymore>().
                AddIngredient<StellarStriker>().
                AddIngredient<MiracleMatter>().
                AddTile<DraedonsForge>().
                Register();
            
            CreateRecipe().
                AddIngredient<Exoblade>().
                Register();
        }
    }
}
