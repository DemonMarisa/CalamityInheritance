using CalamityInheritance.Core.Utils;
using CalamityMod.NPCs;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

#pragma warning disable RS0030

namespace CalamityInheritance.Common.CalamityModCross.ILChange
{
    public class ChangePierceResistNPC : ModSystem
    {
        public static HashSet<int> NoPierceResist = [];
        public delegate void ModifyHitByProjectileDelegate(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers);
        public override void Load()
        {
            NoPierceResist = [];
            if (CIUtils.HasCalamity())
                HookCalModifyProj();
        }
        public override void Unload()
        {
            NoPierceResist = null;
        }

        [JITWhenModsEnabled("CalamityMod")]
        public static void HookCalModifyProj()
        {
            MethodInfo sd = typeof(PierceResistNPC).GetMethod(nameof(PierceResistNPC.ModifyHitByProjectile));
            MonoModHooks.Add(sd, ModifyHitByProjectile_Hook);
        }

        [JITWhenModsEnabled("CalamityMod")]
        public static void ModifyHitByProjectile_Hook(ModifyHitByProjectileDelegate orig, NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (NoPierceResist.Contains(projectile.type))
                return;
            orig(npc, projectile, ref modifiers);
        }
    }
}
