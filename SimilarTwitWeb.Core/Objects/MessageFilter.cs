using System;

namespace SimilarTwitWeb.Core.Objects
{
    public struct MessageFilter
    {
        public int? UserId { get; set; }
        public DateTime? MinCreationDate { get; set; }
    }
}
