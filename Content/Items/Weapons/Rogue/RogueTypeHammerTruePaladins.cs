﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Weapons.Rogue;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Summon;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    [LegacyName("RogueTypeFallenPaladinsHammer")]
    public class RogueTypeHammerTruePaladins: RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.damage = 70;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 20f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.height = 28;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<RogueTypeHammerTruePaladinsProj>();
            Item.shootSpeed = 14f;
        }
        
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool stealth = player.CheckStealth();
            if (!stealth)
                return true;

            int onlyHoming = Projectile.NewProjectile(source, position, velocity*1.6f , type, (int)(damage * 1.5f), knockback, player.whoAmI, 0f, 0f, -3f);
            int homeAndHanging = Projectile.NewProjectile(source, position, velocity*1.8f ,ModContent.ProjectileType<RogueTypeHammerTruePaladinsProjClone>(), (int)(damage * 0.65f), knockback, player.whoAmI, 0f, 0f, -3f);
            Main.projectile[onlyHoming].Calamity().stealthStrike = true;
            Main.projectile[homeAndHanging].Calamity().stealthStrike = true;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PaladinsHammer).
                AddIngredient<RogueTypeHammerPwnageLegacy>().
                AddIngredient<ScoriaBar>(5).
                AddIngredient<AshesofCalamity>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
