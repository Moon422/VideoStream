using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoStream.Application.DTOs;
using VideoStream.Application.UseCases;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VideoStream.Presentation.Models.Videos;
using VideoStream.Presentation.Models;

namespace VideoStream.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    private readonly UploadVideoUseCase _uploadVideo;
    private readonly IVideoRepository _videos;

    public VideoController(UploadVideoUseCase uploadVideo, IVideoRepository videos)
    {
        _uploadVideo = uploadVideo;
        _videos = videos;
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<ActionResult> Upload(
        [FromForm] string title,
        [FromForm] int channelId,
        [FromForm] string? description,
        [FromForm] string? tags,
        [FromForm(Name = "file")] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Video file is required");

        var dto = new VideoUploadDto
        {
            Title = title,
            ChannelId = channelId,
            Description = description ?? string.Empty,
            Tags = tags ?? string.Empty,
            VideoStream = file.OpenReadStream(),
            Subtitles = new Dictionary<string, Stream>()
        };

        var result = await _uploadVideo.ExecuteAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var entity = await _videos.GetByIdAsync(id);
        if (entity == null)
            return NotFound();

        var dto = Map(entity);
        return Ok(dto);
    }

    [HttpGet("by-channel/{channelId:int}")]
    public async Task<ActionResult> GetByChannel(int channelId, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        var paged = await _videos.GetByChannelIdAsync(channelId, page, pageSize);
        var dto = new PagedResponse<VideoOverviewModel>
        {
            Items = paged.Select(Map).ToList(),
            PageIndex = paged.PageIndex,
            PageSize = paged.PageSize,
            TotalItems = paged.TotalItems,
            TotalPages = paged.TotalPages
        };
        return Ok(dto);
    }

    private static VideoOverviewModel Map(Video v) => new()
    {
        Id = v.Id,
        Title = v.Title,
        Description = v.Description,
        Tags = v.Tags,
        ChannelId = v.ChannelId,
        ThumbnailPath = v.ThumbnailPath
    };
}
