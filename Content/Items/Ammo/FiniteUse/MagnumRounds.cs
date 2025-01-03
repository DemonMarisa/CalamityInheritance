﻿using CalamityInheritance.Content.Projectiles.Typeless.FiniteUse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Ammo.FiniteUse
{
    public class MagnumRounds : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.crit += 4;
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 12;
            Item.consumable = true;
            Item.knockBack = 8f;
            Item.value = 10000;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<MagnumRound>();
            Item.shootSpeed = 12f;
            Item.ammo = ModContent.ItemType<MagnumRounds>(); // CONSIDER -- Would item.type work here instead of a self reference?
        }
    }
}
