using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Projectiles
{
    public abstract class BaseStickyProj : ModProjectile, ILocalizedModType
    {
        public bool isSticky;
        public Vector2 RelativePos;
        public int Target = -1;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(isSticky);
            writer.WriteVector2(RelativePos);
            writer.Write(Projectile.ai[0]);
            ExSend(writer);
        }
        public virtual void ExSend(BinaryWriter writer)
        {

        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            isSticky = reader.ReadBoolean();
            RelativePos = reader.ReadVector2();
            Projectile.ai[0] = reader.ReadSingle();
            ExRead(reader);
        }
        public virtual void ExRead(BinaryReader reader)
        {

        }
        public override bool ShouldUpdatePosition()
        {
            return !isSticky;
        }
        public override void AI()
        {
            if (isSticky)
            {
                if (Target >= 0 && Target < Main.maxNPCs)
                {
                    NPC npc = Main.npc[Target];
                    Projectile.Center = npc.Center + RelativePos;
                }
            }
            ExAI();
        }
        public virtual void ExAI()
        {

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Target = target.whoAmI;
            isSticky = true;
            RelativePos = Projectile.Center - target.Center;
            Projectile.netUpdate = true;
            Projectile.netSpam = 0;
            ExOnHit(target, hit, damageDone);
        }
        public virtual void ExOnHit(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
    }
}
