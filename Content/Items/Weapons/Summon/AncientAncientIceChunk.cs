using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Summon;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Summon
{
    public class AncientAncientIceChunk : CISummon, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<AncientIceChunk>();
        }
        public override void SetDefaults()
        {
            Item.damage = 31;
            Item.mana = 10;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item30;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<AncientClasper>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, player.LocalMouseWorld(), velocity, ProjectileType<AncientClasper>(), damage, knockback, player.whoAmI);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            return false;
        }
    }
}
