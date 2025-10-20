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
    private readonly AddVideoUseCase _addVideo;
    private readonly UploadVideoUseCase _uploadVideo;
    private readonly AddSubtitlesUseCase _addSubtitlesUseCase;
    private readonly IVideoRepository _videos;

    public VideoController(AddVideoUseCase addVideo,
        UploadVideoUseCase uploadVideo,
        IVideoRepository videos,
        AddSubtitlesUseCase addSubtitlesUseCase)
    {
        _addVideo = addVideo;
        _videos = videos;
        _uploadVideo = uploadVideo;
        _addSubtitlesUseCase = addSubtitlesUseCase;
    }

    [HttpPost("information")]
    public async Task<ActionResult> AddVideoInformation([FromBody] UploadVideoRequest request)
    {
        var dto = new AddVideoInformationDto
        {
            Title = request.Title,
            ChannelId = request.ChannelId,
            Description = request.Description ?? string.Empty,
            Tags = request.Tags ?? string.Empty,
        };

        var result = await _addVideo.ExecuteAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id:int}/upload")]
    public async Task<IActionResult> UploadVideo(int id, IFormFile formFile)
    {
        if (formFile is null || formFile.Length <= 0)
        {
            return BadRequest("Video is required.");
        }

        var extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
        if (extension != ".mp4")
            return BadRequest("Only .mp4 files are allowed.");

        if (formFile.ContentType != "video/mp4")
            return BadRequest("Invalid file type. Expected video/mp4.");

        await _uploadVideo.ExecuteAsync(new UploadVideoDto
        {
            VideoId = id,
            VideoStream = formFile.OpenReadStream()
        });

        return Ok("Video uploaded");
    }

    [HttpPost("{id:int}/upload-subtitles")]
    public async Task<IActionResult> UploadSubtitles(int id, IList<IFormFile> formFiles)
    {
        formFiles = formFiles.Where(file => file is not null && file.Length > 0).ToList();
        if (formFiles is null || !formFiles.Any())
        {
            return BadRequest("Subtitle file(s) are required.");
        }

        var subtitles = new Dictionary<string, Stream>();
        foreach (var file in formFiles)
        {
            var filenameWithoutExt = string.Join("", file.FileName.Split('.')[..^1]);
            subtitles.Add(filenameWithoutExt, file.OpenReadStream());
        }

        await _addSubtitlesUseCase.ExecuteAsync(new AddVideoSubtitlesDto
        {
            VideoId = id,
            Subtitles = subtitles
        });

        return Ok();
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
