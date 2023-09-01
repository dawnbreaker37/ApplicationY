namespace ApplicationY.Interfaces
{
    public interface IBase<T> where T : class
    {
        public Task<int> GetCountByIdAsync(int Id);
    }
}
