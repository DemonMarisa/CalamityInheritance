using CalamityInheritance.Content.Items.Ammo.FiniteUse;
using CalamityInheritance.Content.Projectiles.Typeless.FiniteUse;
using CalamityInheritance.Utilities;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Typeless.FiniteUse
{
    public class Hydra : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Typeless";
        public static readonly SoundStyle UseSound = new("CalamityInheritance/Sounds/Item/Hydra");
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.width = 66;
            Item.height = 30;
            Item.useTime = 33;
            Item.useAnimation = 33;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 10f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<ExplosiveShotgunShell>();
            Item.useAmmo = ModContent.ItemType<ExplosiveShells>();
            if (CalamityPlayer.areThereAnyDamnBosses)
                Item.CalamityInheritance().timesUsed = 1;
        }
        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(UseSound, player.Center);
            return true;
        }

        public override bool OnPickup(Player player)
        {
            if (CalamityPlayer.areThereAnyDamnBosses)
                Item.CalamityInheritance().timesUsed = 1;

            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return Item.CalamityInheritance().timesUsed < 1;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void UpdateInventory(Player player)
        {
            if (!CalamityPlayer.areThereAnyDamnBosses)
                Item.CalamityInheritance().timesUsed = 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 8; ++index)
            {
                float SpeedX = velocity.X + Main.rand.Next(-40, 41) * 0.01f;
                float SpeedY = velocity.Y + Main.rand.Next(-40, 41) * 0.01f;

                Projectile.NewProjectile(source, position, new Vector2(SpeedX, SpeedY), type, damage, knockback, player.whoAmI);
            }

            if (CalamityPlayer.areThereAnyDamnBosses)
            {
                player.HeldItem.CalamityInheritance().timesUsed++;
                for (int i = 0; i < Main.InventorySlotsTotal; i++)
                {
                    if (player.inventory[i].type == Item.type && player.inventory[i] != player.HeldItem)
                        player.inventory[i].CalamityInheritance().timesUsed++;
                }
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Shotgun).
                AddIngredient(ItemID.IronBar, 20).
                AddIngredient(ItemID.IllegalGunParts).
                AddIngredient(ItemID.Ectoplasm, 20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
