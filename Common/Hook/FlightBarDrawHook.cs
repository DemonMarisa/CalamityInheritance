using CalamityInheritance.World;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.UI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.Hook
{
    public class FlightBarDrawHook
    {
        public static void Load(Mod mod)
        {
            MethodInfo originalMethod = typeof(FlightBar).GetMethod(nameof(FlightBar.GetApplicableBorder));
            MonoModHooks.Add(originalMethod, GetApplicableBorder_Hook);
        }

        private static Texture2D GetApplicableBorder_Hook(FlightBar self, CalamityPlayer modPlayer)
        {
            if (CIWorld.Defiled)
                return FlightBar.disabledBarTexture;
            if (modPlayer.Player.equippedWings != null && modPlayer.Player.wingTimeMax == 0 && modPlayer.Player.mount._data.flightTimeMax == 0)
                return FlightBar.disabledBarTexture;
            if ((modPlayer.infiniteFlight || FlightBar.RidingInfiniteFlightMount(modPlayer.Player)) && FlightBar.completedAnimation)
                return FlightBar.infiniteBarTexture;
            if (modPlayer.weakPetrification || modPlayer.vHex || modPlayer.icarusFolly || modPlayer.DoGExtremeGravity)
                return FlightBar.limitedBarTexture;
            return FlightBar.borderTexture;
        }
    }
}
