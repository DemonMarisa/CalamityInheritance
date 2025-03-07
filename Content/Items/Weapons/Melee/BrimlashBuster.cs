using CalamityInheritance.Content.Projectiles.Melee;
using Humanizer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class BrimlashBuster: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = Item.height = 72;
            Item.damage = 126;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 25;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.rare = ItemRarityID.Cyan;
            Item.shootSpeed = 18f;
            Item.shoot = ModContent.ProjectileType<BrimlashBusterProj>();
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float getDamageBoost = 1.0f;
            if(Main.rand.NextBool(3))
                getDamageBoost = 3f;
            damage.ApplyTo(getDamageBoost);
        }
    }
}