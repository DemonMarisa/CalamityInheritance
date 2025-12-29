using CalamityInheritance.Content.Projectiles.Melee.Swords;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class BladecrestOathswordLegacy : GeneralWeaponClass
    {
        public override void ExSD()
        {
            Item.width = 56;
            Item.height = 56;
            Item.damage = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            //是的，这是新建射弹
            Item.shoot = ProjectileType<BloodScytheLegacy>();
            Item.shootSpeed = 6f;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 240);
        }
        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OnFire3, 240);
        }
    }
}
