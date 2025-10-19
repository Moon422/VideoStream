using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoStream.Domain.Entities;

public class Video : BaseEntity, ICreationLogged, IModificationLogged, ISoftDeleted
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(1024)]
    public string Tags { get; set; } = string.Empty;

    [Required]
    public int ChannelId { get; set; }

    public Channel? Channel { get; set; }

    [Required, MaxLength(1024)]
    public string FilePath { get; set; } = string.Empty;

    [Required, MaxLength(40)]
    public string FileName { get; set; } = string.Empty;

    [MaxLength(1024)]
    public string HlsMasterPlaylistPath { get; set; } = string.Empty;

    public IList<Subtitle> Subtitles { get; set; } = [];

    [MaxLength(1024)]
    public string ThumbnailPath { get; set; } = string.Empty;

    public VideoStatus Status { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public bool Deleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}

public enum VideoStatus { Pending, Processing, Ready, Failed }
