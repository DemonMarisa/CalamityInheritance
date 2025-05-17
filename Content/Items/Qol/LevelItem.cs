using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Qol
{
    public class LevelItem : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Tools";
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

            if (player.altFunctionUse == 2)
            {
                cIPlayer.meleeLevel = 0;
                cIPlayer.rangeLevel = 0;
                cIPlayer.magicLevel = 0;
                cIPlayer.summonLevel = 0;
                cIPlayer.rogueLevel = 0;

                cIPlayer.meleePool = 0;
                cIPlayer.rangePool = 0;
                cIPlayer.magicPool = 0;
                cIPlayer.summonPool = 0;
                cIPlayer.roguePool = 0;
            }
            else
            {
                cIPlayer.meleeLevel = 15;
                cIPlayer.rangeLevel = 15;
                cIPlayer.magicLevel = 15;
                cIPlayer.summonLevel = 15;
                cIPlayer.rogueLevel = 15;

                cIPlayer.meleePool = 12500;
                cIPlayer.rangePool = 12500;
                cIPlayer.magicPool = 12500;
                cIPlayer.summonPool = 12500;
                cIPlayer.roguePool = 12500;
            }
            return true;
        }
    }
}
