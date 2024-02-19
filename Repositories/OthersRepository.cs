using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class OthersRepository : Base<User>, IOthers
    {
        private readonly Context _context;
        public OthersRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<DisabledProject?> DisabledProjectInfoAsync(int Id)
        {
            if (Id != 0) return await _context.DisabledProjects.AsNoTracking().Select(d => new DisabledProject { DisabledAt = d.DisabledAt, ProjectId = d.ProjectId, Description = d.Description, Project = d.Project != null ? new Project { Name = d.Project.Name, CreatedAt = d.Project.CreatedAt } : null }).FirstOrDefaultAsync(d => d.ProjectId == Id);
            else return null;
        }

        public IQueryable<GetUserRole_ViewModel>? GetAllRoles(int CurrentRoleId)
        {
            if (CurrentRoleId == 0) return _context.Roles.AsNoTracking().Select(r => new GetUserRole_ViewModel { RoleId = r.Id, RoleName = r.Name });
            else return _context.Roles.AsNoTracking().Where(r => r.Id <= CurrentRoleId).Select(r => new GetUserRole_ViewModel { RoleId = r.Id, RoleName = r.Name });
        }

        public async Task<string?> GetRoleNameAsync(int RoleId)
        {
            return await _context.Roles.AsNoTracking().Where(r => r.Id == RoleId).Select(r => r.Name).FirstOrDefaultAsync();
        }

        public async Task<GetUserRole_ViewModel?> GetUserFullRoleInfoAsync(int UserId, int RoleId)
        {
            if (UserId != 0)
            {
                return await _context.Roles.AsNoTracking().Where(x => x.Id == RoleId).Select(x => new GetUserRole_ViewModel { RoleId = RoleId, RoleName = x.Name }).FirstOrDefaultAsync();
            }
            else return null;
        }

        public async Task<int> GetUserRoleAsync(int UserId)
        {
            if (UserId != 0)
            {
                return await _context.UserRoles.AsNoTracking().Where(x => x.UserId == UserId).Select(x => x.RoleId).FirstOrDefaultAsync();
            }
            else return 0;
        }

        public async Task<int> SendPurgeAsync(int Id, int SenderId, int UserId, string Description)
        {
            if(UserId == 0)
            {
                if (Id != 0 && SenderId != 0 && !String.IsNullOrEmpty(Description) && Description.Length <= 900) {
                    bool AnyPurgeForThisProjectFromThisUser = await _context.Purges.AsNoTracking().AnyAsync(p => p.UserId == SenderId && p.ProjectId == Id);
                    if (!AnyPurgeForThisProjectFromThisUser)
                    {
                        Purge purge = new Purge
                        {
                            Description = Description,
                            SentAt = DateTime.Now,
                            IsClosed = false,
                            ProjectId = Id,
                            SenderId = SenderId,
                            UserId = null
                        };

                        await _context.AddAsync(purge);
                        await _context.SaveChangesAsync();

                        return Id;
                    }
                }
            }
            else
            {
                if (UserId != 0 && SenderId != 0 && !String.IsNullOrEmpty(Description) && Description.Length <= 900)
                {
                    bool AnyPurgeForThisUserFromThisSender = await _context.Purges.AsNoTracking().AnyAsync(p => p.UserId == SenderId && p.UserId == UserId);
                    if (!AnyPurgeForThisUserFromThisSender)
                    {
                        Purge purge = new Purge
                        {
                            Description = Description,
                            SentAt = DateTime.Now,
                            IsClosed = false,
                            ProjectId = null,
                            SenderId = SenderId,
                            UserId = UserId
                        };

                        await _context.AddAsync(purge);
                        await _context.SaveChangesAsync();

                        return UserId;
                    }
                }
            }
            return 0;
        }
    }
}
