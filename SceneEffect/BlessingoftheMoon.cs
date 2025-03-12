using CalamityMod.CalPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.BiomeManagers;
using CalamityMod.Systems;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System;

namespace CalamityInheritance.SceneEffect
{
    public class BlessingoftheMoon : ModSceneEffect
    {

        public override SceneEffectPriority Priority
        {
            get
            {
                return SceneEffectPriority.Environment;
            }
        }
        public override int Music =>(new int?(MusicLoader.GetMusicSlot(Mod,"Music/BlessingoftheMoon"))).Value;
        public override bool IsSceneEffectActive(Player player)
        {
            return (double)(Main.LocalPlayer.position.Y / 16f) <= Main.worldSurface * 0.35 && PlanetoidsCounts.Planetoids && CIConfig.Instance.BlessingoftheMoon;
        }
    }
}
