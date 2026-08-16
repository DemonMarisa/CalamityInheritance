using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            ArmorModifyHitNPC(target, ref modifiers);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            PotionOnHit(target);
            ArmorOnHitNPC(target, hit, damageDone);
            OnHitNPC_Accessories(target, hit, damageDone);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ArmorOnHitNPCWithProj(proj, target, hit, damageDone);
            OnHitNPCWithProj_Accessories(proj, target, hit, damageDone);
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ArmorOnHitNPCWithItem(item, target, hit, damageDone);
        }
    }
}
