using System;

namespace PixelPass
{
    public class AccountInfo
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Notes { get; set; }
        public DateTime Expiration { get; set; }

        public bool IsExpired => (Expiration < DateTime.Now);

        public override string ToString()
        {
            if (IsExpired)
            {
                return $"{Title} (expired)";
            }
            return Title;
        }
    }
}
