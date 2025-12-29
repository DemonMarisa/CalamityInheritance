using CalamityInheritance.Content.BaseClass;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Flails
{
    public class CrescentMoonFlailLegacy : CIBaseWhip, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public int moonCounter = 6;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override Color SpecialDrawColor => default;
        public override int ExudeDustType => 176;
        public override int WhipDustType => 176;
        public override int HandleHeight => 56;
        public override int BodyType1StartY => 60;
        public override int BodyType1SectionHeight => 22;
        public override int BodyType2StartY => 86;
        public override int BodyType2SectionHeight => 18;
        public override int TailStartY => 108;
        public override int TailHeight => 50;

        public override void ExtraBehavior()
        {
            Player player = Main.player[Projectile.owner];
            if (moonCounter > 0)
                moonCounter--;
            if (moonCounter <= 0 && Projectile.owner == Main.myPlayer)
            {
                Vector2 vectorBruh = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
                if (player.direction != 1)
                {
                    vectorBruh.X = player.bodyFrame.Width - vectorBruh.X;
                }
                if (player.gravDir != 1f)
                {
                    vectorBruh.Y = player.bodyFrame.Height - vectorBruh.Y;
                }
                vectorBruh -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f;
                Vector2 newCenter = player.RotatedRelativePoint(Main.player[Projectile.owner].position + vectorBruh, true) + Projectile.velocity;
                int moonDamage = (int)(Projectile.damage * 0.18f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), newCenter.X, newCenter.Y, 0f, 0f, ProjectileType<CrescentMoonProjLegacy>(), moonDamage, 0f, Projectile.owner, 0f, 0f);
                moonCounter = 6;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<Nightwither>(), 240);
            Projectile.localAI[1] = 4f;
        }
    }
}
