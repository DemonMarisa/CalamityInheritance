using System.IO;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using CalamityInheritance.Content.Items;
using Terraria.ModLoader;
using Microsoft.Build.Evaluation;
using CalamityInheritance.Content.Items.Weapons;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    [LegacyName("KelvinCatalystBoomerangMelee")]
    public class MeleeTypeKelvinCatalystProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => $"{Generic.WeaponRoute}/Melee/MeleeTypeKelvinCatalyst";
        public int AIState = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.coldDamage = true;
            Projectile.extraUpdates = 1; //给了一个额外更新
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(AIState);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            AIState = reader.ReadInt32();
        }
        
        public override void AI()
        {
            /*
            if(CIFunction.IsThereNpcNearby(ModContent.NPCType<CalamitasRebornPhase2>(), Main.player[Projectile.owner], 3000f) || Main.zenithWorld)  
            {
                Projectile.localAI[1] += 1f;
                if (Projectile.localAI[1] % (Main.zenithWorld? 10 : 72) == 0)
                OnHitEffects();
            }
            */
            VisualAudioEffects();
            BoomerangAI();
        }
        
        private void BoomerangAI()
        {
            switch (AIState)
            {
                case 0:
                    Projectile.localAI[0] += 1f;
                    if (Projectile.localAI[0] >= 60f)
                        ResetStats();
                    break;
                case 1:
                    ReturnToPlayer();
                    break;
            }
        }
        private void ReturnToPlayer()
        {
            Player player = Main.player[Projectile.owner];
            float returnSpeed = 20f;
            float acceleration = 2f;
            CIFunction.BoomerangReturningAI(player, Projectile, returnSpeed, acceleration);
            if (Main.myPlayer == Projectile.owner)
                if (Projectile.Hitbox.Intersects(player.Hitbox))
                    Projectile.Kill();
        }
        private void VisualAudioEffects()
        {
            Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.3f / 255f, Main.DiscoR * 0.4f / 255f, Main.DiscoR * 0.5f / 255f);

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 60;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.Center);
            }

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceRod, 0f, 0f, 100, default, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;

            Projectile.rotation += 0.25f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ResetStats();
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
        private void ResetStats()
        {
            AIState = 1;
            Projectile.localAI[0] = 0f;
            Projectile.width = Projectile.height = 60;
            Projectile.tileCollide = false;
            Projectile.netUpdate = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 240);
            OnHitEffects();
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Frostburn2, 240);
            OnHitEffects();
        }
        private void OnHitEffects()
        {
            int maxSpawns =  3;
            if (Projectile.owner == Main.myPlayer && Projectile.numHits < maxSpawns)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 velocity = (MathHelper.TwoPi * i / 5f).ToRotationVector2() * 4f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                                             Projectile.Center,
                                             velocity,
                                             ModContent.ProjectileType<MeleeTypeKelvinCatalystProjStar>(),
                                             (int)(Projectile.damage * 0.7f), //冰星伤害0.5→0.7
                                             Projectile.knockBack * 0.5f,
                                             Projectile.owner);
                }
                SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
