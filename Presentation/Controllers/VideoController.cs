using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoStream.Application.DTOs;
using VideoStream.Application.UseCases;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VideoStream.Presentation.Models.Videos;
using VideoStream.Presentation.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using VideoStream.Domain.Exceptions;
using System;
using Microsoft.AspNetCore.Authorization;

namespace VideoStream.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    private readonly CreateVideoUseCase _addVideo;
    private readonly UploadVideoUseCase _uploadVideo;
    private readonly UploadSubtitlesUseCase _addSubtitlesUseCase;
    private readonly GetVideoByIdUseCase _getVideoById;
    private readonly GetVideosByChannelIdUseCase _getVideosByChannelId;

    public VideoController(CreateVideoUseCase addVideo,
        UploadVideoUseCase uploadVideo,
        UploadSubtitlesUseCase addSubtitlesUseCase,
        GetVideoByIdUseCase getVideoById,
        GetVideosByChannelIdUseCase getVideosByChannelId)
    {
        _addVideo = addVideo;
        _uploadVideo = uploadVideo;
        _addSubtitlesUseCase = addSubtitlesUseCase;
        _getVideoById = getVideoById;
        _getVideosByChannelId = getVideosByChannelId;
    }

    [HttpPost("information")]
    public async Task<ActionResult> AddVideoInformation([FromBody] CreateVideoRequest request)
    {
        try
        {
            var result = await _addVideo.ExecuteAsync(request.ToAddVideoInformationDto());
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result.ToVideoModel());
        }
        catch (UserNotFoundException)
        {
            return NotFound();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return BadRequest("Something went wrong! Please try again.");
        }
    }

    [HttpPost("{id:int}/upload")]
    // [RequestFormLimits(MultipartBodyLengthLimit = 100_000_000_000_000)]
    [DisableRequestSizeLimit]
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

        try
        {
            await _uploadVideo.ExecuteAsync(new UploadVideoDto
            {
                VideoId = id,
                VideoStream = formFile.OpenReadStream()
            });

            return Ok("Video uploaded");
        }
        catch (UserNotFoundException)
        {
            return NotFound();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return BadRequest("Something went wrong! Please try again.");
        }
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

        try
        {
            await _addSubtitlesUseCase.ExecuteAsync(new UploadVideoSubtitlesDto
            {
                VideoId = id,
                Subtitles = subtitles
            });

            return Ok();
        }
        catch (UserNotFoundException)
        {
            return NotFound();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return BadRequest("Something went wrong! Please try again.");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var videoDto = await _getVideoById.ExecuteAsync(id);
        if (videoDto is null)
            return NotFound();

        return Ok(videoDto.ToVideoModel());
    }

    [HttpGet("by-channel/{channelId:int}")]
    public async Task<ActionResult> GetByChannel(int channelId, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        var videoDtos = await _getVideosByChannelId.ExecuteAsync(channelId, page, pageSize);
        var videoModels = videoDtos.Select(v => v.ToVideoOverviewModel()).ToList();
        return Ok(new PagedResponse<VideoOverviewModel>
        {
            Items = videoModels,
            PageIndex = videoDtos.PageIndex,
            PageSize = videoDtos.PageSize,
            TotalItems = videoDtos.TotalItems,
            TotalPages = videoDtos.TotalPages
        });
    }
}
