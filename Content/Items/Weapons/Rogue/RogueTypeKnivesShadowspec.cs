using System;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class RogueTypeKnivesShadowspec : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 62;
            Item.damage = 1200;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 8;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item39;
            Item.autoReuse = true;

            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.Calamity().devItem = true;

            Item.shoot = ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>();
            Item.shootSpeed = 9f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // if(!player.Calamity().StealthStrikeAvailable())
            // {
                float knifeSpeed = Item.shootSpeed;
                Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
                float mouseXDist = Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
                float mouseYDist = Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                if (player.gravDir == -1f)
                {
                    mouseYDist = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - realPlayerPos.Y;
                }
                float mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                if ((float.IsNaN(mouseXDist) && float.IsNaN(mouseYDist)) || (mouseXDist == 0f && mouseYDist == 0f))
                {
                    mouseXDist = player.direction;
                    mouseYDist = 0f;
                    mouseDistance = knifeSpeed;
                }
                else
                {
                    mouseDistance = knifeSpeed / mouseDistance;
                }
                mouseXDist *= mouseDistance;
                mouseYDist *= mouseDistance;
                int knifeAmt = 4;
                if (Main.rand.NextBool())
                {
                    knifeAmt++;
                }
                if (Main.rand.NextBool(4))
                {
                    knifeAmt++;
                }
                if (Main.rand.NextBool(6))
                {
                    knifeAmt++;
                }
                if (Main.rand.NextBool(8))
                {
                    knifeAmt++;
                }
                for (int i = 0; i < knifeAmt; i++)
                {
                    float knifeSpawnXPos = mouseXDist;
                    float knifeSpawnYPos = mouseYDist;
                    float randOffsetDampener = 0.05f * i;
                    knifeSpawnXPos += Main.rand.Next(-35, 36) * randOffsetDampener;
                    knifeSpawnYPos += Main.rand.Next(-35, 36) * randOffsetDampener;
                    mouseDistance = (float)Math.Sqrt((double)(knifeSpawnXPos * knifeSpawnXPos + knifeSpawnYPos * knifeSpawnYPos));
                    mouseDistance = knifeSpeed / mouseDistance;
                    knifeSpawnXPos *= mouseDistance;
                    knifeSpawnYPos *= mouseDistance;
                    float x4 = realPlayerPos.X;
                    float y4 = realPlayerPos.Y;
                    Projectile.NewProjectile(source, x4, y4, knifeSpawnXPos, knifeSpawnYPos, type, damage, knockback, player.whoAmI, 0f, 0f);
                }
            // }
            // else
            // {
            //     int newType = ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>();
            //     int stealth = Projectile.NewProjectile(source, position, velocity * 2, newType, damage, knockback, Main.myPlayer, 0f, 0f, 0f);
            //      if(stealth.WithinBounds(Main.maxProjectiles))
            //         Main.projectile[stealth].Calamity().stealthStrike = true;
            // }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RogueTypeKnivesEmpyrean>().
                AddIngredient<CoreofCalamity>(2).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();
            
            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                Register();
        }
    }
}
