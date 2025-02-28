// using System;
// using CalamityMod;
// using CalamityMod.Buffs.DamageOverTime;
// using CalamityMod.Items.Weapons.Melee;
// using CalamityMod.Projectiles;
// using CalamityMod.Projectiles.Healing;
// using Microsoft.Xna.Framework;
// using Terraria;
// using Terraria.Audio;
// using Terraria.ID;
// using Terraria.ModLoader;

// namespace CalamityInheritance.Content.Projectiles.Rogue
// {
//     public class RogueTypeKnivesShadowspecProjClone: ModProjectile, ILocalizedModType
//     {
//         public new string LocalizationCategory => "Content.Projectiles.Rogue";
//         public static readonly float Acceleration = 0.98f; //飞行加速度
//         public override void SetStaticDefaults()
//         {
//             ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
//             ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
//         }

//         public override void SetDefaults()
//         {
//             Projectile.width = 14;
//             Projectile.height = 14;
//             Projectile.friendly = true;
//             Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
//             Projectile.penetrate = 1;
//             Projectile.timeLeft = 650;
//             Projectile.extraUpdates = 10;
//         }
//         /*********************AI逻辑************************
//         *描述:  潜伏时掷出一支拥有极高速的直线飞刀，这个飞刀没有追踪能力
//         *超高速的直线飞刀击中敌怪时，会先造成一定程度的滞留伤害，而后，从敌怪内部爆出10~18个普通飞刀
//         *普通飞刀采用类似于宇宙灾兵的逻辑
//         *还没开始写，先全部注释掉,  等需要做了就会继续写
//         *[无-剑制]
//         ****************************************************/
//         public override void AI()
//         {
//             Player projOwner = Main.player[Projectile.owner];
//             //获取超高速飞刀的双坐标飞行速度
//             float tryGetVelocityX = Projectile.velocity.X; 
//             float tryGetVelocityY = Projectile.velocity.Y; 
//             FlyingSound(); //飞刀飞行期间的音效
//             FlyingDust();
            


//         }


//         //Give it a custom hitbox shape so it may remain rectangular and elongated
//         public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
//         {
//             float collisionPoint = 0f;
//             float bladeHalfLength = 25f * Projectile.scale / 2f;
//             float bladeWidth = 14f * Projectile.scale;

//             Vector2 direction = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2();

//             return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - direction * bladeHalfLength, Projectile.Center + direction * bladeHalfLength, bladeWidth, ref collisionPoint);
//         }

//         public override void OnKill(int timeLeft)
//         {
//             for (int i = 0; i < 3; i++)
//             {
//                 int illustrious = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurificationPowder, 0f, 0f, 100, default, 0.8f);
//                 Main.dust[illustrious].noGravity = true;
//                 Main.dust[illustrious].velocity *= 1.2f;
//                 Main.dust[illustrious].velocity -= Projectile.oldVelocity * 0.3f;
//             }
//         }

//         public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
//         {
            
//         }

//         public override void OnHitPlayer(Player target, Player.HurtInfo info)
//         {
//             target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);

//             int heal = (int)Math.Round(info.Damage * 0.015);
//             if (heal > ShadowknivesLifeStealCap)
//                 heal = ShadowknivesLifeStealCap;

//             if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0)
//                 return;

//             CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ModContent.ProjectileType<RoyalHeal>(), ShadowknivesLifeStealRange);
//         }

//         public override bool PreDraw(ref Color lightColor)
//         {
//             CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
//             return false;
//         }
//         public void FlyingSound() //飞行时的音效
//         {
//             Projectile.localAI[0] += 1f;
//             if(Projectile.localAI[0] > 5f && Projectile.localAI[0] < 10f) //飞刀速度极快，因此这5f可能仅仅只是一瞬 
//                 SoundEngine.PlaySound(SoundID.AbigailCry with {Volume = 0.7f}, Projectile.Center);//阿比盖尔的哭泣
//         }
//         public void FlyingDust()
//         {
//             Vector2 getPos = new Vector2(16f, 0).RotatedByRandom(MathHelper.ToRadians(360f));
//             Vector2 getVel = new Vector2(16f, 0).RotatedBy(getPos.ToRotation());
//             float xflyingVel = Projectile.velocity.X * 0.5f + getVel.X; 
//             float yflyingVel = Projectile.velocity.Y * 0.5f + getVel.Y; 
//             float dScale = 1.5f;
//             Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) +getPos, DustID.GemEmerald, new Vector2(xflyingVel, yflyingVel), 50, default, dScale);
//             dust.noGravity = true;
//         }
//     }
// }
