using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class MagnusEye: CIMagic, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 76;
            Item.height = 48;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 100;
            Item.knockBack = 2f;
            Item.mana = 12;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 20; 
            Item.shoot = ModContent.ProjectileType<MagnusProj>();
            Item.shootSpeed = 14f;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
            Item.value = CIShopValue.RarityMaliceDrop;
        }
    }
}