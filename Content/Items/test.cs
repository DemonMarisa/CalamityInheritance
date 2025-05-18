using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using System.Security.Authentication;
using Microsoft.Xna.Framework;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System;
using CalamityInheritance.NPCs;
using CalamityMod.NPCs.Yharon;
using CalamityInheritance.NPCs.Boss.Yharon;

namespace CalamityInheritance.Content.Items
{
    public class Test : CIMelee, ILocalizedModType
    {
        //别改这个为大写了，他每次拉去的时候图片的文件总是变成小写 
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 55;
            Item.DamageType = DamageClass.Generic;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 42;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 10;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool? UseItem(Player player)
        {
            CalamityInheritancePlayer cIPlayer = player.CIMod();
            /*
            if (player.altFunctionUse == 2)
                Main.NewText($"Calamitas Clone P2: {CIGlobalNPC.LegacyCalamitasCloneP2}");
            else
                Main.NewText($"Calamitas Clone P1: {CIGlobalNPC.LegacyCalamitasClone}");
            */
            if (player.altFunctionUse == 2)
                Main.NewText($"LegacyYharon P2: {CIGlobalNPC.LegacyYharon}");
            else
                Main.NewText($"Calamitas Clone P1: {CIGlobalNPC.LegacyCalamitasClone}");
            return true;
        }
    }
}
