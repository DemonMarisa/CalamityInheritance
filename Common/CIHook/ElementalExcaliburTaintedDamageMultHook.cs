using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityMod;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.Projectiles.Melee;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CIHook
{
    public static class ElementalExcaliburTaintedDamageMultHook
    {
        public static void Load()
        {
            MethodInfo originalMethod = typeof(TaintedBladeSlasher).GetMethod(nameof(TaintedBladeSlasher.OnHitNPC));
            MonoModHooks.Add(originalMethod, OnHitNPC_Hook);
        }

        public static void OnHitNPC_Hook(TaintedBladeSlasher self, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(self.Owner.ActiveItem() == Main.item[ModContent.ItemType<ElementalExcalibur>()])
            {
                hit.SourceDamage *= 2;
                ItemLoader.OnHitNPC(self.Owner.ActiveItem(), self.Owner, target, hit, damageDone * 2);
                NPCLoader.OnHitByItem(target, self.Owner, self.Owner.ActiveItem(), hit, damageDone * 2);
                PlayerLoader.OnHitNPC(self.Owner, target, hit, damageDone * 2);
            }
            else
            {
                ItemLoader.OnHitNPC(self.Owner.ActiveItem(), self.Owner, target, hit, damageDone);
                NPCLoader.OnHitByItem(target, self.Owner, self.Owner.ActiveItem(), hit, damageDone);
                PlayerLoader.OnHitNPC(self.Owner, target, hit, damageDone);
            }
        }

    }
}
