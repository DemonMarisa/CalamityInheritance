using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class AncientDragonsBreath: CIRanged, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 36;
            Item.DamageType = DamageClass.Ranged;
            //面板250->200, 不然巨龙之息拼尽全力不敌远古巨龙之息，泪目了
            Item.damage = 200;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISoundID.SoundShoutgunTactical;
            Item.rare = CIConfig.Instance.SpecialRarityColor ? ModContent.RarityType<YharonFire>() :ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.knockBack = 9.5f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 spread = velocity.RotatedByRandom(MathHelper.ToRadians(3f)) * Main.rand.NextFloat(0.9f, 1.1f);
                Projectile.NewProjectile(source, position, spread, ModContent.ProjectileType<DragonsBreathRound>(), damage, knockback, Main.myPlayer);
            }
            return false;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-15, 10);
    }
}