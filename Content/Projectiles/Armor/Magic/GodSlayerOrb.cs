using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Armor.Magic
{
    public class GodSlayerOrb : CIArmorProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 190 && target.CanBeChasedBy(Projectile);
        public override void AI()
        {
            int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
            Main.dust[d].noGravity = true;
            Main.dust[d].velocity *= 0f;
            if (Projectile.timeLeft < 190)
            {
                Projectile.extraUpdates = 2;
                Projectile.HomeInNPC(3000f, 12f, 25f, null, !Projectile.tileCollide);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIGodSlayerInferno>(), 180);
        }
    }
}
