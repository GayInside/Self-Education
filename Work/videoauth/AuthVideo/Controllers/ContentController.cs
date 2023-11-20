using AuthVideo.Domain.Entities;
using AuthVideo.Domain.EntitiesModels;
using AuthVideo.Domain.InfrastructureInterfaces;
using AuthVideo.Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AuthVideo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : Controller
    {
        public const string PATH = @"C:\Users\User\Desktop\GitHub\videoauth\";
        private IRepository<Content> _contentRepository;
        private IContentService _contentService;

        public ContentController(IRepository<Content> contentRepository, IContentService contentService)
        {
            _contentRepository = contentRepository;
            _contentService = contentService;
        }


        [Authorize]
        [HttpGet("GetContent")]
        public IActionResult GetContent(string contentName)
        {
            if (contentName is null)
            {
                return BadRequest("incorrect");
            }

            return Ok(_contentRepository.GetAllByName(contentName));
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteContent")]
        public IActionResult DeleteContent(int id)
        {
            var content = _contentRepository.GetById(id);
            if (content is null)
            {
                return BadRequest("No such content");
            }

            System.IO.File.Delete(content.ImageURL ?? throw new ArgumentNullException());
            System.IO.File.Delete(content.VideoURL ?? throw new ArgumentNullException());

            _contentRepository.Delete(content);
            return Ok();
        }


        [DisableRequestSizeLimit]
        [Authorize(Roles = "admin")]
        [HttpPut("ChangeContent")]
        public async Task<IActionResult> ChangeContent([FromForm] ContentChangeModel content)
        {
            var previousContent = _contentRepository.GetById(content.Id);

            if (content is null || previousContent is null)
            {
                return BadRequest("Invalid response");
            }

            if (content.Video is not null)
                await ChangeVideo(previousContent, content.Video);

            if (content.Image is not null)
                await ChangeImage(previousContent, content.Image);

            if (content.Title is not null)
                ChangeTitle(previousContent, content.Title);

            if (content.Description is not null)
                ChangeDescription(previousContent, content.Description);

            _contentRepository.UpdateEntity(previousContent);

            return Ok(previousContent);
        }

        private async Task ChangeVideo(Content content, IFormFile video)
        {
            string extencion = Path.GetExtension(video.FileName);
            switch (extencion)
            {
                case ".mp4":
                    break;
                //TODO Добавить другие расширения :)
                default:
                    throw new ArgumentException();
            }

            System.IO.File.Delete(content.VideoURL ?? throw new ArgumentNullException());

            using (var fileStream = new FileStream(content.VideoURL, FileMode.Create))
            {
                await video.CopyToAsync(fileStream);
            }
        }

        private async Task ChangeImage(Content content, IFormFile image)
        {
            string extencion = Path.GetExtension(image.FileName);
            switch (extencion)
            {
                case ".jpg":
                    break;

                case ".png":
                    break;
                //TODO Добавить другие расширения :)
                default:
                    throw new ArgumentException();
            }

            System.IO.File.Delete(content.ImageURL ?? throw new ArgumentNullException());

            using (var fileStream = new FileStream(content.ImageURL, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
        }

        private void ChangeDescription(Content content, string description)
        {
            content.Description = description;
        }

        private void ChangeTitle(Content content, string title)
        {
            content.Title = title;
        }

        private string GetVideoPath(IFormFile videoForm)
        {
            string extencion = Path.GetExtension(videoForm.FileName);

            var pathBuilder = new StringBuilder(PATH);
            switch (extencion)
            {
                case ".mp4":
                    pathBuilder.Append(@"video\");
                    break;
                //TODO Добавить другие расширения :)
                default:
                    throw new ArgumentException();
            }

            return pathBuilder.ToString();
        }

        private string GetImagePath(IFormFile imageForm)
        {
            string extencion = Path.GetExtension(imageForm.FileName);

            var pathBuilder = new StringBuilder(PATH);
            switch (extencion)
            {
                case ".png":
                    pathBuilder.Append(@"img\");// + _contentService.GetFileName(contentModel.File.FileName);
                    break;

                case ".jpg":
                    pathBuilder.Append(@"img\");// + _contentService.GetFileName(contentModel.File.FileName);
                    break;
                //TODO добавить еще форматов
                default:
                    throw new ArgumentException();
            }

            return pathBuilder.ToString();
        }


        [Authorize(Roles = "admin")]
        [Route("AddContent")]
        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<IActionResult> AddContent([FromForm] ContentModel contentModel)
        {
            if (contentModel == null || contentModel.Video is null || contentModel.Image is null || contentModel.Video.Length <= 0)
            {
                return BadRequest("Invalid input");
            }

            StringBuilder imageRootBuilder;
            StringBuilder videoRootBuilder;
            try
            {
                imageRootBuilder = new StringBuilder(GetImagePath(contentModel.Image));
                videoRootBuilder = new StringBuilder(GetVideoPath(contentModel.Video));
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid input");
            }

            var content = new Content
            {
                VideoURL = "Temporary",
                ImageURL = "Temporary",
                Title = contentModel.Title,
                Description = contentModel.Description
            };

            _contentRepository.Add(content);
            string idStr = _contentRepository.Get("Temporary")?.Id.ToString() ?? throw new ArgumentNullException();

            imageRootBuilder.Append(idStr);
            videoRootBuilder.Append(idStr);

            imageRootBuilder.Append(Path.GetExtension(contentModel.Image.FileName));
            videoRootBuilder.Append(Path.GetExtension(contentModel.Video.FileName));

            content.VideoURL = videoRootBuilder.ToString();
            content.ImageURL = imageRootBuilder.ToString();

            _contentRepository.UpdateEntity(content);

            using (var fileStream = new FileStream(content.VideoURL, FileMode.Create))
            {
                await contentModel.Video.CopyToAsync(fileStream);
            }

            using (var fileStream = new FileStream(content.ImageURL, FileMode.Create))
            {
                await contentModel.Image.CopyToAsync(fileStream);
            }

            return Ok(content);
        }

    }
}
