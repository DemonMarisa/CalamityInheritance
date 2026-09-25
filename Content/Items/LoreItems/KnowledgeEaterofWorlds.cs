using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Projectiles.Typeless.HomeIn;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeEaterofWorlds : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                AddEffect(player);
            }
        }
        public static void AddEffect(Player player)
        {
            player.CI().AddEffect("KnowledgeEaterofWorlds", () =>
            {
                int damage = player.CalcIntDamage<GenericDamageClass>(50);
                float knockBack = 1f;

                if (Main.rand.NextBool(30))
                {
                    int defualtProj = 0;

                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == ProjectileType<TheDeadlyMicrobeProjectile>())
                            defualtProj++;
                    }

                    if (Main.rand.Next(15) >= defualtProj && defualtProj < 6)
                    {
                        int projFirstStack = 3;
                        int projSecStack = 24;

                        for (int j = 0; j < projFirstStack; j++)
                        {
                            int projPos = Main.rand.Next(200 - j * 2, 400 + j * 2);
                            Vector2 center = player.Center;
                            center.X += Main.rand.NextFloat(-projPos, projPos + 1);
                            center.Y += Main.rand.NextFloat(-projPos, projPos + 1);

                            if (!Collision.SolidCollision(center, projSecStack, projSecStack) && !Collision.WetCollision(center, projSecStack, projSecStack))
                            {
                                center.X += projSecStack / 2;
                                center.Y += projSecStack / 2;

                                if (Collision.CanHit(player.Center, 1, 1, center, 1, 1) || Collision.CanHit(new Vector2(player.Center.X, player.position.Y - 50f), 1, 1, center, 1, 1))
                                {
                                    int tileX = (int)center.X / 16;
                                    int tileY = (int)center.Y / 16;
                                    bool ifBounce = true;

                                    if (ifBounce)
                                    {
                                        for (int k = 0; k < Main.maxProjectiles; k++)
                                        {
                                            if (Main.projectile[k].active && Main.projectile[k].owner == player.whoAmI && Main.projectile[k].type == ProjectileType<TheDeadlyMicrobeProjectile>() && (center - Main.projectile[k].Center).Length() < 48f)
                                            {
                                                ifBounce = false;
                                                break;
                                            }
                                        }

                                        if (ifBounce && Main.myPlayer == player.whoAmI)
                                        {
                                            IEntitySource entitySource = player.GetSource_ItemUse(player.HeldItem);
                                            Projectile.NewProjectile(entitySource, center.X, center.Y, 0f, 0f, ProjectileType<TheDeadlyMicrobeProjectile>(), damage, knockBack, player.whoAmI, 0f, 0f);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}
