using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ApplicationY.Repositories
{
    public class SessionRepository : Base<SessionRepository>, ISessionInfo
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMemoryCache _cache;

        public SessionRepository(Context context, UserManager<User> userManager, SignInManager<User> signInManager, IMemoryCache cache) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _cache = cache;
        }

        public async Task<int> AddSessionInfoAsync(AddSessionInfo_ViewModel Model)
        {
            if (Model != null)
            {
                SessionInfo sessionInfo = new SessionInfo()
                {
                    SessionDate = DateTime.Now,
                    IsClosed = false,
                    Location = Model.Location,
                    UserId = Model.UserId
                };
                await _context.AddAsync(sessionInfo);
                await _context.SaveChangesAsync();

                return sessionInfo.Id;
            }
            else return 0;
        }

        public IQueryable<SessionInfo>? GetSessionInfos(int Id)
        {
            if (Id != 0) return _context.SessionInfos.AsNoTracking().Where(s => s.UserId == Id).Select(s => new SessionInfo { Id = s.Id, IsClosed = s.IsClosed, Location = s.Location, SessionDate = s.SessionDate });
            else return null;
        }

        public async Task<bool> TerminateAllSessionsAsync(int UserId)
        {
            if(UserId != 0)
            {
                User? UserInfo = await _userManager.FindByIdAsync(UserId.ToString());
                if (UserInfo != null)
                {
                    IdentityResult? Result = await _userManager.UpdateSecurityStampAsync(UserInfo);
                    if(Result.Succeeded)
                    {
                        //await _signInManager.RefreshSignInAsync(UserInfo);

                        int SessionsClear_Result = await _context.SessionInfos.AsNoTracking().Where(s => s.UserId == UserId).ExecuteUpdateAsync(s => s.SetProperty(s => s.IsClosed, true));
                        if (SessionsClear_Result != 0) return true;
                    }
                }
            }
            return false;
        }

        public async Task<int> TerminateSessionAsync(int Id, int UserId)
        {
            return 0;
            //if(Id != 0 && UserId != 0)
            //{

            //}
        }
    }
}
