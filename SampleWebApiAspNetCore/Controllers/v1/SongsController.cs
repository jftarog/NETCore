using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ISongRepository _songRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<SongsController> _linkService;

        public SongsController(
            ISongRepository songRepository,
            IMapper mapper,
            ILinkService<SongsController> linkService)
        {
            _songRepository = songRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetAllSongs))]
        public ActionResult GetAllSongs(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<SongEntity> songItems = _songRepository.GetAll(queryParameters).ToList();

            var allItemCount = _songRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = songItems.Select(x => _linkService.ExpandSingleItem(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleSong))]
        public ActionResult GetSingleSong(ApiVersion version, int id)
        {
            SongEntity songItem = _songRepository.GetSingle(id);

            if (songItem == null)
            {
                return NotFound();
            }

            SongDto item = _mapper.Map<SongDto>(songItem);

            return Ok(_linkService.ExpandSingleItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddSong))]
        public ActionResult<SongDto> AddSong(ApiVersion version, [FromBody] SongCreateDto songCreateDto)
        {
            if (songCreateDto == null)
            {
                return BadRequest();
            }

            SongEntity toAdd = _mapper.Map<SongEntity>(songCreateDto);

            _songRepository.Add(toAdd);

            if (!_songRepository.Save())
            {
                throw new Exception("Creating a songitem failed on save.");
            }

            SongEntity newSongItem = _songRepository.GetSingle(toAdd.Id);
            SongDto songDto = _mapper.Map<SongDto>(newSongItem);

            return CreatedAtRoute(nameof(GetSingleSong),
                new { version = version.ToString(), id = newSongItem.Id },
                _linkService.ExpandSingleItem(songDto, songDto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateSong))]
        public ActionResult<SongDto> PartiallyUpdateSong(ApiVersion version, int id, [FromBody] JsonPatchDocument<SongUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            SongEntity existingEntity = _songRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            SongUpdateDto SongUpdateDto = _mapper.Map<SongUpdateDto>(existingEntity);
            patchDoc.ApplyTo(SongUpdateDto);

            TryValidateModel(SongUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(SongUpdateDto, existingEntity);
            SongEntity updated = _songRepository.Update(id, existingEntity);

            if (!_songRepository.Save())
            {
                throw new Exception("Updating a songitem failed on save.");
            }

            SongDto songDto = _mapper.Map<SongDto>(updated);

            return Ok(_linkService.ExpandSingleItem(songDto, songDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveSong))]
        public ActionResult RemoveSong(int id)
        {
            SongEntity songItem = _songRepository.GetSingle(id);

            if (songItem == null)
            {
                return NotFound();
            }

            _songRepository.Delete(id);

            if (!_songRepository.Save())
            {
                throw new Exception("Deleting a songitem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateSong))]
        public ActionResult<SongDto> UpdateSong(ApiVersion version, int id, [FromBody] SongUpdateDto SongUpdateDto)
        {
            if (SongUpdateDto == null)
            {
                return BadRequest();
            }

            var existingSongItem = _songRepository.GetSingle(id);

            if (existingSongItem == null)
            {
                return NotFound();
            }

            _mapper.Map(SongUpdateDto, existingSongItem);

            _songRepository.Update(id, existingSongItem);

            if (!_songRepository.Save())
            {
                throw new Exception("Updating a songitem failed on save.");
            }

            SongDto songDto = _mapper.Map<SongDto>(existingSongItem);

            return Ok(_linkService.ExpandSingleItem(songDto, songDto.Id, version));
        }

        [HttpGet("GetRandomSong", Name = nameof(GetRandomSong))]
        public ActionResult GetRandomSong()
        {
            ICollection<SongEntity> songItems = _songRepository.GetRandomSong();

            IEnumerable<SongDto> dtos = songItems.Select(x => _mapper.Map<SongDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomSong), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
