﻿using CalamityMod.Items.Weapons.Rogue;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Rogue;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class PhantomLance : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.knockBack = 5f;

            Item.width = 62;
            Item.height = 68;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.value = Item.buyPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.maxStack = 9999;
            Item.UseSound = SoundID.Item1;
            Item.consumable = true;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();

            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<PhantomLanceProj>();
        }

        public override float StealthDamageMultiplier => 1.75f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(150).
                AddIngredient(ItemID.SpectreBar).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}