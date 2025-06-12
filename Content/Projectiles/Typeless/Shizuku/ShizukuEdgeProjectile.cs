using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    //Bro, 我才不会学灾厄那样把重复的功能写在一个工程里面，明显把左右键拆出来单独做要方便而且更容易管理的多。
    //public class ShizukuEdgeProjectile : ModProjectile, ILocalizedModType
    //{
    //    public new string LocalizationCategory => "Content.Projectiles.Typeless";
    //    public override string Texture => "CalamityInheritance/Content/Items/Weapons/Typeless/ShizukuEdge";
    //    public Player Owner => Main.player[Projectile.owner];
    //    public ref float Timer => ref Projectile.ai[0];
    //    public ref float SpinTimer => ref Projectile.ai[1];
    //    public ref float SpinAngle => ref Projectile.localAI[0];
    //    //法器自转一圈需要的帧数。 
    //    const float TotalFrames = 15f;
    //    public bool CanShootProj = false;
    //    public static Vector2 HoldingOffset => new(-5, 10f);

    //    public override void SetStaticDefaults()
    //    {
    //        ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
    //        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
    //    }
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 108;
    //        Projectile.width = 84;
    //        Projectile.scale *= 1.2f;
    //        Projectile.DamageType = DamageClass.Generic;
    //        Projectile.friendly = true;
    //        Projectile.penetrate = -1;
    //        Projectile.timeLeft = 90000;
    //        Projectile.tileCollide = false;
    //        Projectile.ignoreWater = true;
    //        Projectile.alpha = 255;
    //        Projectile.usesLocalNPCImmunity = true;
    //        Projectile.localNPCHitCooldown = 1;
    //    }
    //    public override void AI()
    //    {
    //        if (Timer == 0f)
    //        {
    //            //刚生成一瞬间时的音效。
    //            Projectile.rotation = Projectile.rotation + MathHelper.PiOver4 + MathHelper.Pi;
    //            SoundEngine.PlaySound(CISoundID.SoundIceRodBlockPlaced, Projectile.Center);
    //            PlayDust();
    //        }
    //        Projectile.Opacity = Utils.GetLerpValue(0f, 15f, Timer, true);
    //        if (Owner.channel)
    //        {
    //            FollowingYourMouse();
    //            if (CanShootProj)
    //                ShootProjectile();
    //        }
    //        else
    //        {
    //            ReturnToOwner();
    //            float idealAngle = Projectile.AngleTo(Owner.Center) + MathHelper.PiOver4;
    //            Projectile.rotation = Projectile.rotation.AngleLerp(idealAngle, 0.1f);
    //            Projectile.rotation = Projectile.rotation.AngleTowards(idealAngle, 0.25f);
    //        }
    //        Owner.itemTime = 2;
    //        Owner.itemAnimation = 2;
    //        AddLight();
    //        Timer++;
    //    }

    //    public void ShouldSpin()
    //    {
    //        //法器并不是刀剑，让他一直转会有一定的影响。这里会给一个定时转的AI。
    //        SpinTimer++;
    //        //角度数量
    //        float anglePerFrame = MathHelper.TwoPi / TotalFrames;
    //        if (SpinTimer < 60f)
    //            return;
    //        //这里需要一个刀光音效
    //        if (SpinTimer == 60f)
    //        {
    //            //旋转一次的时候才会允许投射一次射弹
    //            SoundEngine.PlaySound(CISoundID.SoundTerraBlade, Projectile.Center);
    //            CanShootProj = true;
    //        }
    //        else CanShootProj = false;

    //        if (SpinTimer > 60f)
    //        {
    //            Projectile.rotation = Projectile.rotation.AngleLerp(MathHelper.TwoPi, anglePerFrame);
    //            if (SpinTimer > 80f)
    //                SpinTimer = 0f;
    //        }
    //    }

    //    public void PlayDust()
    //    {
    //        //一圈冰系粒子，受重力
    //        int dType = Main.rand.NextBool() ? DustID.IceRod : DustID.IceTorch;
    //        CIFunction.DustCircle(Projectile.Center, 32f, Main.rand.NextFloat(0.8f, 1.21f), dType, true, 12f, 200);
    //    }
    //    //射弹在指针位置鬼畜
    //    public void FollowingYourMouse()
    //    {
    //        if (Projectile.owner != Main.myPlayer)
    //            return;

    //        if (Projectile.WithinRange(Main.MouseWorld, Projectile.velocity.Length() * 0.7f))
    //        {
    //            Projectile.Center = Main.MouseWorld;
    //            ShouldSpin();
    //        }
    //        else
    //        {
    //            Projectile.velocity = (Projectile.velocity * 3f + Projectile.DirectionTo(Main.MouseWorld) * 19f) / 4f;
    //            float shouldAngleTo = Projectile.AngleTo(Main.MouseWorld) + MathHelper.PiOver4;
    //            Projectile.rotation = Projectile.rotation.AngleLerp(shouldAngleTo, 0.1f);
    //            Projectile.rotation = Projectile.rotation.AngleTowards(shouldAngleTo, 0.25f);
    //        }
    //        Projectile.netSpam = 0;
    //        Projectile.netUpdate = true;
    //    }
    //    public void ReturnToOwner()
    //    {
    //        Projectile.Center = Vector2.Lerp(Projectile.Center, Owner.Center, 0.02f);
    //        Projectile.velocity = Projectile.DirectionTo(Owner.Center) * 22f;
    //        if (Projectile.Hitbox.Intersects(Owner.Hitbox))
    //            Projectile.Kill();
    //    }
    //    public void ShootProjectile()
    //    {
    //        #region 初始化
    //        int scytherProj = ModContent.ProjectileType<ScythePlaceholder>();
    //        int ghostRandom = Main.rand.Next(0, 3);
    //        ghostRandom = ghostRandom switch
    //        {
    //            0 => ModContent.ProjectileType<SoulSmallPlaceholder>(),
    //            1 => ModContent.ProjectileType<SoulMidPlaceholder>(),
    //            _ => ModContent.ProjectileType<SoulLargePlaceholder>(),
    //        };
    //        var projSrc = Projectile.GetSource_FromThis();
    //        float scytheDmg = Projectile.damage * 1.2f;
    //        float ghostDmg = Projectile.damage * 0.4f;
    //        #endregion
    //        ShootGhost(ghostRandom, ghostDmg, projSrc);
    //        ShootScythe(scytherProj, scytheDmg, projSrc);
    //    }

    //    public void ShootScythe(int scytherProj, float scytheDmg, IEntitySource projSrc)
    //    {
    //        Vector2 srcPos = Projectile.Center - HoldingOffset;
    //        //最后给予一定的随机度
    //        Vector2 finalPos = new(srcPos.X, srcPos.Y + 5f);
    //        //最终位置
    //        Vector2 velocity = srcPos - finalPos;
    //        //转速度向量
    //        float projSpeed = 16f;
    //        float length = velocity.Length();
    //        length = projSpeed / length;
    //        velocity.X *= length;
    //        velocity.Y *= length;
    //        Projectile.NewProjectile(projSrc, srcPos, velocity, scytherProj, (int)scytheDmg, 0f, Owner.whoAmI);
    //    }

    //    public void ShootGhost(int ghostType, float ghostDmg, IEntitySource projSrc)
    //    {
    //        for (int i = 0; i < 5; i++)
    //        {
    //            Vector2 srcPos = Projectile.Center - HoldingOffset;
    //            //最后给予一定的随机度
    //            Vector2 finalPos = new(srcPos.X + Main.rand.NextFloat(-8f, 9f), srcPos.Y + 5f);
    //            //最终位置
    //            Vector2 velocity = srcPos - finalPos;
    //            //转速度向量
    //            float projSpeed = 16f;
    //            float length = velocity.Length();
    //            length = projSpeed / length;
    //            velocity.X *= length;
    //            velocity.Y *= length;
    //            Projectile.NewProjectile(projSrc, srcPos, velocity, ghostType, (int)ghostDmg, 0f, Owner.whoAmI);
    //        }
    //    }

    //    public void AddLight()
    //    {
    //        //补光
    //        Lighting.AddLight(Projectile.Center, TorchID.White);
    //    }
    //    //绘制需要考虑描边
    //    public override bool PreDraw(ref Color lightColor)
    //    {
    //        if (Owner.dead /*|| !Owner.channel*/)
    //        {
    //            //给每个射弹贴图提供不同的角度
    //            multipleDrawPos[0] += aimDirection * -12f;
    //            multipleDrawPos[1] = multipleDrawPos[0] - (Projectile.rotation + MathHelper.PiOver4).ToRotationVector2() * Vector2.Distance(multipleDrawPos[0], multipleDrawPos[1]);
    //        }
    //        for (int i = 0; i < multipleDrawPos.Length; i++)
    //        {
    //            //转角。
    //            multipleDrawPos[i] -= (Projectile.oldRot[i] + MathHelper.PiOver4).ToRotationVector2() * Projectile.height / 2f;
    //        }
    //        //常规绘制。
    //        Vector2 manProjDrawPos = Projectile.Center - Main.screenPosition;
    //        for (int j = 0; j < 6; j++)
    //        {
    //            float rotation = Projectile.oldRot[j] - MathHelper.PiOver2;
    //            if (Owner.channel)
    //                rotation += 0.15f;
    //            Color afterImageColor = Color.Lerp(Color.White, Color.Transparent, 1f - (float)Math.Pow(Utils.GetLerpValue(0, 6, j), 1.4D)) * Projectile.Opacity;
    //            Main.EntitySpriteDraw(texture, manProjDrawPos, null, afterImageColor, rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
    //        }
    //        return false;
    //    }
    //}
}