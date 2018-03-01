using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanelDevice.Models
{
    [Table("HtmlSave")]
    public class HtmlSave
    {
        [Key]
        public int ID { get; set; }
        [AllowHtml]
        public string HtmlFile { get; set; }
    }
}