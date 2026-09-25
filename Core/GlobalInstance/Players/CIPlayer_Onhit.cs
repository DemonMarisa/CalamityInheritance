using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public Dictionary<int, int> HitAddBuff = new Dictionary<int, int>();
        public void HitReset()
        {
            HitAddBuff.Clear();
        }
        public void AddHitAddBuff(int type, int time)
        {
            if (HitAddBuff.ContainsKey(type))
            {
                if (HitAddBuff[type] < time)
                    HitAddBuff[type] = time;
            }
            else
            {
                HitAddBuff.Add(type, time);
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            ArmorModifyHitNPC(target, ref modifiers);
            ModifyHitNPC_Accessories(target, ref modifiers);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            PotionOnHit(target);
            ArmorOnHitNPC(target, hit, damageDone);
            OnHitNPC_Accessories(target, hit, damageDone);
            if (HitAddBuff.Count != 0)
            {
                foreach (int type in HitAddBuff.Keys)
                {
                    int time = HitAddBuff[type];
                    target.AddBuff(type, time);
                }
            }
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
