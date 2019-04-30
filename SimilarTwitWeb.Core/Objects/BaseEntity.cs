using System;

namespace SimilarTwitWeb.Core.Objects
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
