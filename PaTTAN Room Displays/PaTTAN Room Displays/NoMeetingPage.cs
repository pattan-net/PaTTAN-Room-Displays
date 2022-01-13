using Microsoft.Maui.Controls;

namespace PaTTAN_Room_Displays
{
	public class NoMeetingPage : ContentPage
	{
		public NoMeetingPage()
		{
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "No meetings :(" }
				}
			};
		}
	}
}