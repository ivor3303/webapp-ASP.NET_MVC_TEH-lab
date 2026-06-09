using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }

        public int RadnaOpremaId { get; set; }
        public virtual RadnaOprema? RadnaOprema { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}