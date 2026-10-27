using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Legendary;
using Steamworks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class BlossomFluxLegendary : LegendaryWeapon
    {

        public override Color DrawColor => new(85, 210, 28);
        public override int BaseDamage => 8;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 40;
            Item.height = 62;
            Item.damage = BaseDamage;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.knockBack = 0.15f;
            Item.shootSpeed = 12f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISoundID.SoundBow;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<BlossomFluxLegendLeaf>();
            Item.useAmmo = AmmoID.Arrow;
        }


        public override Vector2? HoldoutOffset()
        {
            // 偏移量
            return new Vector2(-10, 0);
        }
        public override bool CanUseItem(Player player)
        {
            bool p = TierOneMatcher;
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 25;
                Item.useAnimation = 25;
                Item.UseSound = SoundID.Item77;
                Item.ArmorPenetration = p? 50 : 0;
            }
            else
            {
                Item.useTime = p? 1 : 2;
                Item.useAnimation = p ? 5 : 10;
                Item.UseSound = SoundID.Item5;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
                        //设置后坐力手持射弹。
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<CIGeneralRecoilProj>(), 0, 0, player.whoAmI);
            if (proj.ModProjectile is CIGeneralRecoilProj holdout)
            {
                holdout.SetUpHoldoutData(Type, 0f, Item.useAnimation, new Vector2(-10, -0f));
            }
            if (player.altFunctionUse == 2)
            {
                //升级2启用时发射5份炸弹
                if (TierTwoMatcher)
                {
                    int pCounts = 5;
                    const float offset = 0.48f;
                    Vector2 summonP = velocity;
                    summonP.Normalize();
                    summonP *= 36f;
                    for (int i = 0; i < pCounts; i++)
                    {
                        float homeAi = TierThreeMatcher? 1f : 0f;
                        float piArrowOffset = i - (pCounts - 1) / 2;
                        if (piArrowOffset == 1 / 2) piArrowOffset += 1 / 2;
                        Vector2 spawn = summonP.RotatedBy(offset * piArrowOffset, new Vector2());
                        Projectile.NewProjectile(source, position.X + spawn.X, position.Y + spawn.Y, velocity.X, velocity.Y, ProjectileType<BlossomFluxLegendBomb>(), damage * 4, knockback * 60, player.whoAmI, 0f, 0f, homeAi);
                    }
                }
                else
                {
                    float homeAi = TierThreeMatcher? 1f : 0f;
                    Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<BlossomFluxLegendBomb>(), damage * 4, knockback * 60, player.whoAmI, 0f, 0f, homeAi);
                }
            }
            else
            {
                //升级2启用时发射两份树叶
                if (TierTwoMatcher)
                {
                    int pCounts = 2;
                    Vector2 summonP = velocity;
                    float rot = velocity.ToRotation();
                    Vector2 Offset = new Vector2(5, -5).RotatedBy(rot);
                    Vector2 Offset2 = new Vector2(5, 5).RotatedBy(rot);
                    summonP.Normalize();
                    summonP *= 36f;
                    for (int i = 0; i < pCounts; i++)
                    {
                        float homeAi = TierThreeMatcher&& Main.rand.NextBool(2) ? 1f : 0f;
                        Projectile.NewProjectile(source, position + (i == 0 ? Offset : Offset2), summonP, ProjectileType<BlossomFluxLegendLeaf>(), damage, knockback, player.whoAmI, 0f, 0f, homeAi);
                    }
                }
                else
                {
                    float homeAi = TierThreeMatcher&& Main.rand.NextBool(2) ? 1f : 0f;
                    Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<BlossomFluxLegendLeaf>(), damage, knockback, player.whoAmI, 0f, 0f, homeAi);
                }
            }
            return false;
        }
    }
}