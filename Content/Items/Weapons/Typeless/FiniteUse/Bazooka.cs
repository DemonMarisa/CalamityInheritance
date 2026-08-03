using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Ammos.FiniteUse;
using CalamityInheritance.Content.Projectiles.Ammo.FiniteUse;
using CalamityInheritance.Core.Utils;
using LAP.Content.RecipeGroupAdd;
using LAP.Core.MiscDate;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Typeless.FiniteUse
{
    public class Bazooka : CITypeless
    {
        public static readonly SoundStyle UseSound = CISounds.BazookaFull;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 500;
            Item.width = 66;
            Item.height = 26;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 10f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<GrenadeRound>();
            Item.useAmmo = ItemType<GrenadeRounds>();
            if (LAPInfo.AnyBossHere)
                Item.CI().timesUsed = 2;
        }

        public override bool OnPickup(Player player)
        {
            if (LAPInfo.AnyBossHere)
                Item.CI().timesUsed = 2;

            return true;
        }
        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(UseSound, player.Center);
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return Item.CI().timesUsed < 2;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override void UpdateInventory(Player player)
        {
            if (!LAPInfo.AnyBossHere)
                Item.CI().timesUsed = 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (LAPInfo.AnyBossHere)
            {
                player.HeldItem.CI().timesUsed++;
                for (int i = 0; i < Main.InventorySlotsTotal; i++)
                {
                    if (player.inventory[i].type == Item.type && player.inventory[i] != player.HeldItem)
                        player.inventory[i].CI().timesUsed++;
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.IronBar, 20).
                AddIngredient(ItemID.IllegalGunParts).
                AddRecipeGroup(LAPRecipeGroup.AnyAdamantiteBar, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
