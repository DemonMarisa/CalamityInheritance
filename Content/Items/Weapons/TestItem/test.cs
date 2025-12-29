using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Rogue;
using LAP.Core.MusicEvent;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.TestItem
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
            Item.shootSpeed = 0;
            Item.shoot = ProjectileType<ScarletDevilBullet>();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int Id = ItemType<Photovisceratorold>();
            // MusicEventManger.AddMusicEventEntry("CalamityInheritance/Music/Tyrant", TimeSpan.FromSeconds(110d), () => true, TimeSpan.FromSeconds(5d));
            return false;
        }
        public override bool? UseItem(Player player)
        {
            CalamityInheritancePlayer cIPlayer = player.CIMod();
            return true;
        }
    }
}
