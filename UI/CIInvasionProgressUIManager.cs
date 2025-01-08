﻿using CalamityMod.UI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Core;

namespace CalamityInheritance.UI
{
    // TODO -- This can be made into a ModSystem with simple OnModLoad and Unload hooks.
    public static class CIInvasionProgressUIManager
    {
        private static readonly List<CIInvasionProgressUI> gUIs = new List<CIInvasionProgressUI>();
        public static int TotalGUIsActive => gUIs.Count(gui => gui.IsActive);
        public static bool AnyGUIsActive => TotalGUIsActive > 0;
        public static CIInvasionProgressUI GetActiveGUI => gUIs.FirstOrDefault(gui => gui.IsActive);
        public static void UpdateAndDraw(SpriteBatch spriteBatch)
        {
            if (AnyGUIsActive)
            {
                if (GetActiveGUI is null)
                    return;
                GetActiveGUI.Draw(spriteBatch);
            }
        }

        public static void LoadGUIs()
        {
            // Look through every type in the mod, and check if it's derived from InvasionProgressUI. If it is, create a copy and save it in the static list.
            Type[] types = AssemblyManager.GetLoadableTypes(CalamityInheritance.Instance.Code);
            foreach (Type type in types)
            {
                // Don't load abstract classes since they cannot have instances.
                if (type.IsAbstract)
                    continue;
                if (type.IsSubclassOf(typeof(CIInvasionProgressUI)))
                    gUIs.Add(Activator.CreateInstance(type) as CIInvasionProgressUI);
            }
        }
        public static void UnloadGUIs() => gUIs.Clear();
    }
}
