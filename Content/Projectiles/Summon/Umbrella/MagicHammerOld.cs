using CalamityMod.Projectiles.Summon.Umbrella;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Summon.Umbrella
{
    public class MagicHammerOld : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        private int counter = 0;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hammer");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.penetrate = 6;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }

        public override void AI()
        {
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] > 10f && Main.rand.NextBool(3))
            {
                Projectile.alpha -= 5;
                if (Projectile.alpha < 50)
                {
                    Projectile.alpha = 50;
                }
            }
            Projectile.rotation += 0.075f;
            if (counter <= 30)
            {
                counter++;
                return;
            }
            int num716 = -1;
            Vector2 vector59 = Projectile.Center;
            float num717 = MagicHat.Range;
            if (Projectile.localAI[0] > 0f)
            {
                Projectile.localAI[0] -= 1f;
            }
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0f && Projectile.localAI[0] == 0f)
            {
                if (player.HasMinionAttackTargetNPC)
                {
                    NPC npc = Main.npc[player.MinionAttackTargetNPC];
                    if (npc.CanBeChasedBy(Projectile, false) && (Projectile.ai[0] == 0f || Projectile.ai[0] == player.MinionAttackTargetNPC + 1))
                    {
                        Vector2 center4 = npc.Center;
                        float num719 = Vector2.Distance(center4, vector59);
                        if (num719 < num717)
                        {
                            num717 = num719;
                            vector59 = center4;
                            num716 = player.MinionAttackTargetNPC;
                        }
                    }
                }
                else
                {
                    for (int num718 = 0; num718 < Main.npc.Length; num718++)
                    {
                        NPC nPC6 = Main.npc[num718];
                        if (nPC6.CanBeChasedBy(Projectile, false) && (Projectile.ai[0] == 0f || Projectile.ai[0] == num718 + 1))
                        {
                            Vector2 center4 = nPC6.Center;
                            float num719 = Vector2.Distance(center4, vector59);
                            if (num719 < num717)
                            {
                                num717 = num719;
                                vector59 = center4;
                                num716 = num718;
                            }
                        }
                    }
                }
                if (num716 >= 0)
                {
                    Projectile.ai[0] = num716 + 1;
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.localAI[0] == 0f && Projectile.ai[0] == 0f)
            {
                Projectile.localAI[0] = 30f;
            }
            bool flag32 = false;
            if (Projectile.ai[0] != 0f)
            {
                int num720 = (int)(Projectile.ai[0] - 1f);
                if (Main.npc[num720].active && !Main.npc[num720].dontTakeDamage && Main.npc[num720].immune[Projectile.owner] == 0)
                {
                    float num721 = Main.npc[num720].position.X + Main.npc[num720].width / 2;
                    float num722 = Main.npc[num720].position.Y + Main.npc[num720].height / 2;
                    float num723 = Math.Abs(Projectile.position.X + Projectile.width / 2 - num721) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - num722);
                    if (num723 < MagicHat.Range * 1.25f)
                    {
                        flag32 = true;
                        vector59 = Main.npc[num720].Center;
                    }
                }
                else
                {
                    Projectile.ai[0] = 0f;
                    flag32 = false;
                    Projectile.netUpdate = true;
                }
            }
            if (flag32)
            {
                Vector2 v = vector59 - Projectile.Center;
                float num724 = Projectile.velocity.ToRotation();
                float num725 = v.ToRotation();
                float num726 = num725 - num724;
                num726 = MathHelper.WrapAngle(num726);
                Projectile.velocity = Projectile.velocity.RotatedBy(num726 * 0.25, default);
            }
            float num727 = Projectile.velocity.Length();
            Projectile.velocity.Normalize();
            Projectile.velocity *= num727 + 0.0025f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 56, 0, Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 dspeed = new Vector2(Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-7f, 7f));
                int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.IceRod, dspeed.X, dspeed.Y, 50, default, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}
