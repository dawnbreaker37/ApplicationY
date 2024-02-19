using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class SubscribersRepository : Base<Subscribtion>, ISubscribe
    {
        private readonly Context _context;
        public SubscribersRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> DeclineSubscribtionAsync(int UserId, int SubscriberId)
        {
            if(UserId != 0 || SubscriberId != 0)
            {
                int Result = await _context.Subscribtions.AsNoTracking().Where(s => !s.IsRemoved && s.UserId == UserId && s.SubscriberId == SubscriberId).ExecuteUpdateAsync(s => s.SetProperty(s => s.IsRemoved, true));
                if (Result != 0) return true;
            }
            return false;
        }

        public async Task<int> GetSubscribersCount(int UserId)
        {
            if (UserId != 0) return await _context.Subscribtions.AsNoTracking().CountAsync(s => s.UserId == UserId && !s.IsRemoved);
            else return 0;
        }

        public IQueryable<GetSubscribtions_ViewModel>? GetSubscribtions(int UserId)
        {
            if (UserId != 0) return _context.Subscribtions.AsNoTracking().Where(s => !s.IsRemoved && s.SubscriberId == UserId).Select(s => new GetSubscribtions_ViewModel { Id = s.Id, SubscriberId = s.SubscriberId, UserId = s.UserId, UserName = s.User!.PseudoName, UserSearchName = s.User.SearchName });
            else return null;
        }

        public async Task<bool> IsUserSubscribed(int UserId, int SubscriberId)
        {
            if (UserId != 0 || SubscriberId != 0)
            {
                bool Result = await _context.Subscribtions.AsNoTracking().AnyAsync(s => s.SubscriberId == SubscriberId && s.UserId == UserId && !s.IsRemoved);
                return Result;
            }
            return false;
        }

        public async Task<bool> SubscribeAsync(int UserId, int SubscriberId)
        {
            if(UserId != 0 || SubscriberId != 0)
            {
                bool HasBeenSubscribedAlready = await _context.Subscribtions.AsNoTracking().AnyAsync(s => s.IsRemoved && s.SubscriberId == SubscriberId && s.UserId == UserId);
                if (HasBeenSubscribedAlready)
                {
                    int Result = await _context.Subscribtions.AsNoTracking().Where(s => s.UserId == UserId && s.SubscriberId == SubscriberId && s.IsRemoved).ExecuteUpdateAsync(s => s.SetProperty(s => s.IsRemoved, false));
                    if (Result != 0) return true;
                }
                else
                {
                    Subscribtion subscribtion = new Subscribtion
                    {
                        UserId = UserId,
                        SubscriberId = SubscriberId,
                        IsRemoved = false
                    };
                    await _context.AddAsync(subscribtion);
                    await _context.SaveChangesAsync();

                    return true; 
                }
            }
            return false;
        }
    }
}
