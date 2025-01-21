using CalamityInheritance.CIPlayer;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.CalPlayer;
using CalamityInheritance.CICooldowns;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Typeless;
using CalamityInheritance.System;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Melee;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Items
{
    public class Test : ModItem, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public new string LocalizationCategory => "Content.Items.Weapons.Melee";

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 55;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 42;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            return base.CanUseItem(player);
        }
    }
}
