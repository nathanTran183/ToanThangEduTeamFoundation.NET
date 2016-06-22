using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Models;

namespace ToanThangEdu.Controllers
{
    public class ThoiKhoaBieuRow
    {
        public string MônHọc;
        public string Thứ;
        public string Buổi;
        public string Thờigian; 
    }
    public class ThoiGianCls{
        public string Thứ;
        public string Buổi;
        public string Thờigian;
    }

    public class ThoiKhoaBieuMaster
    {
        public string MonHoc;
        public List<ThoiGianCls> listThoiGian;
    }
    public class UserPeriodsController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: Periods
        public ActionResult Index()
        {
            var ThoiKhoaBieu = new List<ThoiKhoaBieuRow>();
            foreach(var item in db.Periods)
            {
                
                var monHocs = (List<string>)item.Subjects.Split(',').ToList();
                var lstMonHoc = new List<string>();
                foreach (var itemChild in monHocs)
                {
                    ThoiKhoaBieuRow thoiKhoaBieu = new ThoiKhoaBieuRow();
                    thoiKhoaBieu.MônHọc = itemChild;
                    thoiKhoaBieu.Thứ = item.DayOfWeek;
                    thoiKhoaBieu.Buổi = item.Time;
                    thoiKhoaBieu.Thờigian = item.DistanceOfTime;
                    ThoiKhoaBieu.Add(thoiKhoaBieu);
                }        
                
            }
            var ThoiKhoaBieuOrder = new List<ThoiKhoaBieuRow>();
            ThoiKhoaBieuOrder = ThoiKhoaBieu.OrderBy(x => x.MônHọc).ToList();

            var thoiKhoaBieuMaster = new List<ThoiKhoaBieuMaster>();
            ThoiKhoaBieuMaster thoiKhoaBieuMasterTmp = new ThoiKhoaBieuMaster();
            List<ThoiGianCls> listThoiGianClsTmp = new List<ThoiGianCls>();
            foreach (var item in ThoiKhoaBieuOrder)
            {
                List<ThoiGianCls> listThoiGianCls = new List<ThoiGianCls>();
                ThoiKhoaBieuMaster tkbMaster = new ThoiKhoaBieuMaster();

                ThoiGianCls thoiGianCls = new ThoiGianCls();                
                thoiGianCls.Buổi = item.Buổi;
                thoiGianCls.Thờigian = item.Thờigian;
                thoiGianCls.Thứ = item.Thứ;

                tkbMaster.MonHoc = item.MônHọc;
                listThoiGianCls.Add(thoiGianCls);
                tkbMaster.listThoiGian = listThoiGianCls;
                if (thoiKhoaBieuMasterTmp.MonHoc == item.MônHọc)
                {
                    var temp = thoiKhoaBieuMaster.Find(x => x.MonHoc == item.MônHọc);
                    temp.listThoiGian.Add(thoiGianCls);                    
                }
                else
                {
                    thoiKhoaBieuMaster.Add(tkbMaster);
                }
                thoiKhoaBieuMasterTmp = tkbMaster;
            }
            ViewBag.ThoiKhoaBieuMaster = thoiKhoaBieuMaster;

            return View();
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
