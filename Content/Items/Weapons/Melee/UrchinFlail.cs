using CalamityMod.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class UrchinFlail : CIMelee, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 36;
            Item.damage = 20;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 6f;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.channel = true;
            // Item.shoot = ModContent.ProjectileType<UrchinBall>();
            Item.shootSpeed = 16f;
        }
    }
}
