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
using CalamityMod.CalPlayer.Dashes;
using CalamityInheritance.CIPlayer.Dash;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace CalamityInheritance
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class CalamityInheritance : Mod
    {
        internal static CalamityInheritance Instance;
        public override void Load()
        {
            Instance = this;
            CIPlayerDashManager.Load();
            CalamityInheritanceLists.LoadLists();
        }
        #region Unload
        public override void Unload()
        {
            CIPlayerDashManager.Unload();
            CalamityInheritanceLists.UnloadLists();
            Instance = null;
            base.Unload();
        }
        #endregion
    }
}
