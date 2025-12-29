using CalamityInheritance.Content.Projectiles.Summon.Worms;
using CalamityInheritance.Rarity;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Summon.Worms
{
    public class StaffofDOG : ModItem, ILocalizedModType
    {
        public static int BaseDamage = 100;
        public new string LocalizationCategory => "Items.Weapons.Summon";
        public override void SetDefaults()
        {
            Item.width = 68;
            Item.height = 68;
            Item.damage = BaseDamage;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 10; // 9 because of useStyle 1
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = RarityType<DeepBlue>();
            Item.UseSound = SoundID.Item113;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<DOGworm>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override bool CanUseItem(Player player)
        {
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.active & p.type == ProjectileType<DOGworm>())
                    return false;
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, Main.MouseWorld, Vector2.Zero, ProjectileType<DOGworm>(), 0, 1, player.whoAmI);
            return false;
        }
    }
}
