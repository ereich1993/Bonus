using System;

namespace SynetecAssessmentApi.Domain
{
    public abstract class Entity
    {
        public int Id { get; set; }

        public Entity(int id)
        {
            Id = id;
        }
    }
}
