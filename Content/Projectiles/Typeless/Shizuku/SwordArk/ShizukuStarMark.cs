using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
{
    public class ShizukuStarMark : ModProjectile, ILocalizedModType
    {
        public override string Texture => GenericProjRoute.InvisProjRoute;
        public Player Owner => Projectile.GetProjOwner();
        public NPC Target => Main.npc[(int)Projectile.ai[0]];
        public ref float TheAttackTimer => ref Projectile.ai[1];
        public override void SetDefaults()
        {
            //这玩意只是个隐形射弹。
            Projectile.width = Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.Opacity = 0f;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (Owner.channel && Owner.ActiveItem().type == ModContent.ItemType<ShizukuSword>() && !Owner.noItems && !Owner.CCed)
                Projectile.timeLeft = 2;
             
            //缓动
            Projectile.rotation += MathHelper.ToRadians(1);
            Projectile.Opacity = MathHelper.Clamp(Projectile.Opacity + 0.04f, 0f, 1f);
            //以世界插值
            // 远离鼠标时，直接插值追踪鼠标（位置插值）
            Projectile.Center = Vector2.Lerp(Projectile.Center, Target.Center, 0.5f);
            Projectile.scale = Projectile.Opacity;
            //有mark的情况下才会发射飞剑
            TheAttackTimer += 1;
            if (TheAttackTimer > 30f)
            {
                float rad = 120f;
                int count = 4;
                float startAngle = -MathHelper.PiOver4;
                float endAngle = MathHelper.PiOver4;
                float angleStep = (endAngle - startAngle) / (count - 1);
                for (int i = 0; i < count; i++)
                {
                    float curAngle = startAngle + angleStep * i;
                    Vector2 targetPos = Owner.Center + curAngle.ToRotationVector2() * rad;
                    Vector2 direction = (targetPos - Owner.Center).SafeNormalize(Vector2.UnitX);
                    Vector2 velocity = 12f * direction;
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Owner.Center, velocity, ModContent.ProjectileType<ShizukuDagger>(), Projectile.originalDamage, 12f, Owner.whoAmI, ai1: Target.whoAmI);
                    proj.timeLeft = 240;
                }
                TheAttackTimer = 0;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mark = ModContent.Request<Texture2D>($"CalamityInheritance/Content/Projectiles/Typeless/Shizuku/SwordArk/{GetType().Name}").Value;
            float scale = 2f - Projectile.scale;
            scale /= 3;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color lerpColor = Color.Lerp(Color.Aqua, Color.GhostWhite, 0.2f);
            Main.spriteBatch.Draw(mark, drawPos, null, lerpColor * Projectile.Opacity, Projectile.rotation, mark.Size() / 2, scale, SpriteEffects.None, 0);
            return false;
        }
    }
}