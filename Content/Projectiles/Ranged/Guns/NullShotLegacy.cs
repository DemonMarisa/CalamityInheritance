using CalamityInheritance.Content.Projectiles;
using CalamityMod.CalPlayer;
using CalamityMod.Dusts;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Projectiles;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged.Guns
{
    public class NullShotLegacy : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Ranged;
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 3f)
            {
                for (int num134 = 0; num134 < 10; num134++)
                {
                    float x = Projectile.position.X - Projectile.velocity.X / 10f * num134;
                    float y = Projectile.position.Y - Projectile.velocity.Y / 10f * num134;
                    int dust = Dust.NewDust(new Vector2(x, y), 1, 1, DustID.MagnetSphere, 0f, 0f, 0, default, 2f);
                    Main.dust[dust].alpha = Projectile.alpha;
                    Main.dust[dust].position.X = x;
                    Main.dust[dust].position.Y = y;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type == NPCID.TargetDummy)
            {
                return;
            }

            int randAmt = (!CalamityPlayer.areThereAnyDamnBosses).ToInt() * 8;
            int nullBuff = Main.rand.Next(randAmt);
            if (!target.boss)
            {
                switch (nullBuff)
                {
                    case 0:
                        if (target.type != NPCType<SuperDummyNPC>())
                            target.damage += 10;
                        break;
                    case 1:
                        target.damage -= 10;
                        break;
                    case 2:
                        target.knockBackResist = 0f;
                        break;
                    case 3:
                        target.knockBackResist = 1f;
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
                        target.scale *= 0.5f;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}