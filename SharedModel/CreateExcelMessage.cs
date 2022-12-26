using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel
{
    public class CreateExcelMessage
    {
      //  public string UserId { get; set; }  // Hangi kullanıcı için excel oluşturucaz
        public int FileId { get; set; }   // Hangi dosyanın excel'i oluşacak
    }
}
