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
        public override bool? UseItem(Player player)
        {
            CalamityInheritancePlayer cIPlayer = player.CIMod();
            
            if (CIConfig.Instance.UIX == 1)
            {
                // 空列表检查
                if (CalStatInflationBACK.PostDOGWeapons == null ||
                    CalStatInflationBACK.PostDOGWeapons.Count == 0)
                {
                    Main.NewText("没有可生成的物品！");
                    return false;
                }

                // 生成所有物品
                foreach (int itemType in CalStatInflationBACK.PostDOGWeapons)
                {
                    player.QuickSpawnItem(player.GetSource_GiftOrReward(), itemType, 1);
                }

                // 显示提示信息
                Main.NewText($"生成了 {CalStatInflationBACK.PostDOGWeapons.Count} 件物品！");
            }
            if (CIConfig.Instance.UIX == 2)
            {
                // 空列表检查
                if (CalStatInflationBACK.PostyharonWeapons == null ||
                    CalStatInflationBACK.PostyharonWeapons.Count == 0)
                {
                    Main.NewText("没有可生成的物品！");
                    return false;
                }

                // 生成所有物品
                foreach (int itemType in CalStatInflationBACK.PostyharonWeapons)
                {
                    player.QuickSpawnItem(player.GetSource_GiftOrReward(), itemType, 1);
                }

                // 显示提示信息
                Main.NewText($"生成了 {CalStatInflationBACK.PostyharonWeapons.Count} 件物品！");
            }
            if (CIConfig.Instance.UIX == 3)
            {
                // 空列表检查
                if (CalStatInflationBACK.PostOldDukeWeapons == null ||
                    CalStatInflationBACK.PostOldDukeWeapons.Count == 0)
                {
                    Main.NewText("没有可生成的物品！");
                    return false;
                }

                // 生成所有物品
                foreach (int itemType in CalStatInflationBACK.PostOldDukeWeapons)
                {
                    player.QuickSpawnItem(player.GetSource_GiftOrReward(), itemType, 1);
                }

                // 显示提示信息
                Main.NewText($"生成了 {CalStatInflationBACK.PostOldDukeWeapons.Count} 件物品！");
            }
            if (CIConfig.Instance.UIX == 4)
            {
                // 空列表检查
                if (CalStatInflationBACK.PostPolterghastWeapons == null ||
                    CalStatInflationBACK.PostPolterghastWeapons.Count == 0)
                {
                    Main.NewText("没有可生成的物品！");
                    return false;
                }

                // 生成所有物品
                foreach (int itemType in CalStatInflationBACK.PostPolterghastWeapons)
                {
                    player.QuickSpawnItem(player.GetSource_GiftOrReward(), itemType, 1);
                }

                // 显示提示信息
                Main.NewText($"生成了 {CalStatInflationBACK.PostPolterghastWeapons.Count} 件物品！");
            }
            return true;
        }
    }
}
