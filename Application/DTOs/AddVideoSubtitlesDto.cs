using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoStream.Application.Interfaces;
using VideoStream.Domain.Entities;

namespace VideoStream.Application.DTOs;

public class AddVideoSubtitlesDto
{
    public int VideoId { get; set; }
    public IDictionary<string, Stream> Subtitles { get; set; }

    public async Task<IList<Subtitle>> ToSubtitleList(ILocalFileStorageService localFileStorageService)
    {
        return await Subtitles.ToAsyncEnumerable()
            .SelectAwait(async kv => new Subtitle
            {
                Language = kv.Key,
                VideoId = VideoId,
                FilePath = await localFileStorageService.SaveSubtitleAsync(VideoId, kv.Key, kv.Value)
            }).ToListAsync();
    }
}