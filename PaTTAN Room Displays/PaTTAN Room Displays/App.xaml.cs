using System.Threading.Tasks;

namespace PaTTAN_Room_Displays;

public partial class App : Application
{
    RoomDataService roomDataService = new RoomDataService();
    public static List<Meeting> meetingList = new List<Meeting>();
    public App()
    {
        InitializeComponent();
        getData();

        if (meetingList.Count > 0)
        {
            MainPage = new MainPage();
        }
        else
        {
            MainPage = new NoMeetingsPage();
        }
    }

    private void getData()
    {
        meetingList = roomDataService.GetRoomData();
    }
}
