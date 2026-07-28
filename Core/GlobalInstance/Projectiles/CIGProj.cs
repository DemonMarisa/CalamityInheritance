using CalamityInheritance.Core.Utils;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.Core.GlobalInstance.Projectiles
{
    public class CIGProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool AMRextra = false;
        public bool Stealth = false;
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            if (Stealth)
                binaryWriter.Write(Stealth);
        }
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            if (Stealth)
                Stealth = binaryReader.ReadBoolean();
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (Stealth)
            {
                projectile.netUpdate = true;
                projectile.netSpam = 0;
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (AMRextra == true)
            {
                IEntitySource source = projectile.GetSource_FromThis();
                int extraProjectileAmt = 4;
                if (projectile.owner == Main.myPlayer)
                    for (int x = 0; x < extraProjectileAmt; x++)
                    {
                        bool fromRight = x > 2;
                        Projectile proj = CIUtils.ProjectileBarrage(source, projectile.Center, projectile.Center, fromRight, 500f, 500f, 0f, 500f, 10f, projectile.type, (int)(projectile.damage * 0.3f), projectile.knockBack, projectile.owner, false, 5f);
                    }

                AMRextra = false;
            }
        }
    }
}
