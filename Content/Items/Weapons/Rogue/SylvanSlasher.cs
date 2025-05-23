﻿using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class SylvanSlasher : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 72;
            Item.damage = 60;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 5;
            Item.knockBack = 3f;
            Item.autoReuse = true;
            Item.height = 78;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.shoot = ModContent.ProjectileType<SylvanSlashAttack>();
            Item.shootSpeed = 24f;
        }
    }
}
