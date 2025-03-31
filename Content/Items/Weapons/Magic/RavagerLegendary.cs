using System;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class RavagerLegendary: CIMagic, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 62;
            Item.damage = 100;
            Item.mana = 5;
            Item.DamageType = DamageClass.Magic;
            Item.useAnimation = Item.useTime = 11;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item88;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<RavagerLegendaryProjAlt>();

            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2)
                mult *= 1.5f;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            //升级的Tooltip:
            if (mp.BetsyTier1)
            {
                string t1 = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Magic.RavagerLegendary.TierOne");
                tooltips.Add(new TooltipLine(Mod, "TIERONE", t1));
            }
            if (mp.BetsyTier2)
            {
                string t2 = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Magic.RavagerLegendary.TierTwo");
                tooltips.Add(new TooltipLine(Mod, "TIERTWO", t2));
            }
            if (mp.BetsyTier1)
            {
                string t3 = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Magic.RavagerLegendary.TierThree");
                tooltips.Add(new TooltipLine(Mod, "TIERTHREE", t3));
            }
            //以下，用于比较复杂的计算
            // int boostPercent = (int)(LegendaryDamage() * 100);
            // string update = this.GetLocalization("LegendaryScaling").Format(
            //     boostPercent.ToString()
            // );
            // tooltips.FindAndReplace("[SCALING]", update);
        }
        public override float UseSpeedMultiplier(Player player)
        {
            if (player.altFunctionUse != 2)
                return 1.2f;
            return 1.50f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var usPlayer = player.CIMod();
            if (player.altFunctionUse == 2)
            {
                int meteorAmt = Main.rand.Next(4, 6);
                //T2样式加强：左键与右键多2颗陨石
                if (usPlayer.BetsyTier2)
                    meteorAmt += 2;
                for (int i = 0; i < meteorAmt; ++i)
                {
                    float SpeedX = velocity.X + Main.rand.Next(-30, 31) * 0.05f;
                    float SpeedY = velocity.Y + Main.rand.Next(-30, 31) * 0.05f;
                    float ai0 = Main.rand.Next(6);
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, ai0, 0.5f + (float)Main.rand.NextDouble() * 0.9f);
                }
                return false;
            }
            else
            {
                float meteorSpeed = Item.shootSpeed;
                Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
                float mouseXDist = Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
                float mouseYDist = Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                if (player.gravDir == -1f)
                {
                    mouseYDist = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - realPlayerPos.Y;
                }
                float mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                if (float.IsNaN(mouseXDist) && float.IsNaN(mouseYDist) || mouseXDist == 0f && mouseYDist == 0f)
                {
                    mouseXDist = player.direction;
                    mouseYDist = 0f;
                    mouseDistance = meteorSpeed;
                }
                else
                {
                    mouseDistance = meteorSpeed / mouseDistance;
                }
                int pAmt = 4;
                if (usPlayer.BetsyTier2)
                    pAmt = 6;
                for (int i = 0; i < pAmt; i++)
                {
                    realPlayerPos = new Vector2(player.position.X + player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                    realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                    realPlayerPos.Y -= 100 * i;
                    mouseXDist = Main.mouseX + Main.screenPosition.X - realPlayerPos.X + Main.rand.Next(-40, 41) * 0.03f;
                    mouseYDist = Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                    if (mouseYDist < 0f)
                    {
                        mouseYDist *= -1f;
                    }
                    if (mouseYDist < 20f)
                    {
                        mouseYDist = 20f;
                    }
                    mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                    mouseDistance = meteorSpeed / mouseDistance;
                    mouseXDist *= mouseDistance;
                    mouseYDist *= mouseDistance;
                    float meteorSpawnXOffset = mouseXDist;
                    float meteorSpawnYOffset = mouseYDist + Main.rand.Next(-40, 41) * 0.02f;
                    float ai0 = Main.rand.Next(6);
                    Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, meteorSpawnXOffset * 0.75f, meteorSpawnYOffset * 0.75f, type, damage, knockback, player.whoAmI, ai0, 0.5f + (float)Main.rand.NextDouble() * 0.9f); //0.3
                }
                return false;
            }
        }
    }
}
