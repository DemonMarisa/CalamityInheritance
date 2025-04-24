using System;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    [LegacyName("EmpyreanKnivesLegacyRogue")]
    public class RogueTypeKnivesEmpyrean: RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
            Item.damage = 400;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10; //使用时间15->8, 面板伤害360->200
            //使用时间8-10，伤害200-400
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item39;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.shoot = ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>();
            Item.shootSpeed = 15f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
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
            for (int j = 2; j <= 16; j *= 2)
            {
                if (Main.rand.NextBool(j))
                    knifeAmt++;
            }
            for (int i = 0; i < knifeAmt; i++)
            {
                float knifeSpawnXPos = Main.MouseWorld.X;
                float knifeSpawnYPos = Main.MouseWorld.Y;
                float randOffsetDampener = 0.05f * i;
                knifeSpawnXPos += Main.rand.Next(-25, 26) * randOffsetDampener;
                knifeSpawnYPos += Main.rand.Next(-25, 26) * randOffsetDampener;
                mouseDistance = (float)Math.Sqrt((double)(knifeSpawnXPos * knifeSpawnXPos + knifeSpawnYPos * knifeSpawnYPos));
                mouseDistance = knifeSpeed / mouseDistance;
                knifeSpawnXPos *= mouseDistance;
                knifeSpawnYPos *= mouseDistance;
                float x4 = realPlayerPos.X;
                float y4 = realPlayerPos.Y;
                if (!player.Calamity().StealthStrikeAvailable())
                    Projectile.NewProjectile(source, x4, y4, knifeSpawnXPos, knifeSpawnYPos, type, damage, knockback, player.whoAmI, 0f, 0f);
                else
                    Projectile.NewProjectile(source, x4, y4, knifeSpawnXPos, knifeSpawnYPos, ModContent.ProjectileType<RogueTypeKnivesEmpyreanProjClone>(), damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.VampireKnives).
                AddIngredient<MonstrousKnives>().
                AddIngredient<CosmiliteBar>(8).
                AddIngredient<DarksunFragment>(8).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
