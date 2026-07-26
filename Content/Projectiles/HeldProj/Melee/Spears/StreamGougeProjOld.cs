using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Content.Projectiles.Melee.Spears;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Spears
{
    public class StreamGougeProjOld : BaseSpear
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<StreamGougeOld>();
        public override float RangeMin => 16;
        public override float RangeMax => 102;
        public override void ExAI()
        {
            if (Projectile.ai[1] == 0f)
            {
                Projectile.localNPCHitCooldown = 10;
                Projectile.usesLocalNPCImmunity = true;
                ShootProj();
                Projectile.ai[1] = 1f;
            }
        }
        public void ShootProj()
        {
            int damage = (int)(Projectile.damage * 0.5f);
            float kb = Projectile.knockBack * 0.5f;
            Vector2 projPos = Projectile.Center + Projectile.velocity;
            Vector2 projVel = Projectile.velocity.SafeNormalize(Vector2.UnitX) * 12f;
            if (Projectile.owner == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), projPos, projVel, ProjectileType<EssenceBeam>(), damage * 3, kb, Projectile.owner, 0f, 0f);

            SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
            //粒子
            ExtraBehavior();
            //顶点处生成传送门粒子
            PortalDust();
        }
        public void PortalDust()
        {
            Vector2 SpawnCenterAdd = Projectile.velocity.SafeNormalize(Vector2.One) * 122;
            int circleDust = 18;
            Vector2 baseDustVel = new Vector2(3.8f, 0f);
            for (int i = 0; i < circleDust; ++i)
            {
                int dustID = 173;
                float angle = i * (MathHelper.TwoPi / circleDust);
                Vector2 dustVel = baseDustVel.RotatedBy(angle);

                int idx = Dust.NewDust(Projectile.Center + SpawnCenterAdd, 1, 1, dustID);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].position = Projectile.Center + SpawnCenterAdd;
                Main.dust[idx].velocity = dustVel;
                Main.dust[idx].scale = 2.4f;
            }
        }
        public void ExtraBehavior()
        {
            int movingDust = 3;
            for (int i = 0; i < movingDust; ++i)
            {
                int dustID = 173;
                Vector2 corner = 0.5f * Projectile.position + 0.5f * Projectile.Center;
                int idx = Dust.NewDust(corner, Projectile.width / 2, Projectile.height / 2, dustID);

                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity = Vector2.Zero;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIGodSlayerInferno>(), 300);
        }
    }
}
