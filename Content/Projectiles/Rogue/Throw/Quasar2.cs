using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Dusts;
using CalamityInheritance.Content.Items.Weapons.Rogue.Throws;
using CalamityInheritance.Content.Projectiles.Rogue.Explosion;
using CalamityInheritance.Core.Utils;
using LAP.Core.MiscDate;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Throw
{
    public class Quasar2 : CIRogueProj
    {
        public override string Texture => GetInstance<Quasar>().Texture;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Quasar");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver4;
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= 1.57f;
            }
            float num472 = Projectile.Center.X;
            float num473 = Projectile.Center.Y;
            float num474 = 600f;
            for (int num475 = 0; num475 < Main.maxNPCs; num475++)
            {
                if (Main.npc[num475].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[num475].Center, 1, 1) && !LAPInfo.AnyBossHere)
                {
                    float npcCenterX = Main.npc[num475].position.X + Main.npc[num475].width / 2;
                    float npcCenterY = Main.npc[num475].position.Y + Main.npc[num475].height / 2;
                    float num478 = Math.Abs(Projectile.position.X + Projectile.width / 2 - npcCenterX) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - npcCenterY);
                    if (num478 < num474)
                    {
                        if (Main.npc[num475].position.X < num472)
                        {
                            Main.npc[num475].velocity.X += 0.25f;
                        }
                        else
                        {
                            Main.npc[num475].velocity.X -= 0.25f;
                        }
                        if (Main.npc[num475].position.Y < num473)
                        {
                            Main.npc[num475].velocity.Y += 0.25f;
                        }
                        else
                        {
                            Main.npc[num475].velocity.Y -= 0.25f;
                        }
                    }
                }
            }
            LAPUtilities.HomeInNPC(Projectile, 1500f, 12f, 35f, 5f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIAstralInfection>(), 180);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<CIAstralInfection>(), 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<CIAstralBlue>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
            if (Projectile.CI().Stealth && Projectile.owner == Main.myPlayer)
            {
                int boomer = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<RadiantExplosion>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0f, 1f);
                Main.projectile[boomer].SetStealthAttack();
                Main.projectile[boomer].height = Main.projectile[boomer].width = 280;
                Main.projectile[boomer].Center = Projectile.Center;
            }
        }
    }
}
