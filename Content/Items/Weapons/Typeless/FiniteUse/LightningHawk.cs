using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Ammos.FiniteUse;
using CalamityInheritance.Content.Projectiles.Ammo.FiniteUse;
using CalamityInheritance.Core.Utils;
using LAP.Core.MiscDate;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Typeless.FiniteUse
{
    public class LightningHawk : CITypeless
    {
        public static readonly SoundStyle UseSound = CISounds.Magnum;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 400;
            Item.width = 50;
            Item.height = 28;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<MagnumRound>();
            Item.useAmmo = ItemType<MagnumRounds>();
            if (LAPInfo.AnyBossHere)
                Item.CI().timesUsed = 3;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 56;
        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(UseSound, player.Center);
            return true;
        }

        public override bool OnPickup(Player player)
        {
            if (LAPInfo.AnyBossHere)
                Item.CI().timesUsed = 3;

            return true;
        }

        public override bool CanUseItem(Player player) => Item.CI().timesUsed < 3;

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

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
                AddIngredient<Magnum>().
                AddIngredient(ItemID.SoulofMight, 20).
                AddIngredient(ItemID.SoulofSight, 20).
                AddIngredient(ItemID.SoulofFright, 20).
                AddIngredient(ItemID.IllegalGunParts).
                AddIngredient(ItemID.HallowedBar, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
