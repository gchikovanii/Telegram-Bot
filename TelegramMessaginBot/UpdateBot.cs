using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramMessaginBot
{
    public struct UpdateBot
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UserName { get; set; }
        public string? Description { get; set; }
        public string RecivedText { get; set; }

    }
}
