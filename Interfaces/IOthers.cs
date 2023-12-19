namespace ApplicationY.Interfaces
{
    public interface IOthers
    {
        public Task<int> SendPurgeAsync(int Id, int SenderId, int UserId, string Description);
    }
}
