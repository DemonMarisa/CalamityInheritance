using System;
using System.Collections.Generic;
using System.Linq;
using CalamityMod.Items.Tools.ClimateChange;
using CalamityMod.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Utilities
{
    public static partial class CalamityInheritanceUtils
    {
        public static int SecondsToFrames(int seconds) => seconds * 60;
        public static int SecondsToFrames(float seconds) => (int)(seconds * 60);
    }
}
