using System.Collections.Generic;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class ChainsawCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ModContent.ItemType<PhotonRipper>();
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float breakBlocks = 1;
            if (player.Calamity().mouseRight && player.whoAmI == Main.myPlayer && !Main.mapFullscreen && !Main.blockMouse)
            {
                breakBlocks = 0;
            }
            int pType = type;
            if (player.CIMod().LoreExo || player.CIMod().PanelsLoreExo)
                pType = ModContent.ProjectileType<ExoChainsawProj>();
            Projectile.NewProjectile(source, position, velocity, pType, damage, knockback, player.whoAmI, 0f, 0f, breakBlocks);
            return false; 
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.WeaponTextPath}.Melee.ChainsawCal") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
    }
}