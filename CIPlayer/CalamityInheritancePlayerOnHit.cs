using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs;
using Terraria.ID;
using CalamityMod.Buffs.StatDebuffs;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region On Hit NPC With Proj
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (Player.whoAmI != Main.myPlayer)
                return;

            CalamityGlobalNPC cgn = target.Calamity();

            if (perforatorLore)
            {
                target.AddBuff(BuffID.Ichor, 90);
            }
            if (hiveMindLore)
            {
                target.AddBuff(BuffID.CursedInferno, 90);
            }

        }
        #endregion

        #region Debuffs
        public void NPCDebuffs(NPC target, bool melee, bool ranged, bool magic, bool summon, bool rogue, bool whip, bool proj = false, bool noFlask = false)
        {
            if ((melee || rogue || whip) && !noFlask)
            {
                if (armorShattering)
                {
                    CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<Crumbling>());
                }
            }
        }
        #endregion
    }
}
