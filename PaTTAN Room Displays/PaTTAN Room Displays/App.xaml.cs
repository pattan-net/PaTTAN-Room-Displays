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

		MainPage = new MainPage();
	}

	private void getData()
    {
		meetingList = roomDataService.GetRoomData();
    }
}
