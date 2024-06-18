using System.Globalization;

namespace ConectaEsporte.API.Models
{
    [Serializable]
    public class DashModel
    {
        public string Name { get; set; }
        public string Picture { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Whatsapp { get; set; }

        public List<MenuModel> Menus { get; set; } = new List<MenuModel>();

        public List<OrganizationModel> Organizations { get; set; } = new List<OrganizationModel>();


        public int TotalCheckin { get; set; }

        public int TotalNotification { get; set; }

        public List<CheckinDetailModel> ListToday { get; set; } = new List<CheckinDetailModel>();

        public List<CheckinDetailModel> ListNext { get; set; } = new List<CheckinDetailModel>();

        public List<CheckinDetailModel> ListDone { get; set; } = new List<CheckinDetailModel>();
    }

    [Serializable]
    public class MenuModel
    {
        public string Name { get; set; }
    }


    [Serializable]
    public class OrganizationModel
    {
        public string Name { get; set; }
        public List<RoutineModel> Routines { get; set; }
    }

    [Serializable]
    public class RoutineModel
    {
        public string Name { get; set; }

        public List<ActivityModel> Activities { get; set; }
    }


    [Serializable]
    public class ActivityModel
    {
        public string Name { get; set; }
    }

 

    [Serializable]
    public class CheckinDetailModel
    {
        public CheckinDetailModel() { }
        public long id {  get; set; }
        public required string Title { get; set; }
        public required string FromEmail{ get; set; }
        public required string FromName { get; set; }

        public DateTime BookedDt { get; set; }
        public string BookedFmtDt => BookedDt.ToString("dd/MM/yyyy");

        public string BookedFmtTime { get { return BookedDt.ToString("HH:mm"); } }

        public bool Booked { get; set; }
    }
}
