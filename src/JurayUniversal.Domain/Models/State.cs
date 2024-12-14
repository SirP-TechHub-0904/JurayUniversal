namespace JurayUniversal.Domain.Models
{
    public class State
    {
        public long Id { get; set; }


        public string StateName { get; set; }

        public virtual ICollection<LocalGoverment> LocalGov { get; set; }

    }
}
