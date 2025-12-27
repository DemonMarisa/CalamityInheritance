using CalamityMod.Projectiles.Melee;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Healing;
using CalamityMod.Projectiles;
using CalamityInheritance.Dusts;
using CalamityInheritance.Content.Projectiles.Melee.Explosion;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class BloodyEdgeEnergySword : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetStaticDefaults()
        {
            // 如果我们想要，可以使用默认的纹理，而不是提供我们自己的纹理
            // public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Excalibur;

            // 如果水母正在放电，并且我们用这种投射物攻击它，它会对我们造成伤害。
            // 这个集合包含 Night's Edge（夜之刃）、Excalibur（圣剑）、Terra Blade（大地之刃，近距离）和 The Horseman's Blade（骑士之刃，近距离）的投射物。
            // 这个集合不包含 True Night's Edge（真·夜之刃）、True Excalibur（真·圣剑）或远距离的 Terra Beam（大地之光）投射物。
            ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
            Main.projFrames[Type] = 4; // 这个投射物有 4 帧。
        }
        public override void SetDefaults()
        {
            // 这里宽度和高度并不重要，因为我们使用了自定义碰撞。
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.penetrate = 3; // 投射物可以穿透并击中3个敌人
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true; // 投射物会进行视线检查，确保不会穿过瓦片造成伤害
            Projectile.ownerHitCheckDistance = 300f; // 投射物能击中目标的最大距离为300像素（即18.75个瓦片）
            Projectile.usesOwnerMeleeHitCD = true; // 投射物使用与近战攻击相同的免疫帧数量
                                                   // 通常，投射物在击中所有可能的敌人后会消失。但在这个情况下，我们希望投射物继续存在，以便展示挥动的视觉效果。
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            // 我们将为这个投射物使用自定义AI。原始的圣剑（Excalibur）使用aiStyle 190。
            Projectile.aiStyle = -1;
            // 将投射物的aiStyle设置为-1，表示使用自定义AI
            // AIType = ProjectileID.Excalibur;

            // 如果使用自定义AI，请添加这行代码。否则，来自药水的视觉效果将在投射物的中心而不是在弧周围生成。
            // 我们将在AI()中自己生成围绕弧的视觉效果。
            Projectile.noEnchantmentVisuals = true;
        }
        public override void AI()
        {
            // 在我们的项目中，我们根据方向、最大时间和缩放比例生成投射物
            // Projectile.ai[0] == 方向
            // Projectile.ai[1] == 最大时间
            // Projectile.ai[2] == 缩放比例
            // Projectile.localAI[0] == 当前时间

            // 大地之刃在生成时会发出额外的音效。
            if (Projectile.localAI[0] == 0f)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item60 with { Volume = 0.65f }, Projectile.position);
            }

            Projectile.localAI[0]++; // Current time that the projectile has been alive.
            Player player = Main.player[Projectile.owner];
            float percentageOfLife = Projectile.localAI[0] / Projectile.ai[1]; // The current time over the max time.
            float direction = Projectile.ai[0];
            float velocityRotation = Projectile.velocity.ToRotation();
            float adjustedRotation = MathHelper.Pi * direction * percentageOfLife + velocityRotation + direction * MathHelper.Pi + player.fullRotation;
            Projectile.rotation = adjustedRotation; // Set the rotation to our to the new rotation we calculated.

            float scaleMulti = 0.6f; // Excalibur, Terra Blade, and The Horseman's Blade is 0.6f; True Excalibur is 1f; default is 0.2f 
            float scaleAdder = 1f; // Excalibur, Terra Blade, and The Horseman's Blade is 1f; True Excalibur is 1.2f; default is 1f 

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = scaleAdder + percentageOfLife * scaleMulti;

            // 使用AI样式190的其他剑类投射物具有不同的效果。
            // 本例仅包含圣剑（Excalibur）的效果。
            // 要查看其他剑的效果，请查看Projectile.cs中的AI_190_NightsEdge()。

            // 在挥动的弧线内生成一些灰尘效果，增加视觉表现力。
            float dustRotation = Projectile.rotation + Main.rand.NextFloatDirection() * MathHelper.PiOver2 * 0.7f;
            Vector2 dustPosition = Projectile.Center + dustRotation.ToRotationVector2() * 84f * Projectile.scale;
            Vector2 dustVelocity = (dustRotation + Projectile.ai[0] * MathHelper.PiOver2).ToRotationVector2();
            if (Main.rand.NextFloat() * 2f < Projectile.Opacity)
            {
                // Original Excalibur color: Color.Gold, Color.White
                Color dustColor = Color.Lerp(Color.OrangeRed, Color.DarkRed, Main.rand.NextFloat() * 0.3f);
                Terraria.Dust coloredDust = Terraria.Dust.NewDustPerfect(Projectile.Center + dustRotation.ToRotationVector2() * (Main.rand.NextFloat() * 80f * Projectile.scale + 20f * Projectile.scale), DustID.FireworksRGB, dustVelocity * 1f, 100, dustColor, 0.4f);
                coloredDust.fadeIn = 0.4f + Main.rand.NextFloat() * 0.15f;
                coloredDust.noGravity = true;
            }

            if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity)
            {
                // Original Excalibur color: Color.White
                Terraria.Dust.NewDustPerfect(dustPosition, DustID.Blood, dustVelocity, 100, Color.OrangeRed * Projectile.Opacity, 1.2f * Projectile.Opacity);
            }

            Projectile.scale *= Projectile.ai[2]; // Set the scale of the projectile to the scale of the item.

            // If the projectile is as old as the max animation time, kill the projectile.
            if (Projectile.localAI[0] >= Projectile.ai[1])
            {
                Projectile.Kill();
            }

            // This for loop spawns the visuals when using Flasks (weapon imbues)
            for (float i = -MathHelper.PiOver4; i <= MathHelper.PiOver4; i += MathHelper.PiOver2)
            {
                Rectangle rectangle = Utils.CenteredRectangle(Projectile.Center + (Projectile.rotation + i).ToRotationVector2() * 70f * Projectile.scale, new Vector2(60f * Projectile.scale, 60f * Projectile.scale));
                Projectile.EmitEnchantmentVisualsAt(rectangle.TopLeft(), rectangle.Width, rectangle.Height);
            }
        }

        // 这里是我们自定义的碰撞逻辑。
        // 该碰撞逻辑仅在投射物在目标范围内时运行，范围由Projectile.ownerHitCheckDistance确定，
        // 或者如果投射物尚未击中所有它可以击中的目标（由Projectile.penetrate决定）。
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // 这是圆锥的周长大小，即范围的大小。原版游戏使用94f来匹配纹理的大小。
            float coneLength = 94f * Projectile.scale;
            // 这个数值影响碰撞开始和结束时的旋转程度。
            // 较大的Pi值会使碰撞逆时针旋转。
            // 较小的Pi值会使碰撞顺时针旋转。
            // (Projectile.ai[0]代表方向)
            float collisionRotation = MathHelper.Pi * 2f / 25f * Projectile.ai[0];
            float maximumAngle = MathHelper.PiOver4; // The maximumAngle is used to limit the rotation to create a dead zone.
            float coneRotation = Projectile.rotation + collisionRotation;

            // 最大角度用于限制旋转，以创建一个死区。
            // Dust.NewDustPerfect(Projectile.Center + coneRotation.ToRotationVector2() * coneLength, DustID.Pixie, Vector2.Zero);
            // Dust.NewDustPerfect(Projectile.Center, DustID.BlueFairy, new Vector2((float)Math.Cos(maximumAngle) * Projectile.ai[0], (float)Math.Sin(maximumAngle)) * 5f); // Assumes collisionRotation was not changed

            // 圆锥的旋转角度是投射物的旋转角度加上碰撞旋转。
            if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation, maximumAngle))
            {
                return true;
            }

            // 第一个圆锥并不是整个挥动弧线，因此我们需要检查第二个圆锥以覆盖弧线的后部。
            float backOfTheSwing = Utils.Remap(Projectile.localAI[0], Projectile.ai[1] * 0.3f, Projectile.ai[1] * 0.5f, 1f, 0f);
            if (backOfTheSwing > 0f)
            {
                float coneRotation2 = coneRotation - MathHelper.PiOver4 * Projectile.ai[0] * backOfTheSwing;

                // Uncomment this line for a visual representation of the cone. The dusts are not perfect, but it gives a general idea.
                Terraria.Dust.NewDustPerfect(Projectile.Center + coneRotation2.ToRotationVector2() * coneLength, DustID.Blood, Vector2.Zero);

                if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation2, maximumAngle))
                {
                    return true;
                }
            }

            return false;
        }
        public override void CutTiles()
        {
            // Here we calculate where the projectile can destroy grass, pots, Queen Bee Larva, etc.
            Vector2 starting = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
            Vector2 ending = (Projectile.rotation + MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
            float width = 60f * Projectile.scale;
            Utils.PlotTileLine(Projectile.Center + starting, Projectile.Center + ending, width, DelegateMethods.CutTiles);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 原版游戏有多个粒子效果，可以在任何地方轻松使用
            // 粒子乐团中的粒子效果是由原版游戏预定义的，大多数不能进行太多自定义。
            // 使用自动补全功能可以查看其他可用的ParticleOrchestraType类型。
            // 在这里，我们随机在目标的碰撞盒内生成圣剑粒子。
            Vector2 position = Main.rand.NextVector2FromRectangle(target.Hitbox);
            float speed = 30f;
            float dire = Main.rand.NextFloatDirection();
            BloodyEdgeSpark.GeneratePrettySparkles(position, speed, dire);
            // 你也可以在敌人位置生成灰尘效果。这里是一个简单的例子：
            //Terraria.Dust.NewDust(Main.rand.NextVector2FromRectangle(target.Hitbox), 0, 0, DustID.Blood);

            // 设置目标的受击方向为远离玩家的方向，以确保击退效果的方向正确。
            hit.HitDirection = Main.player[Projectile.owner].Center.X < target.Center.X ? 1 : -1;

            var source = Projectile.GetSource_FromThis();
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<BloodExplosion>(), Projectile.damage * 2, Projectile.knockBack);
            target.AddBuff(BuffID.BloodButcherer, 120);

            Player player = Main.player[base.Projectile.owner];
            if (target.type != NPCID.TargetDummy && target.canGhostHeal && !player.moonLeech)
            {
                int healAmount = Main.rand.Next(2) + 3;
                player.statLife += healAmount;
                player.HealEffect(healAmount);
            }
        }
        // 从 Main.DrawProj_Excalibur() 方法中提取的代码
        // 查看其他剑类型的源代码。
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(1, 4);    // 定义源矩形，指定使用纹理的哪一帧
            Vector2 origin = sourceRectangle.Size() / 2f;
            float scale = Projectile.scale * 1.1f;
            SpriteEffects spriteEffects = !(Projectile.ai[0] >= 0f) ? SpriteEffects.FlipVertically : SpriteEffects.None;     // 根据投射物的方向设置精灵效果（是否垂直翻转）
            float percentageOfLife = Projectile.localAI[0] / Projectile.ai[1]; // 计算插值时间，用于平滑过渡效果
            float lerpTime = Utils.Remap(percentageOfLife, 0f, 0.6f, 0f, 1f) * Utils.Remap(percentageOfLife, 0.6f, 1f, 1f, 0f);
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            lightingColor = Utils.Remap(lightingColor, 0.2f, 1f, 0f, 1f);

            Color backDarkColor = new Color(150, 35, 15); // 原始圣剑颜色：Color(180, 160, 60)
            Color middleMediumColor = new Color(180, 20, 20); // 原始圣剑颜色：Color(255, 255, 80)
            Color frontLightColor = new Color(200, 15, 0); // 原始圣剑颜色：Color(255, 240, 150)

            Color whiteTimesLerpTime = Color.Red * lerpTime * 0.5f;
            whiteTimesLerpTime.A = (byte)(whiteTimesLerpTime.A * (1f - lightingColor));
            Color faintLightingColor = whiteTimesLerpTime * lightingColor * 0.5f;
            faintLightingColor.G = (byte)(faintLightingColor.G * lightingColor);
            faintLightingColor.B = (byte)(faintLightingColor.R * (0.25f + lightingColor * 0.75f));

            // 绘制圣剑的后部
            Main.EntitySpriteDraw(texture, position, sourceRectangle, backDarkColor * lightingColor * lerpTime, Projectile.rotation + Projectile.ai[0] * MathHelper.PiOver4 * -1f * (1f - percentageOfLife), origin, scale, spriteEffects, 0f);
            // 绘制受光照影响的非常微弱的部分
            Main.EntitySpriteDraw(texture, position, sourceRectangle, faintLightingColor * 0.15f, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, scale, spriteEffects, 0f);
            // 绘制圣剑的中部
            Main.EntitySpriteDraw(texture, position, sourceRectangle, middleMediumColor * lightingColor * lerpTime * 0.3f, Projectile.rotation, origin, scale, spriteEffects, 0f);
            // 绘制圣剑的前部
            Main.EntitySpriteDraw(texture, position, sourceRectangle, frontLightColor * lightingColor * lerpTime * 0.5f, Projectile.rotation, origin, scale * 0.975f, spriteEffects, 0f);
            // 绘制圣剑的细顶部线条（最后一帧）
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.6f * lerpTime, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, scale, spriteEffects, 0f);
            // 绘制圣剑的细中部线条（最后一帧）
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.5f * lerpTime, Projectile.rotation + Projectile.ai[0] * -0.05f, origin, scale * 0.8f, spriteEffects, 0f);
            // 绘制圣剑的细底部线条（最后一帧）
            Main.EntitySpriteDraw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.4f * lerpTime, Projectile.rotation + Projectile.ai[0] * -0.1f, origin, scale * 0.6f, spriteEffects, 0f);

            // 在挥动的圆周周围绘制一些火花
            for (float i = 0f; i < 8f; i += 1f)
            {
                float edgeRotation = Projectile.rotation + Projectile.ai[0] * i * (MathHelper.Pi * -2f) * 0.025f + Utils.Remap(percentageOfLife, 0f, 1f, 0f, MathHelper.PiOver4) * Projectile.ai[0];
                Vector2 drawPos = position + edgeRotation.ToRotationVector2() * (texture.Width * 0.5f - 6f) * scale;
                DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawPos, new Color(0, 255, 0, 0) * lerpTime * (i / 9f), middleMediumColor, percentageOfLife, 0f, 0.5f, 0.5f, 1f, edgeRotation, new Vector2(0f, Utils.Remap(percentageOfLife, 0f, 1f, 3f, 0f)) * scale, Vector2.One * scale);
            }

            // 在投射物的前部绘制一个大的星形火花
            Vector2 drawPos2 = position + (Projectile.rotation + Utils.Remap(percentageOfLife, 0f, 1f, 0f, MathHelper.PiOver4) * Projectile.ai[0]).ToRotationVector2() * (texture.Width * 0.5f - 4f) * scale;
            DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawPos2, new Color(0, 255, 0, 0) * lerpTime * 0.5f, middleMediumColor, percentageOfLife, 0f, 0.5f, 0.5f, 1f, 0f, new Vector2(2f, Utils.Remap(percentageOfLife, 0f, 1f, 4f, 1f)) * scale, Vector2.One * scale);

            // 取消注释以下行以可视化投射物的大小
            // Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, position, sourceRectangle, Color.Orange * 0.75f, 0f, origin, scale, spriteEffects);

            return false;
        }

        // 从 Main.DrawPrettyStarSparkle() 方法复制而来，该方法是私有的
        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawPos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
            Color bigColor = shineColor * opacity * 0.5f;
            bigColor.A = 0;
            Vector2 origin = sparkleTexture.Size() / 2f;
            Color smallColor = drawColor * 0.5f;
            float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
            Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
            bigColor *= lerpValue;
            smallColor *= lerpValue;
            // 绘制明亮的、较大的部分
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
            // 绘制暗淡的、较小的部分
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
        }
    }
}
