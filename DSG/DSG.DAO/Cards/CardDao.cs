﻿using DSG.BusinessEntities;

namespace DSG.DAO.Cards
{
    public class CardDao : ICardDao
    {
        private CardManagementDbContext _ctx;

        public CardManagementDbContext Ctx
        {
            get { return _ctx; }
            set { _ctx = value; }
        }

        public void InsertCard(Card card)
        {
            Ctx.Card.Add(card);

            Ctx.SaveChanges();
        }
    }
}
