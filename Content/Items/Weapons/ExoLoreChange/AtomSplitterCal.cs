using System.Collections.Generic;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class AtomSplitterCal : GlobalItem
    {
        private SoundStyle[] UseSoundAlt =
        [
            CISoundMenu.AtomToss1,
            CISoundMenu.AtomToss2,
            CISoundMenu.AtomToss3
        ];
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<TheAtomSplitter>();
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (player.CheckExoLore())
                damage.Base = 1457;
        }
        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            if (player.CheckExoLore())
                crit += 16f;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string path = $"{Generic.WeaponTextPath}Rogue.AtomChange";
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.WeaponTextPath}Rogue.AtomChange") : null;
            if (t != null) tooltips.InsertNewLineToFinalLineTest(Mod, path);
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.CIMod().LoreExo || player.CIMod().PanelsLoreExo)
                velocity *= 2.4f;
            base.ModifyShootStats(item, player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (player.CIMod().LoreExo || player.CIMod().PanelsLoreExo)
                item.UseSound = Utils.SelectRandom(Main.rand, UseSoundAlt);
            else
                item.UseSound = CISoundID.SoundWeaponSwing;
            return true;
        }
        //重写shoot属性
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool stealth = player.Calamity().StealthStrikeAvailable();
            if (player.CIMod().LoreExo || player.CIMod().PanelsLoreExo)
            {
                int s = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AtomProjRework>(), damage, knockback, player.whoAmI, -1f);
                Main.projectile[s].Calamity().stealthStrike = stealth;
            }
            else
            {
                int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, -1f);
                Main.projectile[p].Calamity().stealthStrike = stealth;
            }
            return false; 
        }
    }
}