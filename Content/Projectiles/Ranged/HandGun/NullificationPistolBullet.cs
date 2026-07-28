using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Ranged.HandGun
{
    public class NullShotLegacy : CIRangedProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 6;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
        }
        public override void AI()
        {
            int dCount = 10;
            for (int i = 0; i < dCount; i++)
            {
                float x = Projectile.position.X - Projectile.velocity.X / dCount * i;
                float y = Projectile.position.Y - Projectile.velocity.Y / dCount * i;
                Dust d = Dust.NewDustDirect(new Vector2(x, y), 1, 1, DustID.MagnetSphere);
                d.scale = 2f;
                d.noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type == NPCID.TargetDummy)
                return;
            if (target.boss)
                return;
            //他这里还有一个玩家类里判是否有boss的判定，我也不知道存在的意义是什么
            //这里跳过了
            int nullBuff = Main.rand.Next(8);
            switch (nullBuff)
            {
                case 0:
                    if (target.damage != 0)
                        target.damage += 10;
                    break;
                case 1:
                    target.damage -= 10;
                    break;
                case 2:
                    target.knockBackResist = 0;
                    break;
                case 3:
                    target.knockBackResist = 10;
                    break;
                case 4:
                    target.defense += 5;
                    break;
                case 5:
                    target.defense -= 5;
                    break;
                case 6:
                    target.scale *= 2f;
                    break;
                case 7:
                    target.scale *= .5f;
                    break;
                default:
                    break;

            }

        }
    }
}
