using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Weapons.Melee;
using CalamityInheritance.Content.Projectiles.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class SoulEdge : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public static readonly SoundStyle ProjectileDeathSound = SoundID.NPCDeath39 with { Volume = 0.5f};

        public override void SetStaticDefaults()
        {
            if(CalamityInheritanceConfig.Instance.CustomShimmer == true)
             //开启微光转化后，灵魂边锋与虚空边锋可以用微光相互转化
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<VoidEdge>()] = ModContent.ItemType<SoulEdge>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<SoulEdge>()] = ModContent.ItemType<VoidEdge>();
            }
        }
        public override void SetDefaults()
        {
            Item.width = 88;
            Item.height = 88;
            Item.damage = 420;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.useAnimation = 23;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SoulEdgeSoulLegacyLarge>();
            Item.shootSpeed = 15f;

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = ModContent.RarityType<AbsoluteGreen>();
        }
        public override bool CanUseItem(Player player)
        {
            if (player.name == "Shizuku" || player.name == "shizuku")
            {
                Item.damage = 1600;
                Item.useTime = 10;
                Item.useAnimation = 10;
            }
            else
            {
                Item.useTime = 23;
                Item.useAnimation = 23;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numShots = 2;
            for (int i = 0; i < numShots; ++i)
            {
                float SpeedX = velocity.X + Main.rand.Next(-40, 41) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-40, 41) * 0.05f;
                float ai1 = Main.rand.NextFloat() + 0.5f;
                // TODO -- unchecked type addition math assumes we can guarantee load order
                // this is extremely unsafe and if TML optimizes autoloading or asset loading it could fail
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, Main.rand.Next(type, type + 3), damage, knockback, player.whoAmI, 0.0f, ai1);
            }
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 300);
        }

        public override void AddRecipes()
        {
            if(CalamityInheritanceConfig.Instance.CustomShimmer == false)
             //关闭微光转化后，灵魂边锋启用合成表
            {
                CreateRecipe().
                    AddIngredient<RuinousSoul>(10).
                    AddIngredient<Necroplasm>(10).
                    AddIngredient(ItemID.Ectoplasm, 10).
                    AddIngredient<Voidstone>(10).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
        }
    }
}