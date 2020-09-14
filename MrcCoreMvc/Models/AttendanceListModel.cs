using Microsoft.AspNetCore.Mvc.Rendering;
using MRCDataLibrary._02_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MrcCoreMvc.Models
{
    public class AttendanceListModel
    {
        public List<AttendanceModel> AttendanceList { get; set; }
        public WorshipModel Worship { get; set; }
    }
}
