using CalamityInheritance.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Animations;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon.Worms
{
    public class Segment(Vector2 pos, float rot)
    {
        public Vector2 Pos = pos;
        public float Rot = rot;
    }
    public class DOGworm : ModProjectile, ILocalizedModType
    {
        public override string Texture => CITextureRegistry.DOGworm_Body.Path;
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        public List<Segment> AllSegment = [];
        bool FireFrame = false;
        public enum AttackState : byte
        {
            PortalGateCharge,
            LaserCharge
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override void AI()
        {
            if (!FireFrame)
            {
                // 这个是必定添加一个尾巴
                Segment segment = new(Projectile.Center, Projectile.rotation);
                AllSegment.Add(segment);
                FireFrame = true;
            }
            if (AllSegment.Count != 0)
            {
                for (int i = 0; i < AllSegment.Count; i++)
                    UpdateSegment(i);
            }
            Projectile.timeLeft = 2;
        }
        public void UpdateSegment(int segmentIndex)
        {
            float aheadSegmentRotation = segmentIndex > 0 ? AllSegment[segmentIndex - 1].Rot : Projectile.rotation;
            Vector2 aheadSegmentCenter = segmentIndex > 0 ? AllSegment[segmentIndex - 1].Pos : Projectile.Center;
            Vector2 offsetToAheadSegment = aheadSegmentCenter - AllSegment[segmentIndex].Pos;
            if (aheadSegmentRotation != AllSegment[segmentIndex].Rot)
            {
                float offsetAngle = MathHelper.WrapAngle(aheadSegmentRotation - AllSegment[segmentIndex].Rot);
                offsetToAheadSegment = offsetToAheadSegment.RotatedBy(offsetAngle * 0.075f);
            }
            AllSegment[segmentIndex].Rot = offsetToAheadSegment.ToRotation();
            if (offsetToAheadSegment != Vector2.Zero)
                AllSegment[segmentIndex].Pos = aheadSegmentCenter - offsetToAheadSegment.SafeNormalize(Vector2.Zero) * 22;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tail = CITextureRegistry.DOGworm_Tail.Value;
            Vector2 TaildrawPos = AllSegment[^1].Pos - Main.screenPosition;
            Vector2 Tailorig = tail.Size() / 2;
            Main.spriteBatch.Draw(tail, TaildrawPos, null, Color.White, AllSegment[^1].Rot + MathHelper.PiOver2, Tailorig, 1f, 0, 0);
            for (int i = 0; i < AllSegment.Count - 1; i++)
            {
                Texture2D body = CITextureRegistry.DOGworm_Body.Value;
                Vector2 bodydrawPos = AllSegment[i].Pos - Main.screenPosition;
                Vector2 bodyorig = body.Size() / 2;
                Main.spriteBatch.Draw(body, bodydrawPos, null, Color.White, AllSegment[i].Rot + MathHelper.PiOver2, bodyorig, 1f, 0, 0);
            }
            Texture2D texture = CITextureRegistry.DOGworm_Head.Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 orig = texture.Size() / 2;
            Main.spriteBatch.Draw(texture, drawPos, null, Color.White, Projectile.rotation + MathHelper.PiOver2, orig, 1f, 0, 0);
            return true;
        }
    }
}
