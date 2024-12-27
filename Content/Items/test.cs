using CalamityInheritance.CIPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using CalamityInheritance.CICooldowns;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityInheritance.Content.Projectiles.Rogue;

namespace CalamityInheritance.Content.Items
{
    public class test : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.damage = 2500;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.useTurn = true;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 114;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.shoot = ModContent.ProjectileType<ExoJet>();
            Item.shootSpeed = 30;
            Item.rare = ModContent.RarityType<Violet>();
        }
        public override bool? UseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();

            //player.AddCooldown(GodSlayerCooldown.ID, CalamityUtils.SecondsToFrames(30));

            return false;
        }
    }
}
