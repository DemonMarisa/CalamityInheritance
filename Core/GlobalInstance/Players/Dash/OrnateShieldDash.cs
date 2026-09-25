using LAP.Core.GlobalInstance.Players.DashSystem;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players.Dash
{
    public class OrnateShieldDash : BasePlayerDash
    {
        public override int ImmuneTime(Player player) => 12;
        public override int DashTime(Player player) => 12;
        public override int DashDelay(Player player) => 22;
        public override DashDamageInfo DashDamageInfo(Player player) => new DashDamageInfo(100, 0, DamageClass.Generic);
        public override float DashSpeed(Player player) => 18.5f;
        public override float DashEndSpeedMult(Player player) => 0.5f;

        public override void DuringDash(Player player)
        {
            for (int d = 0; d < 3; d++)
            {
                Dust iceDashDust = Dust.NewDustPerfect(player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-15f, 15f)) - player.velocity * 1.7f, Main.rand.NextBool(4) ? 223 : 180, -player.velocity.RotatedByRandom(MathHelper.ToRadians(10f)) * Main.rand.NextFloat(0.1f, 0.8f), 0, default, Main.rand.NextFloat(0.6f, 0.8f));
                iceDashDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                iceDashDust.noGravity = true;
                iceDashDust.fadeIn = 0.5f;
                if (iceDashDust.type == 180)
                    iceDashDust.scale = Main.rand.NextFloat(1.6f, 2.2f);
            }
        }
        public override void OnHitNPC(Player player, NPC target, int DamageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 180);
        }
    }
}
