using System;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
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
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Rogue";
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
            int knifeAmt = 4;
            for (int i = 2; i < 9; i += 2)
            {
                if (Main.rand.NextBool(i))
                    knifeAmt++;
            }
            bool stealth = player.CheckStealth();
            int pType = stealth ? ModContent.ProjectileType<RogueTypeKnivesShadowspecProjClone>() : type;
            for (int i = 0; i < knifeAmt; i++)
            {
                float spreadX = Main.rand.Next(-35, 36) * 0.05f * i;
                float spreadY = Main.rand.Next(-35, 36) * 0.05f * i;
                Vector2 tarPos = new (spreadX, spreadY);
                Vector2 distVec = velocity + tarPos;
                float tarDist = distVec.Length();
                tarDist = Item.shootSpeed / tarDist;
                tarPos.X *= tarDist;
                tarPos.Y *= tarDist;
                int p =Projectile.NewProjectile(source, position, distVec, pType, damage, knockback, Main.myPlayer); 
                Main.projectile[p].Calamity().stealthStrike = stealth;
            }
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
