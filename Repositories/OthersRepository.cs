﻿using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using Microsoft.CodeAnalysis;
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

        public async Task<int> SendPurgeAsync(int Id, int SenderId, int UserId, string Description)
        {
            if(UserId == 0)
            {
                if (Id != 0 && SenderId != 0 && !String.IsNullOrEmpty(Description) && Description.Length <= 900) {
                    bool AnyPurgeForThisProjectFromThisUser = await _context.Purges.AnyAsync(p => p.UserId == SenderId && p.ProjectId == Id);
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
                    bool AnyPurgeForThisUserFromThisSender = await _context.Purges.AnyAsync(p => p.UserId == SenderId && p.UserId == UserId);
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
