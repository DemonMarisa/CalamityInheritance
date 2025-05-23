﻿using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Utilities;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class HavocsBreathLegacy : FlamethrowerSpecial, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<HavocsBreath>();
        }
        public override void SetDefaults()
        {
            Item.damage = 67;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 50;
            Item.height = 18;
            Item.useTime = 5;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.UseSound = SoundID.Item34;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BrimstoneFireFriendlyLegacy>();
            Item.shootSpeed = 8.5f;
            Item.useAmmo = AmmoID.Gel;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextBool();

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}
