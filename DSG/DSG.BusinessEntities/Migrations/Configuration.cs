﻿namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DSG.BusinessEntities.CardManagementDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DSG.BusinessEntities.CardManagementDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            context.CardType.AddOrUpdate(
                x => x.Id,
                Enum.GetValues(typeof(CardTypeEnum))
                    .OfType<CardTypeEnum>()
                    .Select(x => new CardType() { 
                        Id = x, 
                        Name = x.ToString(), 
                        IsKingdomCard = (x == CardTypeEnum.Action || x == CardTypeEnum.Treasure || x == CardTypeEnum.Victory || x == CardTypeEnum.Night) })
                    .ToArray());
        }
    }
}
