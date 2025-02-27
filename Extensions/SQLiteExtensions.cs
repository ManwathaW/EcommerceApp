using SQLite;


namespace EcommerceApp.Extensions
{
    public static class SQLiteExtensions
    {
        public static async Task<bool> AnyAsync<T>(this AsyncTableQuery<T> query) where T : new()
        {
            return await query.CountAsync() > 0;
        }
    }
}
