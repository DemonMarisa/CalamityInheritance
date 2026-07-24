using CalamityInheritance.Content.BaseClass.Buff;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Buff.DamageBuffs
{
    public class CIBrimstoneFlames : CIDamageBuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.CrimsonSpray);
                dust.noGravity = true;
                dust.velocity = new Vector2(0, Main.rand.NextFloat(-3f, -5f)) + npc.velocity;
                dust.scale = 1.6f;
                for (int i = 0; i < 3; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(npc.position + new Vector2(Main.rand.NextFloat(-10f, 10f), npc.height / 2), npc.width, npc.height, DustID.GemRuby);
                    dust2.noGravity = true;
                    dust2.velocity = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-1f, -3f)) + npc.velocity;
                    dust2.scale = 1.4f;
                }
                Lighting.AddLight(npc.position, 0.05f, 0.01f, 0.01f);
            }
        }
    }
}
