using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev.Data
{
    public interface IBaseEntity
    {
    }
    public interface IBaseEntity<TKey> : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        TKey Id { get; set; }

        [Column(Order = 1)]
        Guid ObjectId { get; set; }

        [DataMember]
        [Required]
        [Column(Order = 2)]
        DateTime CreatedDate { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        [Column(Order = 3)]
        string CreatorIP { get; set; }

        [DataMember]
        [Required]
        [MaxLength(18)]
        [Column(Order = 4)]
        Guid CreatorUserId { get; set; }

        [DataMember]
        [Column(Order = 5)]
        DateTime? ModifiedDate { get; set; }

        [DataMember]
        [StringLength(50)]
        [Column(Order = 6)]
        string ModifierIP { get; set; }

        [DataMember]
        [MaxLength(18)]
        [Column(Order = 7)]
        Guid? ModifierUserId { get; set; }
    }
}
