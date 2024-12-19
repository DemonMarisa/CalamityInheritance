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

namespace CalamityInheritance.Content.Items
{
    public class test : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 70;
            Item.damage = 70;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 42;
            Item.scale = 2;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 12f;
        }
        public override bool? UseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();

            player.AddCooldown(DraconicElixirCooldown.ID, CalamityUtils.SecondsToFrames(30));

            return false;
        }
    }
}
