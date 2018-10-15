using System;

namespace Light.Data.Sample
{
    public interface ITeUser
    {
        string Account { get; set; }
        string Address { get; set; }
        int? Area { get; set; }
        DateTime Birthday { get; set; }
        CheckLevelType? CheckLevelType { get; set; }
        double? CheckPoint { get; set; }
        bool? CheckStatus { get; set; }
        bool DeleteFlag { get; set; }
        string Email { get; set; }
        GenderType Gender { get; set; }
        double HotRate { get; set; }
        int Id { get; set; }
        DateTime? LastLoginTime { get; set; }
        int LevelId { get; set; }
        int LoginTimes { get; set; }
        int Mark { get; set; }
        string NickName { get; set; }
        string Password { get; set; }
        int? RefereeId { get; set; }
        DateTime RegTime { get; set; }
        int Status { get; set; }
        string Telephone { get; set; }

        string ToString();
    }
}