using System;
using System.Linq;
using CalamityInheritance.Utilities;
using CalamityMod.Buffs.Alcohol;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.Placeables;
using CalamityMod.Cooldowns;
using CalamityMod.Enums;
using CalamityMod.Items.Accessories;
using CalamityMod.NPCs;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Systems;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities.Terraria.Utilities;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region Update Bad Life Regen
        public override void UpdateBadLifeRegen()
        {
            CalamityInheritancePlayer modPlayer = Player.CalamityInheritance();

        }
        #endregion
    }
}
