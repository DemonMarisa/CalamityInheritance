using CalamityMod.CalPlayer;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Items;
using CalamityMod.Particles;
using CalamityMod.Projectiles;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.GameContent.Animations.IL_Actions.NPCs;
using Terraria.Graphics.Renderers;

namespace CalamityInheritance
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class CalamityInheritance : Mod
    {

        public override void Load()
        {
            CalamityInheritanceLists.LoadLists();
        }
    }
}
