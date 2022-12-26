using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RabbitMQWeb.CreateExcell.Hubs;
using RabbitMQWeb.CreateExcell.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQWeb.CreateExcell.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private readonly MyApplicationDbContext _context;
        private readonly IHubContext<MyHub> _hubContext;    // Hub a bağlandım

        public FilesController(MyApplicationDbContext context, IHubContext<MyHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int fileId)
        {
            if (file is not { Length: > 0 }) return BadRequest();


            var userFile = await _context.UserFileInfos.FirstAsync(x => x.Id == fileId);

            var filePath = userFile.FileName + Path.GetExtension(file.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", filePath);    



            using FileStream stream = new(path, FileMode.Create);       // ilgli dizinde dosyayı oluşturur

            await file.CopyToAsync(stream);         // içeriğini kopyalar ilgli dizine


            userFile.CreatedDate = DateTime.Now;
            userFile.FilePath = filePath;
            userFile.FileStatus = FileStatus.Completed;

            await _context.SaveChangesAsync();
            //SignalR notification oluşturulacak
            await _hubContext.Clients.User(userFile.UserId).SendAsync("CompletedFile"); // Sadece sistemde giriş yapan kullanıcıya gönderecek

            return Ok();
        }
    }
}
