using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer.Dash;
using System.Reflection;
using CalamityInheritance.UI;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Texture;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Common;
using CalamityMod.Items.Accessories;

namespace CalamityInheritance
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class CalamityInheritance : Mod
    {
        internal static CalamityInheritance Instance;

        public static readonly BindingFlags UniversalBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        internal Mod infernumMode = null;

        public override void Load()
        {
            Instance = this;

            CIPlayerDashManager.Load();
            CalamityInheritanceLists.LoadLists();

            if (!Main.dedServ)
            {
                LoadClient();
            }

            infernumMode = null;
            ModLoader.TryGetMod("InfernumMode", out infernumMode);

            if (CalamityLists.pierceResistExceptionList != null)
            {
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<MurasamaSlashnew1>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<MurasamaSlashold>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<ExoArrowTealExoLore>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<DragonBowFlame>());
            }
            CalamityInheritanceTexture.LoadTexture();

            #region Hook
            CalamityInheritanceDashHook.Load(this);
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
            infernumMode = null;
            Instance = null;
            base.Unload();
        }
        #endregion
    }
}
