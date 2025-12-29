using System.Collections.Generic;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class ChainWeapon : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemType<TheJailor>();
        public override bool AltFunctionUse(Item item, Player player) => player.CIMod().LoreExo || player.CIMod().PanelsLoreExo;
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CheckExoLore() ? Language.GetTextValue($"{Generic.WeaponTextPath}Ranged.ChainWeaponChange") : null;
            if (t != null)
                tooltips.Add(new TooltipLine(Mod, "Name", t));

        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //复写发射方式。
            if (player.altFunctionUse != 2)
            {
                Vector2 shootVelocity = velocity;
                Vector2 shootDirection = shootVelocity.SafeNormalize(Vector2.UnitX * player.direction);
                Vector2 gunTip = position + shootDirection * item.scale * 45f;
                Projectile.NewProjectile(source, gunTip, shootVelocity, item.shoot, damage, knockback, player.whoAmI);
            }
            if (player.altFunctionUse == 2 && player.CheckExoLore())
            {
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, item.shoot, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}