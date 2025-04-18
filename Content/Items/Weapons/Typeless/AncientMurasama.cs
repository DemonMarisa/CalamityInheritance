using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Rarity.Special;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Typeless

{
	public class AncientMurasama : ModItem, ILocalizedModType
	{
		public new string LocalizationCategory => $"{Generic.WeaponLocal}.Typeless";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
		{
			Item.width = 72;
			Item.damage = 1145;
			Item.knockBack = 14.1919810f;
			Item.value = 314159274;
			Item.rare = ModContent.RarityType<MurasamRed>();
			Item.shootSpeed = 15f;
			Item.height = 78;
			Item.DamageType = DamageClass.Generic;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 5;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<AncientMurasamaProj>();		
		}
	}
}
