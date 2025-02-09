using CalamityMod.CalPlayer;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using static Terraria.GameContent.Animations.IL_Actions.NPCs;
using Terraria.Graphics.Renderers;
using CalamityMod.CalPlayer.Dashes;
using CalamityInheritance.CIPlayer.Dash;
using MonoMod.RuntimeDetour;
using System.Reflection;
using CalamityInheritance.UI;
using CalamityMod.UI;
using CalamityMod.Rarities;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Texture;
using CalamityModMusic.Items.Placeables;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.CIPlayer;
using System;
using System.Linq;
using Terraria.GameContent.Drawing;

namespace CalamityInheritance
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class CalamityInheritance : Mod
    {
        internal static CalamityInheritance Instance;

        public static readonly BindingFlags UniversalBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public override void Load()
        {
            Instance = this;

            CIPlayerDashManager.Load();
            CalamityInheritanceLists.LoadLists();

            if (!Main.dedServ)
            {
                LoadClient();
            }

            if (CalamityLists.pierceResistExceptionList != null)
            {
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<MurasamaSlashnew1>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<MurasamaSlashold>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<ExoArrowTealExoLore>());
            }
            CalamityInheritanceTexture.LoadTexture();
            #region Hook
            MethodInfo originalMethod = typeof(CalamityPlayer).GetMethod("ModDashMovement",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            MethodInfo hookMethod = typeof(CalamityInheritancePlayer).GetMethod("ModDashMovement_Hook",BindingFlags.Static | BindingFlags.Public);

            new Hook(originalMethod, hookMethod).Apply();
            #endregion
        }
        public void LoadClient()
        {
            AstralArcanumUI.Load(this);
        }

        #region Unload
        public override void Unload()
        {
            CIPlayerDashManager.Unload();
            AstralArcanumUI.Unload();
            CalamityInheritanceLists.UnloadLists();
            if (CalamityLists.pierceResistExceptionList != null)
            {
                CalamityLists.pierceResistExceptionList.Remove(ModContent.ProjectileType<MurasamaSlashnew1>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<MurasamaSlashold>());
            }
            CalamityInheritanceTexture.UnloadTexture();
            Instance = null;
            base.Unload();
        }
        #endregion
    }
}
