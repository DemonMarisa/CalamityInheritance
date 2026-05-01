using LAP.Content.Particles;
using LAP.Core.GlobalInstance.Players.DashSystem;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer.Dash
{
    public class StatisNinjaBeltDash : BasePlayerDash
    {
        public override int ImmuneTime(Player player) => 12;
        public override int DashTime(Player player) => 12;
        public override int DashDelay(Player player) => 12;
        public override DashDamageInfo DashDamageInfo(Player player) => new DashDamageInfo(0, 0, DamageClass.Melee);
        public override float DashSpeed(Player player) => 18f;
        public override float DashEndSpeedMult(Player player) => 0.33f;
        public int Time;
        public override bool CanHitNPC(Player player, NPC target)
        {
            return false;
        }
        public override void OnDashStart(Player player)
        {
        }
        public override void DuringDash(Player player)
        {
            for (int d = 0; d < 3; d++)
            {
                Vector2 pos = new (player.Center.X + Main.rand.Next(-4, 4), player.Center.Y + Main.rand.Next(-12, 12));
                new CampSmoke(pos, Vector2.Zero, Color.White, 45, Main.rand.NextFloat(MathHelper.TwoPi), 0.45f, 0.2f).Spawn();
            }
        }
    }
}
