using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.HitEffect;
using CalamityInheritance.Content.Particles;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using LAP.Content.Particles;
using LAP.Core.IDSets;
using LAP.Core.Presets.Content;
using LAP.Core.StateMachine.SynedHitEffect;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Professional
{
    public class ElementalQuiver : CIAccessories
    {
        public override int AccessoriesStyle => Professional;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 30;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().ElemQuiver = true;
            player.GetDamage(DamageClass.Ranged) += 0.25f;
            player.GetCritChance(DamageClass.Ranged) += 20;
            player.ammoCost80 = true;
            player.lifeRegen += 4;
            player.pickSpeed -= 0.100f;
            player.magicQuiver = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.keyState.IsKeyDown(Keys.LeftShift) && Main.mouseRight && Main.mouseRightRelease)
            {
                Player owner = Main.LocalPlayer;
                Main.LocalPlayer.CI().ElemQuiverSpiltStyle++;
                if (Main.LocalPlayer.CI().ElemQuiverSpiltStyle > 3)
                    Main.LocalPlayer.CI().ElemQuiverSpiltStyle = 0;
                HitEffectManager.SpawnHitEffect(HitEffectManager.HEType<MiscTransEffect>(), owner.whoAmI, owner.GetSource_FromThis(), owner.Center, Vector2.Zero);
            }
        }
        public static void ProjSpilt(Projectile projectile)
        {
            if (!projectile.CI().CanSplit || projectile.owner != Main.myPlayer)
                return;
            if (LAPIDSet.HeldProj.Contains(projectile.type) || projectile.minion || !projectile.friendly || projectile.hostile || projectile.damage < 5)
            {
                projectile.CI().CanSplit = false;
                return;
            }
            if (projectile.DamageType != DamageClass.Ranged)
            {
                projectile.CI().CanSplit = false;
                return;
            }
            if (projectile.whoAmI == projectile.Owner().heldProj)
            {
                projectile.CI().CanSplit = false;
                return;
            }
            bool canSpilt = false;
            if (projectile.Owner().CI().ElemQuiverSpiltStyle == 1 || projectile.Owner().CI().ElemQuiverSpiltStyle == 2)
            {
                if (Main.player[projectile.owner].miscCounter % 60 == 0 && LAPUtilities.FinalExtraUpdate(projectile))
                    canSpilt = true;
            }
            else
            {
                int a = (int)(50 * MathF.Sqrt(projectile.MaxUpdates));
                if (Main.rand.NextBool(a))
                    canSpilt = true;
            }
            if (!canSpilt)
                return;
            float spread = 180f * 0.0174f;
            double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (double)(spread / 2f);
            if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200)
            {
                int projectile2 = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(startAngle) * 8.0), (float)(Math.Cos(startAngle) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                int projectile3 = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, (float)((double)(0f - (float)Math.Sin(startAngle)) * 8.0), (float)((double)(0f - (float)Math.Cos(startAngle)) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                Main.projectile[projectile2].DamageType = DamageClass.Default;
                Main.projectile[projectile3].DamageType = DamageClass.Default;
                Main.projectile[projectile2].noDropItem = true;
                Main.projectile[projectile3].noDropItem = true;
                Main.projectile[projectile2].CI().CanSplit = false;
                Main.projectile[projectile3].CI().CanSplit = false;
                if (projectile.Owner().CI().ElemQuiverSpiltStyle == 1 || projectile.Owner().CI().ElemQuiverSpiltStyle == 3)
                {
                    Main.projectile[projectile2].timeLeft = 60;
                    Main.projectile[projectile3].timeLeft = 60;
                }
            }
        }
    }
}
