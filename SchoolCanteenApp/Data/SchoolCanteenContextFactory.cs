namespace SchoolCanteenApp.Data
{
    public class SchoolCanteenContextFactory
    {
        public SchoolCanteenContext Create()
        {
            return new SchoolCanteenContext();
        }
    }
}