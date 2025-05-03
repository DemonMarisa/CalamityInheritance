using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Melee;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Magic;

namespace CalamityInheritance.Content.Items
{
    public class Test : CIMelee, ILocalizedModType
    {
        public static string WeaponRoute => "CalamityInheritance/Content/Items";
        //别改这个为大写了，他每次拉去的时候图片的文件总是变成小写 
        public override string Texture => $"{WeaponRoute}/Test";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 55;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 5;
            Item.useTime = 5;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 42;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<AlphaBeam>();
        }/*
        public override bool CanUseItem(Player player)
        {
            return true;
        }*/
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.altFunctionUse == 2)
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AlphaBeam>(), damage, knockback, player.whoAmI, 0f, 0f, 1f);
            else
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AlphaBeam>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
            return false;
        }
        /*
        public override bool? UseItem(Player player)
        {
            CalamityInheritancePlayer cIPlayer = player.CIMod();
            cIPlayer.meleeLevel = 0;
            cIPlayer.meleePool = 0;
            cIPlayer.rangeLevel = 0;
            cIPlayer.rangePool = 0;
            cIPlayer.magicLevel = 0;
            cIPlayer.magicPool = 0;
            cIPlayer.summonLevel = 0;
            cIPlayer.summonPool = 0;
            cIPlayer.rogueLevel = 0;
            cIPlayer.roguePool = 0;
            
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
            
            return true;
        }
        */
    }
}
