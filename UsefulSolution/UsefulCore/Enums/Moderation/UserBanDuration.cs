using System;
using System.ComponentModel.DataAnnotations;

namespace UsefulCore.Enums.Moderation
{
    public enum UserBanDuration
    {
        [Display(Name = "1 Week")]
        OneWeek = 0,

        [Display(Name = "1 Month")]
        OneMonth = 1,

        [Display(Name = "1 Year")]
        OneYear = 2,

        [Display(Name="Until the end of time")]
        Permanent = 3
    }

    public static class UserBanDurationExtensions
    {
        public static TimeSpan ToTimeSpan(this UserBanDuration banDurationEnum)
        {
            switch (banDurationEnum)
            {
                case UserBanDuration.OneWeek:
                    return TimeSpan.FromDays(7);
                case UserBanDuration.OneMonth:
                    return TimeSpan.FromDays(31);
                case UserBanDuration.OneYear:
                    return TimeSpan.FromDays(365);
                default:
                    return TimeSpan.FromDays(36500);
            }
        }
    }
}
