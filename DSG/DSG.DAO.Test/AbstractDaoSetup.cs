namespace DSG.DAO.Test
{
    public class AbstractDaoSetup
    {
        public void CleanDatabase()
        {
            string sqlText = "TRUNCATE TABLE DominionExpansion";

            DatabaseConnection databaseConnection = new DatabaseConnection();
            databaseConnection.DefineAndExecuteCommandNonQuery(sqlText);
        }

        public void SetupTestDatabase()
        { 
            string sqlText = "INSERT INTO DominionExpansion (Name) VALUES " +
                "('Welt'), " +
                "('Seaside')";

            DatabaseConnection databaseConnection = new DatabaseConnection();
            databaseConnection.DefineAndExecuteCommandNonQueryWithRowCheck(sqlText);
        }
    }
}
