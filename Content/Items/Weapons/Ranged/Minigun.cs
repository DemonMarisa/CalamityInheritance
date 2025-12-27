using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.CalProjChange;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class Minigun : CIRanged, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        
        public override void SetDefaults()
        {
            Item.damage = 550;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 92;
            Item.height = 44;
            // 速射机枪使用时间3->2
            // 使用时间2 - 4，伤害275-500，音效鬼畜了
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 22f;
            Item.useAmmo = AmmoID.Bullet;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override void UseItemFrame(Player player)
        {
            player.ChangeDir(Math.Sign((player.Calamity().mouseWorld - player.Center).X));
            float rotation = (player.Center - player.Calamity().mouseWorld).ToRotation() * player.gravDir + MathHelper.PiOver2;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
            CIFunction.NoHeldProjUpdateAim(player, 0, 1);
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + Main.rand.Next(-15, 16) * 0.05f;
            float SpeedY = velocity.Y + Main.rand.Next(-15, 16) * 0.05f;
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() > 0.8f;

        public override void AddRecipes()
        {
            //取消金源锭需求
            CreateRecipe().
                AddIngredient(ItemID.ChainGun).
                AddIngredient<ClockGatlignum>().
                AddIngredient<CosmiliteBar>(12).
                AddIngredient<AscendantSpiritEssence>(5).
                AddDecraftCondition(CalamityConditions.DownedYharon).
                AddCustomShimmerResult(ModContent.ItemType<ACTMinigun>()).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
