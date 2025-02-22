using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class DragonBowFlame: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.scale = 1.5f;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.hide = true;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.hide) //called on first AI tick only - more initializations
            {
                Projectile.hide = false;
                Projectile.ai[1] = -1f;

                if (Projectile.ai[0] != 0f) //if empowered fireball
                {
                    Projectile.extraUpdates = 1;
                    Projectile.localAI[0] = Main.rand.Next(30);

                    if (Projectile.ai[0] == 2f) //if homing fireball
                        Projectile.timeLeft += 180;
                }

                Projectile.netUpdate = true;
            }

            //intangible until it's in completely open space
            if (!Projectile.tileCollide && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.tileCollide = true;
                Projectile.netUpdate = true;
            }

            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 60f) //dragon dust trail counter, but only empowered proj spawns it
            {
                Projectile.localAI[0] = 0f;

                //只有:射弹是追踪射弹属性，才会以一定概率生成一个额外的追踪射弹，且射弹速度为1.5f倍率，伤害为1.8f，执行这条指令后生成的新射弹将无法在执行这条指令
                if (Projectile.ai[0] == 0f && Projectile.ai[2] == 1f && Projectile.owner == Main.myPlayer && Main.rand.NextBool(3))
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                                             Projectile.Center,
                                             Projectile.velocity * 1.5f,
                                             ModContent.ProjectileType<DragonBowFlame>(),
                                             (int)(Projectile.damage * 1.8f),
                                             Projectile.knockBack * 3f,
                                             Projectile.owner,
                                             2f,
                                             0,
                                             -1f);
            }

            Projectile.localAI[1]++;
            if (Projectile.localAI[1] > 8f) //homing counter, checks every 8/2=4 ticks
            {
                Projectile.localAI[1] = 0f;

                if (Projectile.ai[0] == 2f && Projectile.ai[1] < 0f) //if homing fireball and no target
                {
                    int possibleTarget = -1;
                    float closestDistance = 50000f;

                    foreach (NPC npc in Main.ActiveNPCs)
                    {
                        if (npc.chaseable && npc.lifeMax > 0 && !npc.dontTakeDamage && !npc.friendly &&
                            !npc.immortal && Collision.CanHit(Projectile.Center, 0, 0, npc.Center, 0, 0))
                        {
                            float distance = Vector2.Distance(Projectile.Center, npc.Center);

                            if (closestDistance > distance)
                            {
                                closestDistance = distance;
                                possibleTarget = npc.whoAmI;
                            }
                        }
                    }

                    Projectile.ai[1] = possibleTarget;
                    Projectile.netUpdate = true;
                }
            }

            if (Projectile.ai[1] != -1f) //if has target
            {
                NPC npc = Main.npc[(int)Projectile.ai[1]];
                //追踪的速度从10f至18f随机
                float homingSpeed = Main.rand.NextFloat(10f, 18f);
                //惯性也一样
                float inertiaSpeed = Main.rand.NextFloat(10f, 18f);

                if (npc.active && npc.chaseable && !npc.dontTakeDamage) //do homing
                {
                    CalamityInheritanceUtils.HomeInOnNPC(Projectile, true, 50000f, homingSpeed, inertiaSpeed);
                }
                else //target not valid, stop homing
                {
                    Projectile.ai[1] = -1;
                    Projectile.netUpdate = true;
                }
            }

            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 1.5f + Main.rand.NextFloat());
            Main.dust[d].noGravity = true;

            Lighting.AddLight(Projectile.Center, 255f / 255f, 154f / 255f, 58f / 255f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 154, 58, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, texture2D13.Width, texture2D13.Height)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2((float)texture2D13.Width / 2f, (float)texture2D13.Height / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (timeLeft != 0)
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

                if (Projectile.ai[0] != 0f && Projectile.owner == Main.myPlayer) //if empowered, make exo arrow and dragon dust
                {
                    Vector2 randomAngle = Projectile.Center + new Vector2(600, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                    Vector2 speed = Projectile.Center - randomAngle;
                    //如果是追踪射弹，龙尘的伤害取弹幕伤害的1/2，否则取1/4
                    int dDustDamage = Projectile.ai[0] == 2f? Projectile.damage / 2 : Projectile.damage / 4;
                    speed /= 30f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), randomAngle.X, randomAngle.Y, speed.X, speed.Y, ModContent.ProjectileType<DragonBowExoArrow>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<DragonDust>(), dDustDamage , Projectile.knockBack * 2f, Projectile.owner);
                }

                Projectile.position = Projectile.Center;
                Projectile.width = 180;
                Projectile.height = 180;
                Projectile.position.X = Projectile.position.X - 90;
                Projectile.position.Y = Projectile.position.Y - 90;

                //just dusts
                const int constant = 16;    //攻速太快了粒子少一点
                float modifier = 4f + 8f * Main.rand.NextFloat();
                for (int i = 0; i < constant; i++)
                {
                    Vector2 rotate = Vector2.Normalize(Projectile.velocity) * modifier;
                    rotate = rotate.RotatedBy((i - (constant / 2 - 1)) * 6.28318548f / constant, default) + Projectile.Center;
                    Vector2 faceDirection = rotate - Projectile.Center;
                    int dust = Dust.NewDust(rotate + faceDirection, 0, 0, DustID.InfernoFork, 0f, 0f, 45, default, 2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity = faceDirection;
                }
                for (int j = 0; j < 4; j++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.InfernoFork, 0f, 0f, 50, default, 1.5f);

                    int fieryDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.InfernoFork, 0f, 0f, 50, default, 1f);
                    Main.dust[fieryDust].noGravity = true;
                    Main.dust[fieryDust].velocity *= 2f;
                }
                for (int k = 0; k < 12; k++)
                {
                    int fieryDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare, 0f, 0f, 0, default, 3f);
                    Main.dust[fieryDust].noGravity = true;
                    Main.dust[fieryDust].velocity *= 3f;
                }

                Projectile.timeLeft = 0; //should avoid infinite loop if a hit npc calls proj.Kill()
                Projectile.penetrate = -1;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
                Projectile.damage /= 3;
                Projectile.Damage();
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 240);

            if (Projectile.ai[0] != 0f && Projectile.owner == Main.myPlayer) //if empowered
            {
                if (Projectile.timeLeft != 0) //will not be called on npcs hit by explosion (only direct hits)
                {
                    //make exo arrow, make meteor
                    Vector2 randomAngle = target.Center + new Vector2(600, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                    Vector2 speed = target.Center - randomAngle;
                    speed /= 30f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), randomAngle.X, randomAngle.Y, speed.X, speed.Y, ModContent.ProjectileType<DragonBowExoArrow>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);

                    Vector2 vel = new Vector2(Main.rand.Next(-400, 401), Main.rand.Next(500, 801));
                    Vector2 pos = target.Center - vel;
                    vel.X += Main.rand.Next(-100, 101);
                    vel.Normalize();
                    vel *= 30f;
                    //发射的射弹如果是追踪的,天降的陨石伤害取2.1f，否则取1.7f
                    int skyFlareDamage = Projectile.ai[0] == 2f? (int)(Projectile.damage * 2.1f) : (int)(Projectile.damage * 1.7f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, vel + target.velocity, ModContent.ProjectileType<SkyFlareFriendly>(), skyFlareDamage, Projectile.knockBack * 5f, Projectile.owner);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 240);

            if (Projectile.ai[0] != 0f && Projectile.owner == Main.myPlayer) //if empowered
            {
                if (Projectile.timeLeft != 0) //will not be called on npcs hit by explosion (only direct hits)
                {
                    //make exo arrow, make meteor
                    Vector2 randomAngle = target.Center + new Vector2(600, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                    Vector2 speed = target.Center - randomAngle;
                    speed /= 30f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), randomAngle.X, randomAngle.Y, speed.X, speed.Y, ModContent.ProjectileType<DragonBowExoArrow>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);

                    Vector2 vel = new Vector2(Main.rand.Next(-400, 401), Main.rand.Next(500, 801));
                    Vector2 pos = target.Center - vel;
                    vel.X += Main.rand.Next(-100, 101);
                    vel.Normalize();
                    vel *= 30f;
                    //发射的射弹如果是追踪的,且并非衍生追踪射弹,天降的陨石伤害取2.4f，否则取1.7f
                    int skyFlareDamage = (Projectile.ai[0] == 2f && Projectile.ai[2] != 1f)? (int)(Projectile.damage * 2.1f) : (int)(Projectile.damage * 1.7f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, vel + target.velocity, ModContent.ProjectileType<SkyFlareFriendly>(), skyFlareDamage, Projectile.knockBack * 5f, Projectile.owner);
                }
            }
        }
    }
}
