using CalamityMod.Projectiles.Typeless;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class LeviathanAmbergrisLegacy : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:20,
            itemHeight:22,
            itemRare:ItemRarityID.Lime,
            itemValue:CIShopValue.RarityPriceLime,
            itemDefense:20
        );
        public override void ExSSD() => Type.ShimmerEach<LeviathanAmbergris>();
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var source = player.GetSource_Accessory(Item);
            player.ignoreWater = true;
            if (!player.lavaWet && !player.honeyWet)
            {
                if (!Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
                {
                    player.endurance += 0.05f;
                    player.GetDamage<GenericDamageClass>() += 0.05f;
                }
                else
                {
                    player.GetDamage<GenericDamageClass>() += 0.1f;
                    player.statDefense += 20;
                    player.moveSpeed += 0.75f;
                }
            }
            if ((double)player.velocity.X > 0 || (double)player.velocity.Y > 0 || player.velocity.X < -0.1 || player.velocity.Y < -0.1)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    int seawaterDamage = (int)player.GetBestClassDamage().ApplyTo(50);
                    Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, 0f, ProjectileType<PoisonousSeawater>(), seawaterDamage, 5f, player.whoAmI, 0f, 0f);
                }
            }
            int seaCounter = 0;
            Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0f, 0.5f, 1.25f);
            int tryGeyBuff = BuffID.Venom;
            float distance = 200f;
            bool flag = seaCounter % 60 == 0;
            int auraDamage = (int)player.GetBestClassDamage().ApplyTo(15);
            int random = Main.rand.Next(5);
            if (player.whoAmI == Main.myPlayer)
            {
                if (random == 0 && player.immune && Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
                {
                    for (int l = 0; l < Main.maxNPCs; l++)
                    {
                        NPC tryGetNPC = Main.npc[l];
                        if (tryGetNPC.active && !tryGetNPC.friendly && tryGetNPC.damage > 0 && !tryGetNPC.dontTakeDamage && !tryGetNPC.buffImmune[tryGeyBuff] && Vector2.Distance(player.Center, tryGetNPC.Center) <= distance)
                        {
                            if (tryGetNPC.FindBuffIndex(tryGeyBuff) == -1)
                            {
                                tryGetNPC.AddBuff(tryGeyBuff, 300, false);
                            }
                            if (flag)
                            {
                                if (player.whoAmI == Main.myPlayer)
                                {
                                    Projectile.NewProjectileDirect(source, tryGetNPC.Center, Vector2.Zero, ProjectileType<DirectStrike>(), auraDamage, 0f, player.whoAmI, l);
                                }
                            }
                        }
                    }
                }
            }
            seaCounter++;
        }
    }
}
