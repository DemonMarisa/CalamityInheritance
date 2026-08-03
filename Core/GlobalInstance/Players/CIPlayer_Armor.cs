using CalamityInheritance.Content.Items.Armor.ArmorBonus;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public bool victideSet = false;
        public bool victideSummon = false;
        public bool DesertProwler = false;
        public bool ReaverRogueSet = false;
        public bool ReaverSummoner = false;
        public bool ReaverMelee = false;
        public bool ReaverMagic = false;
        public bool ReaverRanged = false;
        public void ResetArmor()
        {
            victideSet = false;
            victideSummon = false;
            DesertProwler = false;
            ReaverRogueSet = false;
            ReaverSummoner = false;
            ReaverMelee = false;
            ReaverMagic = false;
        }
        public void ArmorOnHitNPC(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.DamageType.CountsAsClass(DamageClass.Ranged))
            {
                DesertProwlerBonus.DesertProwlerArmorBonus_OnHitNPCProj(this, Player, proj, target, hit, damageDone);
            }
        }
    }
}
