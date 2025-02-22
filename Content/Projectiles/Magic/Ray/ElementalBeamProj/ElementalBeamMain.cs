using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Typeless;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj
{
    public class ElementalBeamMain : BaseLaserbeamProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override float MaxScale => 1f;
        public override float MaxLaserLength => 1500f;
        public override float Lifetime => 30f;
        public override Color LightCastColor => Color.White;
        public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayStart", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayMid", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayEnd", AssetRequestMode.ImmediateLoad).Value;

        public ref float ShardCooldown => ref Projectile.ai[1];
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.tileCollide = false;
            Projectile.timeLeft = (int)Lifetime;
        }

        public override void ExtraBehavior()
        {
            // Generate a burst of bubble-like nebula dust.
            if (!Main.dedServ && Time == 5f)
            {
                int totalBubbles = 24;
                for (int i = 0; i < totalBubbles; i++)
                {
                    Dust nebulaBubble = Dust.NewDustPerfect(Projectile.Center, 180);
                    nebulaBubble.velocity = Main.rand.NextVector2Circular(6f, 6f);
                    nebulaBubble.scale = Main.rand.NextFloat(2f, 3f);
                    nebulaBubble.noGravity = true;
                }
            }
        }

        public override void DetermineScale() => Projectile.scale = Projectile.timeLeft / Lifetime * MaxScale;

        public override bool PreDraw(ref Color lightColor)
        {
            DrawBeamWithColor(Color.White * 0.9f, Projectile.scale);
            DrawBeamWithColor(Color.White * 0.9f, Projectile.scale * 0.5f);
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.numHits > 0)
                Projectile.damage = (int)(Projectile.damage * 0.95f);
            if (Projectile.damage < 1)
                Projectile.damage = 1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
            if (ShardCooldown > 0f)
                return;

            // The "Center" of the laser is actually the start of it in this context.
            // Collision is done separately. This might have a slight offset due to collision
            // boxes, but that should be negligible.
            float lengthFromStart = Projectile.Distance(target.Center);

            int totalShards = (int)MathHelper.Lerp(4, 7, MathHelper.Clamp(lengthFromStart / MaxLaserLength * 1.5f, 0f, 1f));
            int shardType = ModContent.ProjectileType<ElementalNer>();
            int shardDamage = (int)(Projectile.damage * 0.5);
            for (int i = 0; i < totalShards; i++)
            {
                int tries = 0;
                Vector2 spawnOffset;
                do
                {
                    spawnOffset = Main.rand.NextVector2CircularEdge(target.width * 0.5f + 40f, target.height * 0.5f + 40f);
                    tries++;
                }
                while (Collision.SolidCollision((target.Center + spawnOffset).ToTileCoordinates().ToVector2(), 4, 4) && tries < 10);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center + spawnOffset, Main.rand.NextVector2CircularEdge(24f, 24f), shardType, shardDamage, Projectile.knockBack, Projectile.owner);
            }

            ShardCooldown = 3f;
            Projectile.netUpdate = true;
            if (!Main.dedServ && Time == 5f)
            {
                int starPoints = 8;
                for (int i = 0; i < starPoints; i++)
                {
                    float angle = MathHelper.TwoPi * i / starPoints;
                    for (int j = 0; j < 12; j++)
                    {
                        float starSpeed = MathHelper.Lerp(2f, 10f, j / 12f);
                        Color dustColor = Color.Lerp(Color.White, Color.Yellow, j / 12f);
                        float dustScale = MathHelper.Lerp(1.6f, 0.85f, j / 12f);

                        Dust fire = Dust.NewDustPerfect(Projectile.Center, 6);
                        fire.velocity = angle.ToRotationVector2() * starSpeed;
                        fire.color = dustColor;
                        fire.scale = dustScale;
                        fire.noGravity = true;
                    }
                }
            }
            target.AddBuff(ModContent.BuffType<ElementalMix>(), 30);
            int type = ModContent.ProjectileType<FuckYou>();
            int boomDamage = (int)(hit.Damage * 1.1);
            int boom = Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, type, boomDamage, hit.Knockback, Projectile.owner, 0f, Main.rand.NextFloat(0.85f, 2f));
            Main.projectile[boom].DamageType = DamageClass.Magic;
        }
    }
}
