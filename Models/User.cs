﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class User : IdentityUser<int>
    {
        public DateTime PasswordResetDate { get; set; }
        public bool IsEmailChanged { get; set; }
        [MinLength(3)]
        [MaxLength(60)]
        public string? PseudoName { get; set; }
        [MinLength(4)]
        [MaxLength(12)]
        public string? SearchName { get; set; }
        [MinLength(6)]
        [MaxLength(6)]
        public string? ReserveCode { get; set; }
        [MaxLength(600)]
        public string? Description { get; set; }
        [MaxLength(50)]
        public string? Link1Tag { get; set; }
        [MaxLength(50)]
        public string? Link2Tag { get; set; }
        public string? Link1 { get; set; }
        public string? Link2 { get; set; }
        public string? ProfilePhoto { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsCompany { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsEasyEntryEnabled { get; set; }
        public bool AreMessagesDisabled { get; set; }
        public bool AreCommentsDisabled { get; set; }
        [ForeignKey("Country")]
        public int? CountryId { get; set; }
        public Country? Country { get; set; }
        public List<SessionInfo>? Sessions { get; set; }
        public List<Notification>? Notifications { get; set; }
        public List<Subscribtion>? Subscribtions { get; set; }
        public List<Project>? Projects { get; set; }
        public List<LikedPost>? LikedPosts { get; set; }
        public List<Purge>? Purges { get; set; }
        public List<Mention>? Mentions { get; set; }
    }
}
