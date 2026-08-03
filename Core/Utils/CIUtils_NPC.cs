using CalamityInheritance.Core.GlobalInstance.NPCs;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static bool Organic(this NPC target)
        {
            if ((target.HitSound != SoundID.NPCHit4 && target.HitSound != SoundID.NPCHit41 && target.HitSound != SoundID.NPCHit2 && target.HitSound != SoundID.NPCHit5 && target.HitSound != SoundID.NPCHit11 && target.HitSound != SoundID.NPCHit30 && target.HitSound != SoundID.NPCHit34 && target.HitSound != SoundID.NPCHit36 && target.HitSound != SoundID.NPCHit42 && target.HitSound != SoundID.NPCHit49 && target.HitSound != SoundID.NPCHit52 && target.HitSound != SoundID.NPCHit53 && target.HitSound != SoundID.NPCHit54 && target.HitSound.HasValue))
            {
                return true;
            }

            return false;
        }
        public static void AddDebuffDamage(this NPC target, int damage)
        {
            target.CI().DeBuffDamage += damage;
        }
    }
}
