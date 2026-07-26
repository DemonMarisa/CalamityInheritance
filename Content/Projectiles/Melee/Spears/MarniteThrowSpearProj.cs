using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Core.Misc;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Spears
{
    public class MarniteThrowSpearProj : CIMeleeProj
    {
        public override string Texture => GetInstance<MarniteSpear>().Texture;
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<MarniteSpear>();
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 15;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[0] += 1f;
            //固定飞行一段距离后才会受重力影响
            if (Projectile.ai[0] > 75f)
            {
                Projectile.velocity.Y += 0.09f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnThrowDust();
        }
        public override void OnKill(int timeLeft)
        {
            OnThrowDust();
        }
        public void OnThrowDust()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustMeteor, Projectile.oldVelocity.X * 0.75f, Projectile.oldVelocity.Y * 0.75f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustFrostDagger, Projectile.oldVelocity.X * 0.75f, Projectile.oldVelocity.Y * 0.75f);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Request<Texture2D>(Texture).Value;
            float offset = MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;
            Vector2 offset2 = new Vector2(-texture.Width / 2, 0).RotatedBy(Projectile.rotation);
            Projectile.BaseProjPreDraw(texture, lightColor, offset2, offset, 1);
            return false;
        }
    }
}
