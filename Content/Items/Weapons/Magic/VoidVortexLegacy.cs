using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class VoidVortexLegacy : CIMagic, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 60;
            Item.width = 130;
            Item.height = 130;
            Item.useTime = 41;
            Item.useAnimation = 41;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Climax2>();
            Item.shootSpeed = 12f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float pSpeed = Item.shootSpeed;
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float mouseX = Main.mouseX + Main.screenPosition.X - vector2.X;
            float mouseY = Main.mouseY + Main.screenPosition.Y - vector2.Y;
            if (player.gravDir == -1f)
            {
                mouseY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - vector2.Y;
            }
            float dist = CIFunction.TryGetVectorMud(mouseX, mouseY);
            if ((float.IsNaN(mouseX) && float.IsNaN(mouseY)) || (mouseX == 0f && mouseY == 0f))
            {
                mouseX = player.direction;
                mouseY = 0f;
            }
            else
            {
                dist = pSpeed / dist;
            }
            vector2 += new Vector2(mouseX, mouseY);
            float spread = 45f * 0.0174f;
            double startAngle = Math.Atan2(velocity.Y, velocity.X) - spread / 2;
            double deltaAngle = spread / 8f;
            double offsetAngle;
            int i;
            float passedVar = 1f;
            for (i = 0; i < 4; i++)
            {
                offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                Vector2 perturbedVelocity1 = new Vector2((float)(Math.Cos(offsetAngle) * velocity.Length()),(float)(Math.Sin(offsetAngle) * velocity.Length()));
                Vector2 perturbedVelocity2 = new Vector2((float)(-Math.Cos(offsetAngle) * velocity.Length()),(float)(-Math.Sin(offsetAngle) * velocity.Length()));
                Projectile.NewProjectile(source , vector2, perturbedVelocity1, type, damage, (int)knockback, player.whoAmI, passedVar, 0f);
                Projectile.NewProjectile(source , vector2, perturbedVelocity2, type, damage, (int)knockback, player.whoAmI, -passedVar, 0f);
                passedVar += 1f;
            }
            return false;
        }
        public override void AddRecipes()
        {
            if (CIServerConfig.Instance.LegendaryitemsRecipes == true)
            {
                CreateRecipe()
                    .AddIngredient<VoltaicClimax>()
                    .AddIngredient<AuricBar>(5)
                    .AddTile(ModContent.TileType<CosmicAnvil>())
                    .Register();

                CreateRecipe()
                    .AddIngredient<VoltaicClimax>()
                    .AddIngredient<AuricBarold>()
                    .AddTile(ModContent.TileType<CosmicAnvil>())
                    .Register();
            }
        }
    }
}
