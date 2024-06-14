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
}
