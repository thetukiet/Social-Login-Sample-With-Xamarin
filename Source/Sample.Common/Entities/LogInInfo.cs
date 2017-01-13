using SQLite;

namespace Sample.Common.Entities
{
    public class LogInInfo
    {
        [PrimaryKey]
        public string Email { set; get; }

        public string ProfileLink { set; get; }

        public string SocialId { set; get; }

        public string Name { set; get; }

        public string AvatarUrl { set; get; }

        public SocialDomain SocialDomain { set; get; }
    }

    public enum SocialDomain
    {
        None = 0,
        Facebook = 1,
        Google = 2,
        Twitter = 3
    }
}
