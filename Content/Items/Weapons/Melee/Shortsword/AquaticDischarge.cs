using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class AquaticDischarge : CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Melee.Shortsword";
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.DamageType = GetInstance<TrueMeleeDamageClass>();
            Item.useTurn = false;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.width = 32;
            Item.height = 32;
            Item.damage = 23;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ProjectileType<AquaticDischargeProj>();
            Item.shootSpeed = 2f;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {

        }
    }
}
